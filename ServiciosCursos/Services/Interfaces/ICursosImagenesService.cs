using ServiciosCursos.DTOs;

namespace ServiciosCursos.Services.Interfaces
{
    public interface ICursosImagenesService
    {
        // Define los métodos que implementará la clase CursosService
        Task<CursoImagenDTO> ObtenerCursoImagenPorIdAsync(int id);
        Task AsignarCursoImagenAsync(CursoImagenDTO c);        
        Task<Boolean> ExisteCursoImagenPorIdAsync(int idCurso);
    }
}