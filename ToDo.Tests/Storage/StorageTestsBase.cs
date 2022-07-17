using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using ToDo.API;

namespace ToDo.Tests.Storage
{
    public abstract class StorageTestsBase
    {
        protected ITaskStorage _storage;

        protected abstract void AddTaskToTestStorage(IToDoTask task);
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
            var addedTask = await _storage.AddNewTask(newTaskId, newTaskDescription, CreateNewMockTask);

            // then
            Assert.IsNotNull(addedTask);
            Assert.AreEqual(1, NumberOfTasksInStorage());
            var taskRetrivedFromStorage = GetTaskWithId(newTaskId);
            Assert.AreEqual(newTaskDescription, addedTask.Description);
            Assert.AreEqual(newTaskDescription, taskRetrivedFromStorage.Description);
            Assert.AreEqual(false, addedTask.HasCompleted);
            Assert.AreEqual(false, taskRetrivedFromStorage.HasCompleted);
        }

        [Test]
        public async Task OnGetAllTasks_ReturnsTrue_RetrivesAllTasksInStorageWithCorrectDetails()
        {
            //given
            var task1 = CreateNewMockTask(1, "This is test task 1", false);
            AddTaskToTestStorage(task1);

            var task2 = CreateNewMockTask(2, "This is test task 2", true);
            AddTaskToTestStorage(task2);

            // when
            var tasks = await _storage.GetAllTasks(CreateNewMockTask);

            // then
            Assert.AreEqual(NumberOfTasksInStorage(), tasks.Count);
            Assert.AreEqual(task1.Description, tasks[1].Description);
            Assert.AreEqual(task2.Description, tasks[2].Description);
            Assert.AreEqual(task1.HasCompleted, tasks[1].HasCompleted);
            Assert.AreEqual(task2.HasCompleted, tasks[2].HasCompleted);
        }

        [Test]
        public async Task OnGetTotalNumberOfTasks_ReturnsNumberOfTasksInStorage()
        {
            //given
            var task1 = CreateNewMockTask(1, "This is test task 1", false);
            AddTaskToTestStorage(task1);

            var task2 = CreateNewMockTask(2, "This is test task 2", true);
            AddTaskToTestStorage(task2);

            // when
            var numberOfTasks = await _storage.GetTotalNumberOfTasks();

            // then
            Assert.AreEqual(2, numberOfTasks);
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
            var newTask = CreateNewMockTask(1, "This is test task", false);
            AddTaskToTestStorage(newTask);

            // when
            bool updatedCompleteness = true;
            bool result = await _storage.UpdateTaskCompleteness(newTask.Id, updatedCompleteness);

            // given
            Assert.IsTrue(result);
            var taskFromStorage = GetTaskWithId(newTask.Id);
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
            var newTask = CreateNewMockTask(1, "old description", false);
            AddTaskToTestStorage(newTask);

            // when
            const string description = "new description";
            bool result = await _storage.UpdateTaskDescription(newTask.Id, "new description");

            // given
            Assert.IsTrue(result);
            var taskFromStorage = GetTaskWithId(newTask.Id);
            Assert.AreEqual(description, taskFromStorage.Description);
            Assert.AreEqual(newTask.HasCompleted, taskFromStorage.HasCompleted);
        }
        #endregion

        private IToDoTask CreateNewMockTask(long id, string description, bool hasCompleted)
        {
            var task = new Mock<IToDoTask>();
            task.SetupGet(t => t.Id).Returns(id);
            task.SetupGet(task => task.Description).Returns(description);
            task.SetupGet(task => task.HasCompleted).Returns(hasCompleted);
            return task.Object;
        }
    }
}
