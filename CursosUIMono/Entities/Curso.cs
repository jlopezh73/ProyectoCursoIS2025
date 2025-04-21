using System;
using System.Collections.Generic;

namespace CursosUIMono.Entities;

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

    public virtual ICollection<CursoParticipante> CursoParticipantes { get; set; } = new List<CursoParticipante>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Profesor? idProfesorNavigation { get; set; }
}
