#include "pch.h"
#include "SQLiteStorage.h"

namespace ToDo::Storage {
SQLiteStorage::SQLiteStorage(std::string databasePath) {
  if(SQLITE_OK == sqlite3_open_v2(databasePath.c_str(), &_database,
                  SQLITE_OPEN_READWRITE | SQLITE_OPEN_CREATE |
                                       SQLITE_OPEN_FULLMUTEX,
                                   nullptr)) {

  }
}

SQLiteStorage::~SQLiteStorage() {
}

std::vector<std::reference_wrapper<API::IToDoTask>> &
SQLiteStorage::GetAllTasks() {
  return *(new std::vector<std::reference_wrapper<API::IToDoTask>>());
}

API::IToDoTask &SQLiteStorage::AddNewTask(
    long taskId, std::string desciption,
    std::function<API::IToDoTask &(long, std::string, bool)> taskCreator) {
  return taskCreator(1, "", true);
}

bool SQLiteStorage::UpdateTaskDescription(
    long taskId, std::string newDescription) {
  return false;
}

bool SQLiteStorage::UpdateTaskHasCompleted(
    long taskId, bool newHasCompleted) {
  return false;
}

long SQLiteStorage::GetTotalNumberOfTasks() const { return 0; }
} // namespace ToDo::Storage
