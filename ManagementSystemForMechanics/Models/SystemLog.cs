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

        public string Description { get; set; }
        public LogType LogType { get; set; }
        public static void Info(string message)
        {

        }
        public static void Error(string message)
        {

        }
        public static void Warning(string message)
        {

        }
    }

    public enum LogType
    {
        Info,
        Error,
        Warning,

    }
}
