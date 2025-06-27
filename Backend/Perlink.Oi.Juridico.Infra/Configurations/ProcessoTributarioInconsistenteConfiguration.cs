using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class ProcessoInconsistenteConfiguration : IEntityTypeConfiguration<ProcessoInconsistente>
    {
        public void Configure(EntityTypeBuilder<ProcessoInconsistente> builder)
        {
            builder.HasKey(x => x.CodigoProcesso);

            builder.Property(x => x.TipoProcesso).HasColumnName("TIPO_PROCESSO");
            builder.Property(x => x.CodigoProcesso).HasColumnName("COD_PROCESSO");

            builder.Property(x => x.NumeroProcesso).HasColumnName("NRO_PROCESSO_CARTORIO");
            builder.Property(x => x.Estado).HasColumnName("COD_ESTADO");
            builder.Property(x => x.ComarcaOrgao).HasColumnName("NOM_COMARCA_ORGAO");
            builder.Property(x => x.VaraMunicipio).HasColumnName("COD_VARA_MUNICIPIO");
            builder.Property(x => x.EmpresadoGrupo).HasColumnName("NOM_EMPRESA_GRUPO");
            builder.Property(x => x.Escritorio).HasColumnName("NOM_ESCRITORIO");
            builder.Property(x => x.Objeto).HasColumnName("NOM_OBJETO");
            builder.Property(x => x.Periodo).HasColumnName("PERIODO_FINAL");
            builder.Property(x => x.ValorTotalCorrigido).HasColumnName("VALOR_TOTAL");
            builder.Property(x => x.ValorTotalPago).HasColumnName("VALOR_TOTAL_PAGO");
        }
    }


    internal class ProcessoTributarioInconsistenteConfiguration : IEntityTypeConfiguration<ProcessoTributarioInconsistente>
    {
        public void Configure(EntityTypeBuilder<ProcessoTributarioInconsistente> builder)
        {
            builder.ToTable("PROCESSO_TRIB_INCONSISTENTE", "JUR");

            builder.HasKey(x => x.CodigoProcesso);

            builder.Property(x => x.TipoProcesso).HasColumnName("TIPO_PROCESSO");
            builder.Property(x => x.CodigoProcesso).HasColumnName("COD_PROCESSO");
            builder.Property(x => x.NumeroProcesso).HasColumnName("NRO_PROCESSO_CARTORIO");
            builder.Property(x => x.Estado).HasColumnName("COD_ESTADO");
            builder.Property(x => x.ComarcaOrgao).HasColumnName("NOM_COMARCA_ORGAO");
            builder.Property(x => x.VaraMunicipio).HasColumnName("COD_VARA_MUNICIPIO");
            builder.Property(x => x.EmpresadoGrupo).HasColumnName("NOM_EMPRESA_GRUPO");
            builder.Property(x => x.Escritorio).HasColumnName("NOM_ESCRITORIO");
            builder.Property(x => x.Objeto).HasColumnName("NOM_OBJETO");
            builder.Property(x => x.Periodo).HasColumnName("PERIODO_FINAL");
            builder.Property(x => x.ValorTotalCorrigido).HasColumnName("VALOR_TOTAL");
            builder.Property(x => x.ValorTotalPago).HasColumnName("VALOR_TOTAL_PAGO");
        }
    }
}