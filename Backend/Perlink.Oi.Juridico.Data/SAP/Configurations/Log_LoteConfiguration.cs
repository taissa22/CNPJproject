using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Data.SAP.Configurations
{
    //[ExcludeFromCodeCoverage]
    //public class Log_LoteConfiguration : IEntityTypeConfiguration<Log_Lote>
    //{
    //    public void Configure(EntityTypeBuilder<Log_Lote> builder)
    //    {


    //        builder.ToTable("LOG_LOTE", "JUR");

           
    //        // PK
    //        builder.Property(bi => bi.Id)
    //            .IsRequired(true)
    //            .HasColumnName("COD_LOTE");

    //        builder.Property(bi => bi.Operacao)
    //           .IsRequired(true)
    //           .HasColumnName("OPERACAO");

    //        builder.Property(bi => bi.DataLog)
    //          .IsRequired(true)
    //          .HasColumnName("DAT_LOG");
           
    //        builder.Property(bi => bi.CodigoStatusPagamentoAntes)
    //        .HasColumnName("COD_STATUS_PAGAMENTO_A");

    //        builder.Property(bi => bi.CodigoStatusPagamentoDepois)
    //       .HasColumnName("COD_STATUS_PAGAMENTO_D");

    //        builder.Property(bi => bi.DescricaoStatusPagamento)
    //      .HasColumnName("DSC_STATUS_PAGAMENTO_D");

    //        builder.Property(bi => bi.NomeUsuario)
    // .IsRequired(true)
    // .HasColumnName("COD_USUARIO_LOG");

    //        builder.Property(bi => bi.UsuarioCodigoRetro)
    //        .IsRequired(true)
    //        .HasColumnName("USR_COD_USUARIO_OPER_RETRO");
            

    //        builder.HasKey(bi => new { bi.Id, bi.Operacao, bi.DataLog });
    //    }
    //}
}