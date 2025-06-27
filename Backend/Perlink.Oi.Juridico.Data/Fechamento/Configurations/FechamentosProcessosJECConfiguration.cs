using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Fechamento.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.Fechamento.Configurations {
    public class FechamentosProcessosJECConfiguration : IEntityTypeConfiguration<FechamentosProcessosJEC> {
        public void Configure(EntityTypeBuilder<FechamentosProcessosJEC> builder) {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("FECHAMENTOS_PROCESSOS_JUIZADOS","JUR");
            
            builder.HasKey(k => new {k.Id, k.MesAnoFechamento, k.DataFechamento});

            builder.Property(p => p.Id).HasColumnName("EMPCE_COD_EMP_CENTRALIZADORA");
            builder.Property(p => p.MesAnoFechamento).HasColumnName("MES_ANO_FECHAMENTO");
            builder.Property(p => p.DataFechamento).HasColumnName("DATA_FECHAMENTO");
            builder.Property(p => p.NumeroMeses).HasColumnName("NUM_MESES_FECHAMENTO");
            builder.Property(p => p.DataGeracao).HasColumnName("DATA_GERACAO");
            builder.Property(p => p.IndicaFechamentoMes).HasConversion(boolConverter).HasColumnName("IND_FECHAMENTO_MES");
            builder.Property(p => p.CodigoUsuario).HasColumnName("USR_COD_USUARIO_GERACAO");
            builder.Property(p => p.CodigoEmpresaCentralizadoraAssociada).HasColumnName("FECHJ_COD_EMP_CENT_ASSOCIADA");
            builder.Property(p => p.MesAnoFechamentoAssociado).HasColumnName("FECHJ_MES_ANO_FECH_ASSOCIADO");
            builder.Property(p => p.DataFechamentoAssociado).HasColumnName("FECHJ_DATA_FECH_ASSOCIADA");
            builder.Property(p => p.ValorCorte).HasColumnName("VALOR_CORTE");
            builder.Property(p => p.TipoDataMediaMovel).HasColumnName("TP_DATA_MEDIA_MOVEL");
        }
    }
}
