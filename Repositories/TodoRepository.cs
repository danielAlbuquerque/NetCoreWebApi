using System.Collections.Generic;
using TodoApi.Data;
using TodoApi.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace TodoApi.Repositories
{
    public class TodoRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TodoItem> Get()
        {
            return _context.TodoItems.AsNoTracking().ToList();
        }

        public TodoItem Get(long id)
        {
            return _context.TodoItems.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
        }

        public void Save(TodoItem todo)
        {
            _context.TodoItems.Add(todo);
            _context.SaveChanges();
        }

        public void Update(TodoItem todo)
        {
            _context.Entry<TodoItem>(todo).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}