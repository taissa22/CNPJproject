using CsvHelper.Configuration;
using SixLabors.ImageSharp.ColorSpaces;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Escritorio
{
    public class ExportarAdvogadoEscritorioResponseMap : ClassMap<ExportarAdvogadoEscritorioResponse>
    {
        public ExportarAdvogadoEscritorioResponseMap()
        {
            Map(row => row.NomeEscritorio).Name("Nome");
            Map(row => row.CNPJ).Name("CNPJ").Convert(x => x.Value.TipoPessoaValor == "J" ? $"\t{x.Value.CNPJ}" : "");
            Map(row => row.CPF).Name("CPF").Convert(x => x.Value.TipoPessoaValor == "F" ? $"\t{x.Value.CPF}" : "");
            Map(row => row.EstadoId).Name("Estado OAB");
            Map(row => row.NumeroOAB).Name("Nro OAB");
            Map(row => row.NomeAdvogado).Name("Nome Advogado");
            Map(row => row.CelularDDD).Name("DDD");
            Map(row => row.Celular).Name("Celular");
            Map(row => row.EhContato).Name("Contato do Escritório");
        }
    }
}
