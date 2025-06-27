using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ProcessoConfiguration : IEntityTypeConfiguration<Processo>
    {
        public void Configure(EntityTypeBuilder<Processo> builder)
        {
            builder.ToTable("PROCESSO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PROCESSO")
               .IsRequired().HasNextSequenceValueGenerator("JUR", "PROC_SEQ_01");

            builder.Property(x => x.NumeroProcesso).HasColumnName("NRO_PROCESSO_CARTORIO");

            builder.Property(x => x.TipoProcessoId).HasColumnName("COD_TIPO_PROCESSO");
            //builder.Property(x => x.TipoProcesso).HasColumnName("COD_TIPO_PROCESSO").HasConversion(ValueConverters.TipoProcessoToInt);

            builder.Property(x => x.EmpresaDoGrupoId).HasColumnName("COD_PARTE_EMPRESA");
            builder.HasOne(x => x.EmpresaDoGrupo).WithMany().HasForeignKey(x => x.EmpresaDoGrupoId);

            builder.Property(x => x.AssuntoId).HasColumnName("COD_ASSUNTO").HasDefaultValue(null);
            builder.HasOne(x => x.Assunto).WithMany().HasForeignKey(x => x.AssuntoId);

            builder.Property(x => x.AcaoId).HasColumnName("COD_ACAO").HasDefaultValue(null);
            builder.HasOne(x => x.Acao).WithMany().HasForeignKey(x => x.AcaoId);

            builder.HasMany(x => x.Lancamentos).WithOne(x => x.Processo).HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.AdvogadoId).HasColumnName("COD_ADVOGADO").HasDefaultValue(null);

            builder.Property(x => x.EscritorioId).HasColumnName("COD_PROFISSIONAL").HasDefaultValue(null);
            builder.HasOne(x => x.Escritorio).WithMany().HasForeignKey(x => x.EscritorioId);

            builder.Property(x => x.EstabelecimentoId).HasColumnName("COD_ESTABELECIMENTO").HasDefaultValue(null);
            builder.HasOne(x => x.Estabelecimento).WithMany().HasForeignKey(x => x.EstabelecimentoId);

            builder.Property(x => x.OrgaoId).HasColumnName("COD_PARTE_ORGAO").HasDefaultValue(null);
            builder.HasOne(x => x.Orgao).WithMany().HasForeignKey(x => x.OrgaoId);

            builder.Property(x => x.EscritorioAcompanhanteId).HasColumnName("COD_ESCRITORIO_ACOMPANHANTE").HasDefaultValue(null);
            builder.HasOne(x => x.EscritorioAcompanhante).WithMany().HasForeignKey(x => x.EscritorioAcompanhanteId);

            builder.Property(x => x.ContadorId).HasColumnName("COD_CONTADOR").HasDefaultValue(null);

            builder.Property(x => x.RegionalId).HasColumnName("COD_REGIONAL").HasDefaultValue(null);
            builder.HasOne(x => x.Regional).WithMany().HasForeignKey(x => x.RegionalId);

            builder.Property(x => x.ComarcaId).HasColumnName("COD_COMARCA").HasDefaultValue(null);
            builder.HasOne(x => x.Comarca).WithMany().HasForeignKey(x => x.ComarcaId);

            builder.Property(x => x.VaraId).HasColumnName("COD_VARA").HasDefaultValue(null);
            builder.HasOne(x => x.Vara).WithMany().HasForeignKey(x => new { x.VaraId, x.ComarcaId, x.TipoVaraId });

            builder.Property(x => x.TipoVaraId).HasColumnName("COD_TIPO_VARA").HasDefaultValue(null);
            builder.HasOne(x => x.TipoVara).WithMany().HasForeignKey(x => x.TipoVaraId);

            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO").HasDefaultValue(null);

            builder.Property(x => x.MunicipioId).HasColumnName("COD_MUNICIPIO").HasDefaultValue(null);

            builder.Property(x => x.ProcedimentoId).HasColumnName("COD_PROCEDIMENTO").HasDefaultValue(null);
            builder.HasOne(x => x.Procedimento).WithMany().HasForeignKey(x => x.ProcedimentoId);
            
            builder.Property(x => x.DataDistribuicao).HasColumnName("DAT_DISTRIBUICAO").HasDefaultValue(null);

            builder.Property(x => x.ClassificacaoProcessoId).HasColumnName("COD_CLASSIFICACAO_PROCESSO");

            builder.Property(x => x.Closing).HasColumnName("COD_CLASSIFICACAO_CLOSING").HasDefaultValue(null);
            
            builder.Property(x => x.ComplementoDeAreaEnvolvidaId).HasColumnName("ID_COMPL_AREA_ENVOLVIDA");
            
            builder.Property(x => x.EsferaId).HasColumnName("ESF_COD_ESFERA");

            builder.Property(x => x.FatoGeradorId).HasColumnName("FTGER_ID_FATO_GERADOR");

            builder.Property(x => x.ClosingClientCo).HasColumnName("COD_CLASSF_CLOSING_CLIENTCO").HasDefaultValue(null);


            #region Entities

            builder.HasMany(x => x.PedidosDoProcesso).WithOne(x => x.Processo).HasForeignKey(x => x.ProcessoId);
            builder.HasMany(x => x.PartesDoProcesso).WithOne(x => x.Processo).HasForeignKey(x => x.ProcessoId);
            builder.HasMany(x => x.PendenciasDoProcesso).WithOne(x => x.Processo).HasForeignKey(x => x.ProcessoId);
            builder.HasMany(x => x.PrazosDoProcesso).WithOne(x => x.Processo).HasForeignKey(x => x.ProcessoId);

            #endregion Entities
        }
    }
}