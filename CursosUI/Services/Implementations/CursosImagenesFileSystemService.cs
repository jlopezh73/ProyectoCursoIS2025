using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CursosUI.DTOs;

using CursosUI.Services.Interfaces;

namespace CursosUI.Services.Implementations
{
    public class CursosImagenesFileSystemService : ICursosImagenesService
    {        
        private readonly IWebHostEnvironment _environment;

        public CursosImagenesFileSystemService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<CursoImagenDTO> ObtenerCursoImagenPorIdAsync(int idCurso)
        {                        
            
            var rutaArchivo = Path.Combine(_environment.WebRootPath,
             "imagenes_cursos", $"{idCurso}.jpg");

            if (!System.IO.File.Exists(rutaArchivo))            
                return null;
            

            var contenidoArchivo = System.IO.File.ReadAllBytes(rutaArchivo);
            return new CursoImagenDTO
            {
                idCurso = idCurso,
                archivo = $"{idCurso}.jpg",
                contenido = contenidoArchivo
            };
        }

        public async Task AsignarCursoImagenAsync(CursoImagenDTO c)
        {            
            // Ruta donde se almacenará la imagen
            var rutaCarpeta = Path.Combine(_environment.WebRootPath, "imagenes_cursos");
            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }

            var rutaArchivo = Path.Combine(rutaCarpeta, $"{c.idCurso}.jpg");

            // Guardar el archivo en el sistema de archivos
            using (var stream = new FileStream(rutaArchivo, FileMode.Create))
            {
                await stream.WriteAsync(c.contenido, 0, c.contenido.Length);                
            }            
        }

        public async Task<bool> ExisteCursoImagenPorIdAsync(int idCurso)
        {
            var rutaArchivo = Path.Combine(_environment.WebRootPath, "imagenes_cursos", $"{idCurso}.jpg");
            return System.IO.File.Exists(rutaArchivo);
        }
        
    }
}