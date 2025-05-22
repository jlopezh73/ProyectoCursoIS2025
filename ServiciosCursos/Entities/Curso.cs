using System;
using System.Collections.Generic;

namespace ServiciosCursos.Entities;

public partial class Curso
{
    public int id { get; set; }

    public string? nombre { get; set; }

    public string? descripcion { get; set; }

    public decimal? precio { get; set; }

    public DateTime? fechaInicio { get; set; }

    public DateTime? fechaTermino { get; set; }

    public int? idProfesor { get; set; }

    public virtual ICollection<CursoImagen> CursoImagens { get; set; } = new List<CursoImagen>();    

}
