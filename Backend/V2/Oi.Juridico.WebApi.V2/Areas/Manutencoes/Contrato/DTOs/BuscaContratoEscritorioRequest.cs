namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs
{
    public class BuscaContratoEscritorioRequest
    {
        public long TipoContrato { get; set; }
        //public int CodEscritorio { get; set; }
        //public string Cnpj { get; set; } = String.Empty;
        public string NomContrato { get; set; } = String.Empty;
    }
}
