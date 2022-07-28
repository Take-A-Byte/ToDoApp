using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDo.API
{
    public interface ITaskController
    {
        /// <summary>
        /// Gets all saved tasks
        /// </summary>
        /// <returns>Returns dictionary of task id and task</returns>
        Task<IReadOnlyDictionary<long, IToDoTask>> GetAllTasks();

        /// <summary>
        /// Adds new task to be done
        /// </summary>
        /// <param name="description">Description of the task</param>
        /// <returns>Newly created task</returns>
        Task<IToDoTask> AddTask(string description);

        Task<bool> DeleteTask(long task);
    }
}
