using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
     internal class PartePedidoProcessoConfiguration : IEntityTypeConfiguration<PartePedidoProcesso>
    {
        public void Configure(EntityTypeBuilder<PartePedidoProcesso> builder)
        {
            builder.ToTable("PARTE_PEDIDO_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.ParteId, x.PedidoId});

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").HasDefaultValue(null);
            builder.HasOne(x => x.Processo).WithMany().HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.ParteId).HasColumnName("COD_PARTE").HasDefaultValue(null);
            builder.HasOne(x => x.Parte).WithMany().HasForeignKey(x => x.ParteId);

            builder.Property(x => x.CodBaseCalculo).HasColumnName("COD_BASE_CALCULO");

            builder.Property(x => x.CodOrientacaoJuridica).HasColumnName("COD_ORIENTACAO_JURIDICA");

            builder.Property(x => x.CodMotivoProvavelZero).HasColumnName("COD_MOT_PROVAVEL_ZERO");

            
            //builder.HasOne(x => x.Processo).WithMany(x => x.PartesDoProcesso).HasForeignKey(x => x.ProcessoId);
        }
    }
}
