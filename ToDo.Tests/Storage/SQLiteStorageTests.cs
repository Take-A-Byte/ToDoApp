using NUnit.Framework;
using Moq;
using System.IO;
using ToDo.API;
using Microsoft.Data.Sqlite;
using ToDo.Storage;

namespace ToDo.Tests.Storage
{
    public class SQLiteStorageTests : StorageTestsBase
    {
        private const string _kDatabasePath = "testDatabase.db";

        [SetUp]
        public void SetUp()
        {
            _storage = new SQLiteStorage(_kDatabasePath);
        }

        [TearDown]
        public void CleanUp()
        {
            ((SQLiteStorage)_storage).Dispose();
            File.Delete(_kDatabasePath);
        }

        [TestCase]
        public override void OnObjectCreation_StorageFileExists()
        {
            // when
            // storage object is created in setup

            // then
            Assert.IsTrue(File.Exists(_kDatabasePath));
        }

        protected override void AddTaskToTestStorage(IToDoTask task)
        {
            using (var connection = new SqliteConnection($"Data Source={_kDatabasePath}"))
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
            using (var connection = new SqliteConnection($"Data Source={_kDatabasePath}"))
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
            using (var connection = new SqliteConnection($"Data Source={_kDatabasePath}"))
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