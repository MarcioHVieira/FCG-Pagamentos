using Pagamentos.Api.Application.DTOs;

namespace Pagamentos.Api.Application.Services
{
    public interface IPagamentoService
    {
        Task<PagamentoResponseDto> ObterPagamento(Guid pedidoId);
        Task<IEnumerable<PagamentoResponseDto>> ObterPagamentos();
        Task EfetuarPagamento(PagamentoDto pagamentoDto);
    }
}
