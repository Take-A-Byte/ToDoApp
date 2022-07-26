#include "pch.h"
#include <future>

#include "TaskController.h"
#include "ToDoTask.h"

namespace ToDo::Core {
API::IToDoTask &CreateTask(long id, std::string description,
                           bool hasCompleted) {
  return *(new ToDoTask(id, description, hasCompleted));
}

TaskController::TaskController(API::ITaskStorage &taskStorage) {
  _taskStorage = &taskStorage;
}

void TaskController::AddTask(
    std::string description,
    std::function<void(std::optional<std::reference_wrapper<API::IToDoTask>>)>
        callback) {
  std::async(std::launch::async, [&]() {
    if (description.empty()) {
      return callback(std::nullopt);
    }

    if (!_newIdForUse) {
      _newIdForUse = _taskStorage->GetTotalNumberOfTasks();
    }

    callback(std::optional<std::reference_wrapper<API::IToDoTask>>{
        _taskStorage->AddNewTask((*_newIdForUse)++, description, CreateTask)});
  });
}

void TaskController::OnTaskAttributesChanged(ToDoTask &sender,
                                             TaskAttribute changedAttribute) {
  switch (changedAttribute) {
  case TaskAttribute::Description:
    _taskStorage->UpdateTaskDescription(sender.GetId(),
                                        sender.GetDescription());
    break;

  case TaskAttribute::HasCompleted:
    _taskStorage->UpdateTaskHasCompleted(sender.GetId(),
                                         sender.GetHasCompleted());
    break;
  }
}

void TaskController::GetAlltasks(
    std::function<void(std::vector<std::reference_wrapper<API::IToDoTask>>)>
        callback) {
  std::async(std::launch::async, [&]() {
    auto tasks = _taskStorage->GetAllTasks();
    for (auto const &refTask : tasks) {
      ToDoTask &task =
          (ToDoTask &)(std::unwrap_reference_t<API::IToDoTask &>(refTask));
      task.RegisterForPropertyChanged(
          [&](ToDoTask &sender, TaskAttribute changedAttribute) {
            OnTaskAttributesChanged(sender, changedAttribute);
          });
    }

    callback(std::vector<std::reference_wrapper<API::IToDoTask>>());
  });
}
} // namespace ToDo::Core
