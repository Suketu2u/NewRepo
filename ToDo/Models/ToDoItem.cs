using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class ToDoItem
    {
        private int priority;

        //public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        public int Priority { get => priority; set => priority = value; }
        public Status Status { get; set; }
    }
    public enum Status
    {
        NotStarted,
        InProgress,
        Completed
    }
}
