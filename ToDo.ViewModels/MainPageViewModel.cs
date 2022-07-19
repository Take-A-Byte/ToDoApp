using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ToDo.API;

namespace ToDo.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private ITaskController _controller;
        private ObservableCollection<EditableTaskViewModel> _tasks = new ObservableCollection<EditableTaskViewModel>();
        private EditableTaskViewModel _toBeAddedTask;

        public ReadOnlyObservableCollection<EditableTaskViewModel> Tasks { get; }

        public MainPageViewModel()
        {
            Tasks = new ReadOnlyObservableCollection<EditableTaskViewModel>(_tasks);
            ToBeAddedTask = new EditableTaskViewModel();
        }

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
                _tasks.Add(new EditableTaskViewModel(task.Value));
            }
        }

        private async Task AddNewTask(string description, Action<IToDoTask> saveTaskFromStorage)
        {
            var newTask = await _controller.AddTask(description);
            if (newTask != null)
            {
                saveTaskFromStorage(newTask);
                _tasks.Add(ToBeAddedTask);
                ToBeAddedTask = new EditableTaskViewModel();
            }
        }
    }
}
