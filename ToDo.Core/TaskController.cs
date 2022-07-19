using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDo.API;

namespace ToDo.Core
{
    public class TaskController : ITaskController
    {
        private readonly ITaskStorage _taskStorage;
        private long? _newIdForUse = null;

        /// <summary>
        /// Creates object of Task Controller
        /// </summary>
        /// <param name="taskStorage">Storage to be used to preserve tasks' states</param>
        public TaskController(ITaskStorage taskStorage)
        {
            _taskStorage = taskStorage;
        }

        public async Task<IToDoTask> AddTask(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return null;
            }

            if (_newIdForUse == null)
            {
                _newIdForUse = await _taskStorage.GetTotalNumberOfTasks();
            }
            return await _taskStorage.AddNewTask((long)_newIdForUse++, description, CreateToDoTask);
        }

        public async Task<IReadOnlyDictionary<long, IToDoTask>> GetAllTasks()
        {
            var tasks = await _taskStorage.GetAllTasks(CreateToDoTask);
            foreach (var task in tasks)
            {
                ((ToDoTask)task.Value).PropertyChanged += OnTaskAttributesChanged;
            }

            return tasks;
        }

        private void OnTaskAttributesChanged(object sender, PropertyChangedEventArgs e)
        {
            ToDoTask toDoTask = (ToDoTask)sender;
            switch (e.PropertyName)
            {
                case nameof(ToDoTask.Description):
                    _taskStorage.UpdateTaskDescription(toDoTask.Id, toDoTask.Description);
                    return;
                case nameof(ToDoTask.HasCompleted):
                    _taskStorage.UpdateTaskCompleteness(toDoTask.Id, toDoTask.HasCompleted);
                    return;
                default:
                    Debug.Fail("We should update storage for this property as well");
                    return;
            }
        }

        private IToDoTask CreateToDoTask(long id, string description, bool hasCompleted)
        {
            var retrivedTask = new ToDoTask(id, description, hasCompleted);
            retrivedTask.PropertyChanged += OnTaskAttributesChanged;
            return retrivedTask;
        }
    }
}