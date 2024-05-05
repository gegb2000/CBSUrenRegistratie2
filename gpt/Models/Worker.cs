using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSUrenRegistratie2.Models
{
    public class Worker
    {
        public int WorkerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal HourlyRate { get; set; }
    }
}
