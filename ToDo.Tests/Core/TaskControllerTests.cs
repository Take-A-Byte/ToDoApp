using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.API;
using ToDo.Core;

namespace ToDo.Tests.Core
{
    public class TaskControllerTests
    {
        private IReadOnlyList<IToDoTask> _tasks;
        private Mock<ITaskStorage> _mockTaskStorage;
        private bool _addedTaskInStorage;

        private TaskController _taskController;

        [SetUp]
        public void SetUp()
        {
            _mockTaskStorage = new Mock<ITaskStorage>();
            _mockTaskStorage.Setup(storage => storage.AddNewTask(It.IsAny<long>(), It.IsAny<string>())).ReturnsAsync(true);
            var tasks = new List<IToDoTask>();
            tasks.Add(_taskController.CreateToDoTask(1, "This is test task 1", false));
            tasks.Add(_taskController.CreateToDoTask(2, "This is test task 2", true));
            _tasks = tasks.AsReadOnly();
            _mockTaskStorage.Setup(storage => storage.GetAllTasks()).ReturnsAsync(_tasks);

            _taskController = new TaskController(_mockTaskStorage.Object);
        }

        [TearDown]
        public void CleanUp()
        {
            _addedTaskInStorage = false;
            _mockTaskStorage = null;
            _taskController = null;
        }

        [Test]
        [TestCase()]
        [TestCase("")]
        [TestCase("   ")]
        public async Task OnAddTask_WithEmptyDescription_AndReturnsFalse_AndDoesNotAddStorage(string description = null)
        {
            // when
            bool result = await _taskController.AddTask(description);

            // then
            Assert.IsFalse(result);
            Assert.IsFalse(_addedTaskInStorage);
        }

        [Test]
        public async Task OnAddTask_WithNonEmptyDescription_ReturnsTrue_AndAddsToStorage()
        {
            // when
            bool result = await _taskController.AddTask("This is description of task");

            // then
            Assert.IsTrue(result);
            Assert.IsTrue(_addedTaskInStorage);
        }

        [Test]
        public async Task OnGetAllTasks_GetAllTasksFromStorage()
        {
            // when
            var retrivedTasks = await _taskController.GetAllTasks();

            //then
            Assert.AreEqual(_tasks.Count, retrivedTasks.Count);
            Assert.AreEqual(_tasks[0].Description, retrivedTasks[0].Description);
            Assert.AreEqual(_tasks[1].Description, retrivedTasks[1].Description);
            Assert.AreEqual(_tasks[0].HasCompleted, retrivedTasks[0].HasCompleted);
            Assert.AreEqual(_tasks[1].HasCompleted, retrivedTasks[1].HasCompleted);
        }

        [Test]
        [TestCase()]
        [TestCase("")]
        [TestCase("   ")]
        public async Task OnDescriptionSetCalled_WithEmptyString_DoesNotUpdateDescription(string description = null)
        {
            // given
            var retrivedTasks = await _taskController.GetAllTasks();
            string oldDescriptionOfTask1 = retrivedTasks[0].Description; 

            // when
            retrivedTasks[0].Description = description;

            // then
            Assert.AreEqual(oldDescriptionOfTask1, retrivedTasks[0]);
        }

        [Test]
        public async Task OnDescriptionSetCalled_WithNonEmptyString_UpdatesDescription_AndRaisesDescriptionChangedEvent()
        {
            // given
            var retrivedTasks = await _taskController.GetAllTasks();

            // when
            const string newDescription = "new description";
            retrivedTasks[0].Description = newDescription;

            // then
            Assert.AreEqual(newDescription, retrivedTasks[0]);
        }

        [Test]
        public async Task OnHasCompletedSetCalled_UpdatesHasCompleted_AndRaisesHasCompletedChangedEventAsync()
        {
            // given
            var retrivedTasks = await _taskController.GetAllTasks();

            // when
            bool newHasCompleted = !retrivedTasks[0].HasCompleted;
            retrivedTasks[0].HasCompleted = newHasCompleted;

            // then
            Assert.AreEqual(newHasCompleted, retrivedTasks[0]);
        }
    }
}
