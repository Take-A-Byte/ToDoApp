﻿namespace ToDo.API
{
    public interface ITaskStorage
    {
        Task<IReadOnlyDictionary<long, IToDoTask>> GetAllTasks(Func<long, string, bool, IToDoTask> taskCreator);
        Task<IToDoTask> AddNewTask(long taskId, string description, Func<long, string, bool, IToDoTask> taskCreator);
        Task<bool> UpdateTaskDescription(long taskId, string description);
        Task<bool> UpdateTaskCompleteness(long taskId, bool completeness);
        Task<long> GetTotalNumberOfTasks();
    }
}
