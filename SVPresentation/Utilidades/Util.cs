using SVRepository.Entities;
using System.Security.Cryptography;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace SVPresentation.Utilidades
{
    /// <summary>
    /// Clase estática que proporciona métodos de utilidad para la aplicación.
    /// </summary>
    /// <remarks>
    /// Esta clase contiene métodos de utilidad que se utilizan en toda la aplicación,
    /// incluyendo generación de códigos, encriptación de contraseñas y generación
    /// de documentos PDF.
    /// </remarks>
    public static class Util
    {
        /// <summary>
        /// Genera un código único de 8 caracteres basado en un GUID.
        /// </summary>
        /// <returns>Un código único de 8 caracteres alfanuméricos.</returns>
        /// <remarks>
        /// Este método genera un GUID, lo convierte a una cadena sin guiones y
        /// toma los primeros 8 caracteres. Es útil para generar códigos únicos
        /// para productos, ventas, etc.
        /// </remarks>
        public static string GenerarCode()
        {
            string guid = Guid.NewGuid().ToString("N").Substring(0, 8);
            return guid;
        }

        /// <summary>
        /// Convierte una cadena de texto en su representación codificada con SHA256.
        /// </summary>
        /// <param name="input">La cadena de texto a codificar.</param>
        /// <returns>El texto codificado en SHA256 como una cadena hexadecimal.</returns>
        /// <remarks>
        /// Este método utiliza el algoritmo SHA256 para generar un hash seguro
        /// de la cadena de entrada. Es especialmente útil para encriptar contraseñas
        /// antes de almacenarlas en la base de datos.
        /// 
        /// El resultado es una cadena hexadecimal de 64 caracteres que representa
        /// el hash SHA256 de la entrada.
        /// </remarks>
        public static string ConvertirASha256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input); // Convierte la cadena a bytes.
                byte[] hash = sha256.ComputeHash(bytes); // Calcula el hash SHA256.

                // Convierte el hash a una cadena hexadecimal.
                StringBuilder resultado = new StringBuilder();
                foreach (byte b in hash)
                {
                    resultado.Append(b.ToString("x2"));
                }

                return resultado.ToString();
            }
        }

        /// <summary>
        /// Genera un documento PDF de venta con la información del negocio y la venta.
        /// </summary>
        /// <param name="oNegocio">Información del negocio que se incluirá en el PDF.</param>
        /// <param name="oVenta">Información de la venta que se incluirá en el PDF.</param>
        /// <param name="imageLogo">Stream que contiene el logo del negocio.</param>
        /// <returns>Un array de bytes que representa el documento PDF generado.</returns>
        /// <remarks>
        /// Este método utiliza la biblioteca QuestPDF para generar un documento PDF
        /// profesional que incluye:
        /// - Encabezado con logo del negocio e información de contacto
        /// - Información del cliente y fecha de emisión
        /// - Tabla detallada de productos vendidos
        /// - Totales de la venta
        /// 
        /// El PDF se genera con un diseño profesional y colores corporativos.
        /// </remarks>
        public static byte[] GeneratePDFVenta(Negocio oNegocio, Venta oVenta, Stream imageLogo)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var arrayPDF = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(50);
                    page.Header().ShowOnce().Row(row =>
                    {
                        row.AutoItem().Height(60).Image(imageLogo, ImageScaling.FitArea);
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text(oNegocio.RazonSocial).FontSize(14).Bold();
                            column.Item().Text(oNegocio.Direccion).FontSize(9);
                            column.Item().Text($"Teléfono: {oNegocio.Celular}").FontSize(9);
                            column.Item().Text($"Correo: {oNegocio.Correo}").FontSize(9);
                        });
                        row.ConstantItem(140).Column(column =>
                        {
                            column.Item().Border(1).BorderColor("#2c6ecb").AlignCenter().Text($"RFC: {oNegocio.RFC}");
                            column.Item().Background("#2c6ecb").Border(1).BorderColor("#2c6ecb").AlignCenter().Text($"Folio:").FontColor("#fff");
                            column.Item().Border(1).BorderColor("#2c6ecb").AlignCenter().Text(oVenta.NumeroVenta);
                        });
                    });
                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(10);
                        column.Item().LineHorizontal(0.5f);
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text(txt =>
                            {
                                txt.Span("Cliente: ").SemiBold().FontSize(10);
                                txt.Span(oVenta.NombreCliente).FontSize(10);
                            });
                            row.RelativeItem().Text(txt =>
                            {
                                txt.Span("Fecha Emisión: ").SemiBold().FontSize(10);
                                txt.Span(oVenta.FechaRegistro).FontSize(10);
                            });
                        });
                        column.Item().LineHorizontal(0.5f);
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });
                            table.Header(header =>
                            {
                                header.Cell().Background("#2c6ecb").Padding(2).Text("Producto").FontColor("#fff");
                                header.Cell().Background("#2c6ecb").Padding(2).Text("Precio").FontColor("#fff");
                                header.Cell().Background("#2c6ecb").Padding(2).Text("Cantidad").FontColor("#fff");
                                header.Cell().Background("#2c6ecb").Padding(2).Text("Total").FontColor("#fff");
                            });
                            foreach (var item in oVenta.RefDetalleVenta)
                            {
                                decimal cantidad = Convert.ToDecimal(item.Cantidad)/Convert.ToDecimal(item.RefProducto.RefCategoria.RefMedida.Valor);
                                string abreviatura = item.RefProducto.RefCategoria.RefMedida.Abreviatura;

                                table.Cell().BorderBottom(0.5f).BorderColor("#313131").Padding(2).Text(item.RefProducto.Descripcion).FontSize(10);
                                table.Cell().BorderBottom(0.5f).BorderColor("#313131").Padding(2).Text($"{oNegocio.SimboloMoneda}: {item.PrecioVenta.ToString("0.00")}").FontSize(10);
                                table.Cell().BorderBottom(0.5f).BorderColor("#313131").Padding(2).Text($"{cantidad.ToString()} {abreviatura}").FontSize(10);
                                table.Cell().BorderBottom(0.5f).BorderColor("#313131").Padding(2).Text($"{oNegocio.SimboloMoneda}: {item.PrecioTotal.ToString("0.00")}").FontSize(10);
                            }

                        });
                        column.Item().AlignRight().Text($"{oNegocio.SimboloMoneda} {oVenta.precioTotal.ToString("0.00")}").FontSize(10);
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Pagina").Italic().FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            });

            return arrayPDF.GeneratePdf();
        }
    }
}
