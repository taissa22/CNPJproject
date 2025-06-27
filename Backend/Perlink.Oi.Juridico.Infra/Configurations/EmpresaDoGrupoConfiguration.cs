using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueGenerators;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.ValueConversion;

namespace Perlink.Oi.Juridico.Infra.Configurations
{
    internal class EmpresaDoGrupoConfiguration : IEntityTypeConfiguration<EmpresaDoGrupo>
    {
        public void Configure(EntityTypeBuilder<EmpresaDoGrupo> builder)
        {
            builder.ToTable("PARTE", "JUR");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD_PARTE")
                .IsRequired().HasNextSequenceValueGenerator("JUR", "PAR_SEQ_01");

            #region CAMPOS MAPEADOS EM PARTE, EMPRESA DO GRUPO, ESTABELECIMENTO
            builder.Property(x => x.Nome).HasColumnName("NOM_PARTE").HasDefaultValue(null);
            builder.Property(x => x.CNPJ).HasColumnName("CGC_PARTE").HasDefaultValue(null);
            builder.Property(x => x.TipoParteValor).HasColumnName("COD_TIPO_PARTE").HasDefaultValue(null);
            builder.Property(x => x.EstadoId).HasColumnName("COD_ESTADO").HasDefaultValue(null);
            builder.Property(x => x.Endereco).HasColumnName("END_PARTE").HasDefaultValue(null);
            builder.Property(x => x.CEP).HasColumnName("COD_CEP").HasDefaultValue(null);
            builder.Property(x => x.Cidade).HasColumnName("DSC_CIDADE").HasDefaultValue(null);
            builder.Property(x => x.Bairro).HasColumnName("DSC_BAIRRO").HasDefaultValue(null);
            builder.Property(x => x.TelefoneDDD).HasColumnName("DDD_TELEFONE").HasDefaultValue(null);
            builder.Property(x => x.Telefone).HasColumnName("TELEFONE").HasDefaultValue(null);
            builder.Property(x => x.FaxDDD).HasColumnName("DDD_FAX").HasDefaultValue(null);
            builder.Property(x => x.Fax).HasColumnName("FAX").HasDefaultValue(null);
            //builder.Property(x => x.Celular).HasColumnName("CELULAR").HasDefaultValue(null);
            builder.Property(x => x.EmpresaCentralizadoraId).HasColumnName("EMPCE_COD_EMP_CENTRALIZADORA").HasDefaultValue(null);
            #endregion CAMPOS MAPEADOS EM PARTE, EMPRESA DO GRUPO, ESTABELECIMENTO

            builder.Property(x => x.RegionalId).HasColumnName("COD_REGIONAL").HasDefaultValue(null);
            builder.HasOne(x => x.Regional).WithMany().HasForeignKey(x => x.RegionalId);

            builder.Property(x => x.CodCentroSap).HasColumnName("COD_CENTRO_SAP").HasDefaultValue(null);

            builder.Property(x => x.CentroCustoId).HasColumnName("COD_CENTRO_CUSTO").HasDefaultValue(null);
            builder.HasOne(x => x.CentroCusto).WithMany().HasForeignKey(x => x.CentroCustoId);

            builder.Property(x => x.FornecedorId).HasColumnName("COD_FORNECEDOR").HasDefaultValue(null);
            builder.HasOne(x => x.Fornecedor).WithMany().HasForeignKey(x => x.FornecedorId);

            builder.Property(x => x.GeraArquivoBB).HasColumnName("IND_GERA_ARQUIVO_BB").HasConversion(ValueConverters.BoolToString).HasDefaultValue(null);
            builder.HasOne(x => x.EmpresaCentralizadora).WithMany().HasForeignKey(x => x.EmpresaCentralizadoraId);

            builder.Property(x => x.CodigoEmpresaSap).HasColumnName("ESAP_COD_EMPRESA_SAP").HasDefaultValue(null);
            builder.HasOne(x => x.EmpresaSap).WithMany().HasForeignKey(x => x.CodigoEmpresaSap);

            builder.Property(x => x.DiretorioBB).HasColumnName("COD_DIRETORIO_BANCO_BRASIL").HasDefaultValue(null);

            builder.Property(x => x.EmpRecuperanda).HasColumnName("IND_EMP_RECUPERANDA").HasConversion(ValueConverters.BoolToString).HasDefaultValue(null);

            builder.HasQueryFilter(x => x.TipoParteValor == TipoParte.EMPRESA_DO_GRUPO.Valor);

            builder.Property(x => x.EmpTrio).HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IND_EMP_TRIO")
                .IsFixedLength();

            #region ENTITIES

            builder.HasOne(x => x.ParteBase).WithOne(x => x.EmpresaDoGrupo).HasForeignKey<EmpresaDoGrupo>(x => x.Id);

            #endregion ENTITIES
        }
    }
}