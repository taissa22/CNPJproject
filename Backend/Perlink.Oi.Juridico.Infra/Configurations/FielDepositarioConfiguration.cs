using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations {

    internal class FielDepositarioConfiguration : IEntityTypeConfiguration<FielDepositario> {

        public void Configure(EntityTypeBuilder<FielDepositario> builder) {
            builder.ToTable("FIEIS_DEPOSITARIOS", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired()
               .HasNextSequenceValueGenerator("jur", "FIEL_SEQ_01");

            builder.Property(x => x.Cpf).HasColumnName("NUM_CPF").IsRequired();

            builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();
        }
    }
}