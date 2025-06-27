using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Evento : Notifiable, IEntity, INotifiable
    {
        private Evento()
        {
        }

        public Evento(int id, string? nome, bool? possuiDecisao, bool? ehCivel, bool? ehTrabalhista, bool? ehRegulatorio, bool? ehPrazo, bool? ehTributarioAdm, bool? ehTributarioJudicial, bool? finalizacaoEscritorio, bool? tipoMulta, bool? ehTrabalhistaAdm, bool? notificarViaEmail, bool? ehJuizado, bool? reverCalculo, bool atualizaEscritorio, bool ehCivelEstrategico, int? instanciaId, bool preencheMulta, bool alterarExcluir, bool ativo, bool ehCriminalJudicial, bool ehCriminalAdm, bool ehCivelAdm, bool exigeComentario, bool finalizacaoContabil, bool ehProcon, bool ehPexJuizado, bool ehPexCivelConsumidor, int? sequencialDecisao, int? idDescricaoEstrategico, string descricaoEstrategico, bool? ativoEstrategico, int? idDescricaoConsumidor, string descricaoConsumidor, bool? ativoConsumidor)
        {
            Id = id;
            Nome = nome;
            PossuiDecisao = possuiDecisao;
            EhCivel = ehCivel;
            EhTrabalhista = ehTrabalhista;
            EhRegulatorio = ehRegulatorio;
            EhPrazo = ehPrazo;
            EhTributarioAdm = ehTributarioAdm;
            EhTributarioJudicial = ehTributarioJudicial;
            FinalizacaoEscritorio = finalizacaoEscritorio;
            TipoMulta = tipoMulta;
            EhTrabalhistaAdm = ehTrabalhistaAdm;
            NotificarViaEmail = notificarViaEmail;
            EhJuizado = ehJuizado;
            ReverCalculo = reverCalculo;
            AtualizaEscritorio = atualizaEscritorio;
            EhCivelEstrategico = ehCivelEstrategico;
            InstanciaId = instanciaId;
            PreencheMulta = preencheMulta;
            AlterarExcluir = alterarExcluir;
            Ativo = ativo;
            EhCriminalJudicial = ehCriminalJudicial;
            EhCriminalAdm = ehCriminalAdm;
            EhCivelAdm = ehCivelAdm;
            ExigeComentario = exigeComentario;
            FinalizacaoContabil = finalizacaoContabil;
            EhProcon = ehProcon;
            EhPexJuizado = ehPexJuizado;
            EhPexCivelConsumidor = ehPexCivelConsumidor;
            SequencialDecisao = sequencialDecisao;
            IdDescricaoEstrategico = idDescricaoEstrategico;
            DescricaoEstrategico = descricaoEstrategico;
            AtivoEstrategico = ativoEstrategico;
            IdDescricaoConsumidor = idDescricaoConsumidor;
            DescricaoConsumidor = descricaoConsumidor;
            AtivoConsumidor = ativoConsumidor;
        }

        public static Evento Criar(string Nome,
                                   bool PossuiDecisao,
                                   bool NotificarViaEmail,
                                   bool EhPrazo,
                                   bool Ativo,
                                   bool EhTrabalhistaAdm,
                                   bool EhCivel,
                                   bool EhCivelEstrategico,
                                   bool EhRegulatorio,
                                   int? InstanciaId,
                                   bool PreencheMulta)
        {
            Evento evento = new Evento();
            evento.Nome = Nome;
            evento.PossuiDecisao = PossuiDecisao;
            evento.NotificarViaEmail = NotificarViaEmail;
            evento.EhPrazo = EhPrazo;
            evento.Ativo = Ativo;
            evento.EhTrabalhistaAdm = EhTrabalhistaAdm;
            evento.EhCivel = EhCivel;
            evento.EhCivelEstrategico = EhCivelEstrategico;
            evento.EhRegulatorio = EhRegulatorio;
            evento.InstanciaId = InstanciaId;
            evento.PreencheMulta = PreencheMulta;
            evento.AlterarExcluir = true;
            evento.Validate();
            return evento;
        }


        public void Atualizar(int Id,
                             string Nome,
                             bool PossuiDecisao,
                             bool? NotificarViaEmail,
                             bool EhPrazo,
                             bool Ativo,
                             bool? EhTrabalhistaAdm,
                             bool? EhCivel,
                             bool EhCivelEstrategico,
                             bool? EhRegulatorio,
                             int? InstanciaId,
                             bool PreencheMulta)
        {
            this.Id = Id;
            this.Nome = Nome;
            this.PossuiDecisao = PossuiDecisao;
            this.NotificarViaEmail = NotificarViaEmail;
            this.EhPrazo = EhPrazo;
            this.Ativo = Ativo;
            this.EhTrabalhistaAdm = EhTrabalhistaAdm;
            this.EhCivel = EhCivel;
            this.EhCivelEstrategico = EhCivelEstrategico;
            this.EhRegulatorio = EhRegulatorio;
            this.InstanciaId = InstanciaId;
            this.PreencheMulta = PreencheMulta;
            Validate();
        }

        public static Evento CriarTrabalhista(string Nome,
                                  bool PossuiDecisao,
                                  bool EhPrazo,
                                  bool? reverCalculo,
                                  bool? finalizacaoEscritorio,
                                  bool finalizacaoContabil,
                                  bool alterarExcluir,
                                  bool ativo)
        {
            Evento evento = new Evento();
            evento.Nome = Nome;
            evento.PossuiDecisao = PossuiDecisao;
            evento.EhPrazo = EhPrazo;
            evento.ReverCalculo = reverCalculo;
            evento.FinalizacaoEscritorio = finalizacaoEscritorio;
            evento.FinalizacaoContabil = finalizacaoContabil;
            evento.AlterarExcluir = alterarExcluir;
            evento.EhTrabalhista = true;
            evento.Ativo = ativo;
            evento.Validate();
            return evento;
        }

        public static Evento CriarTributario(string Nome,
                                  bool PossuiDecisao,
                                  bool EhPrazo,
                                  bool atualizaEscritorio,
                                  int tipoProcesso, 
                                  bool ativo)
        {
            Evento evento = new Evento();
            evento.Nome = Nome;
            evento.PossuiDecisao = PossuiDecisao;
            evento.EhPrazo = EhPrazo;
            evento.AtualizaEscritorio = atualizaEscritorio;
            evento.Ativo = ativo;
            evento.EhTributarioAdm = tipoProcesso == 4;
            evento.EhTributarioJudicial = tipoProcesso == 5;
            evento.AlterarExcluir = true;
            evento.Ativo = ativo;

            evento.Validate();
            return evento;
        }

        public void AtualizarTributario(int id,
                                        string Nome,
                                        bool PossuiDecisao,
                                        bool EhPrazo,
                                        bool atualizaEscritorio,
                                        int tipoProcesso,
                                        bool ativo)
        {
            this.Id = id;
            this.Nome = Nome;
            this.PossuiDecisao = PossuiDecisao;
            this.EhPrazo = EhPrazo;
            this.AtualizaEscritorio = atualizaEscritorio;
            this.Ativo = ativo;
            this.EhTributarioAdm = tipoProcesso == 4;
            this.EhTributarioJudicial = tipoProcesso == 5;
            Validate();           
        }

        public void AtualizarTrabalhista(int Id,
                                  string nome,
                                  bool possuiDecisao,
                                  bool ehPrazo,
                                  bool? reverCalculo,
                                  bool? finalizacaoEscritorio,
                                  bool finalizacaoContabil,
                                  bool alterarExcluir)
        {
            this.Id = Id;
            this.Nome = nome;
            this.PossuiDecisao = possuiDecisao;
            this.EhPrazo = ehPrazo;
            this.ReverCalculo = reverCalculo;
            this.FinalizacaoEscritorio = finalizacaoEscritorio;
            this.FinalizacaoContabil = finalizacaoContabil;
            this.AlterarExcluir = alterarExcluir;
            this.EhTrabalhista = true;
            this.Ativo = true;
            Validate();
        }

        public void Validate()
        {
        }

        public void AtualizarSequencialDecisao(int sequencial)
        {
            this.SequencialDecisao = sequencial;
        }

        public int Id { get; private set; }
        public string? Nome { get; private set; }
        public bool? PossuiDecisao { get; private set; }
        public bool? EhCivel { get; private set; }
        public bool? EhTrabalhista { get; private set; }
        public bool? EhRegulatorio { get; private set; }
        public bool? EhPrazo { get; private set; }
        public bool? EhTributarioAdm { get; private set; }
        public bool? EhTributarioJudicial { get; private set; }
        public bool? FinalizacaoEscritorio { get; private set; }
        public bool? TipoMulta { get; private set; }
        public bool? EhTrabalhistaAdm { get; private set; }
        public bool? NotificarViaEmail { get; private set; }
        public bool? EhJuizado { get; private set; }
        public bool? ReverCalculo { get; private set; }
        public bool AtualizaEscritorio { get; private set; }
        public bool EhCivelEstrategico { get; private set; }
        public int? InstanciaId { get; private set; }
        public bool PreencheMulta { get; private set; }
        public bool AlterarExcluir { get; private set; }
        public bool Ativo { get; private set; }
        public bool EhCriminalJudicial { get; private set; }
        public bool EhCriminalAdm { get; private set; }
        public bool EhCivelAdm { get; private set; }
        public bool ExigeComentario { get; private set; }
        public bool FinalizacaoContabil { get; private set; }
        public bool EhProcon { get; private set; }
        public bool EhPexJuizado { get; private set; }
        public bool EhPexCivelConsumidor { get; private set; }
        public int? SequencialDecisao { get; private set; }
        public int? IdDescricaoEstrategico { get; set; }
        public string DescricaoEstrategico { get; set; }
        public bool? AtivoEstrategico { get; set; }
        public int? IdDescricaoConsumidor { get; set; }
        public string DescricaoConsumidor { get; set; }
        public bool? AtivoConsumidor { get; set; }
    }
}