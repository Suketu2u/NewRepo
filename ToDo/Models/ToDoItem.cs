using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class ToDoItem
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public int Priority { get;set; }
        public Status Status { get; set; }
        public Category Category { get; set; }
    }
    public enum Status
    {
        NotStarted,
        InProgress,
        Completed
    }
    public enum Category
    {
        Normal,
        Urgent
    }
}
