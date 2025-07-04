﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.Data;
using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.Entities;
using System;


namespace Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.Data.Configurations
{
    public partial class EmpresasCentralizadorasConfiguration : IEntityTypeConfiguration<EmpresasCentralizadoras>
    {
        public void Configure(EntityTypeBuilder<EmpresasCentralizadoras> entity)
        {
            entity.HasKey(e => e.Codigo)
                .HasName("EMPCE_PK");

            entity.ToTable("EMPRESAS_CENTRALIZADORAS");

            entity.Property(e => e.Codigo)
                //.HasPrecision(4)
                .HasColumnName("CODIGO");

            entity.Property(e => e.CodConvenioBb)
                //.HasPrecision(4)
                .HasColumnName("COD_CONVENIO_BB");

            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOME");

            entity.Property(e => e.NumAgenciaDepositaria)
                //.HasPrecision(4)
                .HasColumnName("NUM_AGENCIA_DEPOSITARIA");

            entity.Property(e => e.NumDigitoAgenciaDepositaria)
                //.HasPrecision(1)
                .HasColumnName("NUM_DIGITO_AGENCIA_DEPOSITARIA");

            entity.Property(e => e.NumOrdemClassifProcesso)
                //.HasPrecision(3)
                .HasColumnName("NUM_ORDEM_CLASSIF_PROCESSO");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<EmpresasCentralizadoras> entity);
    }
}
