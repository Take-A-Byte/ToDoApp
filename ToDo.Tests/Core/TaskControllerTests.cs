using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDo.API;
using ToDo.Core;

namespace ToDo.Tests.Core
{
    public class TaskControllerTests
    {
        private List<IToDoTask> _tasks;
        private Mock<ITaskStorage> _mockTaskStorage;

        private TaskController _taskController;

        [SetUp]
        public void SetUp()
        {
            _tasks = new List<IToDoTask>();
            _mockTaskStorage = new Mock<ITaskStorage>();
            _taskController = new TaskController(_mockTaskStorage.Object);

            _mockTaskStorage.Setup(storage => storage.AddNewTask(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(
                (long id, string description) =>
                {
                    _tasks.Add(CreateMockTask(id, description, false));
                    return true;
                });

            _mockTaskStorage.Setup(storage => storage.GetTotalNumberOfTasks()).ReturnsAsync(() => _tasks.Count);
            _mockTaskStorage.Setup(storage => storage.GetAllTasks(It.IsAny<Func<long, string, bool, IToDoTask>>()))
                .ReturnsAsync((Func<long, string, bool, IToDoTask> actualTaskCreator) =>
                {
                    Dictionary<long, IToDoTask> actualTasks = new Dictionary<long, IToDoTask>();
                    foreach(var task in _tasks)
                    {
                        actualTasks.Add(task.Id, actualTaskCreator(task.Id, task.Description, task.HasCompleted));
                    }
                    return actualTasks;
                }
                );
            _mockTaskStorage.Setup(storage => storage.UpdateTaskDescription(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync((long id, string description) =>
                {
                    _tasks.Find(x => x.Id == id).Description = description;
                    return true;
                });
            _mockTaskStorage.Setup(storage => storage.UpdateTaskCompleteness(It.IsAny<long>(), It.IsAny<bool>()))
                .ReturnsAsync((long id, bool hasCompleted) =>
                {
                    _tasks.Find(x => x.Id == id).HasCompleted = hasCompleted;
                    return true;
                });
        }

        [TearDown]
        public void CleanUp()
        {
            _tasks.Clear();
            _tasks = null;
            _mockTaskStorage = null;
            _taskController = null;
        }

        [Test]
        [TestCase()]
        [TestCase("")]
        [TestCase("   ")]
        public async Task OnAddTask_WithEmptyDescription_AndReturnsFalse_AndDoesNotAddStorage(string description = null)
        {
            // given
            Trace.Listeners.Clear();

            // when
            bool result = await _taskController.AddTask(description);

            // then
            Assert.IsFalse(result);
            Assert.AreEqual(0, _tasks.Count);
        }

        [Test]
        public async Task OnAddTask_WithNonEmptyDescription_ReturnsTrue_AndAddsToStorage()
        {
            // when
            bool result = await _taskController.AddTask("This is description of task");

            // then
            Assert.IsTrue(result);
            Assert.AreEqual(1, _tasks.Count);
        }

        [Test]
        public async Task OnGetAllTasks_GetAllTasksFromStorage()
        {
            // given
            _tasks.Add(CreateMockTask(1, "This is test task 1", false));
            _tasks.Add(CreateMockTask(2, "This is test task 2", true));

            // when
            var retrivedTasks = await _taskController.GetAllTasks();

            //then
            Assert.AreEqual(_tasks.Count, retrivedTasks.Count);
            Assert.AreEqual(_tasks[0].Description, retrivedTasks[1].Description);
            Assert.AreEqual(_tasks[1].Description, retrivedTasks[2].Description);
            Assert.AreEqual(_tasks[0].HasCompleted, retrivedTasks[1].HasCompleted);
            Assert.AreEqual(_tasks[1].HasCompleted, retrivedTasks[2].HasCompleted);
        }

        [Test]
        [TestCase()]
        [TestCase("")]
        [TestCase("   ")]
        public async Task OnDescriptionSetOnRetrivedTask_WithEmptyString_DoesNotUpdateDescription(string description = null)
        {
            // given
            string oldDescriptionOfTask1 = "This is test task 1";
            _tasks.Add(CreateMockTask(1, oldDescriptionOfTask1, false));
            var retrivedTasks = await _taskController.GetAllTasks();

            // when
            retrivedTasks[1].Description = description;

            // then
            Assert.AreEqual(oldDescriptionOfTask1, retrivedTasks[1].Description);
        }

        [Test]
        public async Task OnDescriptionSetWithRetrivedTask_WithNonEmptyString_UpdatesDescription_AndRaisesDescriptionChangedEvent()
        {
            // given
            _tasks.Add(CreateMockTask(0, "This is test task 1", false));
            var retrivedTasks = await _taskController.GetAllTasks();

            // when
            const string newDescription = "new description";
            retrivedTasks[0].Description = newDescription;

            // then
            Assert.AreEqual(newDescription, retrivedTasks[0].Description);
        }

        [Test]
        public async Task OnHasCompletedSetOnRetrivedTask_UpdatesHasCompleted_AndRaisesHasCompletedChangedEventAsync()
        {
            // given
            _tasks.Add(CreateMockTask(1, "This is test task 1", false));
            var retrivedTasks = await _taskController.GetAllTasks();

            // when
            bool newHasCompleted = !retrivedTasks[1].HasCompleted;
            retrivedTasks[1].HasCompleted = newHasCompleted;

            // then
            Assert.AreEqual(newHasCompleted, retrivedTasks[1].HasCompleted);
        }

        private static IToDoTask CreateMockTask(long id, string description, bool hasCompleted)
        {
            var task = new Mock<IToDoTask>();
            task.SetupGet(t => t.Id).Returns(id);
            task.SetupGet(task => task.Description).Returns(() => description);
            task.SetupSet(task => task.Description = It.IsAny<string>())
                .Callback<string>((newDescription) => description = newDescription);
            task.SetupGet(task => task.HasCompleted).Returns(() => hasCompleted);
            task.SetupSet(task => task.HasCompleted = It.IsAny<bool>())
                .Callback<bool>((newHasCompleted) => hasCompleted = newHasCompleted);
            return task.Object;
        }

    }
}
