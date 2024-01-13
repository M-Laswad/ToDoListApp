namespace ToDoListApp2.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsDone { get; set; }

        public string TodoUserId { get; set; } = string.Empty;
        public TodoUser? User { get; set; }
    }
}
