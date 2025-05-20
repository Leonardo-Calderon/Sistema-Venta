using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVServices.Implementation
{
    internal class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository )
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<List<Usuario>> Lista(string buscar = "")
        {
            return await _usuarioRepository.Lista(buscar);
        }
        public async Task<string> Crear(Usuario objeto)
        {
            return await _usuarioRepository.Crear(objeto);
        }

        public async Task<string> Editar(Usuario objeto)
        {
            return await _usuarioRepository.Editar(objeto);
        }

        public async Task<string> Eliminar(int idUsuario)
        {
            return await _usuarioRepository.Eliminar(idUsuario);
        }

        public async Task<Usuario> Login(string usuario, string clave)
        {
            return await _usuarioRepository.Login(usuario, clave);
        }

        public async Task<int> VerificarCorreo(string correo)
        {
            return await _usuarioRepository.VerificarCorreo(correo);
        }

        public async Task ActualizarClave(int idUsuario, string nuevaClave, int resetear)
        {
            await _usuarioRepository.ActualizarClave(idUsuario, nuevaClave, resetear);
        }
    }

}
