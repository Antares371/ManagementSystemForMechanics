using ManagementSystemForMechanics.DAL;
using ManagementSystemForMechanics.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class VehicleWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private DBContext context;
        public List<VehicleMark> VehicleMarks { get; set; }
        public List<VehicleModel> VehicleModels { get; set; }
        public List<FuelType> FuelTypes { get; set; }

        public Vehicle Vehicle { get; set; }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged(nameof(Message)); }
        }

        private string vehicleMotorCapacity;
        public string VehicleMotorCapacity
        {
            get { return vehicleMotorCapacity; }
            set { vehicleMotorCapacity = value; NotifyPropertyChanged(nameof(VehicleMotorCapacity)); }
        }

        private string vehiclePower;
        public string VehiclePower
        {
            get { return vehiclePower; }
            set { vehiclePower = value; NotifyPropertyChanged(nameof(VehiclePower)); }
        }

        private string vehicleVersion;
        public string VehicleVersion
        {
            get { return vehicleVersion; }
            set { vehicleVersion = value; NotifyPropertyChanged(nameof(VehicleVersion)); }
        }

        private VehicleMark vehicleMark;
        public VehicleMark VehicleMark
        {
            get { return vehicleMark; }
            set { vehicleMark = value; NotifyPropertyChanged(nameof(VehicleMark)); }
        }

        private VehicleModel vehicleModel;
        public VehicleModel VehicleModel
        {
            get { return vehicleModel; }
            set { vehicleModel = value; NotifyPropertyChanged(nameof(VehicleModel)); }
        }

        private FuelType vehicleFuelType;
        public FuelType VehicleFuelType
        {
            get { return vehicleFuelType; }
            set
            {
                vehicleFuelType = value;
                NotifyPropertyChanged(nameof(vehicleFuelType));
            }
        }

        public ObservableCollection<VehicleService> VehicleServicesList { get; } = new ObservableCollection<VehicleService>();

        private string vehicleYear;
        public string VehicleYear
        {
            get { return vehicleYear; }
            set { vehicleYear = value; NotifyPropertyChanged(nameof(VehicleYear)); }
        }

        private string vehicleVIN;
        public string VehicleVIN
        {
            get { return vehicleVIN; }
            set { vehicleVIN = value; NotifyPropertyChanged(nameof(VehicleVIN)); }
        }


        private string vehicleRegistrationNumber;
        public string VehicleRegistrationNumber
        {
            get { return vehicleRegistrationNumber; }
            set { vehicleRegistrationNumber = value; NotifyPropertyChanged(nameof(VehicleRegistrationNumber)); }
        }

        private string vehicleColor;
        public string VehicleColor
        {
            get { return vehicleColor; }
            set { vehicleColor = value; NotifyPropertyChanged(nameof(VehicleColor)); }
        }


        private bool loadingMessageVisibility;
        public bool LoadingMessageVisibility
        {
            get { return loadingMessageVisibility; }
            set
            {
                loadingMessageVisibility = value;
                NotifyPropertyChanged(nameof(LoadingMessageVisibility));
            }
        }

        private string loadingMessage;
        public string LoadingMessage
        {
            get { return loadingMessage; }
            set { loadingMessage = value; NotifyPropertyChanged(nameof(LoadingMessage)); }
        }

        public event EventHandler VehicleUpdated;

        public VehicleWindow()
        {
            LoadingMessage = "Proszę czekać...";
            LoadingMessageVisibility = true;
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
            LoadingMessageVisibility = false;
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
            Message = $"Ostatnia modyfikacja: {Vehicle.GetLastModificationDate(Vehicle)}";

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
            Task.Factory.StartNew(() =>
            {
                LoadingMessageVisibility = true;
                Message = $"Zapisywanie zmian...";
                Task.Delay(2500);
            })
            .ContinueWith((task) =>
            {
                if (Vehicle == null)
                {
                    Vehicle = new Vehicle();
                    context.Vehicles.Add(Vehicle);
                }
                ApplayVehicleChanges();
                context.SaveChanges();
            })
            .ContinueWith((task) =>
            {
                OnVehicleUpdated(EventArgs.Empty);
                LoadingMessageVisibility = false;
                Message = $"Zmiany zostały zapisane.{Vehicle.GetLastModificationDate(Vehicle)}";
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

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
