using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Hotel_C4ta.Utils
{
    class BillGenerator
    {
        /// <summary>
        /// Genera la factura en PDF y devuelve los bytes del archivo.
        /// Además, opcionalmente guarda una copia en el escritorio.
        /// </summary>
        public static byte[] GenerateInvoice(
            string clientName,
            string clientDni,
            int roomId,
            DateTime startDate,
            DateTime endDate,
            decimal total,
            string paymentMethod,
            bool saveToDesktop = true)
        {
            try
            {
                // Valores por defecto
                clientName ??= "Unknown client";
                clientDni ??= "Without DNI";
                paymentMethod ??= "No specified";

                // Nombre seguro para archivo
                string safeDni = clientDni.Replace(" ", "").Replace("/", "").Replace(":", "");
                string fileName = $"Bill_{safeDni}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                // Crear documento
                using (PdfDocument document = new PdfDocument())
                {
                    document.Info.Title = "Bill Hotel C4TA";
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    // Fuentes
                    XFont headerFont = new XFont("Arial", 20, XFontStyleEx.Bold);
                    XFont subHeaderFont = new XFont("Arial", 14, XFontStyleEx.Bold);
                    XFont bodyFont = new XFont("Arial", 12, XFontStyleEx.Regular);
                    XFont boldFont = new XFont("Arial", 12, XFontStyleEx.Bold);
                    XFont footerFont = new XFont("Arial", 10, XFontStyleEx.Italic);

                    int y = 50;

                    // Encabezado
                    gfx.DrawString("HOTEL C4TA", headerFont, XBrushes.DarkBlue, new XPoint(200, y));
                    y += 30;
                    gfx.DrawLine(XPens.Black, 40, y, page.Width - 40, y);
                    y += 30;

                    // Datos del cliente
                    gfx.DrawString("Bill issued to:", subHeaderFont, XBrushes.Black, new XPoint(50, y));
                    y += 25;
                    gfx.DrawString($"Name: {clientName}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 20;
                    gfx.DrawString($"DNI: {clientDni}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 20;
                    gfx.DrawString($"Room: {roomId}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 30;

                    // Detalles de la reserva
                    gfx.DrawString("Booking details:", subHeaderFont, XBrushes.Black, new XPoint(50, y));
                    y += 25;
                    gfx.DrawString($"From: {startDate:dd/MM/yyyy}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 20;
                    gfx.DrawString($"To: {endDate:dd/MM/yyyy}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 20;
                    gfx.DrawString($"Payment method: {paymentMethod}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 30;

                    // Línea divisoria
                    gfx.DrawLine(XPens.Gray, 40, y, page.Width - 40, y);
                    y += 40;

                    // Total
                    gfx.DrawString("TOTAL:", boldFont, XBrushes.Black, new XPoint(350, y));
                    gfx.DrawString($"${total:F2}", subHeaderFont, XBrushes.DarkRed, new XPoint(420, y));
                    y += 60;

                    // Pie de página
                    gfx.DrawLine(XPens.Gray, 40, y, page.Width - 40, y);
                    y += 30;
                    gfx.DrawString("Thank you for staying at Hotel C4TA", footerFont, XBrushes.Gray, new XPoint(180, y));

                    // Guardar en memoria (para DB)
                    using (var ms = new MemoryStream())
                    {
                        document.Save(ms, false); // false = no cierra el stream
                        byte[] pdfBytes = ms.ToArray();

                        // Guardar copia en escritorio si se pide
                        if (saveToDesktop)
                        {
                            File.WriteAllBytes(desktopPath, pdfBytes);
                        }

                        return pdfBytes;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Abre el PDF desde un arreglo de bytes (lo guarda temporalmente).
        /// </summary>
        public static void OpenInvoice(byte[] pdfBytes, int billId)
        {
            try
            {
                string tempFile = Path.Combine(Path.GetTempPath(), $"Bill_{billId}.pdf");
                File.WriteAllBytes(tempFile, pdfBytes);
                Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open PDF: {ex.Message}");
            }
        }
    }
}