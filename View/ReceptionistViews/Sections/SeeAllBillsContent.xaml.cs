using Hotel_C4ta.Model;
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
        private DatabaseConnection _db = new DatabaseConnection();

        public SeeAllBillsContent()
        {
            InitializeComponent();
            LoadBills();
        }

        private void LoadBills()
        {
            var result = new List<dynamic>();

            using (var conn = _db.OpenConnection())
            {
                if (conn == null) return;

                try
                {
                    string sql = @"
                        SELECT b.Number, b.IssueDate, b.Total, b.IdBooking,
                               p.PaymentMethod, p.Amount, p.Dates,
                               b.PdfFile
                        FROM Bill b
                        LEFT JOIN Payment p ON b.Number = p.BillNumber
                        ORDER BY b.IssueDate DESC";

                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new
                            {
                                Number = reader.GetInt32(0),
                                IssueDate = reader.GetDateTime(1),
                                Total = reader.GetDecimal(2),
                                IdBooking = reader.GetInt32(3),
                                PaymentMethod = reader.IsDBNull(4) ? "Pendiente" : reader.GetString(4),
                                Amount = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                                PaymentDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                PdfBytes = reader.IsDBNull(7) ? null : (byte[])reader[7]
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar facturas/pagos: " + ex.Message);
                }
                finally
                {
                    _db.CloseConnection();
                }
            }

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
                        $"Factura_{Guid.NewGuid()}.pdf"
                    );

                    System.IO.File.WriteAllBytes(tempPath, pdfBytes);
                    Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al abrir PDF: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("No hay PDF disponible para esta factura.");
            }
        }
    }
}
