using System;
using System.Collections.Generic;

#nullable disable

namespace TaskTrackerWebApi.Models
{
    public partial class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
