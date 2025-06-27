using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarTipoDocumentoCommand : Validatable, IValidatable
    {
        public string Descricao { get; set; }
        public bool MarcadoCriacaoProcesso { get; set; } = false;
        public bool Ativo { get; set; } = true;
        public int CodTipoProcesso { get; set; }
        public int? CodTipoPrazo { get; set; }
        public bool IndRequerDatAudiencia { get; set; } = false;
        public bool IndPrioritarioFila { get; set; } = false;
        public bool IndDocumentoProtocolo { get; set; } = false;
        public bool IndDocumentoApuracao { get; set; }
        public bool IndEnviarAppPreposto { get; set; }
        public int? IdEstrategico { get; set; }
        public int? IdConsumidor { get; set; }

        public override void Validate()
        {
            if (Descricao.Length > 100)
            {
                AddNotification(nameof(Descricao), "O campo Descricao permite no máximo 100 caracteres");
            }

            if (CodTipoProcesso <= 0)
            {
                AddNotification(nameof(CodTipoProcesso), "Campo requerido");
            }

            if (CodTipoProcesso == 1 && (IndRequerDatAudiencia)) {
                AddNotification(nameof(CodTipoProcesso), "Tipo de Documento Cível Consumidor não aceita parâmetro Requer data de audiência");
            }

            if (CodTipoProcesso == 1 && (IndDocumentoProtocolo))
            {
                AddNotification(nameof(CodTipoProcesso), "Tipo de Documento Cível Consumidor não aceita parâmetro Utilizado em protocolo");
            }

            if (CodTipoProcesso == 1 && (CodTipoPrazo.HasValue))
            {
                AddNotification(nameof(CodTipoProcesso), "Tipo de Documento Cível Consumidor não aceita parâmetro Tipo de prazo");
            }



            if (CodTipoProcesso == 7 && (IndDocumentoProtocolo))
            {
                AddNotification(nameof(CodTipoProcesso), "Tipo de Documento Juizado Especial Cível não aceita parâmetro Utilizado em protocolo");
            }

            if (CodTipoProcesso == 7 && (CodTipoPrazo.HasValue))
            {
                AddNotification(nameof(CodTipoProcesso), "Tipo de Documento Juizado Especial Cível não aceita parâmetro Tipo de prazo");
            }


            if (CodTipoProcesso == 17 && (IndPrioritarioFila))
            {
                AddNotification(nameof(CodTipoProcesso), "Tipo de Documento Procon não aceita parâmetro Prioritário na Fila de Cadastro de Processo");
            }

            if (CodTipoProcesso == 17 && (IndRequerDatAudiencia))
            {
                AddNotification(nameof(CodTipoProcesso), "Tipo de Documento Procon não aceita parâmetro Requer data de audiência");
            }

            if (CodTipoProcesso == 17 && (CodTipoPrazo.HasValue))
            {
                AddNotification(nameof(CodTipoProcesso), "Tipo de Documento Juizado Especial Cível não aceita parâmetro Tipo de prazo");
            }


            if ((CodTipoProcesso == 14 || CodTipoProcesso == 15) && (MarcadoCriacaoProcesso))
            {
                AddNotification(nameof(CodTipoProcesso), "Parâmetro Cadastra Processo não esperado para o tipo de processo informado");
            }

            if ((CodTipoProcesso == 14 || CodTipoProcesso == 15) && (IndPrioritarioFila))
            {
                AddNotification(nameof(CodTipoProcesso), "Parâmetro Prioritário na Fila de Cadastro de Processo não esperado para o tipo de processo informado");
            }

            if ((CodTipoProcesso == 14 || CodTipoProcesso == 15) && (IndRequerDatAudiencia))
            {
                AddNotification(nameof(CodTipoProcesso), "Parâmetro Requer data de audiência não esperado para o tipo de processo informado");
            }

            if ((CodTipoProcesso == 14 || CodTipoProcesso == 15) && (IndDocumentoProtocolo))
            {
                AddNotification(nameof(CodTipoProcesso), "Parâmetro Utilizado em protocolo não esperado para o tipo de processo informado");
            }

            if ((CodTipoProcesso == 2 || CodTipoProcesso == 3
                || CodTipoProcesso == 4 || CodTipoProcesso == 5
                || CodTipoProcesso == 6 || CodTipoProcesso == 9
                || CodTipoProcesso == 12 || CodTipoProcesso == 18) && (MarcadoCriacaoProcesso))
            {
                AddNotification(nameof(CodTipoProcesso), "Parâmetro Cadastra Processo não esperado para o tipo de processo informado");
            }

            if ((CodTipoProcesso == 2 || CodTipoProcesso == 3
                || CodTipoProcesso == 4 || CodTipoProcesso == 5
                || CodTipoProcesso == 6 || CodTipoProcesso == 9
                || CodTipoProcesso == 12 || CodTipoProcesso == 18) && (IndPrioritarioFila))
            {
                AddNotification(nameof(CodTipoProcesso), "Parâmetro Prioritário na Fila de Cadastro de Processo não esperado para o tipo de processo informado");
            }

            if ((CodTipoProcesso == 2 || CodTipoProcesso == 3
                || CodTipoProcesso == 4 || CodTipoProcesso == 5
                || CodTipoProcesso == 6 || CodTipoProcesso == 9
                || CodTipoProcesso == 12 || CodTipoProcesso == 18) && (IndRequerDatAudiencia))
            {
                AddNotification(nameof(CodTipoProcesso), "Parâmetro Requer data de audiência não esperado para o tipo de processo informado");
            }

            if ((CodTipoProcesso == 2 || CodTipoProcesso == 3
                || CodTipoProcesso == 4 || CodTipoProcesso == 5
                || CodTipoProcesso == 6 || CodTipoProcesso == 9
                || CodTipoProcesso == 12 || CodTipoProcesso == 18) && (IndDocumentoProtocolo))
            {
                AddNotification(nameof(CodTipoProcesso), "Parâmetro Utilizado em protocolo não esperado para o tipo de processo informado");
            }

            if ((CodTipoProcesso == 2 || CodTipoProcesso == 3
                || CodTipoProcesso == 4 || CodTipoProcesso == 5
                || CodTipoProcesso == 6 || CodTipoProcesso == 9
                || CodTipoProcesso == 12 || CodTipoProcesso == 18) && (CodTipoPrazo.HasValue))
            {
                AddNotification(nameof(CodTipoProcesso), "Parâmetro Tipo de prazo não esperado para o tipo de processo informado");
            }

            if (IndDocumentoApuracao && !(new[] { TipoProcesso.CIVEL_CONSUMIDOR.Id, TipoProcesso.JEC.Id, TipoProcesso.PROCON.Id }.Contains(CodTipoProcesso)))
            {
                AddNotification(nameof(IndDocumentoApuracao), "Parâmetro Documento Apuração não esperado para o tipo de processo informado.");
            }

            if (IndEnviarAppPreposto && !IndDocumentoApuracao)
            {
                AddNotification(nameof(IndEnviarAppPreposto), "Parâmetro Enviar para App Preposto só deve ser marcado para tipos de documento de apuração ");
            }

        }
    }
}
