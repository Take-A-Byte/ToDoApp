﻿using ToDo.API;

namespace ToDo.Storage
{
    public class SQLiteStorage : ITaskStorage
    {
        public Task<bool> AddNewTask(long taskId, string description)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IToDoTask>> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTaskCompleteness(long taskId, bool completeness)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTaskDescription(long taskId, string description)
        {
            throw new NotImplementedException();
        }
    }
}