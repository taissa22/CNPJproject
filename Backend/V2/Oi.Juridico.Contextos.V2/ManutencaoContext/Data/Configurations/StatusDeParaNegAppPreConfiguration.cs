﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oi.Juridico.Contextos.V2.ManutencaoContext.Data;
using Oi.Juridico.Contextos.V2.ManutencaoContext.Entities;
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.ManutencaoContext.Data.Configurations
{
    public partial class StatusDeParaNegAppPreConfiguration : IEntityTypeConfiguration<StatusDeParaNegAppPre>
    {
        public void Configure(EntityTypeBuilder<StatusDeParaNegAppPre> entity)
        {
            entity.ToTable("STATUS_DE_PARA_NEG_APP_PRE");

            entity.Property(e => e.Id)
                .HasPrecision(4)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");

            entity.Property(e => e.Descricao)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DESCRICAO");

            entity.Property(e => e.Substatus)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("SUBSTATUS")
                .HasDefaultValueSql("'N' ")
                .IsFixedLength();

            entity.Property(e => e.TipoContato)
                .HasPrecision(4)
                .HasColumnName("TIPO_CONTATO");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<StatusDeParaNegAppPre> entity);
    }
}
