using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    class ClassesGarantiasConfiguration : IEntityTypeConfiguration<ClassesGarantias>
    {
        public void Configure(EntityTypeBuilder<ClassesGarantias> builder)
        {
            builder.ToTable("CLASSES_GARANTIAS", "JUR");
            builder.HasKey(pk => pk.Id).HasName("CODIGO");

            builder.Property(c => c.Id).HasColumnName("CODIGO");
            builder.Property(c => c.CodigoTipoLancamento).HasColumnName("TPLC_COD_TIPO_LANCAMENTO");
            builder.Property(c => c.CodigoTipoSaldoGarantia).HasColumnName("TSGAR_COD_TIPO_SALDO_GARANTIA");
            builder.Property(c => c.Descricao).HasColumnName("DESCRICAO");
            builder.Property(c => c.ValorMultiplicador).HasColumnName("VALOR_MULTIPLICADOR");
        }
    }
}
