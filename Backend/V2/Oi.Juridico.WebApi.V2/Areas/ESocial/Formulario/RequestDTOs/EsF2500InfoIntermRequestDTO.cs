using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Extensions;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2500InfoIntermRequestDTO
    {
        public int? InfointermDia { get; set; }
        public string? InfointermHrstrab { get; set; }
        public long? IdEsF2500Infointerm { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            if (!InfointermDia.HasValue)
            {
                mensagensErro.Add("O campo \"Dia do Mês Trabalhado\" é obrigatório.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
