

using ServiciosCursos.DTOs;
using ServiciosCursos.Entities;
namespace ServiciosCursos.Services.Interfaces;


public interface IBitacoraService {
    public void RegistrarAccion(UsuarioAccionDTO accion);    
}