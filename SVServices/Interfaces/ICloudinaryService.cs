
using SVServices.Recursos.Cloudinary;

namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para la gestión de imágenes en Cloudinary.
    /// </summary>
    /// <remarks>
    /// Esta interfaz proporciona métodos para subir y eliminar imágenes en el servicio de almacenamiento
    /// en la nube Cloudinary. Cloudinary es una plataforma que permite gestionar, transformar y optimizar
    /// imágenes y videos en la nube.
    /// </remarks>
    public interface ICloudinaryService
    {
        /// <summary>
        /// Sube una imagen al servicio Cloudinary.
        /// </summary>
        /// <param name="nombreImagen">Nombre que se asignará a la imagen en Cloudinary.</param>
        /// <param name="formatoImagen">Stream que contiene los datos de la imagen a subir.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un objeto CloudinaryResponse con la información de la imagen subida.</returns>
        /// <remarks>
        /// Este método sube una imagen al servicio Cloudinary y retorna información sobre la imagen
        /// subida, incluyendo el identificador público y la URL segura para acceder a la imagen.
        /// </remarks>
        Task<CloudinaryResponse> SubirImagen(string nombreImagen, Stream formatoImagen);

        /// <summary>
        /// Elimina una imagen del servicio Cloudinary.
        /// </summary>
        /// <param name="publicId">Identificador público de la imagen a eliminar.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es true si la eliminación fue exitosa, false en caso contrario.</returns>
        /// <remarks>
        /// Este método elimina permanentemente una imagen del servicio Cloudinary utilizando
        /// su identificador público. La eliminación es irreversible.
        /// </remarks>
        Task<bool> EliminarImagen(string publicId);
    }
}
