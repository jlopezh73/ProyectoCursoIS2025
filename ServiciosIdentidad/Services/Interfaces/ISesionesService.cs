using ServiciosIdentidad.DTOs;

namespace ServiciosIdentidad.Services.Interfaces;

public interface ISesionesService {
    public UsuarioSesionDTO? BuscarUltimaSesion(UsuarioDTO usuario);
    public UsuarioSesionDTO? GenerarSesion(UsuarioDTO usuario, string ip);    
}