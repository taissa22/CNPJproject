using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class EmpresaSapConfiguration : IEntityTypeConfiguration<EmpresaSap>
    {
        public void Configure(EntityTypeBuilder<EmpresaSap> builder)
        {
            builder.ToTable("EMPRESAS_SAP", "JUR");

            builder.HasKey(x => x.Codigo);
            builder.Property(x => x.Codigo).HasColumnName("CODIGO").IsRequired();
            builder.Property(x => x.Sigla).HasColumnName("SIGLA");
            builder.Property(x => x.Nome).HasColumnName("NOME");
            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO").HasConversion(ValueConverters.BoolToString);
        }
    }
}