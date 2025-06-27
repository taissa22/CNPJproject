using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class EmpresaCentralizadoraAgendamentoFechAutoConfiguration : IEntityTypeConfiguration<EmpresaCentralizadoraAgendamentoFechAuto>
    {
        public void Configure(EntityTypeBuilder<EmpresaCentralizadoraAgendamentoFechAuto> builder)
        {
            builder.ToTable("EMP_CENT_AGENDAM_FECH_AUTO", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID_EMP_CENT_AGENDAM_FECH_AUTO").IsRequired();

            builder.Property(x => x.CodSolicitacaoFechamento).HasColumnName("COD_SOLIC_FECHAMENTO_CONT").IsRequired();
        }
    }
}