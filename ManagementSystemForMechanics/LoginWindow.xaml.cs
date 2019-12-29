using ManagementSystemForMechanics.DAL;
using ManagementSystemForMechanics.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string login;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                txtLogin.Text = login;
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                txtPassword.Password = password;
            }
        }

        public Account Account { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            LoadingIcon.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadingIcon.Visibility = Visibility.Visible;

            if (!ValidInputData())
                return;

            Account account = GetAccount();
            if (account == null)
            {
                ShowMessage("User do not exist. Incorrect login or password.");
                Password = string.Empty;
                return;
            }
            MainWindow mainWindow = new MainWindow(account);
            mainWindow.Show();

            Close();

        }

        private Account GetAccount()
        {
            Account account = null;
            using (DBContext context = new DBContext())
            {
                account = context.Accounts.Where(a => a.Login == Login && a.Password == Password && a.IsActive).AsParallel().FirstOrDefault();
                if (account != null)
                {
                    account.IsLogged = true;
                }
                context.SaveChanges();
            }

            return account;
        }

        private Account LoginAccount(string login, string password)
        {
            Thread.Sleep(5000);

            return null;
        }

        private bool ValidInputData()
        {
            if (string.IsNullOrEmpty(Login))
            {

                ShowMessage("Login can not be empty.");
                Password = string.Empty;
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {

                ShowMessage("Password can not be empty.");
                return false;
            }
            return true;
        }



        private async Task<Account> GetAccountAsync(string login, string password)
        {
            Account account;
            using (DBContext context = new DBContext())
            {
                account = context.Accounts.Where(a => a.Login == login && a.Password == password && a.IsActive).AsParallel().FirstOrDefault();
            }
            return account;
        }





        private void ShowMessage(string hint)
        {
            txtMessage.Text = hint;
            if (LoadingIcon.Visibility == Visibility.Visible)
                LoadingIcon.Visibility = Visibility.Hidden;

        }

        private void TxtLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            login = ((TextBox)sender).Text;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = ((PasswordBox)sender).Password;
        }
    }
}
