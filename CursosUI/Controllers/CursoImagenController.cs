using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CursosUI.DTOs;
using Microsoft.AspNetCore.Authorization;
using CursosUI.Services.Interfaces;

namespace CursosUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CursoImagenController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ICursosImagenesService _cursosImagenesService;
        private readonly RespuestaPeticionDTO _respuestaPeticionDTO;

        public CursoImagenController(IWebHostEnvironment environment, ICursosImagenesService cursosImagenesService,
                                     RespuestaPeticionDTO respuestaPeticionDTO)
        {
            _cursosImagenesService = cursosImagenesService;
            _respuestaPeticionDTO = respuestaPeticionDTO;
            _environment = environment;
        }        

        // POST: api/CursoImagen/{idCurso}
        [HttpPost("{idCurso}")]
        public async Task<IActionResult> AsignarImagen(int idCurso, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                _respuestaPeticionDTO.Mensaje = "No se ha subido ningún archivo.";
                _respuestaPeticionDTO.Exito = false;                

            }  else {         
                var contenido = new byte[archivo.Length];
                await archivo.OpenReadStream().ReadAsync(contenido, 0, (int)archivo.Length);
                await _cursosImagenesService.AsignarCursoImagenAsync(new CursoImagenDTO
                {
                    archivo = $"/api/CursoImagen/{idCurso}",
                    contenido = contenido,
                    idCurso = idCurso
                });
                _respuestaPeticionDTO.Mensaje = "Archivo subido con éxito.";
                _respuestaPeticionDTO.Exito = true;                
            }
            return new JsonResult(_respuestaPeticionDTO);
        }

        // GET: api/CursoImagen/{idCurso}
        [HttpGet("{idCurso}")]
        public async Task<IActionResult> ObtenerImagen(int idCurso)
        {
            var cursoImagen = await _cursosImagenesService.ObtenerCursoImagenPorIdAsync(idCurso);

            return new FileContentResult(cursoImagen.contenido, "image/jpeg")
            {
                FileDownloadName = cursoImagen.archivo
            };
        }

        // GET: api/CursoImagen/Existe/{idCurso}
        [HttpGet("Existe/{idCurso}")]
        public async Task<IActionResult> ExisteImagen(int idCurso)
        {
            var existeImagen = await _cursosImagenesService.ExisteCursoImagenPorIdAsync(idCurso);

            if (existeImagen)
                return new JsonResult(new RespuestaPeticionDTO
                {                
                    Exito = true,
                    Datos = $"/api/CursoImagen/{idCurso}"
                });
            else
                return new JsonResult(new RespuestaPeticionDTO
                {                
                    Exito = false                    
                });
        }
    }
}