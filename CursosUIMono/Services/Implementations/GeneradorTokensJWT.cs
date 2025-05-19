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
    private readonly IConfiguration _configuration;
    public GeneradorTokensJWTService(IConfiguration configuration, IBitacoraService _bitacoraService) {
        
        this._bitacoraService = _bitacoraService;
        _configuration = configuration;
    }
    
    public String GenerarToken(UsuarioDTO usuario, String key, int noHoras, int idUsuarioSesion) {
        var issuer = _configuration["JWTSettings:Issuer"];
        var audience = _configuration["JWTSettings:Audience"];
        var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));        
        var expiraToken = DateTime.Now.AddHours(noHoras).AddHours(6);
        var identidad = new ClaimsIdentity(new List<Claim>{ 
            new Claim(JwtRegisteredClaimNames.Iss, issuer),            
            new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoElectronico),            
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),            
            new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString()),
            new Claim(ClaimTypes.Role, usuario.Puesto),
            new Claim("puesto", usuario.Puesto),            
            new Claim("idUsuario", usuario.ID.ToString()),
            new Claim("idUsuarioSesion", idUsuarioSesion.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, usuario.nombreCompleto),                  
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
                FechaHora = DateTime.Now,
                IDUsuarioSesion = idUsuarioSesion,     
                Accion = "Generación de nuevo Token"
            });
        return token;
    }
    
}