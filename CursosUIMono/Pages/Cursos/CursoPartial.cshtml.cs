using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using CursosUIMono.Services.Interfaces;
using CursosUIMono.DTOs;

namespace CursosUIMono.Pages.Cursos;

public class CursoPartialModel 
{    
    public CursoDTO Curso { get; set; }    
    public List<ProfesorDTO> Profesores { get; set; }
        
}
    
