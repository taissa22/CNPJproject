using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class ParteConfiguration : IEntityTypeConfiguration<Parte>
    {

        public void Configure(EntityTypeBuilder<Parte> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("PARTE", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("COD_PARTE");

            builder.Property(bi => bi.Id)
                   .IsRequired()
                   .HasColumnName("COD_PARTE")
                   .HasMaxLength(10);

            builder.Property(bi => bi.TipoParte)
                   .HasColumnName("COD_TIPO_PARTE")
                   .HasMaxLength(1);

            builder.Property(bi => bi.Endereco)
                   .HasColumnName("END_PARTE")
                   .HasMaxLength(1);

            builder.Property(bi => bi.Cgc)
                   .HasColumnName("CGC_PARTE")
                   .HasMaxLength(14);

            builder.Property(bi => bi.Cpf)
                   .HasColumnName("CPF_PARTE")
                   .HasMaxLength(11);

            builder.Property(bi => bi.CarteiraTrabalho)
            .HasColumnName("CARTEIRA_TRABALHO_PARTE")
            .HasMaxLength(8);

            builder.Property(bi => bi.Nome)
                .HasColumnName("NOM_PARTE")
                .HasMaxLength(400);

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

            builder.Property(bi => bi.Cep)
               .HasColumnName("COD_CEP")
               .HasMaxLength(8);

            builder.Property(bi => bi.SiglaEstado)
               .HasColumnName("COD_ESTADO")
               .HasMaxLength(3);

            builder.Property(bi => bi.Cidade)
               .HasColumnName("DSC_CIDADE")
               .HasMaxLength(30);


            builder.Property(bi => bi.Bairro)
                 .HasColumnName("DSC_BAIRRO")
                 .HasMaxLength(30);

            builder.Property(bi => bi.Competencia)
               .HasColumnName("SEQ_COMPETENCIA")
               .HasMaxLength(4);

            builder.Property(bi => bi.Regional)
             .HasColumnName("COD_REGIONAL")
             .HasMaxLength(4);

            builder.Property(bi => bi.Rg)
            .HasColumnName("RG_PARTE")
            .HasMaxLength(4);

            builder.Property(bi => bi.CodigoCentroSap)
           .HasColumnName("COD_CENTRO_SAP")
           .HasMaxLength(4);

            builder.Property(bi => bi.SiglaArquivoSap)
           .HasColumnName("SGL_ARQUIVO_SAP")
           .HasMaxLength(4);

            builder.Property(bi => bi.Fornecedor)
            .HasColumnName("COD_FORNECEDOR")
            .HasMaxLength(8);

            builder.Property(bi => bi.CodigoCentroCusto)
                     .HasColumnName("COD_CENTRO_CUSTO")
                     .HasMaxLength(4);

            builder.Property(bi => bi.IndicadorGeraArquivo)
                    .HasColumnName("IND_GERA_ARQUIVO_BB")
                     .HasConversion(boolConverter);

            builder.Property(bi => bi.CodigoEmpresaCentralizadora)
                    .HasColumnName("EMPCE_COD_EMP_CENTRALIZADORA")
                    .HasMaxLength(4);
           builder.HasOne(bi => bi.EmpresaCentralizadora)
              .WithMany(a => a.Partes).HasForeignKey(s => s.CodigoEmpresaCentralizadora);

            builder.Property(bi => bi.CodigoEmpresaSap)
                   .HasColumnName("ESAP_COD_EMPRESA_SAP")
                   .HasMaxLength(4);
            builder.HasOne(bi => bi.Empresa_Sap)
                    .WithMany(a => a.Partes).HasForeignKey(s => s.CodigoEmpresaSap);

            builder.Property(bi => bi.ValorCartaFianca)
                  .HasColumnName("VALOR_CARTA_FIANCA");

            builder.Property(bi => bi.DataCartaFianca)
                   .HasColumnName("DATA_CARTA_FIANCA");

            builder.Property(bi => bi.EnderecoAdicionais)
                   .HasColumnName("DESCRICAO_END_ADICIONAIS")
                   .HasMaxLength(4000);

            builder.Property(bi => bi.TelefoneAdicionais)
                  .HasColumnName("DESCRICAO_TEL_ADICIONAIS")
                  .HasMaxLength(4000);

            builder.Property(bi => bi.CodigoMigracaoSistema)
                 .HasColumnName("COD_MIGRACAO_SISTEMA");

            builder.Property(bi => bi.IndicadorCpfCgcValido)
                .HasColumnName("IND_CPF_CNPJ_VALIDO")
                 .HasConversion(boolConverter);

            builder.Property(bi => bi.CodigoDiretorioBancoBrasil)
                .HasColumnName("COD_DIRETORIO_BANCO_BRASIL")
                .HasMaxLength(4);





        }
    }
}
