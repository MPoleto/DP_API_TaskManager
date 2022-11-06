using Microsoft.AspNetCore.Mvc;
using dp_api_taskManager.Service;
using dp_api_taskManager.Models;

namespace dp_api_taskManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskToDoController : ControllerBase
    {
        private readonly ITaskToDoService _service;

        public TaskToDoController(ITaskToDoService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var task = _service.GetById(id);

            if(task == null) return NotFound();

            return Ok(task);
        }

        [HttpGet("ObterTodos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var tasks = _service.GetAll();
            return Ok(tasks);
        }

        [HttpGet("ObterPorTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByTitle(string title)
        {
            var task = _service.GetByTitle(title);

            if(task.Count() == 0) return NotFound();

            return Ok(task);
        }

        [HttpGet("ObterPorData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByDate(DateTime date)
        {
            var task = _service.GetByDate(date);
            
            if(task.Count() == 0) return NotFound();

            return Ok(task);
        }

        [HttpGet("ObterPorStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByStatus(EnumStatusTask status)
        {
            var task = _service.GetByStatus(status);
            
            if(task == null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create(TaskToDo task)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (task.Date == null)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            var result = _service.Create(task);

            return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, TaskToDo updateTask)
        {
            var task = _service.GetById(id);

            if (task == null)
                return NotFound();
            
            if (!ModelState.IsValid)
                return BadRequest();

            if (task.Date == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            
            task.Title = updateTask.Title;
            task.Description = updateTask.Description;
            task.Date = updateTask.Date;
            task.Status = updateTask.Status;

            _service.Update(task);

            return Ok(task);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var task = _service.GetById(id);

            if (task == null)
                return NotFound();
                
            _service.Delete(task);

            return NoContent();
        }

    }
}