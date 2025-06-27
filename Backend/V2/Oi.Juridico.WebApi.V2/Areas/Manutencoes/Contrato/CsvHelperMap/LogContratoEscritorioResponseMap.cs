using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Oi.Juridico.Contextos.V2.ManutencaoContratoEscritorioContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs;
using Perlink.Oi.Juridico.Infra.ValueObjects;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.CsvHelperMap
{
    public class LogContratoEscritorioResponseMap : ClassMap<LogContratoEscritorioResponse>
    {
        public LogContratoEscritorioResponseMap()
        {
            Map(row => row.OperacaoContrato).Name("Operação").TypeConverter<OperacaoConverter>();
            Map(row => row.DatLogContrato).Name("Data e Hora da Operação");
            Map(row => row.CodUsuario).Name("Código Usuário Log").TypeConverter<LongNumberConverter>();
            Map(row => row.CodContratoEscritorio).Name("Código do Contrato Escritório").TypeConverter<LongNumberConverter>();
            Map(row => row.CodTipoContratoEscritorioA).Name("Código do Tipo do Contrato Escritório Antes");
            Map(row => row.CodTipoContratoEscritorioD).Name("Código do Tipo do Contrato Escritório Depois");
            Map(row => row.IndAtivoA).Name("Status Ativo Antes");
            Map(row => row.IndAtivoD).Name("Status Ativo Depois");
            Map(row => row.IndConsideraCalculoVepA).Name("Status Considera Calculo Vep Antes");
            Map(row => row.IndConsideraCalculoVepD).Name("Status Considera Calculo Vep Depois");
            Map(row => row.NumContratoJecVcA).Name("Número Contrato JEC/VC Antes").TypeConverter<LongNumberConverter>();
            Map(row => row.NumContratoJecVcD).Name("Número Contrato JEC/VC Depois").TypeConverter<LongNumberConverter>();
            Map(row => row.NumContratoProconA).Name("Número Contrato Procon Antes").TypeConverter<LongNumberConverter>();
            Map(row => row.NumContratoProconD).Name("Número Contrato Procon Depois").TypeConverter<LongNumberConverter>();
            Map(row => row.NomContratoA).Name("Nome Contrato Antes");
            Map(row => row.NomContratoD).Name("Nome Contrato Depois");
            Map(row => row.NumSgpagJecVcA).Name("Número SGPAG JEC/VC Antes").TypeConverter<LongNumberConverter>();
            Map(row => row.NumSgpagJecVcD).Name("Número SGPAG JEC/VC Depois").TypeConverter<LongNumberConverter>();
            Map(row => row.NumSgpagProconA).Name("Número SGPAG Procon Antes").TypeConverter<LongNumberConverter>();
            Map(row => row.NumSgpagProconD).Name("Número SGPAG Procon Depois").TypeConverter<LongNumberConverter>();
            Map(row => row.CnpjA).Name("CNPJ Antes").TypeConverter<LongNumberConverter>();
            Map(row => row.CnpjD).Name("CNPJ Depois").TypeConverter<LongNumberConverter>();
            Map(row => row.ValVepA).Name("VEP Antes").TypeConverter<DecimalToN2Converter>();
            Map(row => row.ValVepD).Name("VEP Depois").TypeConverter<DecimalToN2Converter>();
            Map(row => row.DatInicioVigenciaA).Name("Data Início da Vigência Antes");
            Map(row => row.DatInicioVigenciaD).Name("Data Início da Vigência Depois");
            Map(row => row.DatFimVigenciaA).Name("Data Fim da Vigencia Antes");
            Map(row => row.DatFimVigenciaD).Name("Data Fim da Vigencia Depois");
            Map(row => row.ValUnitarioJecCcA).Name("Preço Unitário JEC/VC Antes").TypeConverter<DecimalToN2Converter>();
            Map(row => row.ValUnitarioJecCcD).Name("Preço Unitário JEC/VC Depois").TypeConverter<DecimalToN2Converter>();
            Map(row => row.ValUnitarioProconA).Name("Preço Unitário Procon Antes").TypeConverter<DecimalToN2Converter>();
            Map(row => row.ValUnitarioProconD).Name("Preço Unitário Procon Depois").TypeConverter<DecimalToN2Converter>();
            Map(row => row.ValUnitAudCapitalA).Name("Preço Unitário Audiências Capital Antes").TypeConverter<DecimalToN2Converter>();
            Map(row => row.ValUnitAudCapitalD).Name("Preço Unitário Audiências Capital Depois").TypeConverter<DecimalToN2Converter>();
            Map(row => row.ValUnitAudInteriorA).Name("Preço Unitário Audiências Interior Antes").TypeConverter<DecimalToN2Converter>();
            Map(row => row.ValUnitAudInteriorD).Name("Preço Unitário Audiências Interior Depois").TypeConverter<DecimalToN2Converter>();
            Map(row => row.IndPermanenciaLegadoA).Name("Status Permanência Legado Antes");
            Map(row => row.IndPermanenciaLegadoD).Name("Status Permanência Legado Depois");
            Map(row => row.NumMesesPermanenciaA).Name("Número Meses Permanência Antes");
            Map(row => row.NumMesesPermanenciaD).Name("Número Meses Permanência Depois");
            Map(row => row.ValDescontoPermanenciaA).Name("% Desconto Permanência Antes");
            Map(row => row.ValDescontoPermanenciaD).Name("% Desconto Permanência Depois");
            Map(row => row.CodContratoAtuacaoA).Name("Código Atuação Antes");
            Map(row => row.CodContratoAtuacaoD).Name("Código Atuação Depois");
            Map(row => row.CodContratoDiretoriaA).Name("Código Diretoria Antes");
            Map(row => row.CodContratoDiretoriaD).Name("Código Diretoria Depois");
        }
    }
}
