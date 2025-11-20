using Pagamentos.Api.Domain.Entities;

namespace Pagamentos.Api.Domain.Interfaces
{
    public interface IPagamentoRepository
    {
        Task<Pagamento?> ObterPorId(Guid id);
        Task<IEnumerable<Pagamento>> ObterTodos();
        Task Adicionar(Pagamento pagamento);
        Task<bool> Existe(Guid pagamentoId);
    }
}
