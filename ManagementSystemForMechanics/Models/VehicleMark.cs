using ManagementSystemForMechanics.Interfaces;
using System;
using System.Collections.Generic;

namespace ManagementSystemForMechanics.Models
{
    public class VehicleMark : IEntityModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modyfied { get; set; }

        public string Name { get; set; }
        public List<VehicleModel> Models { get; set; }

        public VehicleMark()
        {
            Models = new List<VehicleModel>();
        }

        public VehicleMark(string name) : this()
        {
            Name = name;
        }
    }
}
