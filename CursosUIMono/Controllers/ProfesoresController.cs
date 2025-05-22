using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CursosUIMono.DTOs;
using CursosUIMono.Services.Interfaces;
using System.Security.Claims;

namespace CursosUIMono.Controllers
{
    [ApiController]    
    [Route("api/[controller]")]
    [Authorize]
    public class ProfesoresController : ControllerBase
    {
        private readonly IProfesoresService _profesoresService;
        private readonly ILogger<ProfesoresController> _logger;
        private readonly RespuestaPeticionDTO _respuestaPeticionDTO;

        public ProfesoresController(ILogger<ProfesoresController> logger, 
                                    IProfesoresService profesoresService,
                                    RespuestaPeticionDTO respuestaPeticionDTO)
        {
            _profesoresService = profesoresService;
            _logger = logger;
            _respuestaPeticionDTO = respuestaPeticionDTO;
        }

        // GET: api/Profesores
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var profesores = await _profesoresService.ObtenerTodosLosProfesoresAsync();
            _respuestaPeticionDTO.Mensaje = "Profesores cargados exitosamente";
            _respuestaPeticionDTO.Exito = true;
            _respuestaPeticionDTO.Datos = profesores;
            return new JsonResult( _respuestaPeticionDTO);
        }

        // GET: api/Profesores/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador Profesores, Administrador General")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var profesor = await _profesoresService.ObtenerProfesorPorIdAsync(id);
            if (profesor == null)
            {
                _respuestaPeticionDTO.Mensaje = "No se encontró el profesor";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = id;
            } else {
                _respuestaPeticionDTO.Mensaje = "Profesores cargados exitosamente";
                _respuestaPeticionDTO.Exito = true;
                _respuestaPeticionDTO.Datos = profesor;
            }
            return new JsonResult(_respuestaPeticionDTO);
        }

        // POST: api/Profesores
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ProfesorDTO profesorDTO)
        {
            if (!ModelState.IsValid)
            {
                _respuestaPeticionDTO.Mensaje = "Datos erroneos del profesor";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = profesorDTO;
                return new JsonResult(_respuestaPeticionDTO);
            } else {
                await _profesoresService.CrearProfesorAsync(profesorDTO);            
                _respuestaPeticionDTO.Mensaje = "Profesor creado exitosamente";
                _respuestaPeticionDTO.Exito = true;          
                return new JsonResult(_respuestaPeticionDTO);                      
            }            
        }

        // PUT: api/Profesores/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ProfesorDTO profesorDTO)
        {
            if (!ModelState.IsValid)
            {
                _respuestaPeticionDTO.Mensaje = "Datos erroneos del profesor";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = profesorDTO;
                return new JsonResult(_respuestaPeticionDTO);                      
            }

            if (id != profesorDTO.id)
            {
                _respuestaPeticionDTO.Mensaje = "El ID del profesor no coincide";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = profesorDTO;                
                return new JsonResult(_respuestaPeticionDTO);                      
            }
            

            await _profesoresService.ActualizarProfesorAsync(profesorDTO);

            _respuestaPeticionDTO.Mensaje = "Profesor modificado exitosamente";
            _respuestaPeticionDTO.Exito = true;
            return new JsonResult(_respuestaPeticionDTO);
        }

        // DELETE: api/Profesores/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var claims = HttpContext.User.Claims;

            // Obtener un claim específico (por ejemplo, rol del usuario)
            var rolUsuario = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Verificar si el usuario tiene el rol necesario
            if (rolUsuario != "Administrador")
            {
                return Forbid("Bearer");
            }

            var profesor = await _profesoresService.ObtenerProfesorPorIdAsync(id);
            if (profesor == null)
            {
                _respuestaPeticionDTO.Mensaje = "No se encontró el profesor";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = id;
                return new JsonResult(_respuestaPeticionDTO);
            }

            await _profesoresService.EliminarProfesorAsync(id);

            _respuestaPeticionDTO.Mensaje = "Profesor eliminado exitosamente";
            _respuestaPeticionDTO.Exito = true;
            return new JsonResult(_respuestaPeticionDTO);
        }
    }
}