using Hotel_C4ta.Application.Services;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Hotel_C4ta.Presentation.Views.ReceptionistViews.Sections
{
    public partial class SeeAllBillsContent : UserControl
    {
        private readonly ServiceManager _serviceManager;

        public SeeAllBillsContent(ServiceManager serviceManager)
        {
            InitializeComponent();
            _serviceManager = serviceManager;
            LoadBillsWithPayments();
        }

        /// <summary>
        /// Carga facturas con la información de pagos
        /// </summary>
        private void LoadBillsWithPayments()
        {
            try
            {
                var billsWithPayments = _serviceManager.BillService.GetBillsWithPayments();
                BillsGrid.ItemsSource = billsWithPayments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al cargar facturas: {ex.Message}");
            }
        }

        /// <summary>
        /// Refresca los datos del grid
        /// </summary>
        public void RefreshBills()
        {
            LoadBillsWithPayments();
        }

        /// <summary>
        /// Abre el PDF asociado a la factura
        /// </summary>
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

                    var processStartInfo = new ProcessStartInfo(tempPath)
                    {
                        UseShellExecute = true
                    };

                    Process.Start(processStartInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error al abrir PDF: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("⚠️ No hay PDF disponible para esta factura.");
            }
        }
    }
}