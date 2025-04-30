using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using CursosUIMono.Services.Interfaces;
using CursosUIMono.DTOs;

namespace ProyectoCursos.Pages.Participantes;

public class ParticipantesModel : PageModel
{
    private ILogger<ParticipantesModel> _logger;
    private readonly IParticipantesService _participantesService;

    public List<ParticipanteDTO> Participantes { get; set; }
    public ParticipanteDTO _Participante { get; set; }

    public ParticipantesModel(ILogger<ParticipantesModel> logger, 
                           IParticipantesService participantesService,
                           ParticipanteDTO Participante)
    {
        _participantesService = participantesService;
        _Participante = Participante;
        _logger = logger;
    }

    public async Task OnGet()
    {
        Participantes = await _participantesService.ObtenerTodosLosParticipantesAsync();
        if (Participantes == null)
        {
            Participantes = new List<ParticipanteDTO>();
        }        
    }
}
    
