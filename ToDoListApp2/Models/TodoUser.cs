using Microsoft.AspNetCore.Identity;

namespace ToDoListApp2.Models
{
    public class TodoUser : IdentityUser
    {
        public ICollection<Todo> Todos { get; set; }
    }
}
