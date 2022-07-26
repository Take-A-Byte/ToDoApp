#include "pch.h"
#include "ToDoTask.h"

namespace ToDo::Core {
ToDoTask::ToDoTask(long id, std::string description, bool hasCompleted) {
  _id = id;
  _description = description;
  _hasCompleted = hasCompleted;
}

long ToDoTask::GetId() const { return _id; }

std::string ToDoTask::GetDescription() const { return _description; }

bool ToDoTask::GetHasCompleted() const { return _hasCompleted; }

void ToDoTask::SetDescription(std::string newDescription) {
  _description = newDescription;
  if (_propertyObserver) {
    _propertyObserver(*this, TaskAttribute::Description);
  }
}

void ToDoTask::SetHasCompleted(bool newHasCompleted) {
  _hasCompleted = newHasCompleted;
  if (_propertyObserver) {
    _propertyObserver(*this, TaskAttribute::HasCompleted);
  }
}

void ToDoTask::RegisterForPropertyChanged(
    std::function<void(ToDoTask &sender, TaskAttribute changedAttribute)>
        propertyObserver) {
  _propertyObserver = propertyObserver;
}
} // namespace ToDo::Core
