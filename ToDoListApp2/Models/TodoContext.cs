using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToDoListApp2.Models
{
    public class TodoContext : IdentityDbContext<TodoUser>
    {
        public DbSet<Todo> Todos { get; set; }
        public TodoContext(DbContextOptions<TodoContext> options) 
            : base(options) { }


    }
}
