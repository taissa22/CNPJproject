using Microsoft.IdentityModel.Tokens;
using Oi.Juridico.Shared.V2.Enums;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2500RemuneracaoRequestDTO
    {
        public DateTime? RemuneracaoDtremun { get; set; }
        public decimal? RemuneracaoVrsalfx { get; set; }
        public byte? RemuneracaoUndsalfixo { get; set; }
        public string? RemuneracaoDscsalvar { get; set; } = string.Empty;

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            if (RemuneracaoUndsalfixo != ESocialUnidadePagamento.NaoAplicavel.ToByte() && RemuneracaoVrsalfx < 0)
            {
                mensagensErro.Add("Valor salário fixo informado deve ser maior ou igual a 0 (zero).");
            }

            if (!RemuneracaoVrsalfx.HasValue && RemuneracaoUndsalfixo != ESocialUnidadePagamento.NaoAplicavel.ToByte())
            {
                mensagensErro.Add("Valor salário fixo informado deve ser maior ou igual a 0 (zero).");
            }

            if (RemuneracaoUndsalfixo == ESocialUnidadePagamento.NaoAplicavel.ToByte() && RemuneracaoVrsalfx > 0)
            {
                mensagensErro.Add($"Valor salário fixo informado deve ser igual a 0 (zero) quando a unidade de pagamento for igual a {ESocialUnidadePagamento.NaoAplicavel.Descricao()}.");
            }

            if (!RemuneracaoVrsalfx.HasValue && RemuneracaoUndsalfixo == ESocialUnidadePagamento.NaoAplicavel.ToByte())
            {
                mensagensErro.Add("Valor salário fixo informado deve ser igual a 0 (zero).");
            }

            if (RemuneracaoUndsalfixo < 0)
            {
                mensagensErro.Add("Unidade de pagamento da parte fixa da remuneração inválida.");
            }
            if (!RemuneracaoUndsalfixo.HasValue)
            {
                mensagensErro.Add("Unidade de pagamento da parte fixa da remuneração inválida.");
            }

            if (RemuneracaoUndsalfixo == ESocialUnidadePagamento.NaoAplicavel.ToByte() && RemuneracaoDscsalvar.IsNullOrEmpty() && RemuneracaoDscsalvar!.Length <= 0)
            {
                mensagensErro.Add($"Quando a unidade de pagamento for igual a {ESocialUnidadePagamento.NaoAplicavel.Descricao()} a descrição do pagamento deve ser preenchida.");
            }

            if (RemuneracaoUndsalfixo == ESocialUnidadePagamento.PorTarefa.ToByte() && RemuneracaoDscsalvar.IsNullOrEmpty() && RemuneracaoDscsalvar!.Length <= 0)
            {
                mensagensErro.Add($"Quando a unidade de pagamento for igual a {ESocialUnidadePagamento.PorTarefa.Descricao()} a descrição do pagamento deve ser preenchida.");
            }

            if (!RemuneracaoDtremun.HasValue)
            {
                mensagensErro.Add("Data vigencia obrigatório.");
            }

            if (RemuneracaoUndsalfixo.HasValue && !Enum.IsDefined(typeof(ESocialUnidadePagamento), (int)RemuneracaoUndsalfixo!))
            {
                mensagensErro.Add("Unidade de pagamento inválida.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
