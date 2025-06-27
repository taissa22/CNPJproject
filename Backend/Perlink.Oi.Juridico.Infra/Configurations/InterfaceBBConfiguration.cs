using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class InterfaceBBConfiguration : IEntityTypeConfiguration<InterfaceBB>
    {
        public void Configure(EntityTypeBuilder<InterfaceBB> builder)
        {
            builder.ToTable("DIRETORIO_BANCO_BRASIL", "JUR");
            builder.HasKey(x => x.CodigoDiretorio);

            builder.Property(x => x.CodigoDiretorio).HasColumnName("COD_DIRETORIO");            
            builder.Property(x => x.Descricao).HasColumnName("DSC_DIRETORIO");
        }
    }
}