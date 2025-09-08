using Hotel_C4ta.Controller;
using Hotel_C4ta.Model;
using Microsoft.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hotel_C4ta.View.AdminViews.Sections
{
    /// <summary>
    /// Lógica de interacción para RoomManagementContent.xaml
    /// </summary>
    public partial class RoomManagementControl : UserControl
    {
        private int _selectedRoomId;
        private readonly RoomController _roomController = new RoomController();
        public RoomManagementControl()
        {
            InitializeComponent(); 
            LoadRooms();
        }

        private void LoadRooms() { 
            var rooms = _roomController.GetAllRooms();
            RoomsGrid.ItemsSource = rooms;
        }

        private void RoomsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomsGrid.SelectedItem is RoomModel room)
            {
                _selectedRoomId = room.RoomID;
                TxtRoomNumber.Text = room.RoomID.ToString();
                TxtRoomFloor.Text = room.RoomFloor.ToString();
                TxtCapacity.Text = room.Capacity.ToString();
                TxtBasePrice.Text = room.BasePrice.ToString();

                CmbStatus.SelectedItem = CmbStatus.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == room.RoomStatus);

                CmbType.SelectedItem = CmbType.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == room.RoomType);

                
                TxtRoomNumber.IsEnabled = false;
                TxtRoomFloor.IsEnabled = false;
                CmbType.IsEnabled = false;
            }
            else
            {
              
                TxtRoomNumber.IsEnabled = true;
                TxtRoomFloor.IsEnabled = true;
                CmbType.IsEnabled = true;
                ClearForm();
            }
        }


        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            string RoomNumber = TxtRoomNumber.Text.Trim();
            int RoomFloor = int.Parse(TxtRoomFloor.Text.Trim());
            string RoomStatus = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            string RoomType = (CmbType.SelectedItem as ComboBoxItem)?.Content.ToString();
            int Capacity = int.Parse(TxtCapacity.Text.Trim());
            decimal BasePrice = decimal.Parse(TxtBasePrice.Text.Trim());

            if (string.IsNullOrWhiteSpace(RoomNumber)
               || string.IsNullOrWhiteSpace(RoomStatus)
               || string.IsNullOrWhiteSpace(RoomType)
               || string.IsNullOrWhiteSpace(TxtRoomFloor.Text.Trim())
               || string.IsNullOrWhiteSpace(TxtCapacity.Text.Trim())
               || string.IsNullOrWhiteSpace(TxtBasePrice.Text.Trim()))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            _roomController.CreateRoom(int.Parse(RoomNumber), RoomFloor, RoomStatus, RoomType, Capacity, BasePrice);

            ClearForm();
            LoadRooms();
        }


        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoomId > 0)
            {
                string RoomStatus = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
                int Capacity = int.Parse(TxtCapacity.Text.Trim());
                decimal BasePrice = decimal.Parse(TxtBasePrice.Text.Trim());
                if (string.IsNullOrWhiteSpace(RoomStatus)
                   || string.IsNullOrWhiteSpace(TxtCapacity.Text.Trim())
                   || string.IsNullOrWhiteSpace(TxtBasePrice.Text.Trim()))
                {
                    MessageBox.Show("All fields are required.");
                    return;
                }
                _roomController.UpdateRoom(_selectedRoomId, RoomStatus, Capacity, BasePrice);
                LoadRooms();
                ClearForm();
            }
            else
            {
                MessageBox.Show("Please select a room to update.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoomId > 0 )
            {
                var result = MessageBox.Show($"Are you sure you want to delete room {_selectedRoomId}?", "Confirm Deletion", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _roomController.DeleteRoom(_selectedRoomId);
                    LoadRooms();
                    ClearForm();
                }
            }
            else
            {
                MessageBox.Show("Please select a room to delete.");
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            TxtRoomNumber.Clear();
            TxtRoomFloor.Clear();
            CmbStatus.SelectedIndex = -1;
            CmbType.SelectedIndex = -1;
            TxtCapacity.Clear();
            TxtBasePrice.Clear();
            _selectedRoomId = 0;
            TxtRoomNumber.IsEnabled = true;
            TxtRoomFloor.IsEnabled = true;
            CmbType.IsEnabled = true;
        }


    }
}
