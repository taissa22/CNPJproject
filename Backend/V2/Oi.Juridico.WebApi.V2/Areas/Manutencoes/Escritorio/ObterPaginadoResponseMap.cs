using CsvHelper.Configuration;
using Oi.Juridico.AddOn.CsvHelperTypeConverters;
using SixLabors.ImageSharp.ColorSpaces;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Escritorio
{
    public class ObterPaginadoResponseMap : ClassMap<ObterPaginadoResponse>
    {
        public ObterPaginadoResponseMap()
        {
            Map(row => row.TipoPessoaValor).Name("Tipo de Pessoa").TypeConverter<TipoPessoaConverter>();
            Map(row => row.Nome).Name("Nome");
            Map(row => row.CNPJ).Name("CNPJ").Convert(x => x.Value.TipoPessoaValor == "J" ? $"\t{x.Value.CNPJ}" : "");
            Map(row => row.CPF).Name("CPF").Convert(x => x.Value.TipoPessoaValor == "F" ? $"\t{x.Value.CPF}" : "");
            Map(row => row.Endereco).Name("Endereço");
            Map(row => row.Bairro).Name("Bairro");
            Map(row => row.Cidade).Name("Cidade");
            Map(row => row.Cep).Name("CEP");
            Map(row => row.EstadoId).Name("Estado");
            Map(row => row.Email).Name("Email");
            Map(row => row.Site).Name("Site");
            Map(row => row.TelefoneDDD).Name("DDD");
            Map(row => row.Telefone).Name("Telefone");
            Map(row => row.CelularDDD).Name("DDD");
            Map(row => row.Celular).Name("Celular");
            Map(row => row.FaxDDD).Name("DDD");
            Map(row => row.Fax).Name("Fax");
            Map(row => row.GljCodGrupoLoteJuizado).Name("Lotes de Juizado");
            Map(row => row.AlertaEm).Name("Alerta da Agenda");
            Map(row => row.IndAreaCivel).Name("Cível Consumidor").TypeConverter<SimNaoConverter>();
            Map(row => row.CivelEstrategico).Name("Cível Estratégico").TypeConverter<SimNaoConverter>();
            Map(row => row.IndAreaTributaria).Name("Tributário Administrativo").TypeConverter<SimNaoConverter>();
            Map(row => row.IndAreaJuizado).Name("Juizado").TypeConverter<SimNaoConverter>();
            Map(row => row.IndAreaTrabalhista).Name("Trabalhista").TypeConverter<SimNaoConverter>();
            Map(row => row.IndAreaCriminalAdministrativo).Name("Criminal Administrativo").TypeConverter<SimNaoConverter>();
            Map(row => row.IndAreaCriminalJudicial).Name("Criminal Judicial").TypeConverter<SimNaoConverter>();
            Map(row => row.IndAreaCivelAdministrativo).Name("Cível Administrativo").TypeConverter<SimNaoConverter>();
            Map(row => row.IndAreaProcon).Name("Procon").TypeConverter<SimNaoConverter>();
            Map(row => row.IndAreaPEX).Name("Pex").TypeConverter<SimNaoConverter>();
            Map(row => row.CodProfissionalSAP).Name("Código SAP");
            Map(row => row.Ativo).Name("Ativo").Convert(x => x.Value.Ativo.GetValueOrDefault() ? "Sim" : "Não");
        }
    }
}
