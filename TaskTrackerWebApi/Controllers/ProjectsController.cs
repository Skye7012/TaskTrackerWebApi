﻿using System;
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
        /// Gets all projects
        /// </summary>
        /// <returns>All projects</returns>
        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetProjects()
        {
            return  _context.Projects.ToList();
        }
        /// <summary>
        /// Gets project by Id
        /// </summary>
        /// <returns>Project by Id</returns>
        /// <response code="404">Project not found by typed Id</response>      
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
        /// <response code="200">Project updated</response>
        /// <response code="400">Typed wrong request</response>
        /// <response code="404">Project not found by typed Id</response>
        [ProducesResponseType(StatusCodes.Status200OK)] 
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
        }
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
        /// <response code="201">New Project created</response>
        /// <response code="400">Typed wrong request</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public  ActionResult<Project> PostProject(string name)
        {
            Project project = new Project();
            project.Name = name;
            _context.Projects.Add(project);
            try {_context.SaveChanges(); }
            catch { return BadRequest(project);}
            return CreatedAtAction("GetProject", new { id = project.Id }, project);
        }


        /// <summary>
        /// Deletes a project by Id
        /// </summary>
        /// <response code="200">Project deleted</response>
        /// <response code="400">Typed wrong request</response>
        /// <response code="404">Project not found by typed Id</response>
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            var project = _context.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
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
