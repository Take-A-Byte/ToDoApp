#pragma once
#include "IToDoTask.h"
#include <functional>
#include <string>

namespace ToDo::Core {
enum class TaskAttribute { Description, HasCompleted };

class ToDoTask : public API::IToDoTask {
public:
  ToDoTask(long id, std::string description, bool hasCompleted = false);

  // Inherited via IToDoTask
  virtual long GetId() const override;
  virtual std::string GetDescription() const override;
  virtual void SetDescription(std::string newDescription) override;
  virtual bool GetHasCompleted() const override;
  virtual void SetHasCompleted(bool newHasCompleted) override;
  void RegisterForPropertyChanged(
      std::function<void(ToDoTask &sender, TaskAttribute changedAttribute)>
          propertyObserver);

private:
  long _id;
  std::string _description;
  bool _hasCompleted;
  std::function<void(ToDoTask &, TaskAttribute changedAttribute)>
      _propertyObserver;
};
} // namespace ToDo::Core