using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FirstWebApi.Models;
using System.Linq;
namespace FirstWebApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
            AddDefaultTodos();
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(todo => todo.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {

            if (item == null) { return BadRequest(); }
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut]
        public IActionResult Update([FromBody] TodoItem item)
        {
            if (item == null  || item.Id < 1)
            {
                return BadRequest();
            }

            var todo = _context.TodoItems.FirstOrDefault(t=> t.Id == item.Id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return Ok();
        }

        // Private parts 
        private void AddDefaultTodos()
        {
            if (!_context.TodoItems.Any())
            {
                _context.TodoItems.Add(new TodoItem { Name = "Your first to do", IsComplete = false });
                _context.SaveChanges();
            }

        }
    }
}