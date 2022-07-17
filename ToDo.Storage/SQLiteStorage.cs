using Microsoft.Data.Sqlite;
using ToDo.API;

namespace ToDo.Storage
{
    public class SQLiteStorage : ITaskStorage, IDisposable
    {
        private readonly ITaskFactory _taskFactory;
        private SqliteConnection _sqLiteConnection;

        public SQLiteStorage(string databasePath, ITaskFactory taskFactory)
        {
            _sqLiteConnection = new SqliteConnection($"Data Source={databasePath}");
            _taskFactory = taskFactory;

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

        public async Task<bool> AddNewTask(long taskId, string description)
        {
            var insertTaskCommand = _sqLiteConnection.CreateCommand();
            insertTaskCommand.CommandText =
                @"
                        INSERT INTO Tasks
                        VALUES ($id, $description, $hasCompleted)
                    ";
            insertTaskCommand.Parameters.AddWithValue("$id", taskId);
            insertTaskCommand.Parameters.AddWithValue("$description", $"{description}");
            insertTaskCommand.Parameters.AddWithValue("$hasCompleted", false);

            bool taskAdded = false;
            try
            {
                taskAdded = await insertTaskCommand.ExecuteNonQueryAsync() == 1;
            } catch {
            }
            
            return taskAdded;
        }

        public async Task<IReadOnlyList<IToDoTask>> GetAllTasks()
        {
            var getAllCommand = _sqLiteConnection.CreateCommand();
            getAllCommand.CommandText =
                @"
                        SELECT * FROM Tasks
                    ";
            using (var reader = await getAllCommand.ExecuteReaderAsync())
            {
                List<IToDoTask> tasks = new List<IToDoTask>();
                while (reader.Read())
                {
                    tasks.Add(_taskFactory.CreateToDoTask(reader.GetInt64(0), reader.GetString(1), reader.GetBoolean(2)));
                }

                return tasks.AsReadOnly();
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
    }
}