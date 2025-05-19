using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CursosUI.DTOs;
using CursosUI.Services.Interfaces;

namespace CursosUI.Services.Implementations
{
    public class CursosImagenesMysqlService : ICursosImagenesService
    {
        //private readonly CursosImagenesDAO _dao;

        public CursosImagenesMysqlService() //CursosImagenesDAO dao)
        {
            //_dao = dao;            
        }

        public async Task<CursoImagenDTO> ObtenerCursoImagenPorIdAsync(int id)
        {
            //var c = await _dao.ObtenerPorIdCursoAsync(id);
            return  new CursoImagenDTO
            {                
                //archivo = c.archivo,
                //scontenido = c.contenido,                
            };
        }

        public async Task AsignarCursoImagenAsync(CursoImagenDTO c)
        {            
            //await _dao.AsignarImagenAsync(c.idCurso, c.archivo, c.contenido);
        }

        public async Task<bool> ExisteCursoImagenPorIdAsync(int idCurso)
        {
            //var c = await _dao.ObtenerPorIdCursoAsync(idCurso);
            //return c != null;
            return false;
        }
        
    }
}