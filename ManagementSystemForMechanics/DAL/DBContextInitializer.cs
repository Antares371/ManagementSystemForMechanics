using ManagementSystemForMechanics.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystemForMechanics.DAL
{
    internal class DBContextInitializer : DropCreateDatabaseIfModelChanges<DBContext>
    {

        protected override void Seed(DBContext context)
        {
            InitPositions(context);
            InitCarsMarks(context);
            InitAccounts(context);
            InitFuelsTypes(context);
            InitServices(context);
            InitVehicles(context);
            InitSystemLogs(context);
            context.SaveChanges();

            base.Seed(context);
        }

        private void InitSystemLogs(DBContext context)
        {
            List<SystemLog> logs = new List<SystemLog>();
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                logs.Add(new SystemLog() { Action = "", LogType = (LogType)r.Next(1, 8) });
            }
            context.SystemLogs.AddRange(logs);
        }

        private void InitServices(DBContext context)
        {
            List<Service> services = new List<Service>() {
                new Service(){Name="Service 1", Price = 100},
                new Service(){Name="Service 2", Price = 200},
                new Service(){Name="Service 3", Price = 450},
                new Service(){Name="Service 4", Price = 25},
                new Service(){Name="Service 5", Price = 500},
                new Service(){Name="Service 6", Price = 135},
                new Service(){Name="Service 7", Price = 70},
                new Service(){Name="Service 8", Price = 45},
                new Service(){Name="Service 9", Price = 10}
                };
            context.Services.AddRange(services);
            context.SaveChanges();

        }

        private void InitPositions(DBContext context)
        {
            List<Position> positions = new List<Position>()
            {
                new Position(){Name = "Young mechanic"},
                new Position(){Name = "Senior mechanic"},
                new Position(){Name = "Specjalist mechanic"},
                new Position(){Name = "Office employer"}
            };
            context.Positions.AddRange(positions);
            context.SaveChanges();
        }

        private void InitAccounts(DBContext context)
        {
            if (context.Accounts.Any())
            {
                return;
            }

            Account serverAccount = new Account() { Login = "Server", Password = "Pass", IsActive = true };
            serverAccount.User = new Mechanic() { Name = "ServerUser", Surname = "0", Email = "ServerUser@as.pl" };
            context.Accounts.Add(serverAccount);

            Account adminAccount = new Account() { Login = "admin", Password = "Pass", IsActive = true };
            adminAccount.User = new Mechanic() { Name = "Administrator", Surname = "1", Email = "Administrator@as.pl" };
            context.Accounts.Add(adminAccount);

            Account mechanicAccount1 = new Account() { Login = "mechanic1", Password = "Pass", IsActive = true };
            mechanicAccount1.User = new Mechanic() { Name = "User", Surname = "1", Email = "User1@as.pl" };
            context.Accounts.Add(mechanicAccount1);
            Account mechanicAccount2 = new Account() { Login = "mechanic2", Password = "Pass", IsActive = false };
            mechanicAccount2.User = new Mechanic() { Name = "User", Surname = "2", Email = "User2@as.pl" };
            context.Accounts.Add(mechanicAccount2);
            Account mechanicAccount3 = new Account() { Login = "mechanic3", Password = "Pass", IsActive = true };
            mechanicAccount3.User = new Mechanic() { Name = "User", Surname = "3", Email = "User3@as.pl" };
            context.Accounts.Add(mechanicAccount3);

            context.SaveChanges();
        }

        private void InitVehicles(DBContext context)
        {
            Random r = new Random();

            for (int i = 0; i < 200; i++)
            {
                Vehicle v1 = new Vehicle()
                {
                    Year = r.Next(1999, 2020),
                    VIN = $"JKLSAOIN{i * 5}{i * 2}AS{i + 2}UH{i}",
                    RegistrationNumber = $"CBY {i.ToString().PadLeft(5, '0')}",
                };
                int id = r.Next(1, 5);
                int ft = r.Next(1, 3);
                v1.Mark = context.VehiclesMarks.Where(p => p.Id == id).FirstOrDefault();
                v1.Model = v1.Mark.Models[r.Next(0, v1.Mark.Models.Count - 1)];
                v1.FuelType = context.FuelTypes.FirstOrDefault(t => t.Id == ft);
                int vsCount = r.Next(1, 10);
                for (int j = 0; j <= vsCount; j++)
                {
                    VehicleService vs = new VehicleService();
                    int sId = r.Next(1, 9);
                    Service service = context.Services.FirstOrDefault(s => s.Id == sId);
                    if (!vs.Services.Contains(service))
                        vs.Services.Add(service);
                    v1.Services.Add(vs);

                }
                context.Vehicles.Add(v1);
                if (i % 100 == 0)
                    context.SaveChanges();
            }
            context.SaveChanges();
        }

        private void InitCarsMarks(DBContext context)
        {
            List<VehicleMark> defaultVehiclesMarks = new List<VehicleMark>();

            VehicleMark opelMark = new VehicleMark() { Name = "Opel" };
            opelMark.Models.Add(new VehicleModel("Corsa"));
            opelMark.Models.Add(new VehicleModel("Astra"));
            opelMark.Models.Add(new VehicleModel("Insignia"));

            VehicleMark fordMark = new VehicleMark() { Name = "Ford" };
            fordMark.Models.Add(new VehicleModel("Mondeo"));
            fordMark.Models.Add(new VehicleModel("Focus"));

            VehicleMark mazdaMark = new VehicleMark() { Name = "Mazda" };
            mazdaMark.Models.Add(new VehicleModel("3"));
            mazdaMark.Models.Add(new VehicleModel("6"));
            mazdaMark.Models.Add(new VehicleModel("323"));

            VehicleMark volvoMark = new VehicleMark() { Name = "Volvo" };
            volvoMark.Models.Add(new VehicleModel("V40"));
            volvoMark.Models.Add(new VehicleModel("V60"));
            volvoMark.Models.Add(new VehicleModel("V90"));
            volvoMark.Models.Add(new VehicleModel("XC40"));
            volvoMark.Models.Add(new VehicleModel("XC60"));
            volvoMark.Models.Add(new VehicleModel("XC90"));

            VehicleMark audiMark = new VehicleMark() { Name = "Audi" };
            audiMark.Models.Add(new VehicleModel("x5"));
            audiMark.Models.Add(new VehicleModel("A1"));
            audiMark.Models.Add(new VehicleModel("A2"));
            audiMark.Models.Add(new VehicleModel("A5"));



            defaultVehiclesMarks.Add(opelMark);
            defaultVehiclesMarks.Add(fordMark);
            defaultVehiclesMarks.Add(mazdaMark);
            defaultVehiclesMarks.Add(audiMark);
            defaultVehiclesMarks.Add(volvoMark);

            context.VehiclesMarks.AddRange(defaultVehiclesMarks);
            context.SaveChanges();

        }

        private void InitFuelsTypes(DBContext context)
        {
            IList<FuelType> defaultFuelsTypes = new List<FuelType>()
            {
                new FuelType() { Name = "Diesel" },
                new FuelType() { Name = "Benzyna" },
                new FuelType() { Name = "Electric" }
            };

            context.FuelTypes.AddRange(defaultFuelsTypes);
            context.SaveChanges();
        }
    }
}
