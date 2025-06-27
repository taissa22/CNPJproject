using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("ACA_MENU", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("COD_MENU");

            builder.Property(bi => bi.Id)
                .HasColumnName("COD_MENU")
                .HasColumnType("VARCHAR2(40)");

            builder.Property(bi => bi.DescricaoMenu)
                .HasColumnName("DESCRICAO_MENU")
                .HasColumnType("VARCHAR2(255)");

            builder.Property(bi => bi.CaminhoMenu)
                .HasColumnName("DSC_CAMINHO_MENU_TELA")
                .HasColumnType("VARCHAR2(255)");

            builder.Property(bi => bi.TipoMenu)
                .HasColumnName("TIPO_MENU")
                .HasColumnType("VARCHAR2(6)");
        }
    }
}
