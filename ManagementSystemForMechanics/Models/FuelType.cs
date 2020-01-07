using ManagementSystemForMechanics.Interfaces;
using System;

namespace ManagementSystemForMechanics.Models
{
    public class FuelType : IEntityModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modyfied { get; set; }

        public string Name { get; set; }


    }
}
