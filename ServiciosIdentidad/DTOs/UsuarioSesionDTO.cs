using System;
using System.Collections.Generic;

namespace ServiciosIdentidad.DTOs;

public partial class UsuarioSesionDTO
{
    public int ID { get; set; }

    public int IDUsuario { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaUltimoAcceso { get; set; }

    public string? DireccionIP { get; set; }

    public string? Token { get; set; }    
}
