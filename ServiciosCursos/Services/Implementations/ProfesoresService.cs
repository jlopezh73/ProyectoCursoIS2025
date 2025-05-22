using System.Collections.Generic;
using System.Threading.Tasks;
using ServiciosCursos.DAOs;
using ServiciosCursos.DTOs;
using ServiciosCursos.Entities;
using ServiciosCursos.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace ServiciosCursos.Services.Implementations
{
    public class ProfesoresService : IProfesoresService
    {
        private const string QUEUE_NAME = "peticiones_profesores";

    private readonly IConnectionFactory _connectionFactory;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper
        = new();

    private IConnection? _connection;
    private IChannel? _channel;
    private string? _replyQueueName;


        public ProfesoresService() //ProfesoresDAO profesoresDAO)
        {
            _connectionFactory = new ConnectionFactory { HostName = "localhost" };
        }


        public async Task<String> ObtenerNombreProfesorPorIdAsync(int id)
        {
            string correlationId = Guid.NewGuid().ToString();
            var props = new BasicProperties
            {
                CorrelationId = correlationId,
                ReplyTo = _replyQueueName
            };

            var tcs = new TaskCompletionSource<string>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
            _callbackMapper.TryAdd(correlationId, tcs);

            try
            {
                var messageBytes = Encoding.UTF8.GetBytes(id.ToString());
                var respuesta = _channel.BasicPublishAsync(exchange: string.Empty, routingKey: QUEUE_NAME,
                    mandatory: true, basicProperties: props, body: messageBytes).ToString();
                return respuesta;
            } catch (Exception ex)
            {

                return "";
            }   
        }
    }
        

            
}