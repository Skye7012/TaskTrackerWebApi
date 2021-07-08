﻿using System;
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
        [HttpGet("~/{id}")]
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
        /// Gets Tasks that are attached to a Project
        /// </summary>
        /// <param name="id">Id of a Project</param>>
        /// <returns>Gets Tasks that are attached to a Project</returns>
        /// <response code="404">Project not found by typed Id</response> 
        /// <response code="200">Got Tasks that are attached to a Project</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("~/GetProjectTasks/{id}")]
        public ActionResult<IEnumerable<Task>> GetProjectTasks(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            var tasks = _context.Tasks.Where(x => x.ProjectId == id);
            return Ok(tasks);
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
        ///        "Status": "ToDo" OR "InProgress" OR "Done", (You may set only one of this three values)
        ///        "Description": "FirstProject description",
        ///        "Priority": 1,
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
        [HttpPut]
        public  IActionResult PutTask(Task task)
        {
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

        /// <summary>
        /// Creates a Task 
        /// </summary>
        /// <param name="name">Name of the task</param>>
        /// <param name="status">May set only 3 values: "ToDo" OR "InProgress" OR "Done"</param>>
        /// <param name="description">Description of the task</param>>
        /// <param name="priority">The lower the number, the more significant the project.
        /// Priotiry cannot be zero. Example: 12 </param>>
        /// <param name="projectId">Id of Project that will keep new Task</param>>
        /// <returns>A newly created Task</returns>
        /// <response code="201">New Task created</response>
        /// <response code="400">Typed wrong request</response>
        /// <response code="404">Project not found by typed Id</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{name}/{status}/{description}/{projectId}")]
        public  ActionResult<Task> PostTask(string name, string status, string description, int? priority, int projectId)
        {
            if (_context.Projects.Find(projectId) is null)
                return NotFound();
            Task task = new Task(name,status, description, priority,projectId);
            try 
            {
                _context.Tasks.Add(task);
                _context.SaveChanges(); 
            }
            catch { return BadRequest(task); }
            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        /// <summary>
        /// Deletes a Task by Id
        /// </summary>
        /// <param name="id">The Id of the Task to be deleted</param>>
        /// <response code="200">Task deleted</response>
        /// <response code="400">Typed wrong request</response>
        /// <response code="404">Task not found by typed Id</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task =  _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            _context.Tasks.Remove(task);
            try { _context.SaveChanges(); return Ok(); }
            catch { return BadRequest(); }
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
