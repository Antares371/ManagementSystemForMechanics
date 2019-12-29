using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForMechanics.Models
{
    internal class Customer : Person
    {

        public List<Vehicle> Vehicels { get; set; }

    }
}
