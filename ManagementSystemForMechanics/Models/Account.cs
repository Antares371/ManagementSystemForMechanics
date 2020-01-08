using ManagementSystemForMechanics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForMechanics.Models
{
    public class Account : IEntityModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DisplayName { get { return $"{User?.Name} {User?.Surname}"; } }

        public DateTime Created { get; set; }

        public DateTime Modyfied { get; set; }

        public virtual Mechanic User { get; set; }

        public bool IsActive { get; set; }
        public bool IsLogged { get; set; }

        public virtual List<Permission> Permissions { get; set; }

        public Account(string login, string password) : this()
        {
            Login = login;
            Password = password;
        }
        public Account()
        {

        }
        public override string ToString()
        {
            return Login;
        }
    }

    public class Permission : IEntityModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modyfied { get; set; }
        public PermissionType Type { get; set; }
        public int PermissionSum { get; set; }
    }

    public enum PermissionType
    {
        Vehicle = 1,
        SettingsPanel = 4,
    }
}
