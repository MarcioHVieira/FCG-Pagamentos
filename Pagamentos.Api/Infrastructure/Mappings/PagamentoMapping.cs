using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pagamentos.Api.Domain.Entities;

namespace Pagamentos.Api.Infrastructure.Mappings
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ValorPago)
                .IsRequired()
                .HasColumnType("numeric(8,2)");

            builder.ToTable("Pagamentos");
        }
    }
}