using Hotel_C4ta.Controller;
using Hotel_C4ta.Model;
using Hotel_C4ta.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_C4ta.View.ReceptionistViews.Sections
{
    /// <summary>
    /// Lógica de interacción para SeeAllBillsContent.xaml
    /// </summary>
    public partial class SeeAllBillsContent : UserControl
    {
        public SeeAllBillsContent()
        {
            InitializeComponent();
            LoadBills();
        }

        private void LoadBills()
        {
            var result = BillsJoinPayments.LoadBillsJoinPayment();
            BillsGrid.ItemsSource = result;
        }

        private void OpenPdf_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is byte[] pdfBytes && pdfBytes.Length > 0)
            {
                try
                {
                    string tempPath = System.IO.Path.Combine(
                        System.IO.Path.GetTempPath(),
                        $"Bill_{Guid.NewGuid()}.pdf"
                    );

                    System.IO.File.WriteAllBytes(tempPath, pdfBytes);
                    Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening PDF: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No PDF available for this bill.");
            }
        }
    }
}
