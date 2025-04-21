using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CursosUIMono.DTOs;
using CursosUIMono.DAOs;
using CursosUIMono.Entities;
using CursosUIMono.Services.Interfaces;

namespace CursosUIMono.Services.Implementations
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