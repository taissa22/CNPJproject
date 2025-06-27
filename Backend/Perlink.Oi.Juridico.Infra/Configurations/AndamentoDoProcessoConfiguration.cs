using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class AndamentoDoProcessoConfiguration : IEntityTypeConfiguration<AndamentoDoProcesso>
    {
        public void Configure(EntityTypeBuilder<AndamentoDoProcesso> builder)
        {
            builder.ToTable("ANDAMENTO_PROCESSO", "JUR");

            builder.HasKey(x => new { x.ProcessoId, x.Sequencial });

            builder.Property(x => x.Sequencial).HasColumnName("SEQ_ANDAMENTO").IsRequired();

            builder.Property(x => x.ProcessoId).HasColumnName("COD_PROCESSO").IsRequired();
            builder.HasOne(x => x.Processo).WithMany().HasForeignKey(x => x.ProcessoId);

            builder.Property(x => x.EventoId).HasColumnName("COD_EVENTO").IsRequired();
            builder.HasOne(x => x.Evento).WithMany().HasForeignKey(x => x.EventoId);

            builder.Property(x => x.DataEvento).HasColumnName("DAT_EVENTO").IsRequired();

            builder.Property(x => x.AcaoId).HasColumnName("COD_ACAO");
            builder.HasOne(x => x.Acao).WithMany().HasForeignKey(x => x.AcaoId);


            builder.Property(x => x.DecisaoId).HasColumnName("COD_DECISAO");            

            //builder.HasOne(x => x.Processo).WithMany(x => x.AndamentosDoProcesso).HasForeignKey(x => x.ProcessoId);
        }
    }
}