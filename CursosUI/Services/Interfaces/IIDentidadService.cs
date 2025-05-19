using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursosUI.DTOs;

namespace CursosUI.Services.Interfaces
{
    public interface IIdentidadService
    {        
        public Task<RespuestaValidacionUsuarioDTO> 
                    ValidarUsuario(PeticionInicioSesionDTO peticionInicioSesion, String ip);
    }
}