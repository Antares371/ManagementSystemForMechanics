﻿using ManagementSystemForMechanics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForMechanics.Models
{
    public class Position : IEntityModel
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modyfied { get; set; }
        public string Name { get; set; }

    }

    public class Mechanic : Person
    {
        public virtual Position Position { get; set; }
    }
}
