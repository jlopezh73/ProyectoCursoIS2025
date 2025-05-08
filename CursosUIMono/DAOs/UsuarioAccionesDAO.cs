using System;
using System.Data.SqlClient;
using CursosUIMono.Entities;

namespace CursosUIMono.DAOs
{
    public class UsuarioAccionesDAO
    {
        private readonly CursosContext _context;

        public UsuarioAccionesDAO(CursosContext context)
        {
            _context = context;
        }
        

        public void AgregarUsuarioAccion(Usuario_Accion usuarioAccion)
        {
            _context.Usuario_Accions.Add(usuarioAccion);
            _context.SaveChanges();
        }
    }    
}