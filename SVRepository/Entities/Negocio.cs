
namespace SVRepository.Entities
{
    /// <summary>
    /// Representa la información de configuración del negocio en el sistema.
    /// </summary>
    /// <remarks>
    /// Esta entidad contiene todos los datos de identificación y configuración
    /// del negocio, incluyendo información fiscal, de contacto y configuración
    /// del sistema como símbolo de moneda y logo.
    /// </remarks>
    public class Negocio
    {
        /// <summary>
        /// Identificador único del negocio.
        /// </summary>
        /// <remarks>
        /// Este valor es generado automáticamente por la base de datos
        /// y se utiliza como clave primaria en la tabla de negocio.
        /// </remarks>
        public int IdNegocio { get; set; }

        /// <summary>
        /// Razón social o nombre legal del negocio.
        /// </summary>
        /// <remarks>
        /// Este campo contiene el nombre oficial del negocio tal como
        /// aparece en los documentos fiscales y legales.
        /// </remarks>
        public string RazonSocial { get; set; } = string.Empty;

        /// <summary>
        /// Registro Federal de Contribuyentes del negocio.
        /// </summary>
        /// <remarks>
        /// Número fiscal único que identifica al negocio ante las autoridades fiscales.
        /// Se utiliza para la emisión de facturas y reportes fiscales.
        /// </remarks>
        public string RFC { get; set; } = string.Empty;

        /// <summary>
        /// Dirección física del negocio.
        /// </summary>
        /// <remarks>
        /// Dirección completa donde se encuentra ubicado el negocio,
        /// incluyendo calle, número, colonia, ciudad y código postal.
        /// </remarks>
        public string Direccion { get; set; } = string.Empty;

        /// <summary>
        /// Número de teléfono celular del negocio.
        /// </summary>
        /// <remarks>
        /// Número de contacto principal del negocio para comunicación
        /// con clientes y proveedores.
        /// </remarks>
        public string Celular { get; set; } = string.Empty;

        /// <summary>
        /// Dirección de correo electrónico del negocio.
        /// </summary>
        /// <remarks>
        /// Email oficial del negocio para comunicación electrónica
        /// y envío de facturas o reportes.
        /// </remarks>
        public string Correo { get; set; } = string.Empty;

        /// <summary>
        /// Símbolo de la moneda utilizada en el negocio.
        /// </summary>
        /// <remarks>
        /// Símbolo que se utilizará para mostrar los precios en el sistema
        /// (ej: $, €, ¥, etc.).
        /// </remarks>
        public string SimboloMoneda { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del archivo del logo del negocio.
        /// </summary>
        /// <remarks>
        /// Nombre del archivo de imagen que contiene el logo del negocio,
        /// utilizado para mostrar en facturas, reportes y la interfaz del sistema.
        /// </remarks>
        public string NombreLogo { get; set; } = string.Empty;

        /// <summary>
        /// URL del sitio web del negocio.
        /// </summary>
        /// <remarks>
        /// Dirección web oficial del negocio, utilizada para referencia
        /// en documentos y comunicación con clientes.
        /// </remarks>
        public string URL { get; set; } = string.Empty;
    }
}
