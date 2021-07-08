using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerWebApi.Models;
using Task = TaskTrackerWebApi.Models.Task;

namespace TaskTrackerWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskTrackerContext _context;

        public TasksController(TaskTrackerContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets all Tasks
        /// </summary>
        /// <returns>All Tasks</returns>
        /// <response code="200">Got Tasks</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<IEnumerable<Task>> GetTasks()
        {
            return  Ok(_context.Tasks.ToList());
        }

        /// <summary>  
        /// Gets Task by Id
        /// </summary>
        /// <param name="id">Id of a Task</param>>
        /// <returns>Task by Id</returns>
        /// <response code="404">Task not found by typed Id</response> 
        /// <response code="200">Got Task</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Task> GetTask(int id)
        {
            var task =  _context.Tasks.Find(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }


        /// <summary>
        /// Updates a Task by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "Id": 1,
        ///        "Name": "FirstProject",
        ///        "Description": "Try to make your first Project",
        ///        "ProjectId": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="task">Modified Task entity</param>>
        /// <returns>Updated Task</returns>
        /// <response code="200">Task updated</response>
        /// <response code="400">Typed wrong request</response>
        /// <response code="404">Task not found by typed Id</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut/*("{id}")*/]
        public  IActionResult PutTask(/*int id, */Task task)
        {
            //if (id != task.Id)
            //{
            //    return BadRequest();
            //}
            if (!TaskExists(task.Id))
                return NotFound();
            else
            {
                _context.Entry(task).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                    return Ok();
                }
                catch {return BadRequest();}

            }
        }

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask(Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
