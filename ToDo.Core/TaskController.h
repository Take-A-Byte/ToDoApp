#pragma once
#include <functional>
#include <optional>

#include "ITaskStorage.h"

namespace ToDo::Core {
class ToDoTask;
enum class TaskAttribute;

class TaskController {
public:
  TaskController(API::ITaskStorage &taskStorage);
  void AddTask(
      std::string description,
      std::function<void(std::optional<std::reference_wrapper<API::IToDoTask>>)> callback);
  void GetAlltasks(std::function<void(std::vector<std::reference_wrapper<API::IToDoTask>>)> callback);

private:
  API::ITaskStorage *_taskStorage;
  std::optional<long> _newIdForUse;

  void OnTaskAttributesChanged(ToDoTask &sender,
                               TaskAttribute changedAttribute);
};
} // namespace ToDo::Core
