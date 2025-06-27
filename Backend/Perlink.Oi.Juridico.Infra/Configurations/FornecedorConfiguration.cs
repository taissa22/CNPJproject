using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;

namespace Perlink.Oi.Juridico.Infra.Configurations {

    internal class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor> {

        public void Configure(EntityTypeBuilder<Fornecedor> builder) {
            builder.ToTable("FORNECEDOR", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_FORNECEDOR")
               .IsRequired().HasSequentialIdGenerator<Fornecedor>("FORNECEDOR");

            builder.Property(x => x.Nome).HasColumnName("NOM_FORNECEDOR");
            builder.Property(x => x.EscritorioId).HasColumnName("COD_ESCRITORIO");
            builder.HasOne(x => x.Escritorio).WithMany().HasForeignKey(x => x.EscritorioId);

            builder.Property(x => x.ProfissionalId).HasColumnName("COD_PROFISSIONAL");
            builder.HasOne(x => x.Profissional).WithMany().HasForeignKey(x => x.ProfissionalId);

            builder.Property(x => x.CNPJ).HasColumnName("NUM_CNPJ");

            builder.Property(x => x.TipoFornecedorId).HasColumnName("COD_TIPO_FORNECEDOR");
        }
    }
}