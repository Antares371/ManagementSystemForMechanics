using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForMechanics.Models
{
    public enum Position
    {
        Intern = 1,
        Young = 2,
        Specjalist = 3,
        Professional = 4
    }

    public class Mechanic : Person
    {
        public Position Position { get; set; }
    }
}
