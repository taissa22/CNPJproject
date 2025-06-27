using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class GrupoCorrecaoGarantiaConfiguration : IEntityTypeConfiguration<GrupoCorrecaoGarantia>
    {
        public void Configure(EntityTypeBuilder<GrupoCorrecaoGarantia> builder)
        {
            builder.ToTable("GRUPOS_CORRECOES_GARANTIAS", "JUR");

            builder.HasKey(pk => pk.Id).HasName("ID");
            
            builder.Property(P => P.Id).IsRequired(true).HasColumnName("ID");
            builder.Property(P => P.CodigoTipoProcesso).IsRequired(true).HasColumnName("TP_COD_TIPO_PROCESSO");
            builder.Property(P => P.Descricao).IsRequired(true).HasColumnName("DESCRICAO");

            //Fks
            builder.HasOne(fk => fk.TipoProcesso)
                    .WithMany(fk => fk.GruposCorrecoesGarantias).HasForeignKey(fk => fk.CodigoTipoProcesso);
        }
    }
}
