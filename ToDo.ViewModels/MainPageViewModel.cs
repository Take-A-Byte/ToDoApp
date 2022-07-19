using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ToDo.API;

namespace ToDo.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private ITaskController _controller;
        private List<EditableTaskViewModel> _unfilteredTasks;
        private ObservableCollection<EditableTaskViewModel> _tasks = new ObservableCollection<EditableTaskViewModel>();
        private Func<string, bool> _doesSatisfySearchQuery = new Func<string, bool>((description) => true);
        private EditableTaskViewModel _toBeAddedTask;

        public MainPageViewModel()
        {
            Tasks = new ReadOnlyObservableCollection<EditableTaskViewModel>(_tasks);
            ToBeAddedTask = new EditableTaskViewModel();
        }

        public ReadOnlyObservableCollection<EditableTaskViewModel> Tasks { get; }

        public EditableTaskViewModel ToBeAddedTask
        {
            get => _toBeAddedTask;
            private set
            {
                if (_toBeAddedTask != null)
                {
                    _toBeAddedTask.AddTaskClicked -= AddNewTask;
                }

                _toBeAddedTask = value;
                if (_toBeAddedTask != null)
                {
                    _toBeAddedTask.AddTaskClicked += AddNewTask;
                }

                OnPropertyChanged();
            }
        }

        public async Task Initialize(ITaskController controller)
        {
            _controller = controller;

            foreach (var task in await _controller.GetAllTasks())
            {
                _unfilteredTasks.Add(new EditableTaskViewModel(task.Value));
                _tasks.Add(new EditableTaskViewModel(task.Value));
            }
        }

        public void FilterTasks(string searchKey)
        {
            if (_unfilteredTasks == null)
            {
                _unfilteredTasks = new List<EditableTaskViewModel>(_tasks);
            }

            _tasks.Clear();
            foreach (var task in _unfilteredTasks)
            {
                _doesSatisfySearchQuery = new Func<string, bool>((description) => FuzzySharp.Fuzz.PartialRatio(searchKey, description) > 45);
                if (string.IsNullOrWhiteSpace(searchKey) || _doesSatisfySearchQuery(task.Description))
                {
                    _tasks.Add(task);
                }
            }
        }

        private async Task AddNewTask(string description, Action<IToDoTask> saveTaskFromStorage)
        {
            var newTask = await _controller.AddTask(description);
            if (newTask != null)
            {
                saveTaskFromStorage(newTask);
                _unfilteredTasks.Add(ToBeAddedTask);
                if (_doesSatisfySearchQuery(ToBeAddedTask.Description))
                {
                    _tasks.Add(ToBeAddedTask);
                }

                ToBeAddedTask = new EditableTaskViewModel();

            }
        }
    }
}
