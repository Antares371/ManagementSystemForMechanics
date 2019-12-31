using ManagementSystemForMechanics.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped]

        public string DisplayName { get { return ToString(); } }

        public virtual VehicleMark Mark { get; set; }
        public virtual VehicleModel Model { get; set; }
        public int? Year { get; set; }
        public MotorType FuelType { get; set; }
        public string RegistrationNumber { get; set; }
        public string VIN { get; set; }
        public string Color { get; set; }
        public int? Power { get; set; }
        public double? MotorCapacity { get; set; }
        public string Version { get; set; }
        public virtual VehicleBodyTypeEnum VehicleBodyType { get; set; }

        public virtual List<VehicleService> Services { get; set; }

        public Vehicle()
        {
            Services = new List<VehicleService>();
        }
        public override string ToString()
        {
            return $"{VIN} {Mark?.Name} {Model?.Name} {Year?.ToString()} {FuelType?.Name} {MotorCapacity?.ToString("0.0")}";
        }
    }


}
