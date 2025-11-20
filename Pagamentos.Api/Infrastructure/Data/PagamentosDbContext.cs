using Fcg.Common.Entities;
using Fcg.Common.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Pagamentos.Api.Domain.Entities;

namespace Pagamentos.Api.Infrastructure.Data
{
    public class PagamentosDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Pagamento> Pagamentos { get; set; }

        public PagamentosDbContext(DbContextOptions<PagamentosDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("pagamentos");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentosDbContext).Assembly);
        }

        public async Task Salvar(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                if (entry.State == EntityState.Added)
                    entry.Property("DataCadastro").CurrentValue = DateTime.UtcNow;

                if (entry.State == EntityState.Modified)
                    entry.Property("DataCadastro").IsModified = false;
            }

            var salvo = await base.SaveChangesAsync(cancellationToken) > 0;

            if (!salvo)
                throw new DbUpdateException("Houve um erro ao tentar persistir os dados");
        }
    }
}
