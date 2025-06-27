using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Configurations
{    

    internal class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToTable("EVENTO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_EVENTO").IsRequired().HasSequentialIdGenerator<Evento>("EVENTO");

            builder.Property(x => x.Nome).HasColumnName("DSC_EVENTO");

            builder.Property(x => x.PossuiDecisao).HasColumnName("IND_DECISAO").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhCivel).HasColumnName("IND_CIVEL").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhTrabalhista).HasColumnName("IND_TRABALHISTA").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhRegulatorio).HasColumnName("IND_REGULATORIO").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhPrazo).HasColumnName("IND_PRAZO").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhTributarioAdm).HasColumnName("IND_TRIBUTARIO_ADM").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhTributarioJudicial).HasColumnName("IND_TRIBUTARIO_JUD").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.FinalizacaoEscritorio).HasColumnName("IND_FINALIZACAO_PROCESSO").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.TipoMulta).HasColumnName("COD_TIPO_MULTA").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhTrabalhistaAdm).HasColumnName("IND_TRABALHISTA_ADM").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.NotificarViaEmail).HasColumnName("IND_NOTIFICAR_VIA_EMAIL").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhJuizado).HasColumnName("IND_JUIZADO").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.ReverCalculo).HasColumnName("IND_REVER_CALCULO").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.AtualizaEscritorio).HasColumnName("IND_ESCRITORIO_ATUALIZA").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhCivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.InstanciaId).HasColumnName("COD_INSTANCIA");

            builder.Property(x => x.PreencheMulta).HasColumnName("IND_PREENCHE_MULTA").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.AlterarExcluir).HasColumnName("IND_ALTERA_EXCLUI").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhCriminalAdm).HasColumnName("IND_CRIMINAL_ADM").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhCriminalJudicial).HasColumnName("IND_CRIMINAL_JUDICIAL").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhCivelAdm).HasColumnName("IND_CIVEL_ADM").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.ExigeComentario).HasColumnName("IND_COMENTARIO_OBRIG").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.FinalizacaoContabil).HasColumnName("IND_FINALIZACAO_CONTABIL").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhProcon).HasColumnName("IND_PROCON").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhPexJuizado).HasColumnName("IND_PEX_JUIZADO").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhPexCivelConsumidor).HasColumnName("IND_PEX_CIVEL_CONSUMIDOR").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.SequencialDecisao).HasColumnName("SEQ_DECISAO");

            builder.Ignore(x => x.IdDescricaoEstrategico);
            builder.Ignore(x => x.DescricaoEstrategico);
            builder.Ignore(x => x.AtivoEstrategico);

            builder.Ignore(x => x.IdDescricaoConsumidor);
            builder.Ignore(x => x.DescricaoConsumidor);
            builder.Ignore(x => x.AtivoConsumidor);
        }
    }
}