using ManagementSystemForMechanics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForMechanics.Models
{
    public class Vehicle : IEntityModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modyfied { get; set; }

        public VehicleMark Mark { get; set; }
        public VehicleModel Model { get; set; }
        public uint Year { get; set; }
        public MotorType FuelType { get; set; }
        public string RegistrationNumber { get; set; }
        public string VIN { get; set; }
        public string Color { get; set; }
        public int Power { get; set; }
        public double MotorCapacity { get; set; }
        public string Version { get; set; }
        public VehicleBodyTypeEnum VehicleBodyType { get; set; }

        public List<VehicleService> Services { get; set; }

    }


}
