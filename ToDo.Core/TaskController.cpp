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

std::optional<std::reference_wrapper<API::IToDoTask>>
TaskController::AddTask(std::string description) {
  if (description.empty()) {
    return std::nullopt;
  }

  auto getNewAddedTask =
      std::async(std::launch::async, [&]() -> API::IToDoTask & {
        if (!_newIdForUse) {
          _newIdForUse = _taskStorage->GetTotalNumberOfTasks();
        }

        return _taskStorage->AddNewTask((*_newIdForUse)++, description,
                                        CreateTask);
      });

  return std::optional<std::reference_wrapper<API::IToDoTask>>{
      getNewAddedTask.get()};
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

std::vector<std::reference_wrapper<API::IToDoTask>>
TaskController::GetAlltasks() {
  auto getAllTasks = std::async(
      std::launch::async,
      [&]() -> std::vector<std::reference_wrapper<API::IToDoTask>> {
        auto tasks = _taskStorage->GetAllTasks();
        for (auto const &refTask : tasks) {
          ToDoTask &task =
              (ToDoTask &)(std::unwrap_reference_t<API::IToDoTask &>(refTask));
          task.RegisterForPropertyChanged(
              [&](ToDoTask &sender, TaskAttribute changedAttribute) {
                OnTaskAttributesChanged(sender, changedAttribute);
              });
        }

        return std::vector<std::reference_wrapper<API::IToDoTask>>();
      });

  return getAllTasks.get();
}
} // namespace ToDo::Core
