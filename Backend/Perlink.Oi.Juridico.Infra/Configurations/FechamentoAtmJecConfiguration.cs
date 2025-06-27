using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class FechamentoAtmJecConfiguration : IEntityTypeConfiguration<FechamentoAtmJec>
    {
        public void Configure(EntityTypeBuilder<FechamentoAtmJec> builder)
        {
            builder.ToTable("V_FECHAMENTO_ATM_JEC_GERAL", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("FAJ_ID");

            builder.Property(x => x.MesAnoFechamento).HasColumnName("MES_ANO_FECHAMENTO");
            builder.Property(x => x.DataFechamento).HasColumnName("DATA_FECHAMENTO");

            builder.Property(x => x.NumeroDeMeses).HasColumnName("NUM_MESES_FECHAMENTO");

            builder.Property(x => x.Mensal).HasColumnName("IND_FECHAMENTO_MES")
                    .HasConversion(ValueConverters.BoolToString);
        }
    }

    internal class AgendamentoFechamentoAtmJecConfiguration : IEntityTypeConfiguration<AgendamentoDeFechamentoAtmJec>
    {
        public void Configure(EntityTypeBuilder<AgendamentoDeFechamentoAtmJec> builder)
        {
            builder.ToTable("AGEND_FECH_ATM_JEC", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("AFAJ_ID").IsRequired()
                .HasNextSequenceValueGenerator("JUR", "AGEND_FECH_ATM_JEC_SEQ");

            builder.Property(x => x.FechamentoId)
                .HasColumnName("FAJ_ID")
                .IsRequired();
            builder.HasOne(x => x.Fechamento).WithMany()
                .HasForeignKey(x => x.FechamentoId);

            builder.Property(x => x.UsuarioId)
                .HasColumnName("USR_COD_USUARIO")
                .IsRequired();
            builder.HasOne(x => x.Usuario).WithMany()
                .HasForeignKey(x => x.UsuarioId);

            builder.Property(x => x.DataAgendamento)
                .HasColumnName("DAT_AGENDAMENTO")
                .IsRequired();

            builder.Property(x => x.InicioDaExecucao)
                .HasColumnName("DAT_INICIO_EXECUCAO")
                .IsRequired();

            builder.Property(x => x.FimDaExecucao)
                .HasColumnName("DAT_FIM_EXECUCAO")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("STATUS")
                .IsRequired();

            builder.Property(x => x.Erro).HasColumnName("MSG_ERRO");

            builder.HasMany(x => x.Indices).WithOne(x => x.Agendamento).HasForeignKey(x => x.Id);
        }
    }

    internal class AgendamentoDeFechamentoAtmUfJecConfiguration : IEntityTypeConfiguration<AgendamentoDeFechamentoAtmUfJec>
    {
        public void Configure(EntityTypeBuilder<AgendamentoDeFechamentoAtmUfJec> builder)
        {
            builder.ToTable("AGEND_FECH_ATM_UF_JEC", "JUR");

            builder.HasKey(x => new { x.Id, x.Estado });

            //AFAJ_ID       NUMBER(8,0)         No
            builder.Property(x => x.Id)
                .HasColumnName("AFAJ_ID").IsRequired();

            //COD_ESTADO    VARCHAR2(3 BYTE)    No
            builder.Property(x => x.Estado)
                .HasColumnName("COD_ESTADO").IsRequired();

            //COD_INDICE    NUMBER(4,0)         No
            builder.Property(x => x.IndiceId)
                .HasColumnName("COD_INDICE").IsRequired();

            //IND_ACUMULADO CHAR(1 BYTE)        No
            builder.Property(x => x.Acumulado)
                .HasColumnName("IND_ACUMULADO").IsRequired()
                .HasConversion(ValueConverters.BoolToString);
        }
    }
}