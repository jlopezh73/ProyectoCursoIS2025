using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursosUIMono.Entities;
using Microsoft.EntityFrameworkCore;

namespace CursosUIMono.DAOs
{
    public class ParticipantesDAO
    {
        private readonly CursosContext _context;

        public ParticipantesDAO(CursosContext context)
        {
            _context = context;
        }

        // Obtener todos los profesores
        public async Task<List<Participante>> ObtenerTodosAsync()
        {
            return await _context.Participantes
                .OrderBy(p => p.nombre)
                .ToListAsync();
        }

        // Obtener un profesor por ID
        public async Task<Participante> ObtenerPorIdAsync(int id)
        {
            return await _context.Participantes.FindAsync(id);
        }

        // Agregar un nuevo participante
        public async Task AgregarAsync(Participante participante)
        {
            await _context.Participantes.AddAsync(participante);
            await _context.SaveChangesAsync();
        }

        // Actualizar un participante existente
        public async Task ActualizarAsync(Participante participante)
        {
            _context.Participantes.Update(participante);
            await _context.SaveChangesAsync();
        }

        // Eliminar un participante
        public async Task EliminarAsync(int id)
        {
            var participante = await ObtenerPorIdAsync(id);
            if (participante != null)
            {
                _context.Participantes.Remove(participante);
                await _context.SaveChangesAsync();
            }
        }
    }
}