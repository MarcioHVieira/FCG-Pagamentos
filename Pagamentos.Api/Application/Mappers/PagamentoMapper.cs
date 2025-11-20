using Pagamentos.Api.Application.DTOs;
using Pagamentos.Api.Domain.Entities;

namespace Pagamentos.Api.Application.Mappers
{
    public static class PagamentoMapper
    {
        public static Pagamento ToDomain(this PagamentoDto pagamentoDto)
        {
            return Pagamento.Criar(pagamentoDto.PedidoId, pagamentoDto.ValorPago, pagamentoDto.FormaPagamento);
        }

        public static PagamentoResponseDto ToDto(this Pagamento pagamento)
        {
            return new PagamentoResponseDto{PedidoId = pagamento.PedidoId, ValorPago = pagamento.ValorPago, 
                                            FormaPagamento = pagamento.FormaPagamento, DataPagamento = pagamento.DataCadastro };
        }
    }
}
