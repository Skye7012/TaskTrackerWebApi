using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTrackerWebApi.Models;

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
        /// Get all projects
        /// </summary>
        /// <returns>All projects</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetProjects()
        {
            return  _context.Projects.ToList();
        }
        /// <summary>
        /// Get project by Id
        /// </summary>
        /// <returns>Project with entered Id</returns>
        /// <response code="404">If entered wrong Id</response>      
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Project> GetProject(int id)
        {
            var project =  _context.Projects.Find(id);

            if (project == null)
            {
                return NotFound();
            }
            return project;
        }

        /// <summary>
        /// Updates a project by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 1,
        ///        "name": "FirstProject"
        ///     }
        ///
        /// </remarks>
        /// <returns>A updated Project</returns>
        /// <response code="201">Returns the updated item</response>
        /// <response code="400">If typed wrong Id</response>
        /// <response code="404">If typed Id not found</response>
        [ProducesResponseType(StatusCodes.Status201Created)] //TODO: change
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public IActionResult PutProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }
            _context.Entry(project).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

           // return NoContent();
        }
        //public IActionResult PutProject(int id, Project project)
        //{
        //    if (id != project.Id)
        //    {
        //        return BadRequest();
        //    }
        //    _context.Entry(project).State = EntityState.Modified;
        //    var UpdEntity = _context.Projects.Find(id);
        //    if (UpdEntity == null)
        //        return NotFound();
        //    else
        //        try
        //        {
        //            _context.SaveChanges();
        //            return Ok();
        //        }
        //        catch { return BadRequest(); }


        //    return NoContent();
        //}

        /// <summary>
        /// Creates a project 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "id": 1,
        ///        "name": "FirstProject"
        ///     }
        ///
        /// </remarks>
        /// <returns>A newly created Project</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public  ActionResult<Project> PostProject(Project project)
        {
            _context.Projects.Add(project);
            try { _context.SaveChanges(); }
            catch { return BadRequest(project);}
            return CreatedAtAction("GetProject", new { id = project.Id }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
