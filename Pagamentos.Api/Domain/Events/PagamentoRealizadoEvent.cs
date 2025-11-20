using Fcg.Common.Enums;

namespace Pagamentos.Api.Domain.Events
{
    public class PagamentoRealizadoEvent
    {
        public Guid PagamentoId { get; set; }
        public Guid PedidoId { get; set; }
        public decimal ValorPago { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
        public string ClienteNome { get; set; }
        public string ClienteEmail { get; set; }
    }
}
