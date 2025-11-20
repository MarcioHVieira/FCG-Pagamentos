using Fcg.Common.Entities;
using Fcg.Common.Enums;

namespace Pagamentos.Api.Domain.Entities
{
    public class Pagamento : EntityBase
    {
        public Guid PedidoId { get; private set; }
        public decimal ValorPago { get; private set; }
        public FormaPagamento FormaPagamento { get; private set; }

        //EF
        protected Pagamento() { }

        private Pagamento(Guid pedidoId, decimal valorPago, FormaPagamento formaPagamento)
        {
            Id = pedidoId;
            PedidoId = pedidoId;
            ValorPago = valorPago;
            FormaPagamento = formaPagamento;
        }

        public static Pagamento Criar(Guid pedidoId, decimal valorPago, FormaPagamento formaPagamento)
        {
            return new Pagamento(pedidoId, valorPago, formaPagamento);
        }
    }
}
