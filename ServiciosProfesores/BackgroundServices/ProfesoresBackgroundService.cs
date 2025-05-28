using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiciosProfesores.Entities;
public class ProfesoresBackgroundService : BackgroundService
{
    private readonly ILogger<ProfesoresBackgroundService> _logger;
    private IConnection _connection;
    private IChannel _channel;    
    private readonly IConfiguration _configuration;

    public ProfesoresBackgroundService(ILogger<ProfesoresBackgroundService> logger,
        IConfiguration configuration)
    {
        _logger = logger;

        // Configuración de conexión a RabbitMQ
        var factory = new ConnectionFactory()
        {
            HostName = configuration["ConnectionStrings:RabbitMQ_Host"]?? "localhost",
            UserName = configuration["ConnectionStrings:RabbitMQ_User"]??"guest",
            Password = configuration["ConnectionStrings:RabbitMQ_Password"]??"guest"
        };

        logger.LogInformation($"Conectando a RabbitMQ en {factory.HostName} con usuario {factory.UserName} y contraseña {factory.Password}.");

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;

        // Declarar la cola
        _channel.QueueDeclareAsync(queue: "peticiones_profesores",
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null).Wait();
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
            using (var dbContext = new CursosContext(_configuration))
            {
                var profesor = dbContext.Profesors
                    .FirstOrDefault(p => p.id == idProfesor);
                
                byte[] responseBytes = null;
                
                if (profesor != null)                
                    responseBytes = Encoding.UTF8.GetBytes(profesor.nombre);
                else
                    responseBytes = Encoding.UTF8.GetBytes("--  Profesor no asignado  --");
                    
                await _channel.BasicPublishAsync(exchange: string.Empty,
                routingKey: props.ReplyTo!,
                mandatory: true,
                basicProperties: replyProps,
                body: responseBytes);
                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                
            }
             
        };

        await _channel.BasicConsumeAsync("peticiones_profesores",
                               false,
                               consumer);

        return;
    }

    public override void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
        base.Dispose();
    }
}