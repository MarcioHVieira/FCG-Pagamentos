using Fcg.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Pagamentos.Api.Domain.Entities;
using Pagamentos.Api.Domain.Interfaces;

namespace Pagamentos.Api.Infrastructure.Data
{
    public class PagamentoRepository : RepositoryBase<Pagamento, PagamentosDbContext>, IPagamentoRepository
    {
        public PagamentoRepository(PagamentosDbContext context) : base(context)
        {
        }

        public override async Task<Pagamento?> ObterPorId(Guid id)
        {
            return await _context.Pagamentos.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> Existe(Guid pagamentoId)
        {
            return await _context.Pagamentos
                .AnyAsync(p => p.Id == pagamentoId);
        }

        public override async Task<IEnumerable<Pagamento>> ObterTodos()
        {
            return await _context.Pagamentos.AsNoTracking().ToListAsync();
        }
    }
}
