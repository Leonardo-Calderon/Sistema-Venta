namespace SVServices.Recursos.Cloudinary
{
    /// <summary>
    /// Representa la respuesta de una operación de Cloudinary.
    /// </summary>
    /// <remarks>
    /// Esta clase encapsula la información retornada por Cloudinary después de una operación
    /// de subida o eliminación de imagen, proporcionando el identificador público y la URL segura.
    /// </remarks>
    public class CloudinaryResponse
    {
        /// <summary>
        /// Identificador público único de la imagen en Cloudinary.
        /// </summary>
        /// <remarks>
        /// Este identificador se utiliza para referenciar la imagen en operaciones posteriores
        /// como eliminación, transformación o generación de URLs.
        /// </remarks>
        public string PublicId { get; set; } = string.Empty;

        /// <summary>
        /// URL segura (HTTPS) de la imagen almacenada en Cloudinary.
        /// </summary>
        /// <remarks>
        /// Esta URL proporciona acceso directo a la imagen a través de HTTPS,
        /// garantizando una conexión segura para el acceso a la imagen.
        /// </remarks>
        public string SecureUrl { get; set; } = string.Empty;
    }
}
