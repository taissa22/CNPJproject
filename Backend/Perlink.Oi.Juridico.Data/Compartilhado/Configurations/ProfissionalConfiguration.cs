using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class ProfissionalConfiguration : IEntityTypeConfiguration<Profissional>
    {
        public void Configure(EntityTypeBuilder<Profissional> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("PROFISSIONAL", "JUR");

            builder.HasKey(c => c.Id)
                                 .HasName("COD_PROFISSIONAL");

            builder.Property(bi => bi.Id)
                                     .IsRequired()
                                     .HasColumnName("COD_PROFISSIONAL");

            builder.Property(bi => bi.CodigoTipoPessoa)
                                     .HasColumnName("COD_TIPO_PESSOA");

            builder.Property(bi => bi.CgcProfissional)
                                     .HasColumnName("CGC_PROFISSIONAL")
                                     .HasMaxLength(14);

            builder.Property(bi => bi.CpfProfissional)
                                     .HasColumnName("CPF_PROFISSIONAL")
                                     .HasMaxLength(11);

            builder.Property(bi => bi.NomeProfissional)
                                     .HasColumnName("NOM_PROFISSIONAL")
                                     .HasMaxLength(400);

            builder.Property(bi => bi.EnderecoProfissional)
                                    .HasColumnName("END_PROFISSIONAL")
                                    .HasMaxLength(60);

            builder.Property(bi => bi.Cep)
                                     .HasColumnName("COD_CEP")
                                     .HasMaxLength(8);

            builder.Property(bi => bi.CodigoEstado)
                                     .HasColumnName("COD_ESTADO")
                                     .HasMaxLength(3);

            builder.Property(bi => bi.Cidade)
                                     .HasColumnName("DSC_CIDADE")
                                     .HasMaxLength(30);

            builder.Property(bi => bi.Bairro)
                                     .HasColumnName("DSC_BAIRRO")
                                     .HasMaxLength(30);

            builder.Property(bi => bi.Telefone)
                                     .HasColumnName("TELEFONE")
                                     .HasMaxLength(9);

            builder.Property(bi => bi.DddTelefone)
                                     .HasColumnName("DDD_TELEFONE")
                                     .HasMaxLength(4);

            builder.Property(bi => bi.Fax)
                                     .HasColumnName("FAX")
                                     .HasMaxLength(9);

            builder.Property(bi => bi.DddFax)
                                     .HasColumnName("DDD_FAX")
                                     .HasMaxLength(4);

            builder.Property(bi => bi.Celular)
                                     .HasColumnName("CELULAR")
                                     .HasMaxLength(9);

            builder.Property(bi => bi.DddCelular)
                                     .HasColumnName("DDD_CELULAR")
                                     .HasMaxLength(4);

            builder.Property(bi => bi.Site)
                                     .HasColumnName("DSC_SITE")
                                     .HasMaxLength(80);

            builder.Property(bi => bi.Email)
                                     .HasColumnName("DSC_EMAIL")
                                     .HasMaxLength(60);

            builder.Property(bi => bi.IndicadorEscritorio)
                                     .HasColumnName("IND_ESCRITORIO")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorContador)
                                     .HasColumnName("IND_CONTADOR")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorAreaCivel)
                                     .HasColumnName("IND_AREA_CIVEL")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorAreaTrabalhista)
                                     .HasColumnName("IND_AREA_TRABALHISTA")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorAreaRegulatoria)
                                     .HasColumnName("IND_AREA_REGULATORIA")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorAreaTributaria)
                                     .HasColumnName("IND_AREA_TRIBUTARIA")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorAlterarValorInternet)
                                     .HasColumnName("IND_ALTERAR_VALOR_INTERNET")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.CodigoAdvogado)
                                     .HasColumnName("SEQ_ADVOGADO")
                                     .HasMaxLength(4);

            builder.Property(bi => bi.CodigoDespesa)
                                     .HasColumnName("SEQ_DESPESA")
                                     .HasMaxLength(6);

            builder.Property(bi => bi.Fornecedor)
                                     .HasColumnName("COD_FORNECEDOR")
                                     .HasMaxLength(7);

            builder.Property(bi => bi.AlertarEm)
                                     .HasColumnName("ALERTA_EM")
                                     .HasMaxLength(3);

            builder.Property(bi => bi.IndicadorAdvogado)
                                     .HasColumnName("IND_ADVOGADO")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.NumeroOabAdvogado)
                                     .HasColumnName("NRO_OAB_ADVOGADO")
                                     .HasMaxLength(7);

            builder.Property(bi => bi.IndicadorAreaJuizado)
                                     .HasColumnName("IND_AREA_JUIZADO")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.CodigoEstadoOab)
                                     .HasColumnName("COD_ESTADO_OAB")
                                     .HasMaxLength(3);

            builder.Property(bi => bi.CodigoGrupoLoteJuizado)
                                     .HasColumnName("GLJ_COD_GRUPO_LOTE_JUIZADO")
                                     .HasMaxLength(4);

            builder.Property(bi => bi.IndicadorCivelEstrategico)
                                    .HasColumnName("IND_CIVEL_ESTRATEGICO")
                                    .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorCriminalAdm)
                                     .HasColumnName("IND_CRIMINAL_ADM")
                                    .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorIndCriminalJudicial)
                                     .HasColumnName("IND_CRIMINAL_JUDICIAL")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorCivelAdm)
                                     .HasColumnName("IND_CIVEL_ADM")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorProcom)
                                     .HasColumnName("IND_PROCON")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorPex)
                                     .HasColumnName("IND_PEX")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.IndicadorContadorPex)
                                     .HasColumnName("IND_CONTADOR_PEX")
                                     .HasConversion(boolConverter);

            builder.Property(bi => bi.EnderecoAdicionais)
                                     .HasColumnName("DESCRICAO_END_ADICIONAIS")
                                     .HasMaxLength(4000);

            builder.Property(bi => bi.TelefoneAdicionais)
                                     .HasColumnName("DESCRICAO_TEL_ADICIONAIS")
                                    .HasMaxLength(4000);

            builder.Property(bi => bi.CodigoProfissionalSap)
                                     .HasColumnName("COD_PROFISSIONAL_SAP")
                                     .HasMaxLength(30);

            builder.Property(bi => bi.IndicadorAtivo)
                                     .HasColumnName("IND_ATIVO")
                                     .HasConversion(boolConverter);

            builder.HasOne(bi => bi.AdvogadoEscritorio).WithMany(a => a.Profissionais).HasForeignKey(s => s.Id);


        }
    }
}
