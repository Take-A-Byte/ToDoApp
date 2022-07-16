using Moq;
using ToDo.API;

namespace ToDo.Tests.Stubs
{
    internal class TaskFactoryStub : ITaskFactory
    {
        public IToDoTask CreateToDoTask(long id, string description, bool hasCompleted)
        {
            var task = new Mock<IToDoTask>();
            task.SetupGet(t => t.Id).Returns(id);  
            task.SetupGet(task => task.Description).Returns(description);
            task.SetupGet(task => task.HasCompleted).Returns(hasCompleted);
            return task.Object;
        }
    }
}
