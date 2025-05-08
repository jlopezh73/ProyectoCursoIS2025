using System.Text;
using System.Security.Cryptography;
using CursosUIMono.DTOs;
using CursosUIMono.Services.Interfaces;
using CursosUIMono.DAOs;
using System.Threading.Tasks;

namespace CursosUIMono.Services.Implementations;
public class IdentidadService : IIdentidadService
{    
    UsuariosDAO _dao;
    ILogger<IdentidadService> _iLogger;
    RespuestaValidacionUsuarioDTO _respuesta;
    ISesionesService _sesionesService;
    public IdentidadService(ILogger<IdentidadService> iLogger, 
            UsuariosDAO dao,  
            ISesionesService sesionesService,
            RespuestaValidacionUsuarioDTO respuesta){
        this._dao = dao;
        this._iLogger = iLogger;
        this._respuesta = respuesta;
        this._sesionesService = sesionesService;
    }
    public async Task<RespuestaValidacionUsuarioDTO> 
                    ValidarUsuario(PeticionInicioSesionDTO peticionInicioSesion, String ip)
    {        
        try {
            byte[] encodedPassword = new UTF8Encoding()
                         .GetBytes(peticionInicioSesion.password);
            byte[] hash = ((HashAlgorithm) CryptoConfig
                          .CreateFromName("MD5")).ComputeHash(encodedPassword);
            string passwordMD5 = BitConverter.ToString(hash)   
                .Replace("-", string.Empty)   
                .ToLower();
            peticionInicioSesion.password = passwordMD5;

            var usuario = await _dao.ObtenerPorPeticionAsync(peticionInicioSesion);
            
            if (usuario != null) {
                var usuarioDTO = new UsuarioDTO() {
                    ID = usuario.ID,
                    correoElectronico = usuario.CorreoElectronico,
                    nombreCompleto = $"{usuario.Nombre}  {usuario.Paterno} {usuario.Materno}",
                    puesto = usuario.Puesto,                
                };
                var sesion = _sesionesService.BuscarUltimaSesion(usuarioDTO);
                if (sesion == null) {
                    sesion = _sesionesService.GenerarSesion(usuarioDTO, 
                        ip);
                } 
                _respuesta.token = sesion.Token;
                _respuesta.correcto = true;
                _respuesta.usuario = usuarioDTO;                                
            } else {
                _respuesta.correcto = false;                
            }            
            return _respuesta;
        } catch (Exception e) {
            _iLogger.LogInformation(e.ToString());
            return new RespuestaValidacionUsuarioDTO() { correcto=false};
        }

    }
}