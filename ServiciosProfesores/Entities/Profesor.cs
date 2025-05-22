using System;
using System.Collections.Generic;

namespace ServiciosProfesores.Entities;

public partial class Profesor
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public string? email { get; set; }

    public string? telefono { get; set; }

    public string? especializacion { get; set; }
    
}
