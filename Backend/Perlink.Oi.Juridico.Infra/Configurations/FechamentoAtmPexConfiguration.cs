using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class FechamentoAtmPexConfiguration : IEntityTypeConfiguration<FechamentoAtmPex>
    {
        public void Configure(EntityTypeBuilder<FechamentoAtmPex> builder)
        {
            builder.ToTable("FECHAMENTO_PEX_MEDIA", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");

            builder.Property(x => x.DataFechamento).HasColumnName("DAT_FECHAMENTO");
            builder.Property(x => x.NumeroDeMeses).HasColumnName("NRO_MESES_MEDIA_HISTORICA");
            builder.Property(x => x.ValDesvioPadrao).HasColumnName("VAL_MULT_DESVIO_PADRAO");
            builder.Property(x => x.CodSolicFechamento).HasColumnName("COD_SOLIC_FECHAMENTO_CONT");
        }
    }

    internal class AgendamentoFechamentoAtmPexConfiguration : IEntityTypeConfiguration<AgendamentoDeFechamentoAtmPex>
    {
        public void Configure(EntityTypeBuilder<AgendamentoDeFechamentoAtmPex> builder)
        {
            builder.ToTable("AGEND_FECH_ATM_PEX", "JUR");

            builder.HasKey(x => x.AgendamentoId);
            builder.Property(x => x.AgendamentoId)
                .HasColumnName("ID").IsRequired()
                .HasNextSequenceValueGenerator("JUR", "AFAP_SEQ_01");


            builder.Property(x => x.CodSolicFechamento)
              .HasColumnName("COD_SOLIC_FECHAMENTO").IsRequired();

            builder.Property(x => x.NumeroDeMeses)
                .HasColumnName("NRO_MESES_MEDIA_HISTORICA");

            builder.Property(x => x.ValDesvioPadrao)
                .HasColumnName("VAL_MULT_DESVIO_PADRAO");

            builder.Property(x => x.DataFechamento)
              .HasColumnName("DAT_FECHAMENTO");

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

            //builder.HasMany(x => x.Indices).WithOne(x => x.Agendamento).HasForeignKey(x => x.Id);
        }
    }

    internal class AgendamentoDeFechamentoAtmUfPexConfiguration : IEntityTypeConfiguration<AgendamentoDeFechamentoAtmUfPex>
    {
        public void Configure(EntityTypeBuilder<AgendamentoDeFechamentoAtmUfPex> builder)
        {
            builder.ToTable("AGEND_FECH_ATM_PEX_UF", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("ID").IsRequired()
                .HasNextSequenceValueGenerator("JUR", "AFAPU_SEQ_01");

            builder.Property(x => x.AgendamentoId)
             .HasColumnName("AG_FECH_ATM_PEX_ID")
             .IsRequired();

            //builder.HasOne(x => x.Agendamento).WithMany()
            //   .HasForeignKey(x => x.AgendamentoId);

            builder.Property(x => x.TipoProcessoId)
               .HasColumnName("COD_TIPO_PROCESSO").IsRequired();

            builder.Property(x => x.CodEstado)
                .HasColumnName("COD_ESTADO").HasMaxLength(2).IsRequired();

            builder.Property(x => x.IndiceId)
                .HasColumnName("COD_INDICE").IsRequired();

        }
    }
}