using Hotel_C4ta.View.AdminViews.Sections;
﻿using Hotel_C4ta.View.AdminViews;
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

namespace Hotel_C4ta.View.AdminViews
{
    /// <summary>
    /// Lógica de interacción para AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
            ContentArea.Content = new DashboardControl();///Show dashboard by default
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new DashboardControl();
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new UserManagementControl();
        }

        private void RoomManagement_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new RoomManagementControl();
        }



    }
}
