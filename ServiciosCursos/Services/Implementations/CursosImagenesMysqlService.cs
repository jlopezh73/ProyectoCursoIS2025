using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiciosCursos.DTOs;
using ServiciosCursos.DAOs;
using ServiciosCursos.Entities;
using ServiciosCursos.Services.Interfaces;

namespace ServiciosCursos.Services.Implementations
{
    public class CursosImagenesMysqlService : ICursosImagenesService
    {
        private readonly CursosImagenesDAO _dao;

        public CursosImagenesMysqlService(CursosImagenesDAO dao)
        {
            _dao = dao;            
        }

        public async Task<CursoImagenDTO> ObtenerCursoImagenPorIdAsync(int id)
        {
            var c = await _dao.ObtenerPorIdCursoAsync(id);
            return  new CursoImagenDTO
            {                
                archivo = c.archivo,
                contenido = c.contenido,                
            };
        }

        public async Task AsignarCursoImagenAsync(CursoImagenDTO c)
        {            
            await _dao.AsignarImagenAsync(c.idCurso, c.archivo, c.contenido);
        }

        public async Task<bool> ExisteCursoImagenPorIdAsync(int idCurso)
        {
            var c = await _dao.ObtenerPorIdCursoAsync(idCurso);
            return c != null;
        }
        
    }
}