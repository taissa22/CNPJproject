using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class FeriadosConfiguration : IEntityTypeConfiguration<Feriados> {
        public void Configure(EntityTypeBuilder<Feriados> builder) {

            builder.ToTable("DATAS_NAO_UTEIS", "JUR");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).IsRequired().HasColumnName("ID");
            builder.Property(c => c.CodigoClassificacaoFeriado).IsRequired().HasColumnName("CDNUT_ID_CLASS_DATA_NAO_UTIL");
            builder.Property(c => c.Data).IsRequired().HasColumnName("DATA");
        }
    }
}
