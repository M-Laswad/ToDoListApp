using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApp2.Models;

namespace ToDoListApp2.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly UserManager<TodoUser> _userManager;
        private readonly TodoContext _context;

        public TodoController(UserManager<TodoUser> userManager, TodoContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var todos = await _context.Todos
                .Where(t => t.TodoUserId == user.Id)
                .ToListAsync();

            return View(todos);
        }

        public async Task<IActionResult> Search(string searchString)
        {
            var user = await _userManager.GetUserAsync(User);
            var todos = await _context.Todos
                .Where(t => t.TodoUserId == user.Id && t.Title.Contains(searchString))
                .ToListAsync();

            return View(todos);
        }

        public async Task<IActionResult> Completed(bool showCompleted = false)
        {
            var user = await _userManager.GetUserAsync(User);
            var todos = await _context.Todos
                .Where(t => t.TodoUserId == user.Id && (showCompleted ? true : t.IsDone))
                .ToListAsync();

            return View(todos);
        }
        
        public async Task<IActionResult> NonCompleted(bool showCompleted = false)
        {
            var user = await _userManager.GetUserAsync(User);
            var todos = await _context.Todos
                .Where(t => t.TodoUserId == user.Id && (showCompleted ? true : !t.IsDone))
                .ToListAsync();

            return View(todos);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Todo todo)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                todo.TodoUserId = user.Id;

                todo.IsDone = false;

                _context.Add(todo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(todo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    todo.TodoUserId = user.Id;
                    
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(todo);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}
