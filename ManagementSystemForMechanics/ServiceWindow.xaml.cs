using ManagementSystemForMechanics.DAL;
using ManagementSystemForMechanics.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private VehicleService vehicleService;

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged(nameof(Message)); }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; NotifyPropertyChanged(nameof(StartDate)); }
        }

        private DateTime finishDate;
        public DateTime FinishDate
        {
            get { return finishDate; }
            set { finishDate = value; NotifyPropertyChanged(nameof(FinishDate)); }
        }

        public ObservableCollection<Service> Services { get; } = new ObservableCollection<Service>();

        private DBContext context;

        public List<Service> ServicesDictionary { get; set; }

        public ServiceWindow()
        {
            DataContext = this;
            context = new DBContext();
            InitializeComponent();
            InitializeListComponents();
        }

        private void InitializeListComponents()
        {
            ServicesDictionary = context.Services.ToList();
        }

        public ServiceWindow(VehicleService service) : this()
        {
            InitializeService(service);
            InitializeServiceComponents();
        }

        private void InitializeServiceComponents()
        {
            StartDate = vehicleService.StartDate;
            FinishDate = vehicleService.FinishDate;
            Services.CollectionChanged -= Services_CollectionChanged;
            foreach (Service service in vehicleService.Services)
            {
                Services.Add(service);
            }
            Services.CollectionChanged += Services_CollectionChanged;
        }

        private void InitializeService(VehicleService vehicleService)
        {

            this.vehicleService = context.VehicleService
                .Include("Services")
                .SingleOrDefault(vs=> vs.Id == vehicleService.Id);
        }

        private void Services_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Service service = (Service)e.NewItems[0];
                    if (service != null)
                        Services.Add(service);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Service serviceToRemove = (Service)e.OldItems[0];
                    if (serviceToRemove != null)
                        Services.Remove(serviceToRemove);
                    break;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
