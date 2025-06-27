using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    public class BancoConfiguration : IEntityTypeConfiguration<Banco>
	{
		public void Configure(EntityTypeBuilder<Banco> builder)
		{

			builder.ToTable("BANCO", "JUR");
			builder.HasKey(bi => bi.Id);

			builder.Property(bi => bi.Id)
				.HasColumnName("COD_BANCO");

			builder.Property(bi => bi.NomeBanco)
				.HasColumnName("NOM_BANCO");

			builder.Property(bi => bi.NumeroBanco)
				.HasColumnName("NRO_BANCO");

			builder.Property(bi => bi.DigitoVerificadorBanco)
				.HasColumnName("DV_NRO_BANCO");

			builder.Property(bi => bi.NumeroAgencia)
				.HasColumnName("NRO_AGENCIA");

			builder.Property(bi => bi.DigitoVerificadorAgencia)
				.HasColumnName("DV_NRO_AGENCIA");


		}
	}
}
