using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ProfissionalConfiguration : IEntityTypeConfiguration<Profissional>
    {
        public void Configure(EntityTypeBuilder<Profissional> builder)
        {
            builder.ToTable("PROFISSIONAL", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PROFISSIONAL")
                .IsRequired().HasNextSequenceValueGenerator("JUR", "PROF_SEQ_01");

            builder.Property(x => x.Nome).HasColumnName("NOM_PROFISSIONAL").IsRequired();
            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.Email).HasColumnName("DSC_EMAIL");
            builder.Property(x => x.CPF).HasColumnName("CPF_PROFISSIONAL");
            builder.Property(x => x.CNPJ).HasColumnName("CGC_PROFISSIONAL");
            builder.Property(x => x.TipoPessoaValor).HasColumnName("COD_TIPO_PESSOA");
            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO");
            builder.Property(x => x.Endereco).HasColumnName("END_PROFISSIONAL");
            builder.Property(x => x.CEP).HasColumnName("COD_CEP");
            builder.Property(x => x.Cidade).HasColumnName("DSC_CIDADE");
            builder.Property(x => x.Bairro).HasColumnName("DSC_BAIRRO");
            builder.Property(x => x.EnderecosAdicionais).HasColumnName("DESCRICAO_END_ADICIONAIS").HasDefaultValue(null).HasColumnType("varchar(4000)"); 

            builder.Property(x => x.TelefoneDDD).HasColumnName("DDD_TELEFONE");
            builder.Property(x => x.Telefone).HasColumnName("TELEFONE");
            builder.Property(x => x.FaxDDD).HasColumnName("DDD_FAX");
            builder.Property(x => x.Fax).HasColumnName("FAX");
            builder.Property(x => x.CelularDDD).HasColumnName("DDD_CELULAR");
            builder.Property(x => x.Celular).HasColumnName("CELULAR");
            builder.Property(x => x.TelefonesAdicionais).HasColumnName("DESCRICAO_TEL_ADICIONAIS").HasDefaultValue(null).HasColumnType("varchar(4000)");

            builder.Property(x => x.EhAdvogado).HasColumnName("IND_ADVOGADO").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.RegistroOAB).HasColumnName("NRO_OAB_ADVOGADO");
            builder.Property(x => x.EstadoOABId).HasColumnName("COD_ESTADO_OAB");

            builder.Property(x => x.IndAreaCivel).HasColumnName("IND_AREA_CIVEL").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAreaJuizado).HasColumnName("IND_AREA_JUIZADO").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAreaRegulatoria).HasColumnName("IND_AREA_REGULATORIA").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAreaTrabalhista).HasColumnName("IND_AREA_TRABALHISTA").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAreaTributaria).HasColumnName("IND_AREA_TRIBUTARIA").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAreaCriminalAdministrativo).HasColumnName("IND_CRIMINAL_ADM").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAreaCriminalJudicial).HasColumnName("IND_CRIMINAL_JUDICIAL").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAreaCivelAdministrativo).HasColumnName("IND_CIVEL_ADM").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAreaPEX).HasColumnName("IND_PEX").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.IndAreaProcon).HasColumnName("IND_PROCON").HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhContador).HasColumnName("IND_CONTADOR").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhContadorPex).HasColumnName("IND_CONTADOR_PEX").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.EhEscritorio).HasColumnName("IND_ESCRITORIO").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.SeqAdvogado).HasColumnName("SEQ_ADVOGADO");                

            builder.HasQueryFilter(x => !x.EhEscritorio);

            #region ENTITIES

            builder.HasOne(x => x.ProfissionalBase).WithOne(x => x.Profissional).HasForeignKey<Profissional>(x => x.Id);

            #endregion ENTITIES
        }
    }
}