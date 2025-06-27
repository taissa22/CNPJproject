using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using Perlink.Oi.Juridico.Infra.ValueObjects;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Pedido : Notifiable, IEntity, INotifiable
    {
        private Pedido()
        {
        }

        public static Pedido CriarTrabalhista(DataString descricao, RiscoPerda riscoPerda, bool ativo, bool provavelZero, ProprioTerceiro proprioTerceiro)
        {
            Pedido pedido = new Pedido()
            {
                Descricao = descricao,
                RiscoPerdaId = riscoPerda.Id,
                ProprioTerceiroId = proprioTerceiro.Id,
                ProvavelZero = provavelZero,
                Ativo = ativo,
                EhTrabalhista = true
            };

            pedido.Validate();
            return pedido;
        }

        public void AtualizarPedidoDoTrabalhista(DataString descricao, RiscoPerda riscoPerda, bool ativo, bool provavelZero, ProprioTerceiro proprioTerceiro)
        {
            Descricao = descricao;
            RiscoPerdaId = riscoPerda.Id;
            ProprioTerceiroId = proprioTerceiro.Id;
            ProvavelZero = provavelZero;
            Ativo = ativo;

            Validate();
        }

        public static Pedido CriarCivelEstrategico(DataString descricao, bool ativo)
        {
            Pedido pedido = new Pedido()
            {
                Descricao = descricao,
                Ativo = ativo,
                EhCivelEstrategico = true
            };

            pedido.Validate();
            return pedido;
        }

        public void AtualizarPedidoDoCivelEstrategico(DataString descricao, bool ativo)
        {
            Descricao = descricao;
            Ativo = ativo;

            Validate();
        }

        public static Pedido CriarCivelConsumidor(DataString descricao, bool ativo, bool audiencia)
        {
            Pedido pedido = new Pedido()
            {
                Descricao = descricao,
                Audiencia = audiencia,
                Ativo = ativo,
                EhCivel = true
            };

            pedido.Validate();
            return pedido;
        }

        public void AtualizarPedidoDoCivelConsumidor(DataString descricao, bool ativo, bool audiencia)
        {
            Descricao = descricao;
            Audiencia = audiencia;
            Ativo = ativo;

            Validate();
        }

        public static Pedido CriarTrabalhistaAdministrativo(string descricao)
        {
            Pedido pedido = new Pedido()
            {
                Descricao = descricao,
                EhTrabalhistaAdministrativo = true
            };
            pedido.Validate();
            return pedido;
        }

        public void AtualizarPedidoDoTrabalhistaAdministrativo(string descricao)
        {
            Descricao = descricao;           
            EhTrabalhistaAdministrativo = true;            
            Validate();
        }
        public static Pedido CriarPedidoDoTributario(string descricao, bool ativoTributarioJudicial, bool ativoTributarioAdminstrativo,
                                                                                 bool ehTributarioJudicial, bool ehTributarioAdministrativo, int grupoId)
        {
            Pedido pedido = new Pedido()
            {
                Descricao = descricao,
                EhTributarioAdministrativo = ehTributarioAdministrativo,
                EhTributarioJudicial = ehTributarioJudicial,
                AtivoTributarioAdministrativo = ativoTributarioAdminstrativo,
                AtivoTributarioJudicial = ativoTributarioJudicial,
                EhTrabalhistaAdministrativo = false,
                GrupoPedidoId = grupoId
            };
            pedido.Validate();
            return pedido;
        }

        public void AtualizarPedidoDoTributario(string descricao, bool ativoTributarioJudicial, bool ativoTributarioAdminstrativo,
                                                                                     bool ehTributarioJudicial, bool ehTributarioAdministrativo, int? grupoId)
        {
            Descricao = descricao;
            EhTributarioAdministrativo = ehTributarioAdministrativo;
            EhTributarioJudicial = ehTributarioJudicial;
            AtivoTributarioAdministrativo = ativoTributarioAdminstrativo;
            AtivoTributarioJudicial = ativoTributarioJudicial;
            GrupoPedidoId = grupoId;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "O Campo descrição é obrigatório.");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(50))
            {
                AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 50 caracteres.");
            }
        }

        public int Id { get; private set; }

        public string Descricao { get; private set; }
        public bool Audiencia { get; private set; }

        public bool Ativo { get; private set; }

        public bool EhCivelEstrategico { get; private set; }
        //public bool EhCivelConsumidor { get; private set; }

        public bool EhTrabalhista { get; private set; }

        public string RiscoPerdaId { get; private set; }

        public RiscoPerda RiscoPerda => Enums.RiscoPerda.PorId(RiscoPerdaId);
        public bool ProvavelZero { get; private set; }

        public string ProprioTerceiroId { get; private set; }
        public ProprioTerceiro ProprioTerceiro => Enums.ProprioTerceiro.PorId(ProprioTerceiroId);

        public bool EhTributarioAdministrativo { get; private set; }
        public bool EhTributarioJudicial { get; private set; }
        public bool AtivoTributarioAdministrativo { get; private set; }
        public bool AtivoTributarioJudicial { get; private set; }
        public bool EhTrabalhistaAdministrativo { get; private set; }
        public int? GrupoPedidoId { get; private set; }
        public GrupoPedido GrupoPedido { get; private set; }
        public bool EhCivel { get; set; }

        public TipoProcesso TipoProcesso
        {
            get
            {
                if (EhTrabalhistaAdministrativo)
                {
                    return TipoProcesso.TRABALHISTA_ADMINISTRATIVO;
                }

                if (EhTributarioJudicial)
                {
                    return TipoProcesso.TRIBUTARIO_ADMINISTRATIVO;
                }

                if (EhTributarioAdministrativo)
                {
                    return TipoProcesso.TRIBUTARIO_ADMINISTRATIVO;
                }

                return TipoProcesso.NAO_DEFINIDO;
            }
        }

    }
}