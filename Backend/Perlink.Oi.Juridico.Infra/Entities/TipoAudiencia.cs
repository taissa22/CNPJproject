using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{

    public sealed class TipoAudiencia : Notifiable, IEntity, INotifiable {

        private TipoAudiencia() {
        }

#nullable enable
        //public TipoAudiencia(int id, string descricao) {
        //    Id = id;
        //    Descricao = descricao;
        //}

        public static TipoAudiencia Criar(string descricao, bool ativo, TipoProcesso tipoProcesso, string? sigla,bool linkVirtual)
        {
            var tipoAudiencia = new TipoAudiencia();

            tipoAudiencia.Descricao = descricao;
            tipoAudiencia.Ativo = ativo;
            tipoAudiencia.Sigla = sigla;
            tipoAudiencia.LinkVirtual = linkVirtual;

            switch (tipoProcesso.Id)
            {
                case 1:
                    tipoAudiencia.Eh_CivelConsumidor = true;
                    break;
                case 2:
                    tipoAudiencia.Eh_Trabalhista = true;
                    break;
                case 3:
                    tipoAudiencia.Eh_Administrativo = true;
                    break;
                case 4:
                    tipoAudiencia.Eh_TributarioAdministrativo = true;
                    break;
                case 5:
                    tipoAudiencia.Eh_TributarioJudicial = true;
                    break;
                case 7:
                    tipoAudiencia.Eh_JuizadoEspecial = true;
                    break;
                case 9:
                    tipoAudiencia.Eh_CivelEstrategico = true;
                    break;
                case 14:
                    tipoAudiencia.Eh_CriminalAdministrativo = true;
                    break;
                case 15:
                    tipoAudiencia.Eh_CriminalJudicial = true;
                    break;
                case 17:
                    tipoAudiencia.Eh_Procon = true;
                    break;
                case 18:
                    tipoAudiencia.Eh_Pex = true;
                    break;
            }


            tipoAudiencia.Validate();
            return tipoAudiencia;
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; }

        public string? Sigla { get; private set; }

        public bool Ativo { get; private set; } = true;

        public TipoProcesso TipoProcesso
        {
            get
            {
                if (Eh_CivelConsumidor)
                {
                    return TipoProcesso.CIVEL_CONSUMIDOR;
                }

                if (Eh_Trabalhista)
                {
                    return TipoProcesso.TRABALHISTA;
                }

                if (Eh_TrabalhistaAdministrativo)
                {
                    return TipoProcesso.TRABALHISTA_ADMINISTRATIVO;
                }

                if (Eh_TributarioAdministrativo)
                {
                    return TipoProcesso.TRIBUTARIO_ADMINISTRATIVO;
                }

                if (Eh_TributarioJudicial)
                {
                    return TipoProcesso.TRIBUTARIO_JUDICIAL;
                }

                if (Eh_JuizadoEspecial)
                {
                    return TipoProcesso.JEC;
                }

                if (Eh_CivelEstrategico)
                {
                    return TipoProcesso.CIVEL_ESTRATEGICO;
                }

                if (Eh_Administrativo)
                {
                    return TipoProcesso.ADMINISTRATIVO;
                }

                if (Eh_CivelAdministrativo)
                {
                    return TipoProcesso.CIVEL_ADMINISTRATIVO;
                }

                if (Eh_CriminalJudicial)
                {
                    return TipoProcesso.CRIMINAL_JUDICIAL;
                }

                if (Eh_Procon)
                {
                    return TipoProcesso.PROCON;
                }

                if (Eh_Pex)
                {
                    return TipoProcesso.PEX;
                }

                if (Eh_CriminalAdministrativo)
                {
                    return TipoProcesso.CRIMINAL_ADMINISTRATIVO;
                }

                return TipoProcesso.NAO_DEFINIDO;
            }
        }

        public bool Eh_CivelConsumidor { get; private set; }
        public bool Eh_Trabalhista { get; private set; }
        public bool Eh_TrabalhistaAdministrativo { get; private set; }
        public bool Eh_TributarioAdministrativo { get; private set; }
        public bool Eh_TributarioJudicial { get; private set; }
        public bool Eh_JuizadoEspecial { get; private set; }
        public bool Eh_CivelEstrategico { get; private set; }
        public bool Eh_Administrativo { get; private set; }
        public bool Eh_CriminalJudicial { get; private set; }
        public bool Eh_Procon { get; private set; }
        public bool Eh_Pex { get; private set; }
        public bool Eh_CriminalAdministrativo { get; private set; }
        public bool Eh_CivelAdministrativo { get; private set; }

        public bool LinkVirtual { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O Campo descrição é obrigatório.");
            }

            if (Descricao.Length > 100)
            {
                AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 100 caracteres.");
            }
        }

        public void Atualizar(string descricao, bool ativo, string? sigla, bool linkVirtual)
        {
            Descricao = descricao;
            Ativo = ativo;
            Sigla = sigla;
            LinkVirtual = linkVirtual;
            Validate();
        }
    }
}