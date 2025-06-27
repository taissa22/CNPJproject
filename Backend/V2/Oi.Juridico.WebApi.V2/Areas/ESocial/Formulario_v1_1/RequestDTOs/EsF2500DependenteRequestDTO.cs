using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Shared.V2.Extensions;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
{
    public class EsF2500DependenteRequestDTO
    {
        public string DependenteCpfdep { get; set; } = string.Empty;
        public string DependenteTpdep { get; set; } = string.Empty;
        public string? DependenteDescdep { get; set; } = string.Empty;

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {            
            var mensagensErro = new List<string>();

            this.DependenteCpfdep = this.DependenteCpfdep.SoNumero();

            if (this.DependenteCpfdep is null)
            {
                mensagensErro.Add("CPF não pode estar vazio.");
            }

            if (!this.DependenteCpfdep.CPFValido())
            {
                mensagensErro.Add("CPF inválido.");
            }

            if (this.DependenteTpdep.Length <= 0)
            {
                mensagensErro.Add("O campo tipo de dependente é obrigatório.");
            }

            if (((this.DependenteDescdep is not null && this.DependenteDescdep.Length == 0) || this.DependenteDescdep is null) && this.DependenteTpdep == ESocialTipoDependente.AgregadoOutros.ToInt().ToString()) 
            {
                mensagensErro.Add("O campo descrição da dependência é obrigatório quando o tipo de depêndencia informado é igual a 99 - Agregado/Outros");
            }

            if (!string.IsNullOrEmpty(this.DependenteTpdep) && this.DependenteTpdep != ESocialTipoDependente.AgregadoOutros.ToInt().ToString() && !string.IsNullOrEmpty(this.DependenteDescdep))
            {
                mensagensErro.Add("O campo 'Descrição da Dependência' não deve ser preenchida quando o tipo de dependente for diferente de 99 - Agregado/Outros");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }   
}
