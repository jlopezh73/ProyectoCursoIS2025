using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using CursosUI.DTOs;
using CursosUI.Services.Interfaces;

namespace CursosUI.Pages.Profesores;

public class ProfesoresModel : PageModel
{
    private ILogger<ProfesoresModel> _logger;
    private readonly IProfesoresService _profesoresService;

    public List<ProfesorDTO> Profesores { get; set; }
    public ProfesorDTO _Profesor { get; set; }

    public ProfesoresModel(ILogger<ProfesoresModel> logger, 
                           IProfesoresService profesoresService,
                           ProfesorDTO Profesor)
    {
        _profesoresService = profesoresService;
        _Profesor = Profesor;
        _logger = logger;
    }

    public async Task OnGet()
    {Profesores = await _profesoresService.ObtenerTodosLosProfesoresAsync();
        
        if (Profesores == null)
        {
            Profesores = new List<ProfesorDTO>();
        }        
    }
}
    
