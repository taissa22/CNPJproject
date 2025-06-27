using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class FechamentoCCMediaConfiguration : IEntityTypeConfiguration<FechamentoCCMedia>
    {
        public void Configure(EntityTypeBuilder<FechamentoCCMedia> builder)
        {
            builder.ToTable("FECH_CIVEL_CONS_MEDIA", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID");

            builder.Property(x => x.DataFechamento).HasColumnName("DATA_FECHAMENTO");
            builder.Property(x => x.NumeroMesesMediaHistorica).HasColumnName("NUM_MESES_MEDIA_HISTORICA");
            builder.Property(x => x.IndMensal).HasColumnName("IND_MENSAL").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.DataIndMensal).HasColumnName("DATA_IND_MENSAL").IsRequired(false);
            builder.Property(x => x.IndBaseGerada).HasColumnName("IND_BASE_GERADA").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.DataGeracao).HasColumnName("DATA_GERACAO");
            builder.Property(x => x.ValorCorte).HasColumnName("VALOR_CORTE").IsRequired(false);
            builder.Property(x => x.PercentualHairCut).HasColumnName("PERC_HAIRCUT");
            builder.Property(x => x.SolicitacaoFechamentoContingencia).HasColumnName("COD_SOLIC_FECHAMENTO_CONT");
            builder.Property(x => x.EmpresaCentralizadoraId).HasColumnName("EMPCE_COD_EMP_CENTRALIZADORA");
            builder.Property(x => x.UsuarioId).HasColumnName("USR_COD_USUARIO");
            builder.Property(x => x.EmpresaCentralizadoraAgenFechAutoId).HasColumnName("COD_SOLIC_FECHAMENTO_CONT");

            builder.Ignore(x => x.EmpresasParticipantes);
            builder.Ignore(x => x.FechamentoGerado);

            #region ENTITIES

            builder.HasOne(x => x.EmpresaCentralizadora).WithMany().HasForeignKey(x => x.EmpresaCentralizadoraId);
            builder.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId);
            builder.HasOne(x => x.EmpresaCentralizadoraAgendamentoFechAuto).WithMany().HasForeignKey(x => x.EmpresaCentralizadoraAgenFechAutoId);

            #endregion ENTITIES
        }
    }
}