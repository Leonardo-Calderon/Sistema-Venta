

using SVRepository.Entities;

namespace SVServices.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> Lista(string buscar = "");
        Task<String> Crear(Usuario objeto);
        Task<String> Editar(Usuario objeto);
        Task<string> Eliminar(int idUsuario);
        Task<Usuario> Login(string usuario, string clave);
        Task<int> VerificarCorreo(string correo);
        Task ActualizarClave(int idUsuario, string nuevaClave, int resetear);
    }
}
