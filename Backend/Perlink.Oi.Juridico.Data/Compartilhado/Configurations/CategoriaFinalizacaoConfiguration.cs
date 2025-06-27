using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class CategoriaFinalizacaoConfiguration : IEntityTypeConfiguration<CategoriaFinalizacao>
    {
        public void Configure(EntityTypeBuilder<CategoriaFinalizacao> builder)
        {
            builder.ToTable("CATEGORIA_FINALIZACAO", "JUR");

            builder.HasKey(pk => new { pk.Id, pk.CodigoFinalizacao });
            
            builder.Property(P => P.Id).IsRequired().HasColumnName("COD_CAT_PAGAMENTO");
            builder.Property(P => P.CodigoFinalizacao).IsRequired().HasColumnName("COD_CFG_FINALIZACAO");

            //Fks
            builder.HasOne(fk => fk.CategoriaPagamento)
                    .WithMany(fk => fk.CategoriaFinalizacoes).HasForeignKey(fk => fk.Id);
        }
    }
}
