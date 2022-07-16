using NUnit.Framework;
using System.Threading.Tasks;

namespace ToDo.Tests.Core
{
    public class TaskControllerTests
    {
        [SetUp]
        public void SetUp()
        {

        }

        [TearDown]
        public void CleanUp()
        {

        }

        [Test]
        [TestCase()]
        [TestCase("")]
        [TestCase("   ")]
        public async Task OnAddTask_WithEmptyDescription_AndReturnsFalse_AndDoesNotAddStorage(string description = null)
        {
            Assert.Fail();
        }

        [Test]
        public async Task OnAddTask_WithNonEmptyDescription_ReturnsTrue_AndAddsToStorage()
        {
            Assert.Fail();
        }

        public async Task OnGetAllTasks_GetAllTasksFromStorage()
        {
            Assert.Fail();
        }
    }
}
