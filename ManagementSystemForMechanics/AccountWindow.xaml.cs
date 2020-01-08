using ManagementSystemForMechanics.DAL;
using ManagementSystemForMechanics.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window, INotifyPropertyChanged
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

        private string name;
        public string UserName
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged(nameof(UserName)); }
        }
        private string surname;
        public string Surname
        {
            get { return surname; }
            set { surname = value; NotifyPropertyChanged(nameof(Surname)); }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; { NotifyPropertyChanged(nameof(PhoneNumber)); } }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; { NotifyPropertyChanged(nameof(Email)); } }
        }

        private string position;
        public string Position
        {
            get { return position; }
            set { position = value; { NotifyPropertyChanged(nameof(Position)); } }
        }

        private string login;
        public string Login
        {
            get { return login; }
            set { login = value; NotifyPropertyChanged(nameof(Login)); }
        }

        public string Password
        {
            get { return passBoxAccountPassword.Password; }
            private set { passBoxAccountPassword.Password = value; }
        }

        public string NewPassword
        {
            get { return passBoxNewPassword.Password; }
            private set { passBoxNewPassword.Password = value; }
        }
        public string ConfirmedNewPassword
        {
            get { return passBoxConfirmedNewPassword.Password; }
            private set { passBoxConfirmedNewPassword.Password = value; }
        }

        public AccountWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        public AccountWindow(Account account) : this()
        {
            this.account = account;
            InitializeAccountComponents();
        }

        private void InitializeAccountComponents()
        {
            UserName = account.User.Name;
            Surname = account.User.Surname;
            PhoneNumber = account.User.PhoneNumber;
            Email = account.User.Email;
            Position = account.User.Position?.Name;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidInputData())
            {
                MessageBox.Show("Błędne wprowadzone dane.");
                return;
            }

            using (DBContext context = new DBContext())
            {
                context.Accounts.Attach(account);
                ApplayChanges();
                context.SaveChanges();
            }
            MessageBox.Show("Hasło zostało zmienione.", "", MessageBoxButton.OK);
        }

        private bool ValidInputData()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Surname) || string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Email))
                return false;
            return true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ApplayChanges()
        {
            account.User.Name = Name;
            account.User.Surname = Surname;
            account.User.PhoneNumber = PhoneNumber;
            account.User.Email = Email;
        }

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (NewPassword != ConfirmedNewPassword)
            {
                MessageBox.Show("Powtórzone hasło różni się od pierwowzoru.", "", MessageBoxButton.OK);
                ClearPasswordForm();
                return;
            }
            if (NewPassword == account.Password)
            {
                MessageBox.Show("Nowe hasło nie może być identyczne.", "", MessageBoxButton.OK);
                ClearPasswordForm();
                return;
            }
            if (Password != account.Password)
            {
                MessageBox.Show("Błędne hasło.", "", MessageBoxButton.OK);
                ClearPasswordForm();
                return;
            }


            using (DBContext context = new DBContext())
            {
                context.Accounts.Attach(account);
                account.Password = NewPassword;
                context.SaveChanges();
            }
            MessageBox.Show("Hasło zostało zmienione.", "", MessageBoxButton.OK);
            ClearPasswordForm();
        }

        private void ClearPasswordForm()
        {
            Password = string.Empty;
            NewPassword = string.Empty;
            ConfirmedNewPassword = string.Empty;
        }
    }
}
