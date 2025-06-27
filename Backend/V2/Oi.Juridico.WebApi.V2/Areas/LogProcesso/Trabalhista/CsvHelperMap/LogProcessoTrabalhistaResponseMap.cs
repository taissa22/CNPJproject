using CsvHelper.Configuration;
using Oi.Juridico.WebApi.V2.Areas.LogProcesso.Trabalhista.DTO;

namespace Oi.Juridico.WebApi.V2.Areas.LogProcesso.Trabalhista.CsvHelperMap
{
    public class LogProcessoTrabalhistaResponseMap : ClassMap<LogProcessoTrabalhistaResponse>
    {
        public LogProcessoTrabalhistaResponseMap()
        {
            Map(row => row.Operacao).Name("Operação");
            Map(row => row.DatLog).Name("Data e Hora da Operação");
            Map(row => row.CodUsuarioUltAlteracao).Name("Código Usuário Log");
            Map(row => row.NomUsuario).Name("Nome Usuário Log");
            Map(row => row.IndUsuarioInternet).Name("Usuário Internet");
            Map(row => row.DatAudiencia).Name("Data Audiência");
            Map(row => row.HoraAudiencia).Name("Hora Audiência");
            Map(row => row.NomReclamadaAntes).Name("Reclamada Antes");
            Map(row => row.NomReclamadaDepois).Name("Reclamada Depois");
            Map(row => row.NomPrepostoAntes).Name("Preposto Antes");
            Map(row => row.NomPrepostoDepois).Name("Preposto Depois");
        }
    }
}
