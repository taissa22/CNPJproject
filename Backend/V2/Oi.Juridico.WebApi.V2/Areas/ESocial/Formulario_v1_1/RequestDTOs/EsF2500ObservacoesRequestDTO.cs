namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
{
    public class EsF2500ObservacoesRequestDTO
    {
        public string? ObservacoesObservacao { get; set; } = string.Empty;

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            if (this.ObservacoesObservacao is null || (this.ObservacoesObservacao is not null && this.ObservacoesObservacao.Length <= 0))
            {
                mensagensErro.Add("O campo observação é obrigatório.");
            }

            if (this.ObservacoesObservacao is not null && this.ObservacoesObservacao.Length > 255)
            {
                mensagensErro.Add("O campo observação não pode conter mais do que 255 caracteres.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
