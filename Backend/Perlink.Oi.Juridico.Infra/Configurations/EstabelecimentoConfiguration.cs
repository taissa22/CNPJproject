using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class EstabelecimentoConfiguration : IEntityTypeConfiguration<Estabelecimento>
    {
        public void Configure(EntityTypeBuilder<Estabelecimento> builder)
        {
            builder.ToTable("PARTE", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PARTE").IsRequired()
                .HasNextSequenceValueGenerator("jur", "PAR_SEQ_01");

            #region CAMPOS MAPEADOS EM PARTE, EMPRESA DO GRUPO, ESTABELECIMENTO
            builder.Property(x => x.Nome).HasColumnName("NOM_PARTE").HasDefaultValue(null);
            builder.Property(x => x.CNPJ).HasColumnName("CGC_PARTE").HasDefaultValue(null);
            builder.Property(x => x.TipoParteValor).HasColumnName("COD_TIPO_PARTE").HasDefaultValue(null);
            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO").HasDefaultValue(null);
            builder.Property(x => x.Endereco).HasColumnName("END_PARTE").HasDefaultValue(null);
            builder.Property(x => x.CEP).HasColumnName("COD_CEP").HasDefaultValue(null);
            builder.Property(x => x.Cidade).HasColumnName("DSC_CIDADE").HasDefaultValue(null);
            builder.Property(x => x.Bairro).HasColumnName("DSC_BAIRRO").HasDefaultValue(null);
            builder.Property(x => x.TelefoneDDD).HasColumnName("DDD_TELEFONE").HasDefaultValue(null);
            builder.Property(x => x.Telefone).HasColumnName("TELEFONE").HasDefaultValue(null);
            builder.Property(x => x.FaxDDD).HasColumnName("DDD_FAX").HasDefaultValue(null);
            builder.Property(x => x.Fax).HasColumnName("FAX").HasDefaultValue(null);
            builder.Property(x => x.Celular).HasColumnName("CELULAR").HasDefaultValue(null);
            builder.Property(x => x.EmpresaCentralizadoraId).HasColumnName("EMPCE_COD_EMP_CENTRALIZADORA").HasDefaultValue(null);
            #endregion CAMPOS MAPEADOS EM PARTE, EMPRESA DO GRUPO, ESTABELECIMENTO

            builder.Property<string>(x => x.CelularDDD).HasColumnName("DDD_CELULAR");

            builder.HasQueryFilter(x => x.TipoParteValor == TipoParte.ESTABELECIMENTO.Valor);
            
            #region ENTITIES

            builder.HasOne(x => x.ParteBase).WithOne(x => x.Estabelecimento).HasForeignKey<Estabelecimento>(x => x.Id);

            #endregion ENTITIES
        }

    }
}