using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    [ExcludeFromCodeCoverage]
    public class PedidoProcessoConfiguration : IEntityTypeConfiguration<PedidoProcesso>
    {
        public void Configure(EntityTypeBuilder<PedidoProcesso> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("PEDIDO_PROCESSO", "JUR");
            builder.HasKey(bi => new { bi.Id, bi.CodigoProcesso });

            builder.Property(bi => bi.Id).HasColumnName("COD_PEDIDO").IsRequired();            
            builder.Property(bi => bi.CodigoProcesso).HasColumnName("COD_PROCESSO").IsRequired();
            builder.Property(bi => bi.Comentario).HasColumnName("COMENTARIO").HasMaxLength(4000);            
            builder.Property(bi => bi.IndicaAcessoRestrito).HasColumnName("IND_ACESSO_RESTRITO")
                                .IsRequired().HasConversion(boolConverter);            

            builder.HasOne(bi => bi.Pedido).WithMany(a => a.PedidosProcessos).HasForeignKey(s => s.Id);
            builder.HasOne(bi => bi.Processo).WithMany(a => a.PedidoProcessos).HasForeignKey(s => s.CodigoProcesso);
        }
    }
}
