#pragma once
#include <string>
#include <winsqlite/winsqlite3.h>

#include "ITaskStorage.h"
#include "IToDoTask.h"

namespace ToDo::Storage {
class SQLiteStorage {
public:
  SQLiteStorage(std::string databasePath);
  virtual ~SQLiteStorage();

  // Inherited via ITaskStorage
  std::vector<std::reference_wrapper<API::IToDoTask>> &
  GetAllTasks();

  virtual API::IToDoTask &
  AddNewTask(long taskId, std::string desciption,
             std::function<API::IToDoTask &(long, std::string, bool)>
                 taskCreator);

  virtual bool UpdateTaskDescription(long taskId,
                                     std::string newDescription);

  virtual bool UpdateTaskHasCompleted(long taskId,
                                      bool newHasCompleted);

  virtual long GetTotalNumberOfTasks() const;

private:
  sqlite3 *_database;
};
} // namespace ToDo::Storage