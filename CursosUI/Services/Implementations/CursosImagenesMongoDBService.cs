using CursosUI.Services.Interfaces;
using CursosUI.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CursosUI.Services.Implementations
{
    public class CursosImagenesMongoDBService : ICursosImagenesService
    {
        private readonly IMongoCollection<BsonDocument> _imagenesCollection;

        public CursosImagenesMongoDBService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Cursos_Imagenes");
            _imagenesCollection = database.GetCollection<BsonDocument>("Imagenes");
        }
        

        public async Task<CursoImagenDTO> ObtenerCursoImagenPorIdAsync(int id)
        {
            var filtro = Builders<BsonDocument>.Filter.Eq("idCurso", id);
            var documento = await _imagenesCollection.Find(filtro).FirstOrDefaultAsync();

            if (documento == null)
            {
                return null;
            }

            var imagenBytes = documento["imagenBase64"].AsByteArray;            

            return new CursoImagenDTO
            {
                idCurso = id,                
                contenido = imagenBytes
            };
        }

        public async Task AsignarCursoImagenAsync(CursoImagenDTO c)
        {
            var filtro = Builders<BsonDocument>.Filter.Eq("idCurso", c.idCurso);
            var actualizacion = Builders<BsonDocument>.Update
                .Set("idCurso", c.idCurso)
                .Set("imagenBase64", c.contenido);                
                
            await _imagenesCollection.UpdateOneAsync(filtro, actualizacion, new UpdateOptions { IsUpsert = true });
        }

        public async Task<bool> ExisteCursoImagenPorIdAsync(int idCurso)
        {
            var filtro = Builders<BsonDocument>.Filter.Eq("idCurso", idCurso);
            var documento = await _imagenesCollection.Find(filtro).FirstOrDefaultAsync();

            return documento != null;
        }
    }
}