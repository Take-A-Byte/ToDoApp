using Moq;
using NUnit.Framework;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDo.API;

namespace ToDo.Tests.Storage
{
    public abstract class StorageTestsBase
    {
        protected ITaskStorage _storage;

        protected abstract void AddTaskToTestStorage(long id, IToDoTask task);
        protected abstract IToDoTask GetTaskWithId(long id);
        protected abstract int NumberOfTasksInStorage();

        [Test]
        public abstract void OnObjectCreation_StorageFileExists();

        [Test]
        public async Task OnAddNewTask_ReturnsTrue_TaskIsSavedInStorage()
        {
            //given
            const long newTaskId = 1;
            const string newTaskDescription = "my new task description";

            // when
            bool resultWhenStringIsEmpty = await _storage.AddNewTask(newTaskId, newTaskDescription);

            // then
            Assert.IsTrue(resultWhenStringIsEmpty);
            Assert.AreEqual(1, NumberOfTasksInStorage());
            var addedTask = GetTaskWithId(newTaskId);
            Assert.AreEqual(newTaskDescription, addedTask.Description);
            Assert.AreEqual(false, addedTask.HasCompleted);
        }

        [Test]
        public async Task OnGetAllTasks_ReturnsTrue_RetrivesAllTasksInStorageWithCorrectDetails()
        {
            //given
            var task1 = CreateNewMockTask("This is test task 1", false);
            AddTaskToTestStorage(1, task1);

            var task2 = CreateNewMockTask("This is test task 2", true);
            AddTaskToTestStorage(2, task2);

            // when
            var tasks = await _storage.GetAllTasks();

            // then
            Assert.AreEqual(NumberOfTasksInStorage(), tasks.Count);
            Assert.AreEqual(task1.Description, tasks[0].Description);
            Assert.AreEqual(task2.Description, tasks[1].Description);
            Assert.AreEqual(task1.HasCompleted, tasks[0].HasCompleted);
            Assert.AreEqual(task2.HasCompleted, tasks[1].HasCompleted);
        }

        #region UpdateTaskCompleteness
        [Test]
        public async Task OnUpdateTaskCompletenessWithIncorrectID_ReturnsFalse_DoesNotUpdateStorage()
        {
            // when
            bool result = await _storage.UpdateTaskCompleteness(1, true);

            // given
            Assert.IsFalse(result);
        }

        [Test]
        public async Task OnUpdateTaskCompletenessWithCorrectID_ReturnsTrue_UpdatesTaskInStorage()
        {
            // given 
            var newTask = CreateNewMockTask("This is test task", false);
            const long id = 1;
            AddTaskToTestStorage(id, newTask);

            // when
            bool updatedCompleteness = true;
            bool result = await _storage.UpdateTaskCompleteness(id, updatedCompleteness);

            // given
            Assert.IsTrue(result);
            var taskFromStorage = GetTaskWithId(id);
            Assert.AreEqual(newTask.Description, taskFromStorage.Description);
            Assert.AreEqual(updatedCompleteness, taskFromStorage.HasCompleted);
        }
        #endregion

        #region UpdateDescription
        [Test]
        public async Task OnUpdateTaskDescriptionWithIncorrectID_ReturnsFalse_DoesNotUpdateStorage()
        {
            // when
            bool result = await _storage.UpdateTaskDescription(1, "new description");

            // given
            Assert.IsFalse(result);
        }

        [Test]
        public async Task OnUpdateTaskDescriptionWithCorrectIDAndDescription_ReturnsTrue_UpdatesTaskInStorage()
        {
            // given 
            var newTask = CreateNewMockTask("old description", false);
            const long id = 1;
            AddTaskToTestStorage(id, newTask);

            // when
            const string description = "new description";
            bool result = await _storage.UpdateTaskDescription(id, "new description");

            // given
            Assert.IsTrue(result);
            var taskFromStorage = GetTaskWithId(id);
            Assert.AreEqual(description, taskFromStorage.Description);
            Assert.AreEqual(newTask.HasCompleted, taskFromStorage.HasCompleted);
        }
        #endregion

        private static IToDoTask CreateNewMockTask(string description, bool hasCompleted)
        {
            var newTask = new Mock<IToDoTask>();
            newTask.SetupGet(task => task.Description).Returns(description);
            newTask.SetupGet(task => task.HasCompleted).Returns(hasCompleted);
            return newTask.Object;
        }
    }
}
