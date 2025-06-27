using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
	public class TipoVaraConfiguration : IEntityTypeConfiguration<TipoVara>
	{
		public void Configure(EntityTypeBuilder<TipoVara> builder)
		{
			var boolConverter = new BoolToStringConverter("N", "S");

			builder.ToTable("TIPO_VARA", "JUR");

			builder.HasKey(pk => pk.Id).HasName("COD_TIPO_VARA");

			builder.Property(tv => tv.Id).HasColumnName("COD_TIPO_VARA");
			builder.Property(tv => tv.NomeTipoVara).HasColumnName("NOM_TIPO_VARA");
			builder.Property(tv => tv.IndicadorCivel).HasConversion(boolConverter).HasColumnName("IND_CIVEL");
			builder.Property(tv => tv.IndicadorTrabalhista).HasConversion(boolConverter).HasColumnName("IND_TRABALHISTA");
			builder.Property(tv => tv.IndicadorTributaria).HasConversion(boolConverter).HasColumnName("IND_TRIBUTARIA");
			builder.Property(tv => tv.IndicadorJuizado).HasConversion(boolConverter).HasColumnName("IND_JUIZADO");
			builder.Property(tv => tv.IndicadorCivelEstrategico).HasConversion(boolConverter).HasColumnName("IND_CIVEL_ESTRATEGICO");
			builder.Property(tv => tv.IndicadorCriminalJudicial).HasConversion(boolConverter).HasColumnName("IND_CRIMINAL_JUDICIAL");
			builder.Property(tv => tv.IndicadorProcon).HasConversion(boolConverter).HasColumnName("IND_PROCON");

		}
	}
}
