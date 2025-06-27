using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class EmpresaCentralizadoraConfiguration : IEntityTypeConfiguration<EmpresaCentralizadora>
    {
        public void Configure(EntityTypeBuilder<EmpresaCentralizadora> builder)
        {
            builder.ToTable("EMPRESAS_CENTRALIZADORAS", "JUR");

            builder.HasKey(x => x.Codigo);
            builder.Property(x => x.Codigo).HasColumnName("CODIGO")
                .IsRequired().HasSequentialIdGenerator<EmpresaCentralizadora>("EMPRESAS_CENTRALIZADORAS");

            builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();

            builder.Property(x => x.Ordem).HasColumnName("NUM_ORDEM_CLASSIF_PROCESSO")
                .IsRequired().HasAutoIncrementValueGenerator<EmpresaCentralizadora>(x => x.Ordem);

            builder.HasMany(x => x.Convenios).WithOne(x => x.EmpresaCentralizadora).HasForeignKey(x => x.CodigoEmpresaCentralizadora);

            builder.Ignore(x => x.FechamentosCCMedia);
        }
    }
}