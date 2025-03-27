using System;
using System.IO;
using System.IO.Compression;
using WatanaClient.API.Exceptions;

namespace WatanaClient.API.Utils
{
    /// <summary>
    /// Utilidades para comprimir y descomprimir archivos PDF
    /// </summary>
    public static class ZipUtils
    {
        /// <summary>
        /// Comprime un PDF y lo convierte a una cadena Base64
        /// </summary>
        /// <param name="pdfBytes">Bytes del archivo PDF</param>
        /// <returns>Cadena Base64 del archivo ZIP</returns>
        /// <exception cref="WatanaException">Se lanza cuando ocurre un error durante la compresión o codificación</exception>
        public static string CompressPdfToZipBase64(byte[] pdfBytes)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
                throw new ArgumentException("Los bytes del PDF no pueden ser nulos o vacíos", nameof(pdfBytes));

            try
            {
                using var memoryStream = new MemoryStream();
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var zipEntry = archive.CreateEntry("document.pdf", CompressionLevel.Optimal);
                    using var zipStream = zipEntry.Open();
                    zipStream.Write(pdfBytes, 0, pdfBytes.Length);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new WatanaException("Error al comprimir el PDF y convertirlo a Base64", ex);
            }
        }

        /// <summary>
        /// Descomprime una cadena Base64 de un ZIP y devuelve los bytes del archivo PDF
        /// </summary>
        /// <param name="zipBase64">Cadena Base64 del archivo ZIP</param>
        /// <returns>Bytes del archivo PDF</returns>
        /// <exception cref="WatanaException">Se lanza cuando ocurre un error durante la decodificación o descompresión</exception>
        public static byte[] DecompressZipBase64ToPdf(string zipBase64)
        {
            if (string.IsNullOrEmpty(zipBase64))
                throw new ArgumentException("La cadena Base64 del ZIP no puede ser nula o vacía", nameof(zipBase64));

            try
            {
                var zipBytes = Convert.FromBase64String(zipBase64);
                using var zipStream = new MemoryStream(zipBytes);
                using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

                // Obtener la primera entrada del ZIP, que debería ser el PDF
                if (archive.Entries.Count == 0)
                    throw new WatanaException("El archivo ZIP no contiene ninguna entrada");

                var entry = archive.Entries[0];
                using var entryStream = entry.Open();
                using var memoryStream = new MemoryStream();
                entryStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
            catch (FormatException ex)
            {
                throw new WatanaException("La cadena Base64 del ZIP no es válida", ex);
            }
            catch (Exception ex) when (!(ex is WatanaException))
            {
                throw new WatanaException("Error al descomprimir el ZIP y obtener el PDF", ex);
            }
        }

        /// <summary>
        /// Convierte un archivo a Base64 sin compresión
        /// </summary>
        /// <param name="fileBytes">Bytes del archivo</param>
        /// <returns>Cadena Base64 del archivo</returns>
        public static string FileToBase64(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length == 0)
                throw new ArgumentException("Los bytes del archivo no pueden ser nulos o vacíos", nameof(fileBytes));

            try
            {
                return Convert.ToBase64String(fileBytes);
            }
            catch (Exception ex)
            {
                throw new WatanaException("Error al convertir el archivo a Base64", ex);
            }
        }

        /// <summary>
        /// Convierte una cadena Base64 a bytes
        /// </summary>
        /// <param name="base64">Cadena Base64</param>
        /// <returns>Bytes del archivo</returns>
        public static byte[] Base64ToFile(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                throw new ArgumentException("La cadena Base64 no puede ser nula o vacía", nameof(base64));

            try
            {
                return Convert.FromBase64String(base64);
            }
            catch (FormatException ex)
            {
                throw new WatanaException("La cadena Base64 no es válida", ex);
            }
            catch (Exception ex)
            {
                throw new WatanaException("Error al convertir la cadena Base64 a bytes", ex);
            }
        }
    }
}