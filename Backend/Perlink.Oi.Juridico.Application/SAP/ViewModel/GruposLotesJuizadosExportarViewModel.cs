using AutoMapper;
using CsvHelper.Configuration.Attributes;
using Perlink.Oi.Juridico.Domain.SAP.Entity;

namespace Perlink.Oi.Juridico.Application.SAP.ViewModel
{
    public class GruposLotesJuizadosExportarViewModel
    {
        [Name("Grupo de Lote de Juizado")]
        public string Descricao { get; set; }

        public static void Mapping(Profile mapper)
        {
            mapper.CreateMap<GruposLotesJuizados, GruposLotesJuizadosExportarViewModel>();
        }
    }
}
