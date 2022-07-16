using NUnit.Framework;
using ToDo.API;

namespace ToDo.Tests.Storage
{
    public abstract class StorageTestsBase
    {
        protected ITaskStorage? _storage;

        [Test]
        public abstract void OnObjectCreation_StorageFileExists();

        #region AddTask
        public void OnAddNewTaskWithNullOrEmptyDescription_ReturnsFalse_NoTaskIsSavedInStorage()
        {
            Assert.Fail();
        }

        [Test]
        public void OnAddNewTask_ReturnsTrue_TaskIsSavedInStorage()
        {
            Assert.Fail();
        }
        #endregion

        #region GetAllTasks
        [Test]
        public void OnGetAllTasks_ReturnsTrue_RetrivesAllTasksInStorageWithCorrectDetails()
        {
            Assert.Fail();
        }
        #endregion

        #region UpdateTaskCompleteness
        [Test]
        public void OnUpdateTaskCompletenessWithIncorrectID_ReturnsFalse_DoesNotUpdateStorage()
        {
            Assert.Fail();
        }

        [Test]
        public void OnUpdateTaskCompletenessWithCorrectID_ReturnsTrue_UpdatesTaskInStorage()
        {
            Assert.Fail();
        }
        #endregion

        #region UpdateDescription
        [Test]
        public void OnUpdateTaskDescriptionWithIncorrectID_ReturnsFalse_DoesNotUpdateStorage()
        {
            Assert.Fail();
        }

        public void OnUpdateTaskDescriptionWithIncorrectDescription_ReturnsFalse_DoesNotUpdateStorage()
        {
            Assert.Fail();
        }

        [Test]
        public void OnUpdateTaskDescriptionWithCorrectIDAndDescription_ReturnsTrue_UpdatesTaskInStorage()
        {
            Assert.Fail();
        }
        #endregion
    }

}
