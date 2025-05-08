using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursosUIMono.DAOs;
using CursosUIMono.DTOs;
using CursosUIMono.Entities;
using CursosUIMono.Services.Interfaces;

namespace CursosUIMono.Services.Providers;

class SesionesService : ISesionesService {
    private UsuarioSesionesDAO _dao;
    private IBitacoraService _bitacoraService;
    private IGeneradorTokensService _generadorTokensService;    
    private int noHoras;
    private String key;
    public SesionesService(UsuarioSesionesDAO dao, 
                           IConfiguration configuration,
                           IBitacoraService bitacoraService, 
                           IGeneradorTokensService _generadorTokensService) {
        this._dao = dao;
        this._bitacoraService = bitacoraService;
        this._generadorTokensService = _generadorTokensService;
        
         noHoras = Int32.Parse(configuration["JWTSettings:Duration"]);
         key = (String) configuration["JWTSettings:Key"];
    }

    public UsuarioSesionDTO? BuscarUltimaSesion(UsuarioDTO usuario) {        
        var sesion =  _dao.BuscarSesionActiva(usuario.ID, noHoras);        
        if (sesion != null) {
            return new UsuarioSesionDTO() {
                ID = sesion.ID,
                FechaInicio = sesion.FechaInicio,
                FechaUltimoAcceso = sesion.FechaUltimoAcceso,
                DireccionIP = sesion.DireccionIP,
                Token = sesion.Token                
            };
        } else {
            return null;
        }
        
    }

    public UsuarioSesionDTO? GenerarSesion(UsuarioDTO usuario, 
                               string ip) {
        var sesion =  _dao.GenerarSesion(usuario.ID, ip);                        
        //sesion.Token = Guid.NewGuid().ToString();
        
        sesion.Token = _generadorTokensService.GenerarToken(usuario,key, noHoras,sesion.ID);

        var sesionDTO = new UsuarioSesionDTO() {
            ID = sesion.ID,
            IDUsuario = sesion.IDUsuario,            
            FechaInicio = sesion.FechaInicio,
            FechaUltimoAcceso = sesion.FechaUltimoAcceso,
            DireccionIP = sesion.DireccionIP,
            Token = sesion.Token            
        };
        
        AsignarTokenSesion(sesionDTO, sesion.Token);
        return sesionDTO;
    }

    private void AsignarTokenSesion(UsuarioSesionDTO sesion, string token) {
        _dao.AsignarTokenSesion(sesion.ID, token);
        _bitacoraService.RegistrarAccion(
            new UsuarioAccionDTO() {
                FechaHora = DateTime.Now,
                IDUsuario = sesion.IDUsuario, 
                IDUsuarioSesion = sesion.ID, 
                Accion = "Inicio de sesión"
            });
    }
    
}