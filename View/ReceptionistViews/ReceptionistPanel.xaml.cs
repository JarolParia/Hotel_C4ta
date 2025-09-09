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
using Hotel_C4ta.View.ReceptionistViews.Sections;

namespace Hotel_C4ta.View.ReceptionistViews
{
    /// <summary>
    /// Lógica de interacción para ReceptionistPanel.xaml
    /// </summary>
    public partial class ReceptionistPanel : Window
    {
        private int _receptionistId;
        private string _receptionistName = string.Empty;

        public ReceptionistPanel(int receptionistId, string receptionistName)
        {
            InitializeComponent();
            _receptionistId = receptionistId;
            _receptionistName = receptionistName;
            lblUsuario.Content = _receptionistName;
            ContentArea.Content = new DashboardContent(); //Show dashboard by default
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new DashboardContent();
        }

        private void RegisterClients_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new RegisterClientsContent();
        }

        private void Search_UpdateClients_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new Search_UpdateClientsContent();
        }
        private void CreateBooking_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new CreateBookingContent(_receptionistId);
        }

       private void RegisterCheckIn_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new RegisterCheckInContent();
        }

        private void RegisterCheckOut_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new RegisterCheckOutContent();
        }

        private void Search_UpdateBooking_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new Search_UpdateBookingContent();
        }

        private void SeeAllBills_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new SeeAllBillsContent();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
