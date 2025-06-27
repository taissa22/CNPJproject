using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class DespesaConfiguration : IEntityTypeConfiguration<Despesa>
    {
        public void Configure(EntityTypeBuilder<Despesa> builder)
        {
            builder.ToTable("LANCAMENTO_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.Sequencial });

            builder.Property(x => x.Sequencial).HasColumnName("COD_LANCAMENTO");

            builder.Property<int>(x => x.ProcessoId).HasColumnName("COD_PROCESSO");
            builder.HasOne(x => x.Processo).WithMany(x => x.Despesas).HasForeignKey(x => x.ProcessoId);

            #region ENTITIES

            builder.HasOne(x => x.Lancamento).WithOne(x => x.Despesa)
               .HasForeignKey<Despesa>(x => new { x.ProcessoId, x.Sequencial });

            #endregion ENTITIES
        }
    }
}