using System.ComponentModel.DataAnnotations;

namespace Publisher.Amqp.Api.Model
{
    public class Conteudo
    {
        [Required]
        public string Mensagem { get; set; }
    }
}