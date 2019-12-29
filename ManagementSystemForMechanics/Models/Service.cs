using ManagementSystemForMechanics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForMechanics.Models
{
    public class VehicleService : IEntityModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modyfied { get; set; }
        public Vehicle Vehicle { get; set; }
        public Service Service { get; set; }
    }

    public class Service : IEntityModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modyfied { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
