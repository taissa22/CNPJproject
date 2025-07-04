﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.ESocialContext.Data.Configurations
{
    public partial class EsF2501InfoprocretConfiguration : IEntityTypeConfiguration<EsF2501Infoprocret>
    {
        public void Configure(EntityTypeBuilder<EsF2501Infoprocret> entity)
        {
            entity.HasKey(e => e.IdEsF2501Infoprocret)
                .HasName("ES_F2501_INFOPROCRET_PK");

            entity.ToTable("ES_F2501_INFOPROCRET");

            entity.Property(e => e.IdEsF2501Infoprocret)
                .ValueGeneratedOnAdd()
                .HasPrecision(15)
                .HasColumnName("ID_ES_F2501_INFOPROCRET");

            entity.Property(e => e.IdEsF2501Infocrirrf)
                .HasPrecision(15)
                .HasColumnName("ID_ES_F2501_INFOCRIRRF");

            entity.Property(e => e.InfoprocretCodsusp)
                .HasPrecision(14)
                .HasColumnName("INFOPROCRET_CODSUSP");

            entity.Property(e => e.InfoprocretNrprocret)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("INFOPROCRET_NRPROCRET");

            entity.Property(e => e.InfoprocretTpprocret)
                .HasPrecision(1)
                .HasColumnName("INFOPROCRET_TPPROCRET");

            entity.Property(e => e.LogCodUsuario)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("LOG_COD_USUARIO");

            entity.Property(e => e.LogDataOperacao)
                .HasPrecision(6)
                .HasColumnName("LOG_DATA_OPERACAO");

            entity.HasOne(d => d.IdEsF2501InfocrirrfNavigation)
                .WithMany(p => p.EsF2501Infoprocret)
                .HasForeignKey(d => d.IdEsF2501Infocrirrf)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ES_F2501_INFOPROCRET_FK1");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<EsF2501Infoprocret> entity);
    }
}
