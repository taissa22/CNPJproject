namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
{
    public class EsF2501InfocrcontribImportRequestDTO : EsF2501InfocrcontribRequestDTO
    {
        public DateTime? CalctribPerref { get; set; }

#pragma warning disable CS0108 // O membro oculta o membro herdado; nova palavra-chave ausente
        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
#pragma warning restore CS0108 // O membro oculta o membro herdado; nova palavra-chave ausente
        {
            var (_, ListaErros) = base.Validar();

            var mensagensErro = ListaErros.ToList();

            if (!this.CalctribPerref.HasValue)
            {
                mensagensErro.Add("O campo \"Período de Referência\" é obrigatório.");
            }

            if (this.CalctribPerref.HasValue && this.CalctribPerref.Value.Date > DateTime.Today)
            {
                mensagensErro.Add("O campo \"Período de Referência\" informado contem uma data inválida.");
            }      

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
