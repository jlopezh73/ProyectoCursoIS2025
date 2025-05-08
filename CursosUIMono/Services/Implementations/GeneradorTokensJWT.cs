using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CursosUIMono.DTOs;
using CursosUIMono.Entities;
using CursosUIMono.Services.Interfaces;

namespace CursosUIMono.Services.Implementations;

class GeneradorTokensJWTService : IGeneradorTokensService {        
    private IBitacoraService _bitacoraService;
    public GeneradorTokensJWTService(IConfiguration configuration, IBitacoraService _bitacoraService) {
        
        this._bitacoraService = _bitacoraService;
    }
    
    public String GenerarToken(UsuarioDTO usuario, String key, int noHoras, int idUsuarioSesion) {
        
        var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));        
        var expiraToken = DateTime.Now.AddHours(noHoras).AddHours(6);
        var identidad = new ClaimsIdentity(new List<Claim>{ 
            new Claim(JwtRegisteredClaimNames.Email, usuario.correoElectronico),            
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),            
            new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString()),
            new Claim(ClaimTypes.Role, usuario.puesto),
            new Claim("puesto", usuario.puesto),
            new Claim("scope", "CursosUIMono"),
            new Claim("idUsuario", usuario.ID.ToString()),
            new Claim("idUsuarioSesion", idUsuarioSesion.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, usuario.nombreCompleto),          
            new Claim(JwtRegisteredClaimNames.NameId, usuario.correoElectronico) 
            });

        var credencialesFirma = new SigningCredentials(tokenKey, 
                                    SecurityAlgorithms.HmacSha256Signature);
        var descriptorTokenSeguridad = new SecurityTokenDescriptor {
            Subject = identidad, 
            Expires = expiraToken, 
            IssuedAt =DateTime.Now,
            NotBefore = DateTime.Now,
            SigningCredentials = credencialesFirma
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(descriptorTokenSeguridad);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        _bitacoraService.RegistrarAccion(
             new UsuarioAccionDTO() {
                IDUsuario = usuario.ID,            
                IDUsuarioSesion = idUsuarioSesion,     
                Accion = "Generación de nuevo Token"
            });
        return token;
    }
    
}