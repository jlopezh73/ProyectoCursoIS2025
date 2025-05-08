using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursosUIMono.DTOs
{
    public class UsuarioDTO
    {
        public int ID { get; set; }
        public String correoElectronico {get; set;}
        public String nombreCompleto {get; set;}        
        public String puesto {get; set;}        
    }
}