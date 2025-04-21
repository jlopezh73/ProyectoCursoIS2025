using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursosUIMono.Entities;
using Microsoft.EntityFrameworkCore;

namespace CursosUIMono.DAOs
{
    public class ProfesoresDAO
    {
        private readonly CursosContext _context;

        public ProfesoresDAO(CursosContext context)
        {
            _context = context;
        }

        // Obtener todos los profesores
        public async Task<List<Profesor>> ObtenerTodosAsync()
        {
            return await _context.Profesors
                .OrderBy(p => p.nombre)
                .ToListAsync();
        }

        // Obtener un profesor por ID
        public async Task<Profesor> ObtenerPorIdAsync(int id)
        {
            return await _context.Profesors.FindAsync(id);
        }

        // Agregar un nuevo profesor
        public async Task AgregarAsync(Profesor profesor)
        {
            await _context.Profesors.AddAsync(profesor);
            await _context.SaveChangesAsync();
        }

        // Actualizar un profesor existente
        public async Task ActualizarAsync(Profesor profesor)
        {
            _context.Profesors.Update(profesor);
            await _context.SaveChangesAsync();
        }

        // Eliminar un profesor
        public async Task EliminarAsync(int id)
        {
            var profesor = await ObtenerPorIdAsync(id);
            if (profesor != null)
            {
                _context.Profesors.Remove(profesor);
                await _context.SaveChangesAsync();
            }
        }
    }
}