using System;
using System.Collections.Generic;
using TaskTrackerWebApi.Controllers;

#nullable disable

namespace TaskTrackerWebApi.Models
{
    public partial class Project
    {
        public enum ProjectStatus {NotStarted,Active,Completed}
        public Project()
        {
            Tasks = new HashSet<Task>();
        }
        public Project(string name, DateTime? startDate, DateTime? completionDate, ProjectStatus status, int? priority)
        {
            Tasks = new HashSet<Task>();
            Name = name;
            StartDate = startDate;
            CompletionDate = completionDate;
            Status = status.ToString();
            Priority = priority;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; }
        public int? Priority { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Task> Tasks { get; set; }

    }
}
