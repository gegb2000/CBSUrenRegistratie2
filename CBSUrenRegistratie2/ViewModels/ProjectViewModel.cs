using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBSUrenRegistratie2.Models;

namespace CBSUrenRegistratie2.ViewModels
{
    public class ProjectViewModel : ObservableObject
    {
        public ObservableCollection<ProjectInfo> Projects { get; set; }

        public ProjectViewModel()
        {
            Projects = new ObservableCollection<ProjectInfo>
        {
            // Sample data initialization
            new ProjectInfo(1, "Redesign Website", "John Doe", DateTime.Now, DateTime.Now.AddDays(100), "Active"),
            new ProjectInfo(2, "Upgrade Server Hardware", "Jane Smith", DateTime.Now, DateTime.Now.AddDays(60), "Planning")
        };
        }
    }
}
