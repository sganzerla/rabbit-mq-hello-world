using System.Text;
using RabbitMQ.Client;

namespace Publisher.Amqp.Console
{
    internal class Send
    {
        public static void Main()

    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
            System.Console.WriteLine(" [x] Sent {0}", message);
        }

        System.Console.WriteLine(" Press [enter] to exit.");
        System.Console.ReadLine();
    }
    }
}