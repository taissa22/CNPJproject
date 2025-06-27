using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using TipoProcessoEnum = Perlink.Oi.Juridico.Infra.Enums.TipoProcesso;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Processo : Notifiable, IEntity, INotifiable
    {
        private Processo()
        {
        }

        public int Id { get; private set; }

        public string NumeroProcesso { get; private set; }

        public int TipoProcessoId { get; private set; }

        public TipoProcessoEnum TipoProcesso() => TipoProcessoEnum.PorId(TipoProcessoId);

        internal int? EmpresaDoGrupoId { get; private set; }
        public EmpresaDoGrupo EmpresaDoGrupo { get; private set; }

        public IReadOnlyCollection<Lancamento> Lancamentos { get; private set; }

        internal int? AssuntoId { get; private set; }
        public Assunto Assunto { get; private set; }

        internal int? AcaoId { get; private set; }
        public Acao Acao { get; private set; }

        public int? AdvogadoId { get; private set; }

        public int? EscritorioId { get; private set; }
        public Escritorio Escritorio { get; private set; }

        internal int? EstabelecimentoId { get; private set; }
        public Estabelecimento Estabelecimento { get; private set; }

        public int? OrgaoId { get; private set; }
        public Orgao Orgao { get; private set; }

        public int? EscritorioAcompanhanteId { get; private set; }
        public Escritorio EscritorioAcompanhante { get; private set; }

        public string EstadoId { get; private set; }
        public EstadoEnum Estado => EstadoEnum.PorId(EstadoId);

        public int? MunicipioId { get; set; }

        public int ComarcaId { get; private set; }
        public Comarca Comarca { get; private set; }

        public int VaraId { get; private set; }
        public Vara Vara { get; private set; }

        public int TipoVaraId { get; private set; }
        public TipoVara TipoVara { get; private set; }

        public int? ContadorId { get; private set; }

        public DateTime? DataDistribuicao { get; private set; }

        public int? ProcedimentoId { get; private set; }

        public Procedimento Procedimento { get; private set; }

        private readonly HashSet<PedidoProcesso> pedidosDoProcessoSet = new HashSet<PedidoProcesso>();

        public IReadOnlyCollection<PedidoProcesso> PedidosDoProcesso => pedidosDoProcessoSet;

        private readonly HashSet<ParteProcesso> partesDoProcessoSet = new HashSet<ParteProcesso>();

        public IReadOnlyCollection<ParteProcesso> PartesDoProcesso => partesDoProcessoSet;

        private readonly HashSet<PendenciaProcesso> PendenciasDoProcessoSet = new HashSet<PendenciaProcesso>();

        public IReadOnlyCollection<PendenciaProcesso> PendenciasDoProcesso => PendenciasDoProcessoSet;
        
        private readonly HashSet<PrazoProcesso> prazosDoProcessoSet = new HashSet<PrazoProcesso>();

        public IReadOnlyCollection<PrazoProcesso> PrazosDoProcesso => prazosDoProcessoSet;

        public IReadOnlyCollection<Despesa> Despesas { get; private set; }

        internal int? RegionalId { get; private set; }
        public Regional Regional { get; private set; }

        public string ClassificacaoProcessoId { get; set; }
        public int? Closing { get; set; }

        public ClassificacaoProcesso ClassificacaoProcesso => ClassificacaoProcesso.PorValor(ClassificacaoProcessoId);
        public int? ComplementoDeAreaEnvolvidaId { get; private set; }
        
        public int? EsferaId { get; private set; }

        public int? FatoGeradorId { get; private set; }

        public void AtualizarRegional(Regional regional)
        {
            RegionalId = regional.Id;
            Regional = regional;
        }

        public int? ClosingClientCo { get; set; } 
    }
}