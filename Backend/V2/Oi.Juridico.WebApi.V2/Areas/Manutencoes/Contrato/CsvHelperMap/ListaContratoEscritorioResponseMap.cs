using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.CsvHelperMap
{
    public class ListaContratoEscritorioResponseMap : ClassMap<ContratoEscritorioResponse>
    {
        public ListaContratoEscritorioResponseMap()
        {
            Map(row => row.CodContratoEscritorio).Name("Código do Contrato Escritório");
            Map(row => row.TipoContratoEscritorio).Name("Tipo Contrato");
            Map(row => row.IndAtivo).Name("Status");
            Map(row => row.IndConsideraCalculoVep).Name("Considerar no Cálculo VEP");
            Map(row => row.NumContratoJecVc).Name("Número Contrato JEC/VC").TypeConverter<LongNumberConverter>();
            Map(row => row.NumContratoProcon).Name("Número Contrato Procon").TypeConverter<LongNumberConverter>();
            Map(row => row.NomContrato).Name("Nome Contrato");
            Map(row => row.NumSgpagJecVc).Name("Número SGPAG JEC/VC").TypeConverter<LongNumberConverter>();
            Map(row => row.NumSgpagProcon).Name("Número SGPAG Procon").TypeConverter<LongNumberConverter>();
            Map(row => row.Cnpj).Name("CNPJ").TypeConverter<LongNumberConverter>();
            Map(row => row.ValVep).Name("VEP").TypeConverter<DecimalToN2Converter>();
            Map(row => row.DatInicioVigencia).Name("Início da Vigência").TypeConverter<DataConverter>();
            Map(row => row.DatFimVigencia).Name("Fim da Vigência").TypeConverter<DataConverter>();
            Map(row => row.ValUnitarioJecCc).Name("Preço Unitário JEC/VC").TypeConverter<DecimalToN2Converter>();
            Map(row => row.ValUnitarioProcon).Name("Preço Unitário Procon").TypeConverter<DecimalToN2Converter>();
            //Map(row => row.ValUnitarioProcon).Name("Preço Unitário PROCON").Convert(c => string.Format("{0:C2}", c.Value.ValUnitarioProcon)); CASO QUERIA COM R$ NO INICIO
            Map(row => row.ValUnitAudCapital).Name("Preço Unitário Audiências Capital").TypeConverter<DecimalToN2Converter>();
            Map(row => row.ValUnitAudInterior).Name("Preço Unitário Audiências Interior").TypeConverter<DecimalToN2Converter>();
            Map(row => row.IndPermanenciaLegado).Name("Permanência Legado");
            Map(row => row.NumMesesPermanencia).Name("Meses Permanência");
            Map(row => row.ValDescontoPermanencia).Name("% Desconto Permanência");
            Map(row => row.ContratoAtuacao).Name("Atuação");
            Map(row => row.ContratoDiretoria).Name("Diretoria");
            Map(row => row.Escritorios).Name("Escritórios");
            Map(row => row.UF).Name("Estados");
        }
    }
}
