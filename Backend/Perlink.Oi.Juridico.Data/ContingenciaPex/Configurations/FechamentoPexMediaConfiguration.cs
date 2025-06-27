using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Entity;

namespace Perlink.Oi.Juridico.Data.ContingenciaPex.Configurations
{
    public class FechamentoPexMediaConfiguration : IEntityTypeConfiguration<FechamentoPexMedia>
    {
        public void Configure(EntityTypeBuilder<FechamentoPexMedia> builder)
        {

            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("FECHAMENTO_PEX_MEDIA", "JUR");
            builder.HasKey(c => c.Id).HasName("ID");
            builder.Property(bi => bi.Id).IsRequired().HasColumnName("ID");

            builder.Property(bi => bi.CodEmpresaCentralizadora).HasColumnName("COD_EMPRESA_CENTRALIZADORA");
            builder.Property(bi => bi.CodSolicFechamentoCont).HasColumnName("COD_SOLIC_FECHAMENTO_CONT");
            builder.Property(bi => bi.CodUsuario).HasColumnName("COD_USUARIO");
            builder.Property(bi => bi.DataFechamento).HasColumnName("DAT_FECHAMENTO");
            builder.Property(bi => bi.DataGeracao).HasColumnName("DAT_GERACAO");
            builder.Property(bi => bi.DataIndMensal).HasColumnName("DAT_IND_MENSAL");
            builder.Property(bi => bi.IndAplicarHaircutProcGar).HasColumnName("IND_APLICAR_HAIRCUT_PROC_GAR").HasConversion(boolConverter);
            builder.Property(bi => bi.IndMensal).HasColumnName("IND_MENSAL").HasConversion(boolConverter);
            builder.Property(bi => bi.NroMesesMediaHistorica).HasColumnName("NRO_MESES_MEDIA_HISTORICA");
            builder.Property(bi => bi.PerHaircut).HasColumnName("PER_HAIRCUT");
            builder.Property(bi => bi.ValMultDesvioPadrao).HasColumnName("VAL_MULT_DESVIO_PADRAO");
        }
    }
}
