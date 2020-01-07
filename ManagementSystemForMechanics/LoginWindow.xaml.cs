using ManagementSystemForMechanics.DAL;
using ManagementSystemForMechanics.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                NotifyPropertyChanged("Login");
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

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged("Message"); }
        }


        public Account Account { get; private set; }
        private bool loadingIconVisibility;

        public bool LoadingIconVisibility
        {
            get { return loadingIconVisibility; }
            set { loadingIconVisibility = value; NotifyPropertyChanged("LoadingIconVisibility"); }
        }


        public LoginWindow() : base()
        {
            DataContext = this;
            InitializeComponent();
            LoadingIconVisibility = false;
            Login = "admin";
            Password = "Pass";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                ShowMessage("Logowanie...");
                LoadingIconVisibility = true;

                if (!ValidInputData())
                    return;

            Task.Factory.StartNew(() =>
            {
                Account = GetAccount();
            })
            .ContinueWith((task) =>
            {
                if (Account == null)
                {
                    ShowMessage("User do not exist. Incorrect login or password.");
                    Password = string.Empty;
                    return;
                }

                LoginAccount(Account);
                MainWindow mainWindow = new MainWindow(Account);
                mainWindow.Show();

                Close();
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

        }

        private void LoginAccount(Account account)
        {
            using (DBContext context = new DBContext())
            {
                context.Accounts.Attach(account).IsLogged = true;
                context.SaveChanges();
            }


        }
        private Account GetAccount()
        {
            using (DBContext context = new DBContext())
            {
                return context.Accounts
                    .Where(a => a.Login == Login && a.Password == Password && a.IsActive)
                    .FirstOrDefault();
            }

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

        private void ShowMessage(string hint)
        {
            Message = hint;
            //if (LoadingIcon.Visibility == Visibility.Visible)
            LoadingIconVisibility = false;

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
