using System.IO;
using ToDo.API;
using ToDo.Core;
using ToDo.Storage;

namespace ToDo
{
    public static class ComponentsFactory
    {
        public static ITaskController CreateTaskController(string storageParentFolderPath)
        {
            var taskStorage = new SQLiteStorage(Path.Combine(storageParentFolderPath, "tasksDatabase.db"));
            return new TaskController(taskStorage);
        }
    }
}