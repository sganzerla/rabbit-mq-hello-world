using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Publisher.Amqp.Api.Model;
using RabbitMQ.Client;

namespace Publisher.Amqp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MensagensController : ControllerBase
    {
        private static Contador _CONTADOR = new Contador();

        private readonly ILogger<MensagensController> _logger;

        [HttpPost]
        public object Post(
             [FromServices] RabbitMQConfigurations configurations,
             [FromBody] Conteudo conteudo)
        {
            lock (_CONTADOR)
            {
                _CONTADOR.Incrementar();

                var factory = new ConnectionFactory()
                {
                    HostName = configurations.HostName,
                    Port = configurations.Port,
                    UserName = configurations.UserName,
                    Password = configurations.Password
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "TestesASPNETCore",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message =
                        $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - " +
                        $"Conteúdo da Mensagem: {conteudo.Mensagem}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "TestesASPNETCore",
                                         basicProperties: null,
                                         body: body);
                }

                return new
                {
                    Resultado = "Mensagem encaminhada com sucesso"
                };
            }
        }
    }
}
