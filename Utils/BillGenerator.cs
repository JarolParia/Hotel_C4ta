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
            string dni,
            string roomNumber,
            DateTime startDate,
            DateTime endDate,
            decimal total,
            string paymentMethod,
            bool saveToDesktop = true)
        {
            try
            {
                // Valores por defecto
                clientName ??= "Cliente desconocido";
                dni ??= "Sin DNI";
                roomNumber ??= "0";
                paymentMethod ??= "No especificado";

                // Nombre seguro para archivo
                string safeDni = dni.Replace(" ", "").Replace("/", "").Replace(":", "");
                string fileName = $"Factura_{safeDni}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                // Crear documento
                using (PdfDocument document = new PdfDocument())
                {
                    document.Info.Title = "Factura Hotel C4TA";
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
                    gfx.DrawString("Factura emitida a:", subHeaderFont, XBrushes.Black, new XPoint(50, y));
                    y += 25;
                    gfx.DrawString($"Nombre: {clientName}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 20;
                    gfx.DrawString($"DNI: {dni}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 20;
                    gfx.DrawString($"Habitación: {roomNumber}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 30;

                    // Detalles de la reserva
                    gfx.DrawString("Detalles de la reserva:", subHeaderFont, XBrushes.Black, new XPoint(50, y));
                    y += 25;
                    gfx.DrawString($"Desde: {startDate:dd/MM/yyyy}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 20;
                    gfx.DrawString($"Hasta: {endDate:dd/MM/yyyy}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 20;
                    gfx.DrawString($"Método de pago: {paymentMethod}", bodyFont, XBrushes.Black, new XPoint(60, y)); y += 30;

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
                    gfx.DrawString("Gracias por hospedarse en Hotel C4TA", footerFont, XBrushes.Gray, new XPoint(180, y));

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
                MessageBox.Show($"Error PDF: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Abre el PDF desde un arreglo de bytes (lo guarda temporalmente).
        /// </summary>
        public static void OpenInvoice(byte[] pdfBytes, int billNumber)
        {
            try
            {
                string tempFile = Path.Combine(Path.GetTempPath(), $"Factura_{billNumber}.pdf");
                File.WriteAllBytes(tempFile, pdfBytes);
                Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir el PDF: {ex.Message}");
            }
        }
    }
}
