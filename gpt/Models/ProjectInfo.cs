using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSUrenRegistratie2.Models
{
    public class ProjectInfo
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Manager { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public ProjectInfo(int projectId, string projectName, string manager, DateTime start, DateTime end, string status)
        {
            ProjectID = projectId;
            ProjectName = projectName;
            Manager = manager;
            StartDate = start;
            EndDate = end;
            Status = status;
        }
    }
}
