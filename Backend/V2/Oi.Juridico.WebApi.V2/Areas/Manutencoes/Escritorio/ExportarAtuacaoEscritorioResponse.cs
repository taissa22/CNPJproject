namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Escritorio
{
    public class ExportarAtuacaoEscritorioResponse
    {
        public string Nome { get; set; } = string.Empty;
        public string TipoPessoaValor { get; set; } = string.Empty;
        public string? CPF { get; set; }
        public string? CNPJ { get; set; }
        public bool Ativo { get; set; }
        public byte CodTipoProcesso { get; set; }
        public string CodEstado { get; set; } = string.Empty;
        public string CodEstadoCivelConsumidor { get; set; } = string.Empty;
        public string CodEstadoJuizado { get; set; } = string.Empty;
    }
}
