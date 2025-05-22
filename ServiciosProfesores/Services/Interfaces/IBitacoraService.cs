

using ServiciosProfesores.DTOs;
using ServiciosProfesores.Entities;
namespace ServiciosProfesores.Services.Interfaces;


public interface IBitacoraService {
    public void RegistrarAccion(UsuarioAccionDTO accion);    
}