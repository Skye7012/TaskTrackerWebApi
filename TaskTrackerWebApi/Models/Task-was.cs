using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TaskTrackerWebApi.Models
{
    public partial class Task
    {
        public Task(string name, string description, int projectId)
        {
            Name = name;
            Description = description;
            ProjectId = projectId;
        }
        //[Required]
        public int Id { get; set; }
       // [Required]
        public string Name { get; set; }
       // [Required]
        public string Description { get; set; }
        //[Required]
        public int ProjectId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Project Project { get; set; }
    }
}
