namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Escritorio
{
    public class ObterPaginadoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public bool? Ativo { get; set; }
        public bool EhEscritorio { get; set; }
        public string Endereco { get; set; } = "";
        public bool CivelEstrategico { get; set; }
        public string TipoPessoaValor { get; set; } = "";
        public string CPF { get; set; } = "";
        public bool IndAreaCivel { get; set; }
        public bool IndAreaJuizado { get; set; }
        public bool IndAreaRegulatoria { get; set; }
        public bool IndAreaTrabalhista { get; set; }
        public bool IndAreaTributaria { get; set; }
        public bool IndAreaCivelAdministrativo { get; set; }
        public bool IndAreaCriminalAdministrativo { get; set; }
        public bool IndAreaCriminalJudicial { get; set; }
        public bool IndAreaPEX { get; set; }
        public bool IndAreaProcon { get; set; }
        public bool IndAdvogado { get; set; }
        public int? Cep { get; set; }
        public string Cidade { get; set; } = "";
        public string Email { get; set; } = "";
        public string EstadoId { get; set; } = "";
        public string Bairro { get; set; } = "";
        public string Site { get; set; } = "";
        public int? AlertaEm { get; set; }
        public string CodProfissionalSAP { get; set; } = "";
        public string CNPJ { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string TelefoneDDD { get; set; } = "";
        public string Celular { get; set; } = "";
        public string CelularDDD { get; set; } = "";
        public string Fax { get; set; } = "";
        public string FaxDDD { get; set; } = "";
        public short? GljCodGrupoLoteJuizado { get; set; }
        public bool enviarAppPreposto { get; set; }
    }
}
