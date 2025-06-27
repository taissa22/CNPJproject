using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AcessoRecenteProcessoConfiguration : IEntityTypeConfiguration<AcessoRecenteProcesso> {
        public void Configure(EntityTypeBuilder<AcessoRecenteProcesso> builder) {
            builder.ToTable("PROCESSO_ACESSO_RECENTES", "JUR");

            builder.HasKey(x => x.Id)
                   .HasName("PRACR_PK");

            builder.Property(x => x.Id).IsRequired()
               .HasColumnName("ID").ValueGeneratedOnAdd()
               .HasValueGenerator((a, b) => new NextSequenceValueGenerator("jur", "PRACR_SEQ_01"));

            builder.Property(x => x.UltimoAcesso).IsRequired().HasColumnName("DATA_ULTIMO_ACESSO");

            builder.Property(x => x.ProcessoId).HasColumnName("PROC_COD_PROCESSO");
            //builder.HasOne(x => x.Processo).WithMany().HasForeignKey("PROC_COD_PROCESSO");

            builder.Property(x => x.Usuario).HasColumnName("USR_COD_USUARIO");
        }
    }
}
