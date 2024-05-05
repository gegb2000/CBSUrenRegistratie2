using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSUrenRegistratie2.Models
{
    public class Break
    {
        public int BreakId { get; set; }
        public int HoursId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
