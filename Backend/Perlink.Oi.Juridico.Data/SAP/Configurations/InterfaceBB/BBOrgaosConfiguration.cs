using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations.InterfaceBB {
    public class BBOrgaosConfiguration : IEntityTypeConfiguration<BBOrgaos> {
        public void Configure(EntityTypeBuilder<BBOrgaos> builder) {
            builder.ToTable("BB_ORGAOS", "JUR");

            builder.HasKey(pk => pk.Id);

            builder.Property(c => c.Id).HasColumnName("ID")
                .ValueGeneratedOnAdd()
                .HasValueGenerator((_, __) => new SequenceValueGenerator("JUR", "BBORG_SEQ_01"))
                .IsRequired(true);
            builder.Property(c => c.Codigo).HasColumnName("CODIGO");
            builder.Property(c => c.Nome).HasColumnName("NOME");
            builder.Property(c => c.CodigoBBTribunal).HasColumnName("BBTRI_ID_BB_TRIBUNAL");
            builder.Property(c => c.CodigoBBComarca).HasColumnName("BBCOM_ID_BB_COMARCA");

            builder.HasOne(bi => bi.BBTribunais).WithMany(a => a.BBOrgaos).HasForeignKey(s => s.CodigoBBTribunal);
            builder.HasOne(bi => bi.BBComarca).WithMany(a => a.BBOrgaos).HasForeignKey(s => s.CodigoBBComarca);
        }
    }
}
