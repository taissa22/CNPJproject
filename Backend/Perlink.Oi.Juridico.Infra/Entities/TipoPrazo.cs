using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class TipoPrazo : Notifiable, IEntity, INotifiable
    {
        private TipoPrazo()
        {
        }

        public static TipoPrazo Criar(string descricao, bool ativo, TipoProcessoManutencao tipoProcesso, bool? ehServico, bool? ehDocumento)
        {
            var tipoPrazo = new TipoPrazo();

            tipoPrazo.Descricao = descricao;
            tipoPrazo.Ativo = ativo;
            tipoPrazo.Eh_Servico = (bool)ehServico;
            tipoPrazo.Eh_Documento = (bool)ehDocumento;

            switch (tipoProcesso.Id)
            {
                case 1:
                    tipoPrazo.Eh_Civel_Consumidor = true;
                    break;
                case 2:
                    tipoPrazo.Eh_Trabalhista = true;
                    break;
                case 3:
                    tipoPrazo.Eh_Administrativo = true;
                    break;
                case 4:
                    tipoPrazo.Eh_Tributario_Administrativo = true;
                    break;
                case 5:
                    tipoPrazo.Eh_Tributario_Judicial = true;
                    break;
                case 7:
                    tipoPrazo.Eh_Juizado_Especial = true;
                    break;
                case 9:
                    tipoPrazo.Eh_Civel_Estrategico = true;
                    break;
                case 14:
                    tipoPrazo.Eh_Criminal_Administrativo = true;
                    break;
                case 15:
                    tipoPrazo.Eh_Criminal_Judicial = true;
                    break;
                case 17:
                    tipoPrazo.Eh_Procon = true;
                    break;
                case 18.1:
                    tipoPrazo.Eh_Pex_Consumidor = true;
                    break;
                case 18.2:
                    tipoPrazo.Eh_Pex_Juizado = true;
                    break;
            }


            tipoPrazo.Validate();
            return tipoPrazo;
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; }
        public TipoProcessoManutencao TipoProcesso
        {
            get
            {
                if (Eh_Civel_Consumidor.HasValue && (bool)Eh_Civel_Consumidor)
                {
                    return TipoProcessoManutencao.CIVEL_CONSUMIDOR;
                }

                if (Eh_Trabalhista.HasValue && (bool)Eh_Trabalhista)
                {
                    return TipoProcessoManutencao.TRABALHISTA;
                }

                if (Eh_Tributario_Administrativo.HasValue && (bool)Eh_Tributario_Administrativo)
                {
                    return TipoProcessoManutencao.TRIBUTARIO_ADMINISTRATIVO;
                }

                if (Eh_Tributario_Judicial.HasValue && (bool)Eh_Tributario_Judicial)
                {
                    return TipoProcessoManutencao.TRIBUTARIO_JUDICIAL;
                }

                if (Eh_Juizado_Especial.HasValue && (bool)Eh_Juizado_Especial)
                {
                    return TipoProcessoManutencao.JEC;
                }

                if (Eh_Civel_Estrategico)
                {
                    return TipoProcessoManutencao.CIVEL_ESTRATEGICO;
                }

                if (Eh_Administrativo)
                {
                    return TipoProcessoManutencao.ADMINISTRATIVO;
                }

                if (Eh_Criminal_Judicial)
                {
                    return TipoProcessoManutencao.CRIMINAL_JUDICIAL;
                }

                if (Eh_Procon)
                {
                    return TipoProcessoManutencao.PROCON;
                }

                if (Eh_Pex_Juizado)
                {
                    return TipoProcessoManutencao.PEX_JUIZADO;
                }

                if (Eh_Pex_Consumidor)
                {
                    return TipoProcessoManutencao.PEX_CONSUMIDOR;
                }

                if (Eh_Criminal_Administrativo)
                {
                    return TipoProcessoManutencao.CRIMINAL_ADMINISTRATIVO;
                }

                return TipoProcessoManutencao.NAO_DEFINIDO;
            }
        }

        public bool? Eh_Civel_Consumidor { get; private set; }
        public bool? Eh_Trabalhista { get; private set; }
        public bool? Eh_Tributario_Administrativo { get; private set; }
        public bool? Eh_Tributario_Judicial { get; private set; }
        public bool? Eh_Juizado_Especial { get; private set; }
        public bool Eh_Civel_Estrategico { get; private set; }
        public bool Eh_Administrativo { get; private set; }
        public bool Eh_Criminal_Judicial { get; private set; }
        public bool Eh_Procon { get; private set; }
        public bool Eh_Pex_Juizado { get; private set; }
        public bool Eh_Pex_Consumidor { get; private set; }
        public bool Eh_Criminal_Administrativo { get; private set; }
        public bool Eh_Documento { get; private set; }
        public bool? Eh_Servico { get; private set; }
        //public bool Eh_Civel_Administrativo { get; private set; }

        public bool Ativo { get; private set; } = true;



        private void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O Campo descrição é obrigatório.");
            }

            if (Descricao.Length > 50)
            {
                AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 50 caracteres.");
            }            
        }

        public void Atualizar(string descricao, bool ativo, bool? ehServico, bool? ehDocumento)
        {
            Descricao = descricao;
            Ativo = ativo;
            Eh_Servico = (bool)ehServico;
            Eh_Documento = (bool)ehDocumento;

            Validate();
        }
    }
}
