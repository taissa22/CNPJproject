using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.DTOs;

namespace Oi.Juridico.WebApi.V2.Areas.DistribuicaoProcessoEscritorio.CsvHelperMap
{
    public class ListaParametrizacaoEscritorioResponseMap : ClassMap<ListarParametrizacaoEscritorioResponse>
    {
        public ListaParametrizacaoEscritorioResponseMap()
        {
            Map(row => row.Codigo).Name("Código da Parametrização");
            Map(row => row.CodEstado).Name("Estado");
            Map(row => row.CodComarca).Name("Código da Comarca");
            Map(row => row.NomComarca).Name("Nome da Comarca");
            Map(row => row.CodVara).Name("Código da Vara");
            Map(row => row.CodTipoVara).Name("Código Tipo Vara");
            Map(row => row.NomTipoVara).Name("Nome da Vara");
            Map(row => row.CodTipoProcesso).Name("Código da Natureza");
            Map(row => row.DscTipoProcesso).Name("Natureza");
            Map(row => row.CodEmpresaCentralizadora).Name("Código Empresa Centralizadora");
            Map(row => row.NomEmpresaCentralizadora).Name("Nome Empresa Centralizadora");
            Map(row => row.Status).Name("Status");
            Map(row => row.CodProfissional).Name("Código do Escritório");
            Map(row => row.NomProfissional).Name("Nome do Escritório");
            Map(row => row.CodSolicitante).Name("Código do Solicitante");
            Map(row => row.NomSolicitante).Name("Nome do Solicitante");
            Map(row => row.DatVigenciaInicial).Name("Inicio da Vigência").TypeConverter<DataHoraConverter>();
            Map(row => row.DatVigenciaFinal).Name("Final da Vigência").TypeConverter<DataHoraConverter>();
            Map(row => row.PorcentagemProcessos).Name("Porcentagem de Processos");
            Map(row => row.Prioridade).Name("Prioridade");
        }
    }
}
