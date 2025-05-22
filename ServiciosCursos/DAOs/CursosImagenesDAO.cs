using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiciosCursos.Entities;
using Microsoft.EntityFrameworkCore;

namespace ServiciosCursos.DAOs
{
    public class CursosImagenesDAO
    {
        private readonly CursosContext _context;

        public CursosImagenesDAO(CursosContext context)
        {
            _context = context;
        }        
        

        // Obtener un curso imagen por ID
        public async Task<CursoImagen> ObtenerPorIdCursoAsync(int id)
        {
            return await _context.CursoImagens                
                .Where( c=> c.idCurso == id).FirstOrDefaultAsync();
        }

        // Agregar o cambiar un nuevo curso
        public async Task AsignarImagenAsync(int idCurso, string archivo, byte[] imagen)
        {
            var cursoImagen = await _context.CursoImagens.Where(ci => ci.idCurso == idCurso).FirstOrDefaultAsync();
            if (cursoImagen == null)
            {
                cursoImagen = new CursoImagen
                {
                    idCurso = idCurso,
                    archivo = archivo,
                    contenido = imagen
                };
                await _context.CursoImagens.AddAsync(cursoImagen);
            }
            else
            {
                cursoImagen.archivo = archivo;
                cursoImagen.contenido = imagen;
                _context.CursoImagens.Update(cursoImagen);
            }
            await _context.SaveChangesAsync();
        }

    }
}