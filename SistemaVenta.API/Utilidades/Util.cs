// En: SistemaVenta.API/Utilidades/Util.cs
using SVRepository.Entities;
using System.Security.Cryptography;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers; // Necesario para los colores estándar

namespace SistemaVenta.API.Utilidades
{
    /// <summary>
    /// Clase estática que proporciona métodos de utilidad para la API del Sistema de Ventas.
    /// </summary>
    /// <remarks>
    /// Esta clase contiene métodos de utilidad que se utilizan en toda la API,
    /// incluyendo generación de códigos, encriptación de contraseñas y generación
    /// de documentos PDF para tickets de venta.
    /// 
    /// Los métodos están diseñados para ser thread-safe y eficientes para uso
    /// en un entorno de API web.
    /// </remarks>
    public static class Util
    {
        /// <summary>
        /// Genera un código aleatorio único de 8 caracteres basado en un GUID.
        /// </summary>
        /// <returns>Un código único de 8 caracteres alfanuméricos.</returns>
        /// <remarks>
        /// Este método genera un GUID, lo convierte a una cadena sin guiones y
        /// toma los primeros 8 caracteres. Es útil para generar códigos únicos
        /// para productos, ventas, etc. en la API.
        /// 
        /// El código generado es thread-safe y garantiza unicidad en la práctica.
        /// </remarks>
        public static string GenerarCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        /// <summary>
        /// Convierte una cadena de texto a su representación hash SHA256.
        /// </summary>
        /// <param name="input">La cadena de texto a convertir.</param>
        /// <returns>El hash SHA256 de la cadena de entrada como una cadena hexadecimal.</returns>
        /// <remarks>
        /// Este método utiliza el algoritmo SHA256 para generar un hash seguro
        /// de la cadena de entrada. Es especialmente útil para encriptar contraseñas
        /// antes de almacenarlas en la base de datos.
        /// 
        /// Nota: Para contraseñas en producción, se recomiendan algoritmos más
        /// seguros como BCrypt, Argon2 o PBKDF2 que incluyen salt y son más
        /// resistentes a ataques de fuerza bruta.
        /// 
        /// El resultado es una cadena hexadecimal de 64 caracteres que representa
        /// el hash SHA256 de la entrada.
        /// </remarks>
        public static string ConvertirASha256(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder resultado = new StringBuilder();
                foreach (byte b in hash)
                {
                    resultado.Append(b.ToString("x2"));
                }
                return resultado.ToString();
            }
        }

        /// <summary>
        /// Genera un documento PDF para un ticket de venta usando QuestPDF.
        /// </summary>
        /// <param name="oNegocio">Información del negocio que se incluirá en el PDF.</param>
        /// <param name="oVenta">Información de la venta que se incluirá en el PDF.</param>
        /// <param name="imageLogo">Stream que contiene el logo del negocio.</param>
        /// <returns>Un array de bytes que representa el documento PDF generado.</returns>
        /// <remarks>
        /// Este método utiliza la biblioteca QuestPDF para generar un documento PDF
        /// profesional para tickets de venta que incluye:
        /// - Encabezado con logo del negocio e información de contacto
        /// - Información del cliente y fecha de la venta
        /// - Tabla detallada de productos vendidos con precios y cantidades
        /// - Totales de la venta
        /// - Diseño profesional optimizado para impresión
        /// 
        /// El PDF se genera con un diseño limpio y profesional que es adecuado
        /// para tickets de venta físicos o digitales.
        /// </remarks>
        public static byte[] GeneratePDFVenta(Negocio oNegocio, Venta oVenta, Stream imageLogo)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);

                    // Cabecera del Documento
                    page.Header().ShowOnce().Row(row =>
                    {
                        row.AutoItem().Height(60).Image(imageLogo, ImageScaling.FitArea);

                        row.RelativeItem().PaddingLeft(15).Column(column =>
                        {
                            column.Item().Text(oNegocio.RazonSocial).FontSize(14).Bold();
                            column.Item().Text(oNegocio.Direccion).FontSize(9);
                            column.Item().Text($"Teléfono: {oNegocio.Celular}").FontSize(9);
                            column.Item().Text($"Correo: {oNegocio.Correo}").FontSize(9);
                        });

                        row.ConstantItem(140).Column(column =>
                        {
                            column.Item().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text($"RFC: {oNegocio.RFC}");
                            column.Item().Background(Colors.Grey.Lighten1).Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("TICKET DE VENTA");
                            column.Item().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(oVenta.NumeroVenta);
                        });
                    });

                    // Contenido del Documento
                    page.Content().PaddingVertical(10).Column(column =>
                    {
                        column.Spacing(10);
                        column.Item().LineHorizontal(0.5f);

                        // Datos del Cliente y Fecha
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text(txt =>
                            {
                                txt.Span("Cliente: ").SemiBold().FontSize(10);
                                txt.Span(oVenta.NombreCliente).FontSize(10);
                            });
                            row.RelativeItem().Text(txt =>
                            {
                                txt.Span("Fecha: ").SemiBold().FontSize(10);
                                txt.Span(oVenta.FechaRegistro).FontSize(10);
                            });
                        });

                        column.Item().LineHorizontal(0.5f);

                        // Tabla de Productos
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Descripción del producto
                                columns.RelativeColumn();   // Precio
                                columns.RelativeColumn();   // Cantidad
                                columns.RelativeColumn();   // Total
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten1).Padding(2).Text("Producto");
                                header.Cell().Background(Colors.Grey.Lighten1).Padding(2).AlignRight().Text("Precio");
                                header.Cell().Background(Colors.Grey.Lighten1).Padding(2).AlignCenter().Text("Cantidad");
                                header.Cell().Background(Colors.Grey.Lighten1).Padding(2).AlignRight().Text("Total");
                            });

                            foreach (var item in oVenta.RefDetalleVenta)
                            {
                                decimal cantidad = Convert.ToDecimal(item.Cantidad) / Convert.ToDecimal(item.RefProducto.RefCategoria.RefMedida.Valor);
                                string abreviatura = item.RefProducto.RefCategoria.RefMedida.Abreviatura;

                                table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).Text(item.RefProducto.Descripcion).FontSize(9);
                                table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).AlignRight().Text($"{oNegocio.SimboloMoneda}{item.PrecioVenta:N2}").FontSize(9);
                                table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).AlignCenter().Text($"{cantidad} {abreviatura}").FontSize(9);
                                table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(2).AlignRight().Text($"{oNegocio.SimboloMoneda}{item.PrecioTotal:N2}").FontSize(9);
                            }
                        });

                        // Total General
                        column.Item().PaddingTop(5).AlignRight().Text($"TOTAL: {oNegocio.SimboloMoneda}{oVenta.precioTotal:N2}").FontSize(12).Bold();
                    });

                    // Pie de Página
                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Página ").FontSize(9);
                        txt.CurrentPageNumber().FontSize(9);
                        txt.Span(" de ").FontSize(9);
                        txt.TotalPages().FontSize(9);
                    });
                });
            }).GeneratePdf();
        }
    }
}