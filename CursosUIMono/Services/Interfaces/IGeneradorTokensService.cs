using CursosUIMono.DTOs;

namespace CursosUIMono.Services.Interfaces;

public interface IGeneradorTokensService {
    public String GenerarToken(UsuarioDTO usuario, String key, int noHoras, int idUsuarioSesion);
}