using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace CursosUIMono.DTOs;

public partial class CursoDTO
{
    public int id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio", AllowEmptyStrings = false)]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]    
    [Display(Name = "Nombre", Prompt = "Nombre del Curso")]
    public string? nombre { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria", AllowEmptyStrings = false)]        
    [Display(Name = "Descripción", Prompt ="Temática del Curso")]
    public string? descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio", AllowEmptyStrings = false)]
    [Range(100.0, 5000.00, ErrorMessage = "El precio debe estar en el rango de 100 a 5,000.00")]    
    [Display(Name = "Precio", Prompt ="Precio del Curso")]
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
    public decimal? precio { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria", AllowEmptyStrings = false)]     
    [Display(Name = "Fecha de Inicio", Prompt ="Fecha cuando comienza el curso")]
    public DateTime? fechaInicio { get; set; }

    [Required(ErrorMessage = "La fecha de término es obligatoria", AllowEmptyStrings = false)]    
    [Display(Name = "Fecha de Término", Prompt ="Fecha cuando termina el curso")]
    public DateTime? fechaTermino { get; set; }


    [Range(1,999999, ErrorMessage = "El  profesor no puede estar vacío.")]
    public int? idProfesor { get; set; }
    
    [DisplayName("Profesor")]
    [Required(ErrorMessage = "El profesor es obligatorio", AllowEmptyStrings = false)]
    public String profesor { get; set; }
    

    public String duracion {
        get {
            if (fechaInicio != null && fechaTermino != null)
            {                
                return $"{fechaInicio?.ToShortDateString()} - {fechaTermino?.ToShortDateString()}";
            }
            return "No definido";
        }
        }

}
