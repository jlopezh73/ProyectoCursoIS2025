using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursosUIMono.DTOs;
using CursosUIMono.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CursosUIMono.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IIdentidadService _identidadService;

        [BindProperty]
        public PeticionInicioSesion peticionInicioSesion {get; set;}

        public LoginModel(ILogger<LoginModel> logger,
                          IIdentidadService identidadService)
        {
            _logger = logger;
            _identidadService = identidadService;
        }

        public void OnGet()
        {
            
        }

        public void OnPost() {
            if (peticionInicioSesion != null) {                
                var respuestaValidacion = 
                       _identidadService.ValidarUsuario(peticionInicioSesion);
                if (respuestaValidacion.correcto) {
                    HttpContext.Session.SetString("correo_usuario", 
                                      respuestaValidacion.usuario.correoElectronico);
                    HttpContext.Session.SetString("nombre_usuario", 
                                      respuestaValidacion.usuario.nombreCompleto);
                    HttpContext.Session.SetString("puesto_usuario", 
                                      respuestaValidacion.usuario.puesto);
                    HttpContext.Session.SetString("token_usuario", 
                                      respuestaValidacion.token);
                    Response.Redirect("/");
                }
            }
        }
    }
}