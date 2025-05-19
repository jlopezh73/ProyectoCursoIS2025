using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CursosUI.DTOs;
using CursosUI.Services.Interfaces;

namespace CursosUI.Services.Implementations
{
    public class CursosService : ICursosService
    {
        //private readonly CursosDAO _dao;

        public CursosService() //CursosDAO dao)
        {
            //_dao = dao;
        }

        public async Task<List<CursoDTO>> ObtenerTodosLosCursosAsync()
        {
            /*var cursosAsync =  await _dao.ObtenerTodosAsync();
            var dtos = cursosAsync.Select(c => new CursoDTO
            {
                id = c.id,
                nombre = c.nombre,
                descripcion = c.descripcion,
                precio = c.precio,
                fechaInicio = c.fechaInicio,
                fechaTermino = c.fechaTermino,
                idProfesor = c.idProfesor,
                profesor = c.idProfesorNavigation?.nombre??""
            }).ToList();
            return dtos;*/
            return null;
        }

        public async Task<CursoDTO> ObtenerCursoPorIdAsync(int id)
        {
            /*var c = await _dao.ObtenerPorIdAsync(id);
            return  new CursoDTO
            {
                id = c.id,
                nombre = c.nombre,
                descripcion = c.descripcion,
                precio = c.precio,
                fechaInicio = c.fechaInicio,
                fechaTermino = c.fechaTermino,
                idProfesor = c.idProfesor                
            };*/
            return null;
        }

        public async Task CrearCursoAsync(CursoDTO c)
        {
            /*var curso =  new Curso
            {
                id = c.id,
                nombre = c.nombre,
                descripcion = c.descripcion,
                precio = c.precio,
                fechaInicio = c.fechaInicio,
                fechaTermino = c.fechaTermino,
                idProfesor = c.idProfesor
            };
            await _dao.AgregarAsync(curso);*/
            return;
        }

        public async Task ActualizarCursoAsync(CursoDTO c)
        {
            /*var curso =  new Curso
            {
                id = c.id,
                nombre = c.nombre,
                descripcion = c.descripcion,
                precio = c.precio,
                fechaInicio = c.fechaInicio,
                fechaTermino = c.fechaTermino,
                idProfesor = c.idProfesor
            };
            await _dao.ActualizarAsync(curso);*/
            return;
        }

        public async Task EliminarCursoAsync(int id)
        {
            //await _dao.EliminarAsync(id);
        }
    }
}