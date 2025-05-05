using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursosUIMono.DTOs;

public class RespuestaValidacionUsuario
{
    public bool correcto {get; set;}
    public UsuarioDTO? usuario {get; set;}
    public string token {get; set;}
}
