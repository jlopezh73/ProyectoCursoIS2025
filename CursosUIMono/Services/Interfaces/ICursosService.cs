using CursosUIMono.DTOs;

namespace CursosUIMono.Services.Interfaces
{
    public interface ICursosService
    {
        // Define los métodos que implementará la clase CursosService
        Task<List<CursoDTO>> ObtenerTodosLosCursosAsync();
        Task<CursoDTO> ObtenerCursoPorIdAsync(int id);
        Task CrearCursoAsync(CursoDTO c);
        Task ActualizarCursoAsync(CursoDTO cursoActualizado);
        Task EliminarCursoAsync(int id);
    }
}