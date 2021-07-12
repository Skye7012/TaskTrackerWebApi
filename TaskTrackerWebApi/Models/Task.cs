using System;
using System.Collections.Generic;

#nullable disable

namespace TaskTrackerWebApi.Models
{
    public partial class Task
    {
        public enum TaskStatus { ToDo, InProgress, Done }
        public Task() { }
        public Task(string name, TaskStatus status, string description, int? priority, int projectId)
        {
            Name = name;
            Status = status.ToString();
            Description = description;
            Priority = priority;
            ProjectId = projectId;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }
        public int ProjectId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Project Project { get; set; }
    }
}
