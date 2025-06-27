namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Escritorio
{
    public class ExportarAdvogadoEscritorioResponse
    {
        public string NomeEscritorio { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public string EstadoId { get; set; } = string.Empty;
        public string NumeroOAB { get; set; } = string.Empty;   
        public string NomeAdvogado { get; set; } = string.Empty;
        public string CelularDDD { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string EhContato { get; set; } = string.Empty;
        public string TipoPessoaValor { get; set; } = string.Empty;
        public bool? Ativo { get; set; }
    }
}
