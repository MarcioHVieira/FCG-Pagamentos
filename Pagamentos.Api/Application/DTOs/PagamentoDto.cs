using Fcg.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pagamentos.Api.Application.DTOs
{
    public class PagamentoDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Guid PedidoId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public decimal ValorPago { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public FormaPagamento FormaPagamento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public Produto Produto { get; set; }

        [JsonIgnore]
        public string ClienteNome { get; set; }

        [JsonIgnore]
        public string ClienteEmail { get; set; }
    }
}
