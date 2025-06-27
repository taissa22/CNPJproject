using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class PedidoProcessoConfiguration : IEntityTypeConfiguration<PedidoProcesso>
    {
        public void Configure(EntityTypeBuilder<PedidoProcesso> builder)
        {
            builder.ToTable("PEDIDO_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.PedidoId });

            builder.Property(x => x.PedidoId).HasColumnName("COD_PEDIDO").IsRequired();
            
            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.HasOne(x => x.Processo).WithMany().HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.Comentario).HasColumnName("COMENTARIO");

            builder.Property(x => x.AcessoRestrito).HasColumnName("IND_ACESSO_RESTRITO")
                .HasConversion(ValueConverters.BoolToString);

            builder.HasOne(x => x.Processo).WithMany(x => x.PedidosDoProcesso).HasForeignKey(x => x.ProcessoId);
        }
    }
}