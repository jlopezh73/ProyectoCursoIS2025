using System.Text;
using System.Security.Cryptography;
using CursosUIMono.DTOs;
using CursosUIMono.Services.Interfaces;

namespace CursosUIMono.Services.Implementations;
public class IdentidadService : IIdentidadService
{    
    HttpClient clienteServicioIdentidad;
    ILogger<IdentidadService> iLogger;
    public IdentidadService(ILogger<IdentidadService> iLogger){
        //IHttpClientFactory httpClientFactory) {        
        //clienteServicioIdentidad = 
        //            httpClientFactory.CreateClient("ServicioIdentidad");
        this.iLogger = iLogger;
    }
    public RespuestaValidacionUsuario 
                    ValidarUsuario(PeticionInicioSesion peticionInicioSesion)
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

            var resultado = clienteServicioIdentidad
                .PostAsJsonAsync<PeticionInicioSesion>
                ("/api/Sesion/usuarioValido", peticionInicioSesion).Result;

            
            resultado.EnsureSuccessStatusCode();
            var respuesta = resultado.Content
                         .ReadFromJsonAsync<RespuestaValidacionUsuario>().Result;    
            return respuesta;
        } catch (Exception e) {
            iLogger.LogInformation(e.ToString());
            return new RespuestaValidacionUsuario() { correcto=false};
        }

    }
}