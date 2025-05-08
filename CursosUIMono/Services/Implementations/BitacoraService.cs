

using System.Text;
using System.Text.Json;
using CursosUIMono.Services.Interfaces;
using CursosUIMono.DTOs;
using CursosUIMono.DAOs;
using CursosUIMono.Entities;

namespace AppServiciosIdentidad.Gateways.Providers;
public class BitacoraService : IBitacoraService {
    private readonly UsuarioAccionesDAO _dao;
    private readonly ILogger<BitacoraService> _logger;
    public BitacoraService(ILogger<BitacoraService> logger,
                           UsuarioAccionesDAO dao) {
        _logger = logger;
        _dao = dao;
        
    }
    public void RegistrarAccion(UsuarioAccionDTO accion) {        
        _dao.AgregarUsuarioAccion(
            new Usuario_Accion() {
                IDUsuario = accion.IDUsuario,
                FechaHora = accion.FechaHora,
                IDUsuarioSesion = accion.IDUsuarioSesion,
                Accion = accion.Accion
            });
    }
}