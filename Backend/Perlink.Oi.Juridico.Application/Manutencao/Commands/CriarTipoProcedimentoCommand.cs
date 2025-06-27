using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarTipoProcedimentoCommand: Validatable, IValidatable
    {
        public string Descricao { get; set; }
        public int? CodTipoParticipacao1 { get; set; }
        public int? CodTipoParticipacao2 { get; set; }
        public bool? IndOrgao1 { get; set; }
        public bool? IndOrgao2 { get; set; }
        public bool? IndProvisionado { get; set; }
        public bool? IndPoloPassivoUnico { get; set; }
        public bool IndAtivo { get; set; }
        public int CodTipoProcesso { get; set; }



        public override void Validate()
        {
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

            if (!new[] { TipoProcesso.ADMINISTRATIVO.Id, TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id, 
                TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id, TipoProcesso.CIVEL_ADMINISTRATIVO.Id, 
                TipoProcesso.CRIMINAL_ADMINISTRATIVO.Id }.Contains(CodTipoProcesso))
            {
                AddNotification("Tipo de Processo", "Tipo de Processo não permitido.");
            }

            if ((CodTipoProcesso == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id || 
                CodTipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id) 
                && CodTipoParticipacao1.GetValueOrDefault() <= 0)
            {
                AddNotification("1º Tipo de Participação", "Campo Requerido");
            }

            if ((CodTipoProcesso == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id ||
                CodTipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id)
                && CodTipoParticipacao2.GetValueOrDefault() <= 0)
            {
                AddNotification("2º Tipo de Participação", "Campo Requerido");
            }
        }
    }   

}
