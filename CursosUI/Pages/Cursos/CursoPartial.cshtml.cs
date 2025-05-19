using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using CursosUI.DTOs;

namespace CursosUI.Pages.Cursos;

public class CursoPartialModel 
{    
    public CursoDTO Curso { get; set; }    
    public List<ProfesorDTO> Profesores { get; set; }
        
}
    
