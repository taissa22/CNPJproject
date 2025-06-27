using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class ProcessoConfiguration : IEntityTypeConfiguration<Processo>
    {
        public void Configure(EntityTypeBuilder<Processo> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");
            builder.ToTable("PROCESSO", "JUR");

            builder.HasKey(pk => pk.Id).HasName("COD_PROCESSO");

            builder.Property(P => P.Id).IsRequired(true).HasColumnName("COD_PROCESSO");
            builder.Property(P => P.NumeroProcessoCartorio).HasColumnName("NRO_PROCESSO_CARTORIO");
            builder.Property(P => P.CodigoComarca).HasColumnName("COD_COMARCA");
            builder.Property(P => P.CodigoVara).HasColumnName("COD_VARA");
            builder.Property(P => P.CodigoTipoVara).HasColumnName("COD_TIPO_VARA");
            builder.Property(P => P.CodigoProfissonal).HasColumnName("COD_PROFISSIONAL");
            builder.Property(P => P.CodigoParteEmpresa).HasColumnName("COD_PARTE_EMPRESA");
            builder.Property(P => P.CodigoEstabelecimento).HasColumnName("COD_ESTABELECIMENTO");
            builder.Property(P => P.CodigoParteOrgao).HasColumnName("COD_PARTE_ORGAO");
            builder.Property(P => P.CodigoTipoProcesso).IsRequired(true).HasColumnName("COD_TIPO_PROCESSO");
            builder.Property(P => P.ResponsavelInterno).IsRequired(false).HasColumnName("COD_RESPONSAVEL_INTERNO");
            builder.Property(P => P.IndicaProcessoAtivo).IsRequired(false).HasColumnName("IND_PROCESSO_ATIVO").HasConversion(boolConverter);
            builder.Property(P => P.DataFinalizacao).IsRequired(false).HasColumnName("DAT_FINALIZACAO");
            builder.Property(P => P.DataFinalizacaoContabil).HasColumnName("DAT_FINALIZACAO_CONTABIL").IsRequired(false);
            builder.Property(P => P.CodigoRiscoPerda).HasColumnName("COD_RISCO_PERDA").IsRequired(false);
            builder.Property(bi => bi.DataCadastro).HasColumnName("DATA_CADASTRO").IsRequired(false);
            builder.Property(bi => bi.IndicadorConsiderarProvisao).HasColumnName("IND_CONSIDERAR_PROVISAO").HasConversion(boolConverter).IsRequired(false);
            builder.Property(bi => bi.PrePos).HasColumnName("PRE_POS").IsRequired(false);
            builder.Property(bi => bi.IndicadorMediacao).HasColumnName("IND_MEDIACAO").HasConversion(boolConverter).IsRequired(false);
            builder.Property(P => P.CodigoClassificacaoProcesso).HasColumnName("COD_CLASSIFICACAO_PROCESSO");
            builder.HasOne(fk => fk.Parte).WithMany(fk => fk.ProcessoParte).HasForeignKey(fk => fk.CodigoParteEmpresa);
            builder.HasOne(fk => fk.Profissional).WithMany(fk => fk.ProcessosProfissional).HasForeignKey(fk => fk.CodigoProfissonal);
            builder.HasOne(fk => fk.Comarca).WithMany(fk => fk.ProcessosComarca).HasForeignKey(fk => fk.CodigoComarca);
            builder.HasOne(fk => fk.TipoVara).WithMany(fk => fk.ProcessosTipoVara).HasForeignKey(fk => fk.CodigoTipoVara);
            builder.HasOne(fk => fk.Vara).WithMany(fk => fk.ProcessosVara).HasForeignKey(fk => new { fk.CodigoComarca, fk.CodigoVara, fk.CodigoTipoVara });
            builder.HasOne(fk => fk.Usuario).WithMany(fk => fk.ProcessosUsuario).HasForeignKey(fk => fk.ResponsavelInterno);
        }
    }
}
