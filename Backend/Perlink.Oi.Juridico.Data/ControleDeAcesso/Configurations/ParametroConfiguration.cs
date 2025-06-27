using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Configurations
{
    public class ParametroConfiguration : IEntityTypeConfiguration<Parametro>
    {
        public void Configure(EntityTypeBuilder<Parametro> builder)
        {
            builder.ToTable("PARAMETRO_JURIDICO", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("COD_PARAMETRO");

            builder.Property(bi => bi.Id)
                .IsRequired()
                .HasColumnName("COD_PARAMETRO")
                .HasColumnType("VARCHAR2(25)");

            builder.Property(bi => bi.TipoParametro)
                .HasColumnName("COD_TIPO_PARAMETRO")
                .HasColumnType("CHAR(1)");

            builder.Property(bi => bi.Descricao)
                .HasColumnName("DSC_PARAMETRO")
                .HasColumnType("VARCHAR2(500)");

            builder.Property(bi => bi.Conteudo)
               .HasColumnName("DSC_CONTEUDO_PARAMETRO")
               .HasColumnType("VARCHAR2(4000)");
        }
    }
}
