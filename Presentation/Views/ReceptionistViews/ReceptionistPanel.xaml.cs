using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
using Hotel_C4ta.Presentation.ReceptionistViews.Sections;
using Hotel_C4ta.Presentation.Views.ReceptionistViews.Sections;
using System.Windows;

namespace Hotel_C4ta.Presentation.Views.ReceptionistViews
{
    /// <summary>
    /// Lógica de interacción para ReceptionistPanel.xaml
    /// </summary>
    public partial class ReceptionistPanel : Window
    {

        private readonly ServiceManager _services;
        private readonly NavigationService _navigationService;
        private readonly User _loggedUser;

        public ReceptionistPanel(ServiceManager service, User loggedUser)
        {
            InitializeComponent();
            _services = service;
            _loggedUser = loggedUser;
            _navigationService = new NavigationService(ContentArea);
            TxtReceptionistName.Text = _loggedUser.FullName;
            _navigationService.Navigate(new DashBoard(_services));
        }
        private void DashBoard_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(new DashBoard(_services));
        }

        private void RegisterClients_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(new RegisterClientContent(_services.ClientService));
        }
        
        private void Search_UpdateClients_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(new Search_UpdateClientsContent(_services.ClientService));
        }

        private void CreateBooking_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(new CreateBookingContent(_services, _loggedUser));
        }

        private void Search_UpdateBooking_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(new Search_UpdateBookingContent(_services));
        }

        private void RegisterCheckIn_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(new RegisterCheckInContent(_services));
        }

        private void RegisterCheckOut_Click(object sender, RoutedEventArgs e)
        {

            //_navigationService.Navigate(new RegisterCheckoutContent(_services));
        }

        private void SeeAllBills_Click(object sender, RoutedEventArgs e)
        {
           // _navigationService.Navigate(new SeeAllBillsContent(_services));
        }
        

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
