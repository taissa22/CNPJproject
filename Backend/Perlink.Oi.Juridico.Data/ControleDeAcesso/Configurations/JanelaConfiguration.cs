using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Configurations
{
    public class JanelaConfiguration : IEntityTypeConfiguration<Janela>
    {
        public void Configure(EntityTypeBuilder<Janela> builder)
        {
            builder.ToTable("ACA_R_JANELAS_MENU", "JUR");
            builder.HasKey(x => new { x.CodAplicacao, x.CodJanela, x.CodMenu });
            
            builder.Property(bi => bi.CodAplicacao)
                .IsRequired()
                .HasColumnName("COD_APLICACAO")
                .HasColumnType("VARCHAR2(4)");
            builder.Property(bi => bi.CodJanela)
                .HasColumnName("COD_JANELA")
                .HasColumnType("VARCHAR2(40)");

            builder.Property(bi => bi.CodMenu)
               .HasColumnName("COD_MENU")
               .HasColumnType("VARCHAR2(40)");
        }
    }
}
