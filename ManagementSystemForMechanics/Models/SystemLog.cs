using ManagementSystemForMechanics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForMechanics.Models
{
    public class SystemLog : IEntityModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modyfied { get; set; }

        public LogType LogType { get; set; }
        public string Action { get; set; }
        public static Account Account { get; set; }
        public DateTime Date { get; protected set; }
    }

    public enum LogType
    {
        Add = 1,
        Modify = 2,
        Remove = 3,
        Login = 4,
        Logout = 5,
        Error = 6,
        Warning = 7,
        Info = 8,

    }
}
