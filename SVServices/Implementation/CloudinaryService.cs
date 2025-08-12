using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using SVServices.Interfaces;
using SVServices.Recursos.Cloudinary;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para la gestión de imágenes en Cloudinary.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz ICloudinaryService y proporciona la funcionalidad
    /// para subir y eliminar imágenes en el servicio de almacenamiento en la nube Cloudinary.
    /// Cloudinary es una plataforma que permite gestionar, transformar y optimizar
    /// imágenes y videos en la nube.
    /// </remarks>
    public class CloudinaryService : ICloudinaryService
    {
        /// <summary>
        /// Instancia de configuración para acceder a los parámetros de Cloudinary.
        /// </summary>
        private readonly IConfiguration _configuracion;

        /// <summary>
        /// Cliente de Cloudinary para realizar operaciones con imágenes.
        /// </summary>
        private readonly Cloudinary _cloudinary;

        /// <summary>
        /// Inicializa una nueva instancia de la clase CloudinaryService.
        /// </summary>
        /// <param name="configuracion">Instancia de configuración que contiene los parámetros de Cloudinary.</param>
        /// <remarks>
        /// El constructor lee la configuración de Cloudinary desde el archivo de configuración
        /// y inicializa el cliente de Cloudinary con las credenciales correspondientes.
        /// </remarks>
        public CloudinaryService(IConfiguration configuracion)
        {
            _configuracion = configuracion ?? throw new ArgumentNullException(nameof(configuracion));

            var cloudName = _configuracion["Cloudinary:CloudName"] ?? throw new InvalidOperationException("La configuración Cloudinary:CloudName no está definida.");
            var apiKey = _configuracion["Cloudinary:ApiKey"] ?? throw new InvalidOperationException("La configuración Cloudinary:ApiKey no está definida.");
            var apiSecret = _configuracion["Cloudinary:ApiSecret"] ?? throw new InvalidOperationException("La configuración Cloudinary:ApiSecret no está definida.");

            _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
        }

        /// <summary>
        /// Sube una imagen al servicio Cloudinary.
        /// </summary>
        /// <param name="nombreImagen">Nombre que se asignará a la imagen en Cloudinary.</param>
        /// <param name="formatoImagen">Stream que contiene los datos de la imagen a subir.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un objeto CloudinaryResponse con la información de la imagen subida.</returns>
        /// <remarks>
        /// Este método sube una imagen al servicio Cloudinary y retorna información sobre la imagen
        /// subida, incluyendo el identificador público y la URL segura para acceder a la imagen.
        /// Las imágenes se almacenan en la carpeta "SistemaDeVenta" para mejor organización.
        /// </remarks>
        public async Task<CloudinaryResponse> SubirImagen(string nombreImagen, Stream formatoImagen)
        {
            if (string.IsNullOrWhiteSpace(nombreImagen))
                throw new ArgumentException("El nombre de la imagen no puede estar vacío.", nameof(nombreImagen));

            if (formatoImagen == null)
                throw new ArgumentNullException(nameof(formatoImagen));

            var cloudinaryResponse = new CloudinaryResponse();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(nombreImagen, formatoImagen),
                AssetFolder = "SistemaDeVenta",
            };

            try
            {
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    cloudinaryResponse.PublicId = uploadResult.PublicId;
                    cloudinaryResponse.SecureUrl = uploadResult.SecureUrl.ToString();
                }
                else
                {
                    cloudinaryResponse.PublicId = string.Empty;
                    cloudinaryResponse.SecureUrl = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al subir la imagen a Cloudinary: {ex.Message}", ex);
            }

            return cloudinaryResponse;
        }

        /// <summary>
        /// Elimina una imagen del servicio Cloudinary.
        /// </summary>
        /// <param name="publicId">Identificador público de la imagen a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es true si la eliminación fue exitosa, false en caso contrario.</returns>
        /// <remarks>
        /// Este método elimina permanentemente una imagen del servicio Cloudinary utilizando
        /// su identificador público. La eliminación es irreversible.
        /// </remarks>
        public async Task<bool> EliminarImagen(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                throw new ArgumentException("El identificador público de la imagen no puede estar vacío.", nameof(publicId));

            try
            {
                var deleteParams = new DeletionParams(publicId);
                var deleteResult = await _cloudinary.DestroyAsync(deleteParams);

                return deleteResult.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al eliminar la imagen de Cloudinary: {ex.Message}", ex);
            }
        }
    }
}
