using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ColunasRelatorioEficienciaEventosConfiguration : IEntityTypeConfiguration<ColunasRelatorioEficienciaEvento>
    {
        public void Configure(EntityTypeBuilder<ColunasRelatorioEficienciaEvento> builder)
        {
            builder.ToTable("COLUNAS_REL_EFICIENCIA_EVENTOS", "JUR");

            builder.HasKey(x => new {x.TipoProcesso, x.RelatorioId, x.ColunaId, x.Sequencial });
            builder.Property(x => x.TipoProcesso).HasColumnName("COREL_COD_TIPO_PROCESSO").IsRequired();
            builder.Property(x => x.RelatorioId).HasColumnName("COREL_COD_RELATORIO").IsRequired(); 
            builder.Property(x => x.EventoId).HasColumnName("COD_EVENTO").IsRequired(); 
            builder.Property(x => x.ColunaId).HasColumnName("COREL_COD_COLUNA").IsRequired();
            builder.Property(x => x.DecisaoId).HasColumnName("DEEVE_COD_DECISAO");        
        }
    }
}