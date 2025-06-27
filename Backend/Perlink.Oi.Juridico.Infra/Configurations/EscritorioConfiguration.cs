using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class EscritorioConfiguration : IEntityTypeConfiguration<Escritorio>
    {
        public void Configure(EntityTypeBuilder<Escritorio> builder)
        {
            builder.ToTable("PROFISSIONAL", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PROFISSIONAL")
                .IsRequired().HasNextSequenceValueGenerator("JUR", "PROF_SEQ_01");

            builder.Property(x => x.Nome).HasColumnName("NOM_PROFISSIONAL").IsRequired();

            builder.Property(x => x.Ativo).HasColumnName("IND_ATIVO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhEscritorio).HasColumnName("IND_ESCRITORIO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.CivelEstrategico).HasColumnName("IND_CIVEL_ESTRATEGICO")
                .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.Endereco).HasColumnName("END_PROFISSIONAL");

            builder.Property(x => x.TipoPessoaValor).HasColumnName("COD_TIPO_PESSOA");

            builder.Property(x => x.CPF).HasColumnName("CPF_PROFISSIONAL");

            builder.Property(x => x.IndAreaCivel).HasColumnName("IND_AREA_CIVEL")
                    .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndAreaJuizado).HasColumnName("IND_AREA_JUIZADO")
                    .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndAreaRegulatoria).HasColumnName("IND_AREA_REGULATORIA")
                    .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndAreaTrabalhista).HasColumnName("IND_AREA_TRABALHISTA")
                    .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndAreaTributaria).HasColumnName("IND_AREA_TRIBUTARIA")
                    .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndAreaCriminalAdministrativo).HasColumnName("IND_CRIMINAL_ADM")
                   .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndAreaCriminalJudicial).HasColumnName("IND_CRIMINAL_JUDICIAL")
                   .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndAreaCivelAdministrativo).HasColumnName("IND_CIVEL_ADM")
               .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndAreaPEX).HasColumnName("IND_PEX")
                   .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.IndAreaProcon).HasColumnName("IND_PROCON")
                   .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.EhContadorPex).HasColumnName("IND_CONTADOR_PEX")
                 .HasConversion(ValueConverters.BoolToString);

            builder.Property(x => x.CEP).HasColumnName("COD_CEP");
            builder.Property(x => x.Cidade).HasColumnName("DSC_CIDADE");

            builder.Property(x => x.Telefone).HasColumnName("TELEFONE");

            builder.Property(x => x.Email).HasColumnName("DSC_EMAIL");
            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO");
            builder.Property(x => x.Bairro).HasColumnName("DSC_BAIRRO");
            builder.Property(x => x.AlertaEm).HasColumnName("ALERTA_EM");
            builder.Property(x => x.CodProfissionalSAP).HasColumnName("COD_PROFISSIONAL_SAP");
            builder.Property(x => x.Site).HasColumnName("DSC_SITE");
            builder.Property(x => x.CNPJ).HasColumnName("CGC_PROFISSIONAL");
            builder.Property(x => x.EhEscritorio).HasColumnName("IND_ESCRITORIO").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.TelefoneDDD).HasColumnName("DDD_TELEFONE");
            builder.Property(x => x.Celular).HasColumnName("CELULAR");
            builder.Property(x => x.CelularDDD).HasColumnName("DDD_CELULAR");
            builder.Property(x => x.Fax).HasColumnName("FAX");
            builder.Property(x => x.FaxDDD).HasColumnName("DDD_FAX");
            builder.Property(x => x.LoteJuizado).HasColumnName("GLJ_COD_GRUPO_LOTE_JUIZADO");

            builder.Property(x => x.EhAdvogado).HasColumnName("IND_ADVOGADO").HasConversion(ValueConverters.BoolToString);
            builder.Property(x => x.RegistroOAB).HasColumnName("NRO_OAB_ADVOGADO");
            builder.Property(x => x.EstadoOABId).HasColumnName("COD_ESTADO_OAB");
            builder.Property(x => x.SeqAdvogado).HasColumnName("SEQ_ADVOGADO");
            builder.Property(x => x.Enviar_App_Preposto).HasColumnName("ENVIAR_APP_PREPOSTO").HasConversion(ValueConverters.BoolToString);

            builder.HasQueryFilter(x => x.EhEscritorio);

            #region ENTITIES

            builder.HasOne(x => x.ProfissionalBase).WithOne(x => x.Escritorio).HasForeignKey<Escritorio>(x => x.Id);

            #endregion ENTITIES
        }
    }
}