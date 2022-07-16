namespace ToDo.API
{
    public interface IToDoTask
    {
        public long Id { get; }
        public string Description { get; set; }
        public bool HasCompleted { get; set; }
    }
}