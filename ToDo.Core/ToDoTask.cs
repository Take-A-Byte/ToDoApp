using System.ComponentModel;
using ToDo.API;

namespace ToDo.Core
{
    internal class ToDoTask : IToDoTask, INotifyPropertyChanged
    {
        private string _description;
        private bool _hasCompleted;

        public ToDoTask(long id, string description, bool hasCompleted = false)
        {
            Id = id;
            _description = description;
            _hasCompleted = hasCompleted;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value && !string.IsNullOrWhiteSpace(value))
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

        public bool HasCompleted
        {
            get
            {
                return _hasCompleted;
            }
            set
            {
                if (_hasCompleted != value)
                {
                    _hasCompleted = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasCompleted)));
                }
            }
        }
    }
}