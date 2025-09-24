using Hotel_C4ta.Application.Services;
using Hotel_C4ta.Domain.Entities;
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

namespace Hotel_C4ta.Presentation.Views.AdminViews.Sections
{
    /// <summary>
    /// Lógica de interacción para RoomManagementControl.xaml
    /// </summary>
    public partial class RoomManagementControl : UserControl
    {
        private readonly RoomService _roomService;
        private int _selectedRoomId = -1;
        public RoomManagementControl(RoomService roomService)
        {
            InitializeComponent();
            _roomService = roomService;
            LoadRooms();
        }
        private void LoadRooms()
        {
            RoomsGrid.ItemsSource = _roomService.GetAllRooms();
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int roomId = int.Parse(TxtRoomNumber.Text);
                int roomFloor = int.Parse(TxtRoomFloor.Text);
                string roomStatus = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
                string roomType = (CmbType.SelectedItem as ComboBoxItem)?.Content.ToString();
                int capacity = int.Parse(TxtCapacity.Text);
                decimal basePrice = decimal.Parse(TxtBasePrice.Text);

                _roomService.CreateRoom(roomId, roomFloor, roomStatus, roomType, capacity, basePrice);

                LoadRooms();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating room: " + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoomId > 0)
            {
                try
                {
                    string roomStatus = (CmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
                    int capacity = int.Parse(TxtCapacity.Text);
                    decimal basePrice = decimal.Parse(TxtBasePrice.Text);

                    _roomService.UpdateRoom(_selectedRoomId, roomStatus, capacity, basePrice);

                    LoadRooms();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating room: " + ex.Message);
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoomId > 0)
            {
                try
                {
                    _roomService.DeleteRoom(_selectedRoomId);

                    LoadRooms();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting room: " + ex.Message);
                }
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void RoomsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomsGrid.SelectedItem is Room room)
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

                // 🔒 Evitamos que se cambien algunos campos al actualizar
                TxtRoomNumber.IsEnabled = false;
                TxtRoomFloor.IsEnabled = false;
                CmbType.IsEnabled = false;
            }
        }

        private void ClearForm()
        {
            TxtRoomNumber.Clear();
            TxtRoomFloor.Clear();
            TxtCapacity.Clear();
            TxtBasePrice.Clear();
            CmbStatus.SelectedIndex = -1;
            CmbType.SelectedIndex = -1;

            _selectedRoomId = 0;

            TxtRoomNumber.IsEnabled = true;
            TxtRoomFloor.IsEnabled = true;
            CmbType.IsEnabled = true;
        }
    }
}

