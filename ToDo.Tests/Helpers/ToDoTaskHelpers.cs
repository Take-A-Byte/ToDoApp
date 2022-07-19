using Moq;
using ToDo.API;

namespace ToDo.Tests.Helpers
{
    internal static class ToDoTaskHelpers
    {
        internal static IToDoTask CreateNewMockTask(long id, string description, bool hasCompleted)
        {
            var mockTask = new Mock<IToDoTask>();
            mockTask.SetupGet(t => t.Id).Returns(id);
            mockTask.SetupGet(task => task.Description).Returns(() => description);
            mockTask.SetupSet(task => task.Description = It.IsAny<string>())
                .Callback<string>((newDescription) => description = newDescription);
            mockTask.SetupGet(task => task.HasCompleted).Returns(() => hasCompleted);
            mockTask.SetupSet(task => task.HasCompleted = It.IsAny<bool>())
                .Callback<bool>((newHasCompleted) => hasCompleted = newHasCompleted);
            return mockTask.Object;
        }
    }
}
