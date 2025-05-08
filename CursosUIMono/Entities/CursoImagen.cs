using System;
using System.Collections.Generic;

namespace CursosUIMono.Entities;

public partial class CursoImagen
{
    public int id { get; set; }

    public string? archivo { get; set; }

    public int? idCurso { get; set; }

    public byte[]? contenido { get; set; }

    public virtual Curso? idCursoNavigation { get; set; }
}
