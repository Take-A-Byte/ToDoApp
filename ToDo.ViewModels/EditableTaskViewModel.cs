using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDo.API;

namespace ToDo.ViewModels
{
    public enum TaskState
    {
        Adding,
        Added,
        Editing
    }

    public class EditableTaskViewModel : BindableBase
    {
        private IToDoTask _task;
        private TaskState _currentState;
        private string _temperoryDescription;

        internal EditableTaskViewModel(IToDoTask task)
        {
            _task = task;
            _temperoryDescription = task.Description;
            CurrentTaskState = TaskState.Added;
        }

        internal EditableTaskViewModel()
        {
            CurrentTaskState = TaskState.Adding;
        }

        public event Action<string> IncorrectDescription;
        internal event Func<string, Action<IToDoTask>, Task> AddTaskClicked;

        public TaskState CurrentTaskState
        {
            get { return _currentState; }
            private set
            {
                _currentState = value;
                OnPropertyChanged();
            }
        }

        public bool HasCompleted
        {
            get { return _task != null ? _task.HasCompleted : false; }
            set
            {
                if (CurrentTaskState == TaskState.Adding)
                {
                    Debug.Fail("Task which is not yet added cannot be complete");
                    return;
                }

                if (_task?.HasCompleted != value)
                {
                    _task.HasCompleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get { return _temperoryDescription; }
            set
            {
                if (_temperoryDescription != value)
                {
                    _temperoryDescription = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SwitchToEdit()
        {
            CurrentTaskState = TaskState.Editing;
        }

        public async Task AcceptChanges()
        {
            if (!String.IsNullOrEmpty(Description))
            {
                if (CurrentTaskState == TaskState.Adding)
                {
                    await AddTaskClicked?.Invoke(Description, (taskFromStorage) => _task = taskFromStorage);
                }
                else
                {
                    _task.Description = Description;
                }

                CurrentTaskState = TaskState.Added;
            }
            else
            {
                IncorrectDescription?.Invoke("Write a description for task before adding");
            }
        }

        public void DiscardChanges()
        {
            if (CurrentTaskState == TaskState.Adding)
            {
                Description = String.Empty;
            }
            else
            {
                Description = _task.Description;
                CurrentTaskState = TaskState.Added;
            }
        }
    }
}
