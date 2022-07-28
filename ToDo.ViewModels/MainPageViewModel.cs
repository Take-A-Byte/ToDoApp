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
        private List<EditableTaskViewModel> _unfilteredTasks = new List<EditableTaskViewModel>();
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

        private async Task DeleteTask(EditableTaskViewModel editableTaskViewModel, long id)
        {
            _toBeAddedTask.DeleteTaskClicked -= DeleteTask;
            await _controller.DeleteTask(id);
            _unfilteredTasks.Remove(editableTaskViewModel);
            _tasks.Remove(editableTaskViewModel);
        }

        public async Task Initialize(ITaskController controller)
        {
            _controller = controller;

            foreach (var task in await _controller.GetAllTasks())
            {
                var taskViewModel = new EditableTaskViewModel(task.Value);
                taskViewModel.DeleteTaskClicked += DeleteTask;
                _unfilteredTasks.Add(taskViewModel);
                _tasks.Add(taskViewModel);
            }
        }

        public void FilterTasks(string searchKey)
        {
            _tasks.Clear();
            foreach (var task in _unfilteredTasks)
            {
                _doesSatisfySearchQuery = new Func<string, bool>((description) => string.IsNullOrWhiteSpace(searchKey) || FuzzySharp.Fuzz.PartialRatio(searchKey, description) > 45);
                if (_doesSatisfySearchQuery(task.Description))
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
                ToBeAddedTask.DeleteTaskClicked += DeleteTask;
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
