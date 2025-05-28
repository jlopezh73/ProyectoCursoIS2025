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
        private ILogger<ProfesoresService> _logger;


        public ProfesoresService(ILogger<ProfesoresService> logger, IConfiguration configuration) //ProfesoresDAO profesoresDAO)
        {
            _connectionFactory = new ConnectionFactory {
                HostName = configuration["ConnectionStrings:RabbitMQ_Host"]?? "localhost",
                UserName = configuration["ConnectionStrings:RabbitMQ_User"]??"guest",
                Password = configuration["ConnectionStrings:RabbitMQ_Password"]??"guest"
            };
            this._logger = logger;
        }


        public async Task<String> ObtenerNombreProfesorPorIdAsync(int id)
        {
            await StartAsync();            
            return await CallAsync(id.ToString());
        }
        
        public async Task StartAsync()
        {
            _connection = await _connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            // declare a server-named queue
            QueueDeclareOk queueDeclareResult = await _channel.QueueDeclareAsync();
            _replyQueueName = queueDeclareResult.QueueName;
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += (model, ea) =>
            {
                string? correlationId = ea.BasicProperties.CorrelationId;

                if (false == string.IsNullOrEmpty(correlationId))
                {
                    if (_callbackMapper.TryRemove(correlationId, out var tcs))
                    {
                        var body = ea.Body.ToArray();
                        var response = Encoding.UTF8.GetString(body);
                        tcs.TrySetResult(response);
                    }
                }

                return Task.CompletedTask;
            };

            await _channel.BasicConsumeAsync(_replyQueueName, true, consumer);
        }

        public async Task<string> CallAsync(string message,
            CancellationToken cancellationToken = default)
        {
            if (_channel is null)
            {
                throw new InvalidOperationException();
            }

            string correlationId = Guid.NewGuid().ToString();
            var props = new BasicProperties
            {
                CorrelationId = correlationId,
                ReplyTo = _replyQueueName
            };

            var tcs = new TaskCompletionSource<string>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
            _callbackMapper.TryAdd(correlationId, tcs);

            var messageBytes = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: QUEUE_NAME,
                mandatory: true, basicProperties: props, body: messageBytes);

            using CancellationTokenRegistration ctr =
                cancellationToken.Register(() =>
                {
                    _callbackMapper.TryRemove(correlationId, out _);
                    tcs.SetCanceled();
                });

            return await tcs.Task;
        }
    }
        

            
}