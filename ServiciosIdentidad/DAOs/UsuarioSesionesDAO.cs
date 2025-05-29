using System;
using System.Data.SqlClient;
using ServiciosIdentidad.Entities;

namespace ServiciosIdentidad.DAOs
{
    public class UsuarioSesionesDAO
    {
        private readonly CursosContext _context;
        private ILogger<UsuarioSesionesDAO> _ilogger;

        public UsuarioSesionesDAO(ILogger<UsuarioSesionesDAO> ilogger, CursosContext context)
        {
            _context = context;
            _ilogger = ilogger;
        }
        

        public Usuario_Sesion? BuscarSesionActiva(int idUsuario, int horas) {
            var hora = DateTime.Now.AddHours(-horas);
            var sesion = _context.Usuario_Sesions
                    .Where(us => us.IDUsuario == idUsuario && us.FechaInicio >= hora)
                    .OrderByDescending(us => us.FechaInicio)
                    .FirstOrDefault();  
            return sesion;        
        }

        public Usuario_Sesion? GenerarSesion(int idUsuario, string ip) {
            Usuario_Sesion sesion = new Usuario_Sesion() {
                IDUsuario = idUsuario,
                FechaInicio = DateTime.Now,
                FechaUltimoAcceso = DateTime.Now,
                DireccionIP = ip
            };
            _ilogger.LogInformation($"Generando sesión para el usuario {idUsuario} con IP {ip} en la fecha {sesion.FechaInicio} y fecha de último acceso {sesion.FechaUltimoAcceso}.");
            _context.Usuario_Sesions.Add(sesion);
            _context.SaveChanges();        
            return sesion;
        }

        public void AsignarTokenSesion(int IDSesion, string token) {
            var sesion = _context.Usuario_Sesions.Find(IDSesion);
            sesion.Token = token;
            _context.SaveChanges();            
        }
    }    
}