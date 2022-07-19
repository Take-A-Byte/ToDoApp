using ToDo.API;
using ToDo.Core;
using ToDo.Storage;

namespace ToDo
{
    public static class ComponentsFactory
    {
        public static ITaskController CreateTaskController(string storagePath)
        {
            var taskStorage = new SQLiteStorage(storagePath);
            return new TaskController(taskStorage);
        }
    }
}
