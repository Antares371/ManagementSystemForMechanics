namespace ManagementSystemForMechanics.Models
{
    public enum VehicleBodyTypeEnum
    {
        Sedan = 1,
        Combi = 2,
        Cabrio = 3,
        Minivan = 4,
        SUV = 5,
        Compact = 6,
        Smart = 7,
        Van = 8
    }

    public class VehicleBodyType
    {
        private VehicleBodyTypeEnum Type { get; set; }
        private VehicleBodyType()
        {

        }

        public override string ToString()
        {
            string typeName = string.Empty;
            switch (Type)
            {
                case VehicleBodyTypeEnum.Sedan:
                    typeName = "Sedan";
                    break;
                case VehicleBodyTypeEnum.Combi:
                    typeName = "Combi";
                    break;
                case VehicleBodyTypeEnum.Cabrio:
                    typeName = "Cabrio";
                    break;
                case VehicleBodyTypeEnum.Minivan:
                    typeName = "Minivan";
                    break;
                case VehicleBodyTypeEnum.SUV:
                    typeName = "SUV";
                    break;
                case VehicleBodyTypeEnum.Compact:
                    typeName = "Compact";
                    break;
                case VehicleBodyTypeEnum.Smart:
                    typeName = "Smart";
                    break;
                case VehicleBodyTypeEnum.Van:
                    typeName = "Van";
                    break;
                default:
                    typeName = "";
                    break;
            }
            return typeName;
        }

    }
}
