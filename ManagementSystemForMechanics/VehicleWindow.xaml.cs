using ManagementSystemForMechanics.DAL;
using ManagementSystemForMechanics.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManagementSystemForMechanics
{
    /// <summary>
    /// Interaction logic for VehicleWindow.xaml
    /// </summary>
    public partial class VehicleWindow : Window
    {

        private DBContext context;
        public List<VehicleMark> VehicleMarks { get; set; }
        public List<VehicleModel> VehicleModels { get; set; }

        public List<FuelType> FuelTypes { get; set; }
        public Vehicle Vehicle { get; set; }
        public string Message { get; set; }
        public string VehicleMotorCapacity { get; set; }
        public string VehiclePower { get; set; }
        public string VehicleVersion { get; set; }
        public VehicleMark VehicleMark { get; set; }
        public VehicleModel VehicleModel { get; set; }
        public FuelType VehicleFuelType { get; set; }
        public ObservableCollection<VehicleService> VehicleServicesList { get; } = new ObservableCollection<VehicleService>();
        public string VehicleYear { get; set; }
        public string VehicleVIN { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string VehicleColor { get; set; }

        public event EventHandler VehicleUpdated;


        public VehicleWindow()
        {
            context = new DBContext();
            InitializeComponent();
            DataContext = this;
            InitializeListComponent();
            Closing += VehicleWindow_Closing;
        }
        public VehicleWindow(Vehicle vehicle) : this()
        {
            InitializeVehicle(vehicle);
            InitializeVehicleComponents();
        }

        private void InitializeListComponent()
        {
            VehicleMarks = context.VehiclesMarks.Include("Models").ToList();
            FuelTypes = context.FuelTypes.ToList();
        }

        private void VehicleWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            context.Dispose();
        }

        protected void OnVehicleUpdated(EventArgs e)
        {
            if (VehicleUpdated == null)
                return;
            VehicleUpdated(this, e);
        }


        private void InitializeVehicleComponents()
        {
            VehicleColor = Vehicle.Color;
            VehicleMotorCapacity = Vehicle.MotorCapacity?.ToString();
            VehiclePower = Vehicle.Power?.ToString();
            VehicleVersion = Vehicle.Version;
            VehicleFuelType = Vehicle.FuelType;
            VehicleMark = Vehicle.Mark;
            VehicleModel = Vehicle.Model;
            VehicleRegistrationNumber = Vehicle.RegistrationNumber;
            VehicleVIN = Vehicle.VIN;
            VehicleYear = Vehicle.Year.ToString();
            InitailizeVehicleServices();
            Message = $"Last modification: {Vehicle.GetLastModificationDate(Vehicle)}";

        }



        private void InitailizeVehicleServices()
        {

            VehicleServicesList.CollectionChanged -= VehicleServicesList_CollectionChanged;
            VehicleServicesList.Clear();
            foreach (VehicleService zadanie in Vehicle.Services)
            {
                VehicleServicesList.Add(zadanie);
            }
            VehicleServicesList.CollectionChanged += VehicleServicesList_CollectionChanged;
        }

        private void VehicleServicesList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    VehicleService vehicleService = (VehicleService)e.NewItems[0];
                    if (vehicleService != null)
                        Vehicle.Services.Add(vehicleService);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    VehicleService vehicleServiceToRemove = (VehicleService)e.OldItems[0];
                    if (vehicleServiceToRemove != null)
                        Vehicle.Services.Remove(vehicleServiceToRemove);
                    break;
            }
        }

        public void InitializeVehicle(Vehicle vehicle)
        {
            Vehicle ve = context.Vehicles
                 .Include("Services.Services")
                 .Include("Mark.Models")
                 .Include("FuelType")
                 .FirstOrDefault(v => v.Id == vehicle.Id);
            Vehicle = ve;

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (Vehicle == null)
            {
                Vehicle = new Vehicle();
                context.Vehicles.Add(Vehicle);
            }
            ApplayVehicleChanges();
            context.SaveChanges();
            OnVehicleUpdated(EventArgs.Empty);
        }

        private void ApplayVehicleChanges()
        {
            //Vehicle.VehicleBodyType = VehicleVehicleBodyType;
            Vehicle.Color = VehicleColor;
            Vehicle.FuelType = VehicleFuelType;
            Vehicle.Mark = VehicleMark;
            Vehicle.Model = VehicleModel;
            if (!string.IsNullOrEmpty(VehicleMotorCapacity))
                Vehicle.MotorCapacity = double.Parse(VehicleMotorCapacity);
            if (!string.IsNullOrEmpty(VehiclePower))
                Vehicle.Power = double.Parse(VehiclePower);
            Vehicle.RegistrationNumber = VehicleRegistrationNumber;
            Vehicle.Version = VehicleVersion;
            Vehicle.VIN = VehicleVIN;
            if (!string.IsNullOrEmpty(VehicleYear))
                Vehicle.Year = int.Parse(VehicleYear);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            context.RollBack();
            InitializeVehicle(Vehicle);
            InitializeVehicleComponents();
        }

        private void ComboBoxMarks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var mark = ((ComboBox)sender).SelectedItem as VehicleMark;
            //VehicleModels = mark.Models;
            //VehicleModel = null;

        }
    }
}
