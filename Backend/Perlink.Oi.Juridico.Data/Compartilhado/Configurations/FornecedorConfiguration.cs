using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Diagnostics.CodeAnalysis;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("FORNECEDOR", "JUR");

            builder.HasKey(c => c.Id)
                   .HasName("COD_FORNECEDOR");

            builder.Property(bi => bi.Id)
                   .HasColumnName("COD_FORNECEDOR")
                   .IsRequired()
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((a, b) => new SequenceIdGenerator("FORNECEDOR"));

            builder.Property(bi => bi.NomeFornecedor)
                .HasColumnName("NOM_FORNECEDOR");

            builder.Property(bi => bi.CodigoTipoFornecedor)
                .HasColumnName("COD_TIPO_FORNECEDOR");

            builder.Property(bi => bi.CodigoFornecedorSAP)
                .HasColumnName("COD_FORNECEDOR_SAP");

            builder.Property(bi => bi.CodigoEscritorio)
                .HasColumnName("COD_ESCRITORIO");
            builder.HasOne(bi => bi.Escritorio).WithMany(a => a.FornecedorEscritorios).HasForeignKey(s => s.CodigoEscritorio);

            builder.Property(bi => bi.CodigoProfissional)
                .HasColumnName("COD_PROFISSIONAL");
            builder.HasOne(bi => bi.Profissional).WithMany(a => a.FornecedorProfissionais).HasForeignKey(s => s.CodigoProfissional);
                
            builder.Property(bi => bi.CodigoBanco)
                .HasColumnName("COD_BANCO");
            builder.HasOne(bi => bi.Banco).WithMany(a => a.Fornecedores).HasForeignKey(s => s.CodigoBanco);

            builder.Property(bi => bi.NumeroCNPJ)
                .HasColumnName("NUM_CNPJ");

            builder.Property(bi => bi.ValorCartaFianca)
             .HasColumnName("VALOR_CARTA_FIANCA");

            builder.Property(bi => bi.DataCartaFianca)
             .HasColumnName("DATA_CARTA_FIANCA");

            builder.Property(bi => bi.IndicaAtivoSAP)
             .HasColumnName("IND_ATIVO_SAP")
             .HasConversion(boolConverter);

            builder.Property(bi => bi.DataAtualizaIndiceAtivo)
            .HasColumnName("DATA_ATU_MANUAL_IND_ATIVO");

            builder.Property(bi => bi.UsuarioUltimaAlteracao)
            .HasColumnName("USU_COD_USU_ATU_MANU_IND_ATIVO");
        }
    }
}