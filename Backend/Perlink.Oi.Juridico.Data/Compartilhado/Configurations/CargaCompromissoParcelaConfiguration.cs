using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
namespace Perlink.Oi.Juridico.Data.Compartilhado.Configurations
{
    public class CargaCompromissoParcelaConfiguration : IEntityTypeConfiguration<CargaCompromissoParcela>
    {
        public void Configure(EntityTypeBuilder<CargaCompromissoParcela> builder)
        {
            var boolConverter = new BoolToStringConverter("N", "S");

            builder.ToTable("CARGA_COMPROMISSO_PARCELA", "JUR");

            builder.HasKey(e => e.Id).HasName("CARGA_COMPROMISSO_PARCELA_PK");

            builder.Property(e => e.Id).HasColumnName("ID").HasColumnType("NUMBER").IsRequired();
            builder.Property(e => e.IdCompromisso).HasColumnName("ID_COMPROMISSO").HasColumnType("NUMBER").IsRequired();
            builder.Property(e => e.SeqLancamento).HasColumnName("SEQ_LANCAMENTO").HasColumnType("NUMBER");
            builder.Property(e => e.NroParcela).HasColumnName("NRO_PARCELA").HasColumnType("NUMBER");
            builder.Property(e => e.Valor).HasColumnName("VALOR").HasColumnType("NUMBER(18,3)");
            builder.Property(e => e.Vencimento).HasColumnName("VENCIMENTO").HasColumnType("DATE");
            builder.Property(e => e.Status).HasColumnName("STATUS").HasColumnType("NUMBER");
            builder.Property(e => e.MotivoExclusao).HasColumnName("MOTIVO_EXCLUSAO").HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.Deletado).HasColumnName("DELETADO").HasMaxLength(1).IsUnicode(false).HasDefaultValue("N").IsFixedLength();
            builder.Property(e => e.DeletadoDatahora).HasColumnName("DELETADO_DATAHORA").HasColumnType("DATE");
            builder.Property(e => e.DeletadoLogin).HasColumnName("DELETADO_LOGIN").HasMaxLength(200).IsUnicode(false);
            builder.Property(e => e.CodLancamento).HasColumnName("COD_LANCAMENTO");
            builder.Property(e => e.CodProcesso).HasColumnName("COD_PROCESSO").IsRequired();
            builder.Property(e => e.MotivoCancelamento).HasColumnName("MOTIVO_CANCELAMENTO").HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.ComentarioCancelamento).HasColumnName("COMENTARIO_CANCELAMENTO").HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.ComentarioEstorno).HasColumnName("COMENTARIO_ESTORNO").HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.UsrSolicCancelamento).HasColumnName("USR_SOLIC_CANCELAMENTO").HasMaxLength(250).IsUnicode(false);
            builder.Property(e => e.DataSolicCancelamento).HasColumnName("DATA_SOLIC_CANCELAMENTO").HasMaxLength(250).IsUnicode(false);            

        }
    }
}
