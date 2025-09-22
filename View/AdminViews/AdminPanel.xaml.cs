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
using Hotel_C4ta.View.AdminViews.Sections;
using Hotel_C4ta.View;


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
            ContentArea.Content = new Dashboard(); //Show dashboard by default
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new Dashboard();
        }

        private void UsersManagement_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new UserManagementControl();
        }
        
        private void RoomsManagement_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new RoomManagementControl();
        }
        
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
