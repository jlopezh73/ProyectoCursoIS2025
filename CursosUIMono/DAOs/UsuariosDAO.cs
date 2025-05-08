using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursosUIMono.DTOs;
using CursosUIMono.Entities;
using Microsoft.EntityFrameworkCore;

namespace CursosUIMono.DAOs
{
    public class UsuariosDAO
    {
        private readonly CursosContext _context;

        public UsuariosDAO(CursosContext context)
        {
            _context = context;
        }

        // Obtener todos los usuarios
        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _context.Usuarios
                .OrderBy(p => p.Paterno)
                .ThenBy(p => p.Materno)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        // Obtener un usuario por ID
        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        // Obtener un usuario por Email y Password
        public async Task<Usuario?> ObtenerPorPeticionAsync(PeticionInicioSesionDTO peticion)                    
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.CorreoElectronico == peticion.correoElectronico && u.Password == peticion.password);
        }
        

        // Agregar un nuevo usuario
        public async Task AgregarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        // Actualizar un usuario existente
        public async Task ActualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        // Eliminar un usuario
        public async Task EliminarAsync(int id)
        {
            var usuario = await ObtenerPorIdAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
    }
}