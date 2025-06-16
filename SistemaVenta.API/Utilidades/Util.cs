// En: SistemaVenta.API/Utilidades/Util.cs
using SVRepository.Entities;
using System.Security.Cryptography;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers; // Necesario para los colores estándar

namespace SistemaVenta.API.Utilidades
{
    public static class Util
    {
        /// <summary>
        /// Genera un código aleatorio de 8 caracteres.
        /// </summary>
        public static string GenerarCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        /// <summary>
        /// Convierte una cadena de texto a su representación SHA256.
        /// Nota: Para contraseñas, se recomiendan algoritmos como BCrypt.
        /// </summary>
        public static string ConvertirASha256(string input)
        {
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
        /// Genera un PDF para un ticket de venta usando QuestPDF.
        /// </summary>
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