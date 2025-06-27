using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class EmpresasCentralizadorasConfiguration : IEntityTypeConfiguration<EmpresasCentralizadoras>
    {
        public void Configure(EntityTypeBuilder<EmpresasCentralizadoras> builder)
        {
            builder.ToTable("EMPRESAS_CENTRALIZADORAS", "JUR");
            builder.HasKey(pk => pk.Id).HasName("CODIGO");

            builder.Property(c => c.Id).HasColumnName("CODIGO");
            builder.Property(c => c.Nome).HasColumnName("NOME");
            builder.Property(c => c.NumeroOrdemClassificacaoProcesso).HasColumnName("NUM_ORDEM_CLASSIF_PROCESSO");
        }
    }
}