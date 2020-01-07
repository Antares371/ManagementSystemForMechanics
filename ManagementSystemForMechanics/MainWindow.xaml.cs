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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManagementSystemForMechanics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Account account;
        public Account Account
        {
            get { return account; }
            set
            {
                account = value;
                AccountData.Text = account.ToString();
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged(nameof(Message)); }
        }
        private string searchedPhase;
        public string SearchedPhase
        {
            get { return searchedPhase; }
            set { searchedPhase = value; NotifyPropertyChanged(nameof(SearchedPhase)); }
        }

        public ObservableCollection<Vehicle> VehiclesList { get; } = new ObservableCollection<Vehicle>();


        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            // Temporary created account

            Closing += MainWindow_Closing;
            Closing += MainWindow_Closing1;

        }

        private void MainWindow_Closing1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Logout())
                e.Cancel = true;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (DBContext context = new DBContext())
            {
                context.Accounts.Attach(Account);
                Account.IsLogged = false;
                context.SaveChanges();
            }
        }

        public MainWindow(Account account) : this()
        {
            Account = account;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchVehicles();
        }

        private void SearchVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            Task.Factory.StartNew(() =>
            {
                ShowMessage($"Searching...");

                if (string.IsNullOrEmpty(SearchedPhase))
                {
                    MessageBox.Show("Please enter your search criteria.");
                    return;
                }
                string phase = SearchedPhase.Trim();
                vehicles = GetVehicles(phase);
            })
            .ContinueWith((task) =>
            {
                ShowMessage($"Result: {vehicles.Count} vehicles");

                VehiclesList.CollectionChanged -= VehiclesList_CollectionChanged;
                VehiclesList.Clear();
                foreach (Vehicle vehicle in vehicles)
                {
                    VehiclesList.Add(vehicle);
                }
                VehiclesList.CollectionChanged += VehiclesList_CollectionChanged;
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void VehiclesList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            return;
            //switch (e.Action)
            //{
            //    case NotifyCollectionChangedAction.Add:
            //        VehicleService vehicleService = (VehicleService)e.NewItems[0];
            //        if (vehicleService != null)
            //            Vehicle.Services.Add(vehicleService);
            //        break;
            //    case NotifyCollectionChangedAction.Remove:
            //        VehicleService vehicleServiceToRemove = (VehicleService)e.OldItems[0];
            //        if (vehicleServiceToRemove != null)
            //            Vehicle.Services.Remove(vehicleServiceToRemove);
            //        break;
            //}
        }

        private List<Vehicle> GetVehicles(string phase)
        {
            using (DBContext context = new DBContext())
            {
                return context.Vehicles
                .Where(v => v.Mark.Name.Contains(phase) ||
                v.Model.Name.Contains(phase) ||
                v.RegistrationNumber.Contains(phase) ||
                v.VIN.Contains(phase))
                .Include(p => p.Mark)
                .Include(p => p.Model)
                .Include(p => p.FuelType)
                .ToList();
            }
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            VehicleWindow vehicleWindow = new VehicleWindow
            {
                Owner = this
            };

            vehicleWindow.Show();
        }

        private void RemoveCarButton_Click(object sender, RoutedEventArgs e)
        {
            Vehicle v = ObjectListView.SelectedItem as Vehicle;
            if (v == null)
                return;
            MessageBoxResult result = MessageBox.Show("Are you sure to delete this vehicle?", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.OK)
            {
                //Remove vehicle
                ShowMessage("Row was removed");
            }
        }

        private void ShowMessage(string message)
        {
            Message = message;
        }

        private void EditCarButton_Click(object sender, RoutedEventArgs e)
        {
            Vehicle v = ObjectListView.SelectedItem as Vehicle;
            if (v == null)
                return;
            VehicleWindow vehicleWindow = new VehicleWindow(v)
            {
                Owner = this
            };
            vehicleWindow.VehicleUpdated += VehicleWindow_VehicleUpdated;
            vehicleWindow.Show();
        }

        private void SearchedPhase_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
                SearchVehicles();

        }

        private void ObjectListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    ListView listView = sender as ListView;
                    if (listView != null && listView.SelectedItems != null && listView.SelectedItems.Count == 1)
                    {
                        Vehicle v = listView.SelectedItem as Vehicle;
                        VehicleWindow vehicleWindow = new VehicleWindow(v);
                        vehicleWindow.VehicleUpdated += VehicleWindow_VehicleUpdated;
                        vehicleWindow.Owner = this;
                        vehicleWindow.Show();

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void VehicleWindow_VehicleUpdated(object sender, EventArgs e)
        {
            SearchVehicles();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(Account)
            {
                Owner = this
            };
            settingsWindow.ShowDialog();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool Logout()
        {

            if (OwnedWindows.Count != 0)
            {
                MessageBox.Show("Please close opened application windows.");
                return false;
            }
            using (DBContext context = new DBContext())
            {
                context.Accounts.Attach(Account);
                Account.IsLogged = false;
                context.SaveChanges();
            }
            return true;
        }

        private void ButtonMyAccount_Click(object sender, RoutedEventArgs e)
        {
            AccountWindow accountWindow = new AccountWindow(Account);
            accountWindow.Owner = this;
            accountWindow.ShowDialog();
        }
    }
}
