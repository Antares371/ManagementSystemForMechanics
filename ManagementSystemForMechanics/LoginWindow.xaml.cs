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

        public Account Account { get; private set; }

        private string login;
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                NotifyPropertyChanged(nameof(Login));
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
            set { message = value; NotifyPropertyChanged(nameof(Message)); }
        }

        private bool loadingIconVisibility;
        public bool LoadingIconVisibility
        {
            get { return loadingIconVisibility; }
            set { loadingIconVisibility = value; NotifyPropertyChanged(nameof(LoadingIconVisibility)); }
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
                    ShowMessage("Podany użytkownik nie istnieje. Niepoprawny login lub hasło.");
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
                    .Include(a => a.User)
                    .Where(a => a.Login == Login && a.Password == Password && a.IsActive)
                    .SingleOrDefault();
            }

        }


        private bool ValidInputData()
        {
            if (string.IsNullOrEmpty(Login))
            {

                ShowMessage("Pole login nie może być puste.");
                Password = string.Empty;
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {

                ShowMessage("Pole hasło nie może być puste.");
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
