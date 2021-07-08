﻿using System;
using System.Collections.Generic;

#nullable disable

namespace TaskTrackerWebApi.Models
{
    public partial class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; }
        public int? Priority { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
