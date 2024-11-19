using Microsoft.AspNetCore.Mvc;
using ToDo.Models;
using ToDo.Service;

namespace ToDo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        [HttpGet]
        public IActionResult GetToDos()
        {
            var toDos = _toDoService.GetToDos();
            if (toDos.Count() == 0) return Ok(new List<ToDoItem>());
            return Ok(toDos);
        }

        [HttpPost]
        public IActionResult AddToDo([FromBody] ToDoItem newToDo)
        {
            try
            {
                var toDo = _toDoService.Add(newToDo);
                return Ok(toDo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult EditToDo([FromBody] ToDoItem updatedToDo)
        {
            try
            {
                var toDo = _toDoService.Edit(updatedToDo);
                return Ok(toDo);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{name}")]
        public IActionResult DeleteToDo(string name)
        {
            try
            {
                _toDoService.Delete(name);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
