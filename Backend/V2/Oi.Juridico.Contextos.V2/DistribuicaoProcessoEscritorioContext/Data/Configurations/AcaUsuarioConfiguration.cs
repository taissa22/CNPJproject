﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Data;
using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Data.Configurations
{
    public partial class AcaUsuarioConfiguration : IEntityTypeConfiguration<AcaUsuario>
    {
        public void Configure(EntityTypeBuilder<AcaUsuario> entity)
        {
            entity.HasKey(e => e.CodUsuario);

            entity.ToTable("ACA_USUARIO");

            entity.Property(e => e.CodUsuario)
                .HasMaxLength(30)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("COD_USUARIO");

            entity.Property(e => e.AreaCodAreaJec)
                .HasPrecision(4)
                .ValueGeneratedOnAdd()
                .HasColumnName("AREA_COD_AREA_JEC");

            entity.Property(e => e.CodOrigemUsuario)
                .HasColumnType("NUMBER")
                .ValueGeneratedOnAdd()
                .HasColumnName("COD_ORIGEM_USUARIO");

            entity.Property(e => e.CodSituacaoUsuario)
                .HasMaxLength(1)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("COD_SITUACAO_USUARIO")
                .IsFixedLength();

            entity.Property(e => e.DatValidadeSenha)
                .HasColumnType("DATE")
                .ValueGeneratedOnAdd()
                .HasColumnName("DAT_VALIDADE_SENHA");

            entity.Property(e => e.DscEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("DSC_EMAIL");

            entity.Property(e => e.IdBo)
                .HasColumnType("NUMBER")
                .ValueGeneratedOnAdd()
                .HasColumnName("ID_BO");

            entity.Property(e => e.NomeUsuario)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasColumnName("NOME_USUARIO");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<AcaUsuario> entity);
    }
}
