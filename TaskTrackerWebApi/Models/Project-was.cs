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
        public Project(string name)
        {
            Tasks = new HashSet<Task>();
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        [System.Text.Json.Serialization.JsonIgnore] 
        public virtual ICollection<Task> Tasks { get; set; }
    }
}