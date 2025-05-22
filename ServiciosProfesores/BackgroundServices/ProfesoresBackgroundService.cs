using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiciosProfesores.Services.Interfaces;
public class ProfesoresBackgroundService : BackgroundService
{
    private readonly ILogger<ProfesoresBackgroundService> _logger;
    private IConnection _connection;
    private IChannel _channel;    
    private readonly IProfesoresService _profesoresService;

    public ProfesoresBackgroundService(ILogger<ProfesoresBackgroundService> logger,
        IProfesoresService profesoresService)
    {
        _logger = logger;

        // Configuración de conexión a RabbitMQ
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;

        // Declarar la cola
        _channel.QueueDeclareAsync(queue: "peticiones_profesores",
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null).Wait();
        _profesoresService = profesoresService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMQ Listener Service iniciado.");


        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            IReadOnlyBasicProperties props = ea.BasicProperties;
            var replyProps = new BasicProperties
            {
                CorrelationId = props.CorrelationId
            };

            _logger.LogInformation($"Mensaje recibido: {message}");

            int idProfesor = 0;
            int.TryParse(message, out idProfesor);
            var profesor = await _profesoresService.ObtenerProfesorPorIdAsync(idProfesor);

            var responseBytes = Encoding.UTF8.GetBytes(profesor.nombre);
            await _channel.BasicPublishAsync(exchange: string.Empty,
                    routingKey: props.ReplyTo!,
                    mandatory: true,
                    basicProperties: replyProps,
                    body: responseBytes);
            await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsumeAsync("peticiones_profesores",
                               true,
                               consumer).Wait();

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        base.Dispose();
    }
}