using ManagementSystemForMechanics.DAL;
using ManagementSystemForMechanics.Models;
using System;
using System.Collections.Generic;
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
    public partial class MainWindow : Window
    {
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


        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;

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



        private async void Search(object sender, RoutedEventArgs e)
        {
            string phase = SearchedPhase.Text.Trim();
            List<Vehicle> vehicles = await GetVehicles(phase);
            ObjectListView.ItemsSource = vehicles;
        }

        private async Task<List<Vehicle>> GetVehicles(string phase)
        {
            using (DBContext context = new DBContext())
            {
                return await context.Vehicles
                    .Where(v => v.Mark.Name.Contains(phase) ||
                    v.Model.Name.Contains(phase) ||
                    v.RegistrationNumber.Contains(phase) ||
                    v.VIN.Contains(phase))
                    .Include(p=> p.Mark)
                    .Include(p=> p.Model)
                    .ToListAsync();
            }
        }

        private void AddNewCar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
