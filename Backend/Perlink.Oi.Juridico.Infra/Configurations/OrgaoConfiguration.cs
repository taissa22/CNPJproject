using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using Perlink.Oi.Juridico.Infra.Enums;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class OrgaoConfiguration : IEntityTypeConfiguration<Orgao>
    {
        public void Configure(EntityTypeBuilder<Orgao> builder)
        {
            builder.ToTable("PARTE", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PARTE")
                .IsRequired().HasNextSequenceValueGenerator("JUR", "PAR_SEQ_01");

            builder.Property(x => x.TipoParteValor).HasColumnName("COD_TIPO_PARTE").HasDefaultValue(null);

            builder.Property(x => x.Nome).HasColumnName("NOM_PARTE").HasDefaultValue(null);

            builder.Property(x => x.SequencialCompetencia).HasColumnName("SEQ_COMPETENCIA");

            builder.Property(x => x.TelefoneDDD).HasColumnName("DDD_TELEFONE").HasDefaultValue(null);
            builder.Property(x => x.Telefone).HasColumnName("TELEFONE").HasDefaultValue(null);

            builder.HasQueryFilter(x =>
                x.TipoParteValor == TipoOrgao.CIVEL_ADMINISTRATIVO.Valor ||
                x.TipoParteValor == TipoOrgao.CRIMINAL_ADMINISTRATIVO.Valor ||
                x.TipoParteValor == TipoOrgao.DEMAIS_TIPOS.Valor);

            #region ENTITIES

            builder.HasMany(x => x.Competencias).WithOne(x => x.Orgao).HasForeignKey(x => x.OrgaoId);

            builder.HasOne(x => x.ParteBase).WithOne(x => x.Orgao).HasForeignKey<Orgao>(x => x.Id);

            #endregion ENTITIES
        }
    }
}