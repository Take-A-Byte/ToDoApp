#pragma once
#include <string>

namespace ToDo::API {
class IToDoTask {
public:
  virtual long GetId() const = 0;
  virtual std::string GetDescription() const = 0;
  virtual void SetDescription(std::string newDescription) = 0;
  virtual bool GetHasCompleted() const = 0;
  virtual void SetHasCompleted(bool newHasCompleted) = 0;
};
} // namespace ToDo::API
