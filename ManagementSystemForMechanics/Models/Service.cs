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

        public virtual List<Service> Services { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        public decimal TotalPrice { get { return Services.Sum(s=> s.Price); } }
        public VehicleService()
        {
            Services = new List<Service>();
        }

    }

    public class Service : IEntityModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modyfied { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<VehicleService> VehicleServices { get;  set; }
    }
}
