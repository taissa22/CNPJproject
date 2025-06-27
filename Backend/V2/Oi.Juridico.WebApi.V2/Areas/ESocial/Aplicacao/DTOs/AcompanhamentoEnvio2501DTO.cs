#nullable disable

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public partial class AcompanhamentoEnvio2501DTO
    {
        public string Cpf { get; set; }
        public string NomeReclamante { get; set; }
        public string NumProcesso { get; set; }
        public int? CodProcesso { get; set; }
        public string Escritorio { get; set; }
        public string Contador { get; set; }
        public DateTime? LogDataOperacao { get; set; }
        public string Ocorrencia { get; set; }
        public string Formulario { get; set; }
        public string EmpreseGrupo { get; set; }
        public string Original_retificacao { get; set;}
        public string Acao { get; set; }
        public string Codigo { get; set; }
        public string Localizacao { get; set; }
        public string PeriodoApuracao { get; set; } = string.Empty;
        public int? IdExecucao { get; set; }
    }
}