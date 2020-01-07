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


        public virtual VehicleBodyTypeEnum BodyType { get; set; }
        public string Color { get; set; }
        public virtual FuelType FuelType { get; set; }
        public virtual VehicleMark Mark { get; set; }
        public virtual VehicleModel Model { get; set; }
        public double? MotorCapacity { get; set; }
        public double? Power { get; set; }
        public string RegistrationNumber { get; set; }
        public virtual List<VehicleService> Services { get; set; }
        public string Version { get; set; }
        public string VIN { get; set; }
        public int? Year { get; set; }


        public Vehicle()
        {
            Services = new List<VehicleService>();
        }

        public static DateTime GetLastModificationDate(Vehicle vehicle)
        {
            DateTime dateTime = DateTime.MinValue;
            if (vehicle.Modyfied > dateTime)
                dateTime = vehicle.Modyfied;
            foreach (var item in vehicle.Services)
            {
                if (item.Modyfied > dateTime)
                    dateTime = item.Modyfied;
            }

            return dateTime;
        }
        public override string ToString()
        {
            return $"{VIN} {Mark?.Name} {Model?.Name} {Year?.ToString()} {FuelType?.Name} {MotorCapacity?.ToString("0.0")}";
        }
    }


}
