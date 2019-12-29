using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForMechanics.Models
{
    internal class Customer : Person
    {

        public virtual List<Vehicle> Vehicels { get; set; }

    }
}
