using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using CursosUI.DTOs;
using CursosUI.Services.Interfaces;
namespace CursosUI.Pages.Cursos;

public class CursosModel : PageModel
{
    private ILogger<CursosModel> _logger;
    private readonly ICursosService _cursosService;
    private readonly IProfesoresService _profesoresService;
    public CursoDTO Curso { get; set; }    
    public List<CursoDTO> Cursos { get; private set; }
    public List<ProfesorDTO> Profesores { get; private set; }
    

    public CursosModel(ILogger<CursosModel> logger, 
                       ICursosService cursosService, 
                       IProfesoresService profesoresService, 
                       CursoDTO curso)
    {
        _cursosService = cursosService;
        _profesoresService = profesoresService;
        _logger = logger;
        Curso = curso;
    }

    public async Task OnGet()
    {
        Cursos = await _cursosService.ObtenerTodosLosCursosAsync();
        Profesores = await _profesoresService.ObtenerTodosLosProfesoresAsync();
    }
}
    
