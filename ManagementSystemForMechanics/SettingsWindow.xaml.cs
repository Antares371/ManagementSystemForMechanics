using ManagementSystemForMechanics.DAL;
using ManagementSystemForMechanics.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public ObservableCollection<VehicleMark> VehiclesMarks { get; } = new ObservableCollection<VehicleMark>();
        public ObservableCollection<Account> Accounts { get; } = new ObservableCollection<Account>();
        public ObservableCollection<Position> Positions { get; } = new ObservableCollection<Position>();
        public ObservableCollection<Service> Services { get; } = new ObservableCollection<Service>();
        public ObservableCollection<SystemLog> Logs { get; } = new ObservableCollection<SystemLog>();
        public ObservableCollection<FuelType> FuelTypes { get; } = new ObservableCollection<FuelType>();

        private DBContext context;
        private Account account;
        public string Message { get; set; }

        public SettingsWindow()
        {
            InitializeComponent();
            context = new DBContext();
            DataContext = this;
            Closing += SettingsWindow_Closing;
            InitializeData();
        }

        public SettingsWindow(Account account) : this()
        {
            this.account = account;
        }

        private void InitializeData()
        {
            InitializeVehicleMarks();
            InitializeFuelTypes();
            InitializeAccounts();
            InitializePositions();
            InitializeSystemLogs();
            InitializeServices();
        }

        private void InitializePositions()
        {
            Positions.CollectionChanged -= _CollectionChanged;
            Positions.Clear();
            foreach (Position position in context.Positions)
            {
                Positions.Add(position);
            }
            Positions.CollectionChanged += _CollectionChanged;
        }

        private void InitializeFuelTypes()
        {
            FuelTypes.CollectionChanged -= _CollectionChanged;
            FuelTypes.Clear();
            foreach (FuelType fuelType in context.FuelTypes)
            {
                FuelTypes.Add(fuelType);
            }
            FuelTypes.CollectionChanged += _CollectionChanged;
        }

        private void _CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddItemToContext(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItemToContext(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
        }

        private void AddItemToContext(IList items)
        {

            if (items[0] is FuelType fuelType)
            {
                context.FuelTypes.Add(fuelType);
                return;
            }

            if (items[0] is VehicleMark vehicleMark)
            {
                context.VehiclesMarks.Add(vehicleMark);
                return;
            }

            if (items[0] is Service service)
            {
                context.Services.Add(service);
                return;
            }

            if (items[0] is VehicleModel vehicleModel)
            {
                context.VehiclesModels.Add(vehicleModel);
                return;
            }

            if (items[0] is Position position)
            {
                context.Positions.Add(position);
                return;
            }


        }

        private void RemoveItemToContext(IList items)
        {

            if (items[0] is FuelType fuelType)
            {
                context.FuelTypes.Remove(fuelType);
                return;
            }

            if (items[0] is VehicleMark vehicleMark)
            {
                context.VehiclesMarks.Remove(vehicleMark);
                return;
            }

            if (items[0] is Service service)
            {
                context.Services.Remove(service);
                return;
            }

            if (items[0] is VehicleModel vehicleModel)
            {
                context.VehiclesModels.Remove(vehicleModel);
                return;
            }

            if (items[0] is Position position)
            {
                context.Positions.Remove(position);
                return;
            }

        }

        private void InitializeServices()
        {
            Services.CollectionChanged -= _CollectionChanged;
            Services.Clear();
            foreach (Service service in context.Services)
            {
                Services.Add(service);
            }
            Services.CollectionChanged += _CollectionChanged;
        }

        private void InitializeSystemLogs()
        {
            Logs.CollectionChanged -= _CollectionChanged;
            Logs.Clear();
            foreach (SystemLog log in context.SystemLogs)
            {
                Logs.Add(log);
            }
            Logs.CollectionChanged += _CollectionChanged;
        }

        private void InitializeAccounts()
        {
            Accounts.CollectionChanged -= _CollectionChanged;
            Accounts.Clear();
            foreach (Account account in context.Accounts)
            {
                Accounts.Add(account);
            }
            Accounts.CollectionChanged += _CollectionChanged;
        }

        private void InitializeVehicleMarks()
        {
            VehiclesMarks.CollectionChanged -= _CollectionChanged;
            VehiclesMarks.Clear();
            foreach (VehicleMark mark in context.VehiclesMarks.Include("Models"))
            {
                VehiclesMarks.Add(mark);
            }
            VehiclesMarks.CollectionChanged += _CollectionChanged;
        }

        private void SettingsWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            context.Dispose();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            context.SaveChanges();
            InitializeData();


        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            context.RollBack();
            InitializeData();

        }
    }
}
