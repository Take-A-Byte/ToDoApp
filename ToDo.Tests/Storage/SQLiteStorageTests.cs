using Moq;
using System.IO;
using ToDo.API;
using Microsoft.Data.Sqlite;
using ToDo.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Storage;

namespace ToDo.Tests.Storage
{
    [TestClass]
    public class SQLiteStorageTests : StorageTestsBase
    {
        private string DatabasePath
        {
            get => Path.Combine(ApplicationData.Current.LocalFolder.Path, "testDatabase.db");
        }

        [TestInitialize]
        public void SetUp()
        {
            _storage = new SQLiteStorage(DatabasePath);
        }

        [TestCleanup]
        public void CleanUp()
        {
            (_storage as SQLiteStorage)?.Dispose();
            File.Delete(DatabasePath);
        }

        [TestMethod]
        public override void OnObjectCreation_StorageFileExists()
        {
            // when
            // storage object is created in setup

            // then
            Assert.IsTrue(File.Exists(DatabasePath));
        }

        protected override void AddTaskToTestStorage(IToDoTask task)
        {
            using (var connection = new SqliteConnection($"Data Source={DatabasePath}"))
            {
                connection.Open();

                var insertTaskCommand = connection.CreateCommand();
                insertTaskCommand.CommandText =
                    @"
                        INSERT INTO Tasks
                        VALUES ($id, $description, $hasCompleted)
                    ";
                insertTaskCommand.Parameters.AddWithValue("$id", task.Id);
                insertTaskCommand.Parameters.AddWithValue("$description", $"{task.Description}");
                insertTaskCommand.Parameters.AddWithValue("$hasCompleted", task.HasCompleted);
                insertTaskCommand.ExecuteNonQuery();
            }
        }

        protected override int NumberOfTasksInStorage()
        {
            using (var connection = new SqliteConnection($"Data Source={DatabasePath}"))
            {
                connection.Open();

                var selectAllCommand = connection.CreateCommand();
                selectAllCommand.CommandText =
                    @"
                        SELECT COUNT(*) FROM Tasks
                    ";
                using (var reader = selectAllCommand.ExecuteReader())
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
        }

        protected override IToDoTask GetTaskWithId(long id)
        {
            using (var connection = new SqliteConnection($"Data Source={DatabasePath}"))
            {
                connection.Open();

                var getTaskWithIdCommand = connection.CreateCommand();
                getTaskWithIdCommand.CommandText =
                    @"
                        SELECT * FROM Tasks
                        WHERE id=$id
                    ";
                getTaskWithIdCommand.Parameters.AddWithValue("$id", id);
                using (var reader = getTaskWithIdCommand.ExecuteReader())
                {
                    var mockTask = new Mock<IToDoTask>();
                    reader.Read();
                    mockTask.SetupGet(task => task.Description).Returns(reader.GetString(1));
                    mockTask.SetupGet(task => task.HasCompleted).Returns(reader.GetBoolean(2));
                    return mockTask.Object;
                }
            }
        }
    }
}