using ToDo.API;

namespace ToDo.Core
{
    internal class ToDoTask : IToDoTask
    {
        public long Id => throw new NotImplementedException();

        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        
        public bool HasCompleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
