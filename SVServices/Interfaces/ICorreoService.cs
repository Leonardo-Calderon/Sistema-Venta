
namespace SVServices.Interfaces
{
    public interface ICorreoService
    {
        Task Enviar(string para, string asunto, string mensajeHTML);
    }
}
