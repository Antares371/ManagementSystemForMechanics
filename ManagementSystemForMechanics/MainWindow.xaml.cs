using ManagementSystemForMechanics.DAL;
using ManagementSystemForMechanics.Models;
using System;
using System.Collections.Generic;
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



        private void Search(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow();
            customerWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
