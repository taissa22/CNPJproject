﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.RelatoriosContingenciaTrabalhistaContext.Data;
using Oi.Juridico.Contextos.RelatoriosContingenciaTrabalhistaContext.Entities;
using System;

namespace Oi.Juridico.Contextos.RelatoriosContingenciaTrabalhistaContext.Data.Configurations
{
    public partial class ItemFechProvisaoPossiveisConfiguration : IEntityTypeConfiguration<ItemFechProvisaoPossiveis>
    {
        public void Configure(EntityTypeBuilder<ItemFechProvisaoPossiveis> entity)
        {
            entity.ToTable("ITEM_FECH_PROVISAO_POSSIVEIS");

            //entity.HasIndex(e => e.IfptrIdItemFechProvTrab, "IFPPO_IFPTR_FK_IX_01");

            entity.Property(e => e.Id)
                .HasColumnType("NUMBER")
                .HasColumnName("ID");

            entity.Property(e => e.IfptrIdItemFechProvTrab)
                .HasColumnType("NUMBER")
                .HasColumnName("IFPTR_ID_ITEM_FECH_PROV_TRAB");

            entity.Property(e => e.NomePedido)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NOME_PEDIDO");

            entity.Property(e => e.PedCodPedido)
                //.HasPrecision(4)
                .HasColumnName("PED_COD_PEDIDO");

            entity.Property(e => e.PerResponsOi)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("PER_RESPONS_OI")
                .HasDefaultValueSql("0 ");

            entity.Property(e => e.PercPerdaPossivel)
                .HasColumnType("NUMBER(7,4)")
                .HasColumnName("PERC_PERDA_POSSIVEL");

            entity.Property(e => e.PercPerdaProvavel)
                .HasColumnType("NUMBER(7,4)")
                .HasColumnName("PERC_PERDA_PROVAVEL");

            entity.Property(e => e.QteExpectativaPerdaH)
                .HasColumnType("NUMBER")
                .HasColumnName("QTE_EXPECTATIVA_PERDA_H");
                //.HasComputedColumnSql("ROUND(\"PERC_PERDA_PROVAVEL\"*\"QTE_PEDIDOS_POSSIVEL_H\",0)", false);

            entity.Property(e => e.QtePedidosPossivelH)
                .HasColumnType("NUMBER")
                .HasColumnName("QTE_PEDIDOS_POSSIVEL_H")
                .HasDefaultValueSql("0 ");

            entity.Property(e => e.QtePedidosPossivelP)
                .HasColumnType("NUMBER")
                .HasColumnName("QTE_PEDIDOS_POSSIVEL_P")
                .HasDefaultValueSql("0	");

            entity.Property(e => e.QtePedidosProvavel)
                .HasColumnType("NUMBER")
                .HasColumnName("QTE_PEDIDOS_PROVAVEL")
                .HasDefaultValueSql("0	");

            entity.Property(e => e.QtePedidosRemoto)
                .HasColumnType("NUMBER")
                .HasColumnName("QTE_PEDIDOS_REMOTO")
                .HasDefaultValueSql("0	");

            entity.Property(e => e.ValProvContJuros)
                .HasColumnType("NUMBER")
                .HasColumnName("VAL_PROV_CONT_JUROS");
            //.HasComputedColumnSql("ROUND(\"PERC_PERDA_PROVAVEL\"*\"VALOR_MEDIO_DESEM_JUROS_PR\"*\"QTE_PEDIDOS_POSSIVEL_P\"*\"PERC_PERDA_POSSIVEL\",2)+ROUND(\"VALOR_MEDIO_DESEM_JUROS_PR\"*\"PERC_PERDA_POSSIVEL\"*(\"PER_RESPONS_OI\"/100)*ROUND(\"PERC_PERDA_PROVAVEL\"*\"QTE_PEDIDOS_POSSIVEL_H\",0),2)", false);

            entity.Property(e => e.ValProvContJurosH)
                .HasColumnType("NUMBER")
                .HasColumnName("VAL_PROV_CONT_JUROS_H");
            //.HasComputedColumnSql("ROUND(\"VALOR_MEDIO_DESEM_JUROS_PR\"*\"PERC_PERDA_POSSIVEL\"*(\"PER_RESPONS_OI\"/100)*ROUND(\"PERC_PERDA_PROVAVEL\"*\"QTE_PEDIDOS_POSSIVEL_H\",0),2)", false);

            entity.Property(e => e.ValProvContJurosP)
                .HasColumnType("NUMBER")
                .HasColumnName("VAL_PROV_CONT_JUROS_P");
                //.HasComputedColumnSql("ROUND(\"PERC_PERDA_PROVAVEL\"*\"VALOR_MEDIO_DESEM_JUROS_PR\"*\"QTE_PEDIDOS_POSSIVEL_P\"*\"PERC_PERDA_POSSIVEL\",2)", false);

            entity.Property(e => e.ValProvContPrincipal)
                .HasColumnType("NUMBER")
                .HasColumnName("VAL_PROV_CONT_PRINCIPAL");
            //.HasComputedColumnSql("ROUND(\"PERC_PERDA_PROVAVEL\"*\"VALOR_MEDIO_DESEM_PRINCIPAL_PR\"*\"QTE_PEDIDOS_POSSIVEL_P\"*\"PERC_PERDA_POSSIVEL\",2)+ROUND(\"VALOR_MEDIO_DESEM_PRINCIPAL_PR\"*\"PERC_PERDA_POSSIVEL\"*(\"PER_RESPONS_OI\"/100)*ROUND(\"PERC_PERDA_PROVAVEL\"*\"QTE_PEDIDOS_POSSIVEL_H\",0),2)", false);

            entity.Property(e => e.ValProvContPrincipalH)
                .HasColumnType("NUMBER")
                .HasColumnName("VAL_PROV_CONT_PRINCIPAL_H");
            //.HasComputedColumnSql("ROUND(\"VALOR_MEDIO_DESEM_PRINCIPAL_PR\"*\"PERC_PERDA_POSSIVEL\"*(\"PER_RESPONS_OI\"/100)*ROUND(\"PERC_PERDA_PROVAVEL\"*\"QTE_PEDIDOS_POSSIVEL_H\",0),2)", false);

            entity.Property(e => e.ValProvContPrincipalP)
                .HasColumnType("NUMBER")
                .HasColumnName("VAL_PROV_CONT_PRINCIPAL_P");
                //.HasComputedColumnSql("ROUND(\"PERC_PERDA_PROVAVEL\"*\"VALOR_MEDIO_DESEM_PRINCIPAL_PR\"*\"QTE_PEDIDOS_POSSIVEL_P\"*\"PERC_PERDA_POSSIVEL\",2)", false);

            entity.Property(e => e.ValorMedioDesemJurosPr)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("VALOR_MEDIO_DESEM_JUROS_PR")
                .HasDefaultValueSql("0	");

            entity.Property(e => e.ValorMedioDesemPrincipalPr)
                .HasColumnType("NUMBER(15,2)")
                .HasColumnName("VALOR_MEDIO_DESEM_PRINCIPAL_PR")
                .HasDefaultValueSql("0	");

            entity.HasOne(d => d.IfptrIdItemFechProvTrabNavigation)
                .WithMany(p => p.ItemFechProvisaoPossiveis)
                .HasForeignKey(d => d.IfptrIdItemFechProvTrab)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IFPPO_IFPTR_FK");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<ItemFechProvisaoPossiveis> entity);
    }
}
