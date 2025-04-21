using CursosUIMono.DTOs;

namespace CursosUIMono.Services.Interfaces
{
    public interface IProfesoresService
    {
        // Define los métodos que implementará la clase ProfesoresService
        Task<List<ProfesorDTO>> ObtenerTodosLosProfesoresAsync();
        Task<ProfesorDTO> ObtenerProfesorPorIdAsync(int id);
        Task CrearProfesorAsync(ProfesorDTO p);
        Task ActualizarProfesorAsync(ProfesorDTO profesorActualizado);
        Task EliminarProfesorAsync(int id);
    }
}