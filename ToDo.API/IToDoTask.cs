namespace ToDo.API
{
    public interface IToDoTask
    {
        /// <summary>
        /// Unique number representing the task
        /// </summary>
        long Id { get; }

        /// <summary>
        /// Description of the task
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Boolean representing if the task has been completed or not
        /// </summary>
        bool HasCompleted { get; set; }
    }
}