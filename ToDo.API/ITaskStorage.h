#pragma once
#include <functional>
#include <vector>

#include "IToDoTask.h"

namespace ToDo::API {
class ITaskStorage {
public:
  virtual std::vector<std::reference_wrapper<IToDoTask>> &GetAllTasks() const = 0;
  virtual IToDoTask &AddNewTask(
      long taskId, std::string desciption,
      std::function<IToDoTask &(long, std::string, bool)> taskCreator) = 0;
  virtual bool UpdateTaskDescription(long taskId,
                                     std::string newDescription) = 0;
  virtual bool UpdateTaskHasCompleted(long taskId, bool newHasCompleted) = 0;
  virtual long GetTotalNumberOfTasks() const = 0;
};
} // namespace ToDo::API
