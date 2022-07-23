#pragma once
#include <optional>
#include <functional>

#include "ITaskStorage.h"

namespace ToDo::Core {
class ToDoTask;
enum class TaskAttribute;

class TaskController {
public:
  TaskController(API::ITaskStorage &taskStorage);
  std::optional<std::reference_wrapper<API::IToDoTask>> AddTask(std::string description);
  std::vector<std::reference_wrapper<API::IToDoTask>> GetAlltasks();

private:
  API::ITaskStorage *_taskStorage;
  std::optional<long> _newIdForUse;

  void OnTaskAttributesChanged(ToDoTask &sender,
                               TaskAttribute changedAttribute);
};
} // namespace ToDo::Core
