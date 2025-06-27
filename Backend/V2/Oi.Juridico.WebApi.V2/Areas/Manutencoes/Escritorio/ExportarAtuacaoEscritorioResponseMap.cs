using CsvHelper.Configuration;
using SixLabors.ImageSharp.ColorSpaces;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Escritorio
{
    public class ExportarAtuacaoEscritorioResponseMap : ClassMap<ExportarAtuacaoEscritorioResponse>
    {
        public ExportarAtuacaoEscritorioResponseMap()
        {
            Map(row => row.Nome).Name("Nome");
            Map(row => row.CNPJ).Name("CNPJ").Convert(x => x.Value.TipoPessoaValor == "J" ? $"\t{x.Value.CNPJ}" : "");
            Map(row => row.CPF).Name("CPF").Convert(x => x.Value.TipoPessoaValor == "F" ? $"\t{x.Value.CPF}" : "");
            Map(row => row.CodEstadoCivelConsumidor).Name("Área de Atuação - Civel Consumidor");
            Map(row => row.CodEstadoJuizado).Name("Área de Atuação - Juizado");
        }
    }
}
