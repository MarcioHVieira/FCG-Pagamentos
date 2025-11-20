using Fcg.Common.Enums;

namespace Pagamentos.Api.Application.DTOs
{
    public class PagamentoResponseDto
    {
        public Guid PedidoId { get; set; }
        public decimal ValorPago { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}
