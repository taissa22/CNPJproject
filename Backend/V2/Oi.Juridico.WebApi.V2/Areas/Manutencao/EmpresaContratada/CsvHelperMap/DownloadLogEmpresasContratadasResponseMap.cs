using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.DTO_s;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencao.EmpresaContratada.CsvHelperMap
{
    public class DownloadLogEmpresasContratadasResponseMap : ClassMap<DownloadLogEmpresasContratadasResponse>
    {
        public DownloadLogEmpresasContratadasResponseMap()
        {
            Map(row => row.CodEmpTerceiroContratada).Name("Código Empresa Terceiro Contratada");
            Map(row => row.Operacao).Name("Operação").TypeConverter<OperacaoConverter>();
            Map(row => row.DatLog).Name("Data e Hora da Operação");
            Map(row => row.CodUsuario).Name("Código Usuário Log");
            Map(row => row.CodEmpresaContratadaA).Name("Código Empresa Contratada Antes");
            Map(row => row.CodEmpresaContratadaD).Name("Código Empresa Contratada Depois");
            Map(row => row.CodTerceiroContratadoA).Name("Código Terceiro Contratado Antes");
            Map(row => row.CodTerceiroContratadoD).Name("Código Terceiro Contratado Depois");
            Map(row => row.NomEmpresaContratadaA).Name("Razão Social Antes");
            Map(row => row.NomEmpresaContratadaD).Name("Razão Social Depois");
            Map(row => row.LoginTerceiroA).Name("Login Terceiro Antes");
            Map(row => row.LoginTerceiroD).Name("Login Terceiro Depois");
        }

    }
}
