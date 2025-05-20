
using SVServices.Recursos.Cloudinary;

namespace SVServices.Interfaces
{
    public interface ICloudinaryService
    {
        Task<CloudinaryResponse> SubirImagen(string nombreImagen, Stream formatoImagen);
        Task<bool> EliminarImagen(string publicId);
    }
}
