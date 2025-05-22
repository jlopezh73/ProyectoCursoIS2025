using ServiciosCursos.DTOs;

namespace ServiciosCursos.Services.Interfaces
{
    public interface IProfesoresService
    {
        // Define los métodos que implementará la clase ProfesoresService
        Task<String> ObtenerNombreProfesorPorIdAsync(int id);        
    }
}