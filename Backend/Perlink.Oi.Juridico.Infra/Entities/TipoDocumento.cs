using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Entities {


    public sealed class TipoDocumento : Notifiable, IEntity, INotifiable {

        private TipoDocumento(){
        }

        public static TipoDocumento Criar(string descricao, bool ativo, TipoProcesso tipoProcesso, 
            bool cadastraProcesso, bool prioritarioNaFila, bool requerDataAudiencia, bool utilizadoEmProtocolo,
            TipoPrazo tipoPrazo, bool indDocumentoApuracao, bool indEnviarAppPreposto)
        {
            TipoDocumento tipoDocumento = new TipoDocumento();
            tipoDocumento.Descricao = descricao.ToUpper();
            tipoDocumento.Ativo = ativo;
            tipoDocumento.CodTipoProcesso = tipoProcesso.Id;
            tipoDocumento.MarcadoCriacaoProcesso = cadastraProcesso;
            tipoDocumento.IndPrioritarioFila = prioritarioNaFila;
            tipoDocumento.IndRequerDatAudiencia = requerDataAudiencia;
            tipoDocumento.IndDocumentoProtocolo = utilizadoEmProtocolo;
            tipoDocumento.CodTipoPrazo = tipoPrazo?.Id ?? null;
            tipoDocumento.IndDocumentoApuracao = indDocumentoApuracao;
            tipoDocumento.IndEnviarAppPreposto = indEnviarAppPreposto;

            tipoDocumento.Validate();
            return tipoDocumento;
        }

        public static TipoDocumento CriarTipoDocumentoConsumidor(string descricao, bool cadastraProcesso, bool prioritarioNaFila, int codTipoProcesso
           )
        {
            TipoDocumento tipoDocumento = new TipoDocumento();
            tipoDocumento.Descricao = descricao.ToUpper();                        
            tipoDocumento.MarcadoCriacaoProcesso = cadastraProcesso;
            tipoDocumento.IndPrioritarioFila = prioritarioNaFila;
            tipoDocumento.CodTipoProcesso = codTipoProcesso;
            tipoDocumento.Validate();
            return tipoDocumento;
        }


        public void AtualizarTipoDocumentoConsumidor(string descricao, bool cadastraProcesso, bool prioritarioNaFila, int codTipoProcesso)
        {
            Descricao = descricao.ToUpper();            
            MarcadoCriacaoProcesso = cadastraProcesso;
            IndPrioritarioFila = prioritarioNaFila;
            CodTipoProcesso = codTipoProcesso;
            Validate();
        }

        public void Atualizar(string descricao, bool ativo, bool cadastraProcesso, bool prioritarioNaFila, 
            bool requerDataAudiencia, bool utilizadoEmProtocolo, TipoPrazo tipoPrazo, bool indDocumentoApuracao, bool indEnviarAppPreposto)
        {
            Descricao = descricao.ToUpper();
            Ativo = ativo;
            MarcadoCriacaoProcesso = cadastraProcesso;
            IndPrioritarioFila = prioritarioNaFila;
            IndRequerDatAudiencia = requerDataAudiencia;
            IndDocumentoProtocolo = utilizadoEmProtocolo;           
            CodTipoPrazo = tipoPrazo?.Id ?? null;
            IndDocumentoApuracao = indDocumentoApuracao;
            IndEnviarAppPreposto = indEnviarAppPreposto;

            Validate();
        }

        public int Id { get; private set; }
        public string Descricao { get; private set; }
        public bool MarcadoCriacaoProcesso { get; private set; }
        public bool Ativo { get; private set; }
        public int CodTipoProcesso { get; private set; }
        public TipoProcesso TipoProcesso { get { return TipoProcesso.PorId(CodTipoProcesso); } }
        public int? CodTipoPrazo { get; private set; }
        public TipoPrazo TipoPrazo { get; private set; }
        public bool IndRequerDatAudiencia { get; private set; }
        public bool IndPrioritarioFila { get; private set; }
        public bool IndDocumentoProtocolo { get; private set; }
        public bool IndDocumentoApuracao { get; private set; }
        public bool IndEnviarAppPreposto { get; private set; }
        

        private void Validate()
        {

            if (Descricao.Length > 100)
            {
                AddNotification(nameof(Descricao), "O campo Descricao permite no máximo 100 caracteres");
            }

            if (CodTipoProcesso <= 0)
            {
                AddNotification(nameof(CodTipoProcesso), "Campo requerido");
            }

            if (CodTipoProcesso == 1 && (IndRequerDatAudiencia))
            {
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

            if (IndEnviarAppPreposto && !IndDocumentoApuracao)
            {
                AddNotification(nameof(IndEnviarAppPreposto), "Parâmetro Enviar para App Preposto só deve ser marcado para tipos de documento de apuração ");
            }

            if (IndDocumentoApuracao && !(new[] { TipoProcesso.CIVEL_CONSUMIDOR.Id, TipoProcesso.JEC.Id, TipoProcesso.PROCON.Id }.Contains(CodTipoProcesso)))
            {
                AddNotification(nameof(IndDocumentoApuracao), "Parâmetro Documento Apuração não esperado para o tipo de processo informado.");
            }

        }

    }
}
