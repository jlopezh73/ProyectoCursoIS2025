using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiciosIdentidad.DTOs;

namespace ServiciosIdentidad.Services.Interfaces
{
    public interface IIdentidadService
    {
        public Task<RespuestaValidacionUsuarioDTO>
                    ValidarUsuario(PeticionInicioSesionDTO peticionInicioSesion, String ip);
        public Task<List<UsuarioDTO>> ObtenerTodosLosUsuariosAsync();
        public Task CrearUsuarioAsync(UsuarioDTO usuarioDTO);
        public Task<UsuarioDTO> ObtenerUsuarioPorIdAsync(int idUsuario);
        public Task ActualizarUsuarioAsync(UsuarioDTO usuarioDTO);
        public Task EliminarUsuarioAsync(int idUsuario);
    }
}