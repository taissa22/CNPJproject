using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("PEDIDO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PEDIDO").IsRequired()
                .HasSequentialIdGenerator<Pedido>("PEDIDO");

            builder.Property(x => x.Descricao).HasColumnName("DSC_PEDIDO");

            builder.Property(x => x.Ativo).HasColumnName("IND_PEDIDO_ATIVO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhTrabalhista).HasColumnName("IND_PEDIDO_TRABALHISTA")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.RiscoPerdaId).HasColumnName("COD_RISCO_PERDA");

            builder.Property(x => x.ProvavelZero).HasColumnName("IND_PROVAVEL_ZERO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.ProprioTerceiroId).HasColumnName("IND_PROPRIO_TERCEIRO").HasDefaultValue("P");


            builder.Property(x => x.EhTributarioAdministrativo).HasColumnName("IND_PEDIDO_TRIBUTARIO_ADM").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhTributarioJudicial).HasColumnName("IND_PEDIDO_TRIBUTARIO_JUD").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhTrabalhistaAdministrativo).HasColumnName("IND_PEDIDO_TRABALHISTA_ADM").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.AtivoTributarioAdministrativo).HasColumnName("IND_ATIVO_TRIBUTARIO_ADM").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.AtivoTributarioJudicial).HasColumnName("IND_ATIVO_TRIBUTARIO_JUD").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhCivel).HasColumnName("IND_PEDIDO_CIVEL").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.GrupoPedidoId).HasColumnName("GRPD_COD_GRUPO_PEDIDO");

            builder.Property(x => x.Audiencia).HasColumnName("IND_REQUER_ATUALIZACAO_DEBITO")
                .HasConversion(ValueConverters.BoolToString);


        }
    }
}