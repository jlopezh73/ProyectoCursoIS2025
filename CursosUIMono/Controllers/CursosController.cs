using Microsoft.AspNetCore.Mvc;
using CursosUIMono.DTOs;
using CursosUIMono.Services.Interfaces;

namespace CursosUIMono.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly ICursosService _cursosService;
        private readonly ILogger<CursosController> _logger;
        private readonly RespuestaPeticionDTO _respuestaPeticionDTO;

        public CursosController(ILogger<CursosController> logger,
                                ICursosService cursosService,
                                RespuestaPeticionDTO respuestaPeticionDTO)
        {
            _cursosService = cursosService;
            _logger = logger;
            _respuestaPeticionDTO = respuestaPeticionDTO;
        }

        // GET: api/Cursos/
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var cursos = await _cursosService.ObtenerTodosLosCursosAsync();
            _respuestaPeticionDTO.Mensaje = "Cursos cargados exitosamente";
            _respuestaPeticionDTO.Exito = true;
            _respuestaPeticionDTO.Datos = cursos;
            return new JsonResult( _respuestaPeticionDTO);
        }

        // GET: api/Cursos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var curso = await _cursosService.ObtenerCursoPorIdAsync(id);
            if (curso == null)
            {
                _respuestaPeticionDTO.Mensaje = "No se encontró el curso";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = id;
            } else {
                _respuestaPeticionDTO.Mensaje = "Curso cargado exitosamente";
                _respuestaPeticionDTO.Exito = true;
                _respuestaPeticionDTO.Datos = curso;
            }
            return new JsonResult(_respuestaPeticionDTO);
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CursoDTO cursoDTO)
        {
            if (!ModelState.IsValid)
            {
                _respuestaPeticionDTO.Mensaje = "Datos erroneos del profesor";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = cursoDTO;
                return new JsonResult(_respuestaPeticionDTO);
            } else {
                await _cursosService.CrearCursoAsync(cursoDTO);            
                _respuestaPeticionDTO.Mensaje = "Curso creado exitosamente";
                _respuestaPeticionDTO.Exito = true;          
                return new JsonResult(_respuestaPeticionDTO);                      
            }            
        }

        // PUT: api/Cursos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CursoDTO cursoDTO)
        {
            if (!ModelState.IsValid)
            {
                _respuestaPeticionDTO.Mensaje = "Datos erroneos del curso";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = cursoDTO;
                return new JsonResult(_respuestaPeticionDTO);                      
            }

            if (id != cursoDTO.id)
            {
                _respuestaPeticionDTO.Mensaje = "El ID del curso no coincide";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = cursoDTO;                
                return new JsonResult(_respuestaPeticionDTO);                      
            }
            

            await _cursosService.ActualizarCursoAsync(cursoDTO);

            _respuestaPeticionDTO.Mensaje = "Curso modificado exitosamente";
            _respuestaPeticionDTO.Exito = true;
            return new JsonResult(_respuestaPeticionDTO);
        }

        // DELETE: api/Cursos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var curso = await _cursosService.ObtenerCursoPorIdAsync(id);
            if (curso == null)
            {
                _respuestaPeticionDTO.Mensaje = "No se encontró el curso";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = id;
                return new JsonResult(_respuestaPeticionDTO);
            }

            await _cursosService.EliminarCursoAsync(id);

            _respuestaPeticionDTO.Mensaje = "Curso eliminado exitosamente";
            _respuestaPeticionDTO.Exito = true;
            return new JsonResult(_respuestaPeticionDTO);
        }
    }
}