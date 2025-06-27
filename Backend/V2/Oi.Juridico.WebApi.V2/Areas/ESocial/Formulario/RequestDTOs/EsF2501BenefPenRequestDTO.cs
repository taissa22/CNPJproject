using System.Collections.Generic;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501BenefPenRequestDTO
    {
        public string? BenefpenCpfdep { get; set; }
        public decimal? BenefpenVlrdepensusp { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
