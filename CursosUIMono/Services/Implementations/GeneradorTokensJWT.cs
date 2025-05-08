using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using Microsoft.IdentityModel.Tokens;
using CursosUIMono.DTOs;
using CursosUIMono.Entities;
using CursosUIMono.Services.Interfaces;

namespace AppServiciosIdentidad.Gateways.Providers;

/*class GeneradorTokensJWT : IGeneradorTokens {    
    private IConfiguration configuration;
    private IServiciosLOGS serviciosLOGS;
    public GeneradorTokensJWT(IConfiguration configuration, IServiciosLOGS serviciosLOGS) {        
        this.configuration = configuration;
        this.serviciosLOGS = serviciosLOGS;
    }
    
    public String GenerarToken(Usuario usuario, int noHoras, int idUsuarioSesion) {
        
        var tokenKey = Encoding.ASCII.GetBytes(configuration["JWTSettings:Key"]);
        int duracion = Int32.Parse(configuration["JWTSettings:Duration"]);
        var expiraToken = DateTime.Now.AddDays(duracion).AddHours(6);
        var identidad = new ClaimsIdentity(new List<Claim>{ 
            new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoElectronico),            
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),            
            new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString()),
            new Claim(ClaimTypes.Role, usuario.Puesto),
            new Claim("puesto", usuario.Puesto),
            new Claim("scope", "CursosAPP"),
            new Claim("idUsuario", usuario.Id.ToString()),
            new Claim("idUsuarioSesion", idUsuarioSesion.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, $"{usuario.Paterno}{usuario.Materno}{usuario.Nombre}"),          
            new Claim(JwtRegisteredClaimNames.NameId, usuario.CorreoElectronico) 
            });

        var credencialesFirma = new SigningCredentials(new SymmetricSecurityKey(tokenKey), 
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

        serviciosLOGS.RegistrarAccion(
             new AccionUsuarioDTO() {
                IDUsuario = usuario.Id,            
                IDUsuarioSesion = idUsuarioSesion,     
                Accion = "Generación de nuevo Token"
            });
        return token;
    }
    
}*/