using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations {

    internal class FeriadoConfiguration : IEntityTypeConfiguration<Feriado> {

        public void Configure(EntityTypeBuilder<Feriado> builder) {
            builder.ToTable("DATAS_NAO_UTEIS", "JUR");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ID");

            builder.Property(x => x.Data).HasColumnName("DATA");
        }
    }
}