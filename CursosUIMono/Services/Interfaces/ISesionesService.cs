using CursosUIMono.DTOs;

namespace CursosUIMono.Services.Interfaces;

public interface ISesionesService {
    public UsuarioSesionDTO? BuscarUltimaSesion(UsuarioDTO usuario);
    public UsuarioSesionDTO? GenerarSesion(UsuarioDTO usuario, string ip);    
}