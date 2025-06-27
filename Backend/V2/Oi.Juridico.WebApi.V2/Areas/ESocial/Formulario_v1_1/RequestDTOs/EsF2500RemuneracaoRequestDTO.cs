using Microsoft.IdentityModel.Tokens;
using Oi.Juridico.Shared.V2.Enums;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
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

            if (this.RemuneracaoUndsalfixo != ESocialUnidadePagamento.NaoAplicavel.ToByte() && this.RemuneracaoVrsalfx < 0)
            {
                mensagensErro.Add("Valor salário fixo informado deve ser maior ou igual a 0 (zero).");
            }

            if (!this.RemuneracaoVrsalfx.HasValue && this.RemuneracaoUndsalfixo != ESocialUnidadePagamento.NaoAplicavel.ToByte())
            {
                mensagensErro.Add("Valor salário fixo informado deve ser maior ou igual a 0 (zero).");
            }

            if (this.RemuneracaoUndsalfixo == ESocialUnidadePagamento.NaoAplicavel.ToByte() && this.RemuneracaoVrsalfx > 0)
            {
                mensagensErro.Add($"Valor salário fixo informado deve ser igual a 0 (zero) quando a unidade de pagamento for igual a {ESocialUnidadePagamento.NaoAplicavel.Descricao()}.");
            }

            if (!this.RemuneracaoVrsalfx.HasValue && this.RemuneracaoUndsalfixo == ESocialUnidadePagamento.NaoAplicavel.ToByte())
            {
                mensagensErro.Add("Valor salário fixo informado deve ser igual a 0 (zero).");
            }

            if (this.RemuneracaoUndsalfixo < 0)
            {
                mensagensErro.Add("Unidade de pagamento da parte fixa da remuneração inválida.");
            }
            if (!this.RemuneracaoUndsalfixo.HasValue)
            {
                mensagensErro.Add("Unidade de pagamento da parte fixa da remuneração inválida.");
            }

            if (this.RemuneracaoUndsalfixo == ESocialUnidadePagamento.NaoAplicavel.ToByte() && this.RemuneracaoDscsalvar.IsNullOrEmpty() && this.RemuneracaoDscsalvar!.Length <= 0)
            {
                mensagensErro.Add($"Quando a unidade de pagamento for igual a {ESocialUnidadePagamento.NaoAplicavel.Descricao()} a descrição do pagamento deve ser preenchida.");
            }

            if (this.RemuneracaoUndsalfixo == ESocialUnidadePagamento.PorTarefa.ToByte() && this.RemuneracaoDscsalvar.IsNullOrEmpty() && this.RemuneracaoDscsalvar!.Length <= 0)
            {
                mensagensErro.Add($"Quando a unidade de pagamento for igual a {ESocialUnidadePagamento.PorTarefa.Descricao()} a descrição do pagamento deve ser preenchida.");
            }

            if (!this.RemuneracaoDtremun.HasValue)
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
