﻿namespace ToDo.API
{
    public interface ITaskStorage
    {
        Task<IEnumerable<IToDoTask>> GetAllTasks();
        Task<bool> AddNewTask(long taskId, string description);
        Task<bool> UpdateTaskDescription(long taskId, string description);
        Task<bool> UpdateTaskCompleteness(long taskId, bool completeness);
    }
}
