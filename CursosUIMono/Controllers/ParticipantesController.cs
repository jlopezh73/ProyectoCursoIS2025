using Microsoft.AspNetCore.Mvc;
using CursosUIMono.DTOs;
using CursosUIMono.Services.Interfaces;

namespace CursosUIMono.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantesController : ControllerBase
    {
        private readonly IParticipantesService _participantesService;
        private readonly ILogger<ParticipantesController> _logger;
        private readonly RespuestaPeticionDTO _respuestaPeticionDTO;

        public ParticipantesController(ILogger<ParticipantesController> logger, 
                                    IParticipantesService ParticipantesService,
                                    RespuestaPeticionDTO respuestaPeticionDTO)
        {
            _participantesService = ParticipantesService;
            _logger = logger;
            _respuestaPeticionDTO = respuestaPeticionDTO;
        }

        // GET: api/Participantes
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var participantes = await _participantesService.ObtenerTodosLosParticipantesAsync();
            _respuestaPeticionDTO.Mensaje = "Participantes cargados exitosamente";
            _respuestaPeticionDTO.Exito = true;
            _respuestaPeticionDTO.Datos = participantes;
            return new JsonResult( _respuestaPeticionDTO);
        }

        [HttpGet("data")]
        public async Task<IActionResult> ObtenerTodos_DataTables()
        {
            var participantes = await _participantesService.ObtenerTodosLosParticipantesAsync();            
            var participantesDT = participantes.Select(p => new 
            {
                id = p.id,
                nombre = $"<a href='#' onclick='editarParticipante({p.id});'>{p.nombre}</a>",                
                telefono = p.telefono,
                email = p.email,
                opciones = $"<button class='btn btn-danger btn-sm' onclick='eliminarParticipante({p.id}, \"{p.nombre}\");'><i class=\"fas fa-trash-alt\"></i> Eliminar</button>"
            }).ToList();
            return new JsonResult( new {data=participantesDT});
        }

        // GET: api/Participantes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var participante = await _participantesService.ObtenerParticipantePorIdAsync(id);
            if (participante == null)
            {
                _respuestaPeticionDTO.Mensaje = "No se encontró el profesor";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = id;
            } else {
                _respuestaPeticionDTO.Mensaje = "Participantes cargados exitosamente";
                _respuestaPeticionDTO.Exito = true;
                _respuestaPeticionDTO.Datos = participante;
            }
            return new JsonResult(_respuestaPeticionDTO);
        }

        // POST: api/Participantes
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ParticipanteDTO participanteDTO)
        {
            if (!ModelState.IsValid)
            {
                _respuestaPeticionDTO.Mensaje = "Datos erroneos del participante";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = participanteDTO;
                return new JsonResult(_respuestaPeticionDTO);
            } else {
                await _participantesService.CrearParticipanteAsync(participanteDTO);            
                _respuestaPeticionDTO.Mensaje = "Profesor creado exitosamente";
                _respuestaPeticionDTO.Exito = true;          
                return new JsonResult(_respuestaPeticionDTO);                      
            }            
        }

        // PUT: api/Participantes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ParticipanteDTO participanteDTO)
        {
            if (!ModelState.IsValid)
            {
                _respuestaPeticionDTO.Mensaje = "Datos erroneos del participante";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = participanteDTO;
                return new JsonResult(_respuestaPeticionDTO);                      
            }

            if (id != participanteDTO.id)
            {
                _respuestaPeticionDTO.Mensaje = "El ID del participante no coincide";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = participanteDTO;                
                return new JsonResult(_respuestaPeticionDTO);                      
            }
            

            await _participantesService.ActualizarParticipanteAsync(participanteDTO);

            _respuestaPeticionDTO.Mensaje = "Participante modificado exitosamente";
            _respuestaPeticionDTO.Exito = true;
            return new JsonResult(_respuestaPeticionDTO);
        }

        // DELETE: api/Participantes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var profesor = await _participantesService.ObtenerParticipantePorIdAsync(id);
            if (profesor == null)
            {
                _respuestaPeticionDTO.Mensaje = "No se encontró el participante";
                _respuestaPeticionDTO.Exito = false;
                _respuestaPeticionDTO.Datos = id;
                return new JsonResult(_respuestaPeticionDTO);
            }

            await _participantesService.EliminarParticipanteAsync(id);

            _respuestaPeticionDTO.Mensaje = "Participante eliminado exitosamente";
            _respuestaPeticionDTO.Exito = true;
            return new JsonResult(_respuestaPeticionDTO);
        }
    }
}