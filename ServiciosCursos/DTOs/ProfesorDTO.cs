using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiciosCursos.DTOs;

public partial class ProfesorDTO
{
    public int id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio", AllowEmptyStrings = false)]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    [DisplayName("Nombre")]        
    public string? nombre { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio", AllowEmptyStrings = false)]
    [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres.")]
    [DisplayName("Correo electrónico")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]    
    
    public string? email { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio", AllowEmptyStrings = false)]
    [StringLength(100, ErrorMessage = "El teléfono no puede exceder los 50 caracteres.")]
    [DisplayName("Teléfono")]    
    
    public string? telefono { get; set; }

    [Required(ErrorMessage = "La especialización es obligatoria", AllowEmptyStrings = false)]
    [StringLength(100, ErrorMessage = "La especialización no puede exceder los 100 caracteres.")]
    [DisplayName("Especialización")] 
    
    public string? especializacion { get; set; }
    
}
