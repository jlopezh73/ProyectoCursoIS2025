using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursosUI.DTOs;
using CursosUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CursosUI.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IIdentidadService _identidadService;

        [BindProperty]
        public PeticionInicioSesionDTO peticionInicioSesion {get; set;}

        public LoginModel(ILogger<LoginModel> logger,
                          IIdentidadService identidadService)
        {
            _logger = logger;
            _identidadService = identidadService;
        }

        public void OnGet()
        {
            
        }

        public async Task OnPost() {
            if (peticionInicioSesion != null) {         
                var ip = Request.HttpContext.Connection.RemoteIpAddress;       
                var respuestaValidacion = 
                       await _identidadService.ValidarUsuario(peticionInicioSesion, ip.ToString());
                if (respuestaValidacion.correcto) {
                    HttpContext.Session.SetString("correo_usuario", 
                                      respuestaValidacion.usuario.CorreoElectronico);
                    HttpContext.Session.SetString("nombre_usuario", 
                                      respuestaValidacion.usuario.nombreCompleto);
                    HttpContext.Session.SetString("puesto_usuario", 
                                      respuestaValidacion.usuario.Puesto);
                    HttpContext.Session.SetString("token_usuario", 
                                      respuestaValidacion.token);
                    Response.Redirect("/");
                }
            }
        }
    }
}