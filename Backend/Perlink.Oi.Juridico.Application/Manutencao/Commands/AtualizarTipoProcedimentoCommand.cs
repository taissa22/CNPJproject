using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarTipoProcedimentoCommand : Validatable, IValidatable
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public int? CodTipoParticipacao1 { get; set; }
        public int? CodTipoParticipacao2 { get; set; }
        public bool? IndOrgao1 { get; set; }
        public bool? IndOrgao2 { get; set; }
        public bool? IndProvisionado { get; set; }
        public bool? IndPoloPassivoUnico { get; set; }
        public bool IndAtivo { get; set; }

        public override void Validate()
        {
            if (Codigo <= 0)
            {
                AddNotification(nameof(Codigo), "O campo Obigatório");
            }

            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(50))
            {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }

            if (IndOrgao1.GetValueOrDefault() && IndOrgao2.GetValueOrDefault())
            {
                AddNotification(nameof(IndOrgao1), "Os campos IndOrgao1 e IndOrgao2 não podem ser marcados ao mesmo tempo");
            }

            if (IndOrgao2.GetValueOrDefault() && IndPoloPassivoUnico.GetValueOrDefault())
            {
                AddNotification(nameof(IndOrgao1), "Os campos IndOrgao2 e IndPoloPassivoUnico não podem ser marcados ao mesmo tempo");
            }

        }
    }

}
