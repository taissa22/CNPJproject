using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
  public  class MigracaoCategoriaPagamentoEstrategicoConfiguration : IEntityTypeConfiguration<MigracaoCategoriaPagamentoEstrategico>
    {
        public void Configure(EntityTypeBuilder<MigracaoCategoriaPagamentoEstrategico> builder)
        {

            builder.ToTable("MIG_CATEGORIA_PAGTO_CIVEL_ESTR", "JUR");
            builder.HasKey(x => new { x.CodCategoriaPagamentoEstra, x.CodCategoriaPagamentoCivel });

            builder.Property(x => x.CodCategoriaPagamentoEstra)
                .HasColumnName("COD_CAT_PAGTO_CIVEL_ESTRAT");

            builder.Ignore(x => x.Id);
            builder.Property(x => x.CodCategoriaPagamentoCivel)
                .HasColumnName("COD_CAT_PAGTO_CIVEL_CONS");


        }
    }
}
