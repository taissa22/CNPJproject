using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    public class Empresas_SapConfiguration : IEntityTypeConfiguration<Empresas_Sap>
    {
        public void Configure(EntityTypeBuilder<Empresas_Sap> builder)
        {
            builder.ToTable("EMPRESAS_SAP", "JUR");

            var boolConverter = new BoolToStringConverter("N", "S");

            builder.HasKey(pk => pk.Id).HasName("CODIGO");

            builder.Property(c => c.Id)
                   .IsRequired()
                   .HasColumnName("CODIGO")
                   .ValueGeneratedOnAdd()
                   .HasValueGenerator((a, b) => new SequenceIdGenerator("EMPRESAS_SAP"));

            builder.Property(c => c.Sigla).HasColumnName("SIGLA");
            builder.Property(c => c.Nome).HasColumnName("NOME");
            builder.Property(c => c.IndicaEnvioArquivoSolicitacao).HasColumnName("IND_ENVIO_ARQ_SOLICITACAO").HasConversion(boolConverter);
            builder.Property(c => c.IndicaAtivo).HasColumnName("IND_ATIVO").HasConversion(boolConverter);
            builder.Property(c => c.CodigoRegiaoSAP).HasColumnName("COD_REGIAO_SAP");
            builder.Property(c => c.CodigoOrganizacaoCompra).HasColumnName("COD_ORGANIZACAO_COMPRAS_SAP");
            
        }
    }
}
