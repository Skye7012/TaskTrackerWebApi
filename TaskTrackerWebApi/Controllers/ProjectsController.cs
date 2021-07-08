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
    public class ProjectsController : ControllerBase 
    {
        private readonly TaskTrackerContext _context;

        public ProjectsController(TaskTrackerContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets all Projects
        /// </summary>
        /// <returns>All Projects</returns>
        /// <response code="200">Got Projects</response>
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetProjects()
        {
            return  Ok(_context.Projects.ToList());
        }


        /// <summary>
        /// Gets Project by Id
        /// </summary>
        /// <param name="id">Id of a Project</param>>
        /// <returns>Project by Id</returns>
        /// <response code="404">Project not found by typed Id</response> 
        /// <response code="200">Got Project</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Project> GetProject(int id)
        {
            var project =  _context.Projects.Find(id);

            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }


        /// <summary>
        /// Updates a Project by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "Id": 1,
        ///        "Name": "FirstProject",
        ///        "StartDate": "2022-01-22"
        ///        "CompletionDate": "2022-01-22T18:57:38"
        ///        "Status": "NotStarted" OR "Active" OR "Completed",
        ///        "Priority": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="project">Modified Project entity</param>>
        /// <returns>Updated Project</returns>
        /// <response code="200">Project updated</response>
        /// <response code="400">Typed wrong request</response>
        /// <response code="404">Project not found by typed Id</response>
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut]
        public IActionResult PutProject(Project project)
        {
            if (!ProjectExists(project.Id))
                return NotFound();
            else
            {
                _context.Entry(project).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                    return Ok();
                }
                catch { return BadRequest(); }
            }
        }


        /// <summary>
        /// Creates a Project 
        /// </summary>
        /// <param name="name">Name of the Project</param>>
        /// <param name="startDate">Date when you started to carry out the project.
        /// Example: 2022-01-22 </param>>
        /// <param name="completionDate">Date when you completed the project.
        /// Example: 2022-01-22T18:57:38 </param>>
        /// <param name="status">May set only 3 values: "NotStarted" OR "Active" OR "Completed"</param>>
        /// <param name="priority">The lower the number, the more significant the project.
        /// Priotiry cannot be zero. Example: 12 </param>>
        /// <returns>A newly created Project</returns>
        /// <response code="201">New Project created</response>
        /// <response code="400">Typed wrong request</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[HttpPost("{name}/{startDate}/{completionDate}/{status}/{priority}")]
        [HttpPost("{name}/{status}")]
        public  ActionResult<Project> PostProject(string name, DateTime? startDate, DateTime? completionDate, string status, int? priority) 
        {
            //TODO: поменять везде try{}catch{} на такой шаблон?
            Project project = new Project(name, startDate, completionDate, status, priority);
            try 
            {
                _context.Projects.Add(project);
                _context.SaveChanges();
            }
            catch { return BadRequest(project);}
            return CreatedAtAction("GetProject", new { id = project.Id }, project);
        }


        /// <summary>
        /// Deletes a Project by Id
        /// </summary>
        /// <remarks>Warning: it's realize cascade delete,
        /// so Tasks that are kept by chosen Project will be deleted</remarks>
        /// <param name="id">The Id of the Project to be deleted to</param>
        /// <response code="200">Project deleted</response>
        /// <response code="400">Typed wrong request</response>
        /// <response code="404">Project not found by typed Id</response>
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            using (TaskTrackerContext db = new TaskTrackerContext())
            {
                var prs = db.Projects.ToList();
            }
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            if(project.Tasks.Count > 0)
            {
                foreach (var task in project.Tasks)
                    try { _context.Tasks.Remove(task); }
                    catch { return BadRequest(); }
            }
            _context.Projects.Remove(project);
            try { _context.SaveChanges(); return Ok(); }
            catch { return BadRequest(); }
        }


        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
