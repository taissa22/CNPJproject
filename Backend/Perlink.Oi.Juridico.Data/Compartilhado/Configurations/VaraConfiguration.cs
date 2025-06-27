using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.Compartilhado
{
    public class VaraConfiguration : IEntityTypeConfiguration<Vara> {
        public void Configure(EntityTypeBuilder<Vara> builder) {
            builder.ToTable("VARA", "JUR");

            builder.HasKey(pk => new { pk.Id, pk.CodigoVara, pk.CodigoTipoVara });

            builder.Property(P => P.Id).IsRequired(true).HasColumnName("COD_COMARCA");
            builder.Property(P => P.CodigoVara).IsRequired(true).HasColumnName("COD_VARA");
            builder.Property(P => P.CodigoTipoVara).IsRequired(true).HasColumnName("COD_TIPO_VARA");
            builder.Property(P => P.EnderecoVara).IsRequired(true).HasColumnName("END_VARA");
            builder.Property(P => P.CodigoEscritorioJuizado).IsRequired(true).HasColumnName("COD_ESCRITORIO_JUIZADO");
            builder.Property(P => P.CodigoVaraBancoDoBrasil).IsRequired(true).HasColumnName("COD_VARA_BB");
            builder.Property(P => P.IdBancoDoBrasilOrgao).IsRequired(true).HasColumnName("BBORG_ID_BB_ORGAO");

            builder.HasOne(v => v.BBOrgaos).WithMany(bbo => bbo.Varas).HasForeignKey(v => v.IdBancoDoBrasilOrgao);
        }
    }
}
