using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using Perlink.Oi.Juridico.Infra.Enums;
using System;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ParteConfiguration : IEntityTypeConfiguration<Parte>
    {
        public void Configure(EntityTypeBuilder<Parte> builder)
        {
            builder.ToTable("PARTE", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PARTE")
                .IsRequired().HasNextSequenceValueGenerator("JUR", "PAR_SEQ_01");
            

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


            builder.Property(x => x.CPF).HasColumnName("CPF_PARTE");            
            builder.Property(x => x.CarteiraDeTrabalho).HasColumnName("CARTEIRA_TRABALHO_PARTE");

            builder.Property<string>(x => x.EnderecosAdicionais).HasColumnName("DESCRICAO_END_ADICIONAIS").HasDefaultValue(null).HasColumnType("varchar(4000)");
                        
            builder.Property<string>(x => x.CelularDDD).HasColumnName("DDD_CELULAR");
            
            builder.Property<string>(x => x.TelefonesAdicionais).HasColumnName("DESCRICAO_TEL_ADICIONAIS").HasDefaultValue(null).HasColumnType("varchar(4000)");

            //  builder.Property<int?>(x => x.CodigoFornecedor).HasColumnName("COD_FORNECEDOR");

            builder.Property<decimal>(x => x.ValorCartaFianca).HasColumnName("VALOR_CARTA_FIANCA")
                .IsRequired().HasDefaultValue(0);
            builder.Property(x => x.DataCartaFianca).HasColumnName("DATA_CARTA_FIANCA").HasDefaultValue(null);

            builder.HasQueryFilter(x => x.TipoParteValor == TipoParte.PESSOA_FISICA.Valor || x.TipoParteValor == TipoParte.PESSOA_JURIDICA.Valor || x.TipoParteValor == TipoParte.EMPRESA_DO_GRUPO.Valor);
            

            #region ENTITIES

            builder.HasOne(x => x.ParteBase).WithOne(x => x.Parte).HasForeignKey<Parte>(x => x.Id);

            #endregion ENTITIES
        }
    }
}