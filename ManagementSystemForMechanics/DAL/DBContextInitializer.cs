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
            if (context.Accounts.Any())
            {
                return;   // DB has been seeded
            }

            Account serverAccount = new Account() { Login = "Server", Password = "Pass", IsActive = true };
            context.Accounts.Add(serverAccount);

            Account adminAccount = new Account() { Login = "admin", Password = "Pass", IsActive = true };
            context.Accounts.Add(adminAccount);
            Account mecanicAccount1 = new Account() { Login = "mecanic", Password = "Pass", IsActive = true };
            context.Accounts.Add(mecanicAccount1);
            Account mecanicAccount2 = new Account() { Login = "mecanic", Password = "Pass", IsActive = true };
            context.Accounts.Add(mecanicAccount2);
            Account mecanicAccount3 = new Account() { Login = "mecanic", Password = "Pass", IsActive = true };
            context.Accounts.Add(mecanicAccount3);

            context.SaveChanges();

            InitCarsMarks(context);
            InitFuelsTypes(context);
            context.SaveChanges();

            base.Seed(context);
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

            VehicleMark volvoMark = new VehicleMark() { Name = "Mazda" };
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
        }

        private void InitFuelsTypes(DBContext context)
        {
            IList<MotorType> defaultFuelsTypes = new List<MotorType>()
            {
                new MotorType() { Name = "Diesel" },
                new MotorType() { Name = "Benzyna" },
                new MotorType() { Name = "Electric" }
            };

            context.FuelTypes.AddRange(defaultFuelsTypes);

        }
    }
}
