using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDo.API
{
    public interface ITaskStorage
    {
        /// <summary>
        /// Gets all tasks from storage
        /// </summary>
        /// <param name="taskCreator">Function that accepts task id, description and its completeness and returns an object of <see cref="IToDoTask"/></param>
        /// <returns>Readonly dictionary of task ids and tasks</returns>
        Task<IReadOnlyDictionary<long, IToDoTask>> GetAllTasks(Func<long, string, bool, IToDoTask> taskCreator);

        /// <summary>
        /// Adds new task to storage
        /// </summary>
        /// <param name="taskId">Id of new task</param>
        /// <param name="description">Description of new task</param>
        /// <param name="taskCreator">Function that accepts task id, description and its completeness and returns an object of <see cref="IToDoTask"/></param>
        /// <returns><see cref="IToDoTask"/> object representing added task</returns>
        Task<IToDoTask> AddNewTask(long taskId, string description, Func<long, string, bool, IToDoTask> taskCreator);

        /// <summary>
        /// Updates description of task that is already present in storage
        /// </summary>
        /// <param name="taskId">Id of task whoes description needs to be updated</param>
        /// <param name="description">New description of the task</param>
        /// <returns>If update was successful</returns>
        Task<bool> UpdateTaskDescription(long taskId, string description);

        /// <summary>
        /// Updates completeness of task that is already present in storage
        /// </summary>
        /// <param name="taskId">Id of task whoes description needs to be updated</param>
        /// <param name="completeness">New value of completeness of the task</param>
        /// <returns>If update was successful</returns>
        Task<bool> UpdateTaskCompleteness(long taskId, bool completeness);

        /// <summary>
        /// Gets the total number of tasks in the storage
        /// </summary>
        /// <returns>Total number of tasks in storage</returns>
        Task<long> GetTotalNumberOfTasks();
    }
}
