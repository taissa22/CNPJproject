using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class FatoGeradorProcessoConfiguration : IEntityTypeConfiguration<FatoGeradorProcesso>
    {
        public void Configure(EntityTypeBuilder<FatoGeradorProcesso> builder)
        {
            builder.ToTable("FATO_GERADOR_PROCESSO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("PROC_COD_PROCESSO").IsRequired(); ;
            
            builder.Property(x => x.FatoGeradorId).HasColumnName("FTGER_ID_FATO_GERADOR").IsRequired();
        }
    }
}