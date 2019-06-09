using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TodoApi.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TodoApi.ViewModels;
using TodoApi.ViewModels.TodoViewModel;
using TodoApi.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers
{   
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : Controller
    {
        private readonly TodoRepository _repository;

        public TodoController(TodoRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public ResultViewModel Post([FromBody] EditorViewModel model)
        {
            model.Validate();

            if (model.Invalid)
            {
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível cadastrar o todo",
                    Data = model.Notifications
                };
            }

            var todo = new TodoItem();
            todo.Name = model.Name;
            todo.IsComplete = model.IsComplete;

            _repository.Save(todo);

            return new ResultViewModel
            {
                Success = true,
                Message = "Todo cadastrado com sucesso",
                Data = todo
            };
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _repository.Get();
        }

        [Route("{id}")]
        [HttpGet]
        public TodoItem GetById(long id)
        {
            return _repository.Get(id);
        }
    }
}