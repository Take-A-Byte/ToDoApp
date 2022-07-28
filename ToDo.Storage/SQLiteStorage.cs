using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ToDo.API;

namespace ToDo.Storage
{
    public class SQLiteStorage : ITaskStorage, IDisposable
    {
        private SqliteConnection _sqLiteConnection;

        /// <summary>
        /// Create a SQlite based storage object
        /// </summary>
        /// <param name="databasePath">Path at which database file should be created</param>
        public SQLiteStorage(string databasePath)
        {
            _sqLiteConnection = new SqliteConnection($"Data Source={databasePath}");

            bool didFileExist = File.Exists(databasePath);
            if (!didFileExist)
            {
                File.Create(databasePath).Dispose();

                _sqLiteConnection.Open();

                var tableCreationCommand = _sqLiteConnection.CreateCommand();
                tableCreationCommand.CommandText =
                    @"
                        CREATE TABLE ""Tasks"" (
                        ""id"" INTEGER,
                        ""description"" TEXT,
                        ""hasCompleted"" INTEGER,
                        PRIMARY KEY(""id"")
                        )
                    ";
                tableCreationCommand.ExecuteNonQuery();
            }
            else
            {
                _sqLiteConnection.Open();
            }
        }

        public void Dispose()
        {
            _sqLiteConnection.Close();
            _sqLiteConnection.Dispose();
            _sqLiteConnection = null;
            SqliteConnection.ClearAllPools();
        }

        public async Task<IToDoTask> AddNewTask(long taskId, string description, Func<long, string, bool, IToDoTask> taskCreator)
        {
            bool hasCompletedForNewTask = false;
            var insertTaskCommand = _sqLiteConnection.CreateCommand();
            insertTaskCommand.CommandText =
                @"
                        INSERT INTO Tasks
                        VALUES ($id, $description, $hasCompleted)
                    ";
            insertTaskCommand.Parameters.AddWithValue("$id", taskId);
            insertTaskCommand.Parameters.AddWithValue("$description", $"{description}");
            insertTaskCommand.Parameters.AddWithValue("$hasCompleted", hasCompletedForNewTask);

            try
            {
                await insertTaskCommand.ExecuteNonQueryAsync();
            }
            catch
            {
                return null;
            }

            return taskCreator(taskId, description, hasCompletedForNewTask);
        }

        public async Task<IReadOnlyDictionary<long, IToDoTask>> GetAllTasks(Func<long, string, bool, IToDoTask> taskCreator)
        {
            var getAllCommand = _sqLiteConnection.CreateCommand();
            getAllCommand.CommandText =
                @"
                        SELECT * FROM Tasks
                    ";
            using (var reader = await getAllCommand.ExecuteReaderAsync())
            {
                Dictionary<long, IToDoTask> tasks = new Dictionary<long, IToDoTask>();
                while (reader.Read())
                {
                    long id = reader.GetInt64(0);
                    tasks.Add(id, taskCreator(id, reader.GetString(1), reader.GetBoolean(2)));
                }

                return tasks;
            }
        }

        public async Task<bool> UpdateTaskCompleteness(long taskId, bool completeness)
        {
            var updateTaskCompletionCommand = _sqLiteConnection.CreateCommand();
            updateTaskCompletionCommand.CommandText =
                @"
                        UPDATE Tasks
                        SET hasCompleted = $hasCompleted
                        WHERE id = $id
                    ";
            updateTaskCompletionCommand.Parameters.AddWithValue("$id", taskId);
            updateTaskCompletionCommand.Parameters.AddWithValue("$hasCompleted", completeness);

            bool taskCompletenessUpdated = false;
            try
            {
                taskCompletenessUpdated = await updateTaskCompletionCommand.ExecuteNonQueryAsync() == 1;
            }
            catch
            {
            }

            return taskCompletenessUpdated;
        }

        public async Task<bool> UpdateTaskDescription(long taskId, string description)
        {
            var updateTaskCompletionCommand = _sqLiteConnection.CreateCommand();
            updateTaskCompletionCommand.CommandText =
                @"
                        UPDATE Tasks
                        SET description = $description
                        WHERE id = $id
                    ";
            updateTaskCompletionCommand.Parameters.AddWithValue("$id", taskId);
            updateTaskCompletionCommand.Parameters.AddWithValue("$description", description);

            bool taskDescriptionupdated = false;
            try
            {
                taskDescriptionupdated = await updateTaskCompletionCommand.ExecuteNonQueryAsync() == 1;
            }
            catch
            {
            }

            return taskDescriptionupdated;
        }

        public async Task<long> GetTotalNumberOfTasks()
        {
            var selectAllCommand = _sqLiteConnection.CreateCommand();
            selectAllCommand.CommandText =
                @"
                        SELECT COUNT(*) FROM Tasks
                    ";
            using (var reader = await selectAllCommand.ExecuteReaderAsync())
            {
                reader.Read();
                return reader.GetInt32(0);
            }
        }

        public async Task<bool> DeleteTask(long id)
        {
            var deleteTaskCommand = _sqLiteConnection.CreateCommand();
            deleteTaskCommand.CommandText =
                @"
                        DELETE
                        FROM ""Tasks""
                        WHERE id = $id
                    ";
            deleteTaskCommand.Parameters.AddWithValue("$id", id);
            bool taskDeleted = false;
            try
            {
                taskDeleted = await deleteTaskCommand.ExecuteNonQueryAsync() == 1;
            }
            catch
            {
            }

            return taskDeleted;
        }
    }
}