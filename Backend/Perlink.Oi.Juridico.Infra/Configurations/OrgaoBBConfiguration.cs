using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class OrgaoBBConfiguration : IEntityTypeConfiguration<OrgaoBB>
    {
        public void Configure(EntityTypeBuilder<OrgaoBB> builder)
        {

            builder.ToTable("BB_ORGAOS","JUR");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID").IsRequired();
            builder.Property(x => x.Nome).HasColumnName("NOME").IsRequired();
            builder.Property(x => x.TribunalBBId).HasColumnName("BBTRI_ID_BB_TRIBUNAL").IsRequired();
            builder.HasOne(x => x.TribunalBB).WithOne();
            builder.Property(x => x.ComarcaBBId).HasColumnName("BBCOM_ID_BB_COMARCA").IsRequired();
            builder.HasOne(x => x.ComarcaBB).WithOne();

        }
    }
}
