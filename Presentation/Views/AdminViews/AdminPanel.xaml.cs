using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Presentation.Views.AdminViews.Sections;
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
using System.Windows.Shapes;
namespace Hotel_C4ta.Presentation.Views.AdminViews
{
    /// <summary>
    /// Lógica de interacción para AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        private readonly NavigationService _navigationService; /// Handles navigation inside the panel
        private readonly ServiceManager _services;
        private readonly User _loggedUser;
        public AdminPanel(ServiceManager services, User user)
        {
            InitializeComponent(); /// Initializes all UI elements defined in AdminPanel.xaml
            _services = services;
            _loggedUser = user;
            _navigationService = new NavigationService(ContentArea); /// Initialize the navigation service with the ContentArea placeholder
            _navigationService.Navigate(new DashBoard(_services)); /// Load the dashboard view as the default screen when the admin panel opens

        }
        private void DashBoard_Click(object sender, RoutedEventArgs e)
        {
            /// Navigate to the dashboard view
            _navigationService.Navigate(new DashBoard(_services));
        }
        private void RoomsManagement_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(new RoomManagementControl(_services.RoomService));
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {

            _navigationService.Navigate(new UserManagementControl(_services.UserService));
        }
        
        
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); /// Close the admin panel window (logs out the admin)
        }
    }
}
