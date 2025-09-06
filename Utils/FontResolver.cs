using PdfSharp.Fonts;
using System;
using System.IO;

namespace Hotel_C4ta.Utils
{
    public class CustomFontResolver : IFontResolver
    {
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Mapear todas las fuentes a Arial básico
            string faceName = "arial";

            if (isBold)
                faceName += "#bold";

            return new FontResolverInfo(faceName);
        }

        public byte[] GetFont(string faceName)
        {
            // Buscar fuente en el sistema
            string fontPath = "";

            switch (faceName.ToLower())
            {
                case "arial":
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    break;
                case "arial#bold":
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialbd.ttf");
                    break;
                default:
                    fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                    break;
            }

            if (File.Exists(fontPath))
            {
                return File.ReadAllBytes(fontPath);
            }

            // Si no encuentra Arial, usar Calibri como respaldo
            string calibriPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "calibri.ttf");
            if (File.Exists(calibriPath))
            {
                return File.ReadAllBytes(calibriPath);
            }

            // Último recurso: devolver null (puede causar error)
            return null;
        }
    }
}