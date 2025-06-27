using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.Compartilhado.Repository;
using Perlink.Oi.Juridico.Data.Logs.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class LoteLancamentoRepository : BaseCrudRepository<LoteLancamento, long>, ILoteLancamentoRepository
    {
        private readonly JuridicoContext Context;
        private readonly LogLoteProcessoRepository logLoteProcessoRepository;
        private readonly borderoRepository borderoRepository;
        private readonly LancamentoProcessoRepository lancamentoProcessoRepository;
        public double TotalValorLotes { get; set; }
        public long QuantidadesLancamentos { get; set; }
        public LoteLancamentoRepository(JuridicoContext Context, IAuthenticatedUser user) : base(Context, user)
        {
            this.Context = Context;

            logLoteProcessoRepository = new LogLoteProcessoRepository(Context, user);
            borderoRepository = new borderoRepository(Context, user);
            lancamentoProcessoRepository = new LancamentoProcessoRepository(Context, user);
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }
        public async Task<long> ObterTotalLancamentosDoLote(long CodigoLote)
        {
            var result = await context.Set<LoteLancamento>()
              .Where(filtro => filtro.Id.Equals(CodigoLote))

              .CountAsync();

            return result;
        }

        private IQueryable<Lote> IQueryableLotesPorAssociacao(LoteFiltroDTO filtros)
        {
            IQueryable<LoteLancamento> query = context.Set<LoteLancamento>();

            if (filtros.IdsNumerosGuia?.Count() > 0)
                query = query.Where(l => filtros.IdsNumerosGuia.Contains(l.LancamentoProcesso.NumeroGuia.Value));
            if (filtros.IdsProcessos?.Count() > 0)
                query = query.Where(l => filtros.IdsProcessos.Contains(l.LancamentoProcesso.Processo.Id));
            if (filtros.IdsTipoLancamentos?.Count() > 0)
                query = query.Where(p => filtros.IdsTipoLancamentos.Contains(p.LancamentoProcesso.CodigoTipoLancamento));
            if (filtros.IdsCategoriasPagamentos?.Count() > 0)
                query = query.Where(p => filtros.IdsCategoriasPagamentos.Contains(p.LancamentoProcesso.CodigoCatPagamento));
            
            if (filtros.StatusProcesso == StatusDoObjetoEnum.Ativo.GetHashCode())
                query = query.Where(p => p.LancamentoProcesso.Processo.IndicaProcessoAtivo == true && p.LancamentoProcesso.Processo.DataFinalizacao == null);
            else if (filtros.StatusProcesso == StatusDoObjetoEnum.Inativo.GetHashCode())
                query = query.Where(p => p.LancamentoProcesso.Processo.IndicaProcessoAtivo == false && p.LancamentoProcesso.Processo.DataFinalizacao != null);
            //p.LancamentoProcesso.Processo.IndicaProcessoAtivo.Value == filtros.StatusProcesso.Equals(StatusDoObjetoEnum.Inativo.GetHashCode())

            if (filtros.StatusContabil == StatusDoObjetoEnum.Ativo.GetHashCode())
                query = query.Where(p => p.LancamentoProcesso.Processo.DataFinalizacaoContabil == null);
            else if (filtros.StatusContabil == StatusDoObjetoEnum.Inativo.GetHashCode())
                query = query.Where(p => p.LancamentoProcesso.Processo.DataFinalizacaoContabil != null);
            
            if (filtros.NumeroContaJudicial?.Count() > 0)
                query = query.Where(p => filtros.NumeroContaJudicial.Contains(p.LancamentoProcesso.NumeroContaJudicial));
            if (filtros.IdsEscritorios?.Count() > 0)
                query = query.Where(l => filtros.IdsEscritorios.Contains(l.LancamentoProcesso.Processo.CodigoProfissonal));
            if (filtros.IdsNumerosLote?.Count() > 0)
                query = query.Where(p => filtros.IdsNumerosLote.Contains(p.Lote.Id));


            IQueryable<Lote> retorno = query.Select(a => a.Lote).Distinct();

            return retorno;
        }

        private IQueryable<Lote> IQueryableFiltroConsultaLote(LoteFiltroDTO filtros)
        {

            var query = IQueryableLotesPorAssociacao(filtros);

            if (filtros.DataCriacaoMaior.HasValue && filtros.DataCriacaoMenor.HasValue)
                query = query.Where(p => p.DataCriacao >= filtros.DataCriacaoMenor.Value && p.DataCriacao <= filtros.DataCriacaoMaior.Value);

            if (filtros.DataCriacaoPedidoMenor.HasValue && filtros.DataCriacaoPedidoMaior.HasValue)
                query = query.Where(p => p.DataCriacaoPedido >= filtros.DataCriacaoPedidoMenor.Value && p.DataCriacaoPedido <= filtros.DataCriacaoPedidoMaior.Value);

            if (filtros.DataCancelamentoLoteInicio.HasValue && filtros.DataCancelamentoLoteFim.HasValue)
                query = query.Where(p => p.DataCancelamentoLote >= filtros.DataCancelamentoLoteInicio.Value && p.DataCancelamentoLote <= filtros.DataCancelamentoLoteFim.Value);

            if (filtros.DataErroProcessamentoInicio.HasValue && filtros.DataErroProcessamentoFim.HasValue)
                query = query.Where(p => p.DataErro >= filtros.DataErroProcessamentoInicio.Value && p.DataErro <= filtros.DataErroProcessamentoFim.Value);

            if (filtros.DataRecebimentoFiscalInicio.HasValue && filtros.DataRecebimentoFiscalFim.HasValue)
                query = query.Where(p => p.DataRecebimentoFisico >= filtros.DataRecebimentoFiscalInicio.Value && p.DataRecebimentoFisico <= filtros.DataRecebimentoFiscalFim.Value);

            if (filtros.DataPagamentoPedidoInicio.HasValue && filtros.DataPagamentoPedidoFim.HasValue)
                query = query.Where(p => p.DataPagamentoPedido >= filtros.DataPagamentoPedidoInicio.Value && p.DataPagamentoPedido <= filtros.DataPagamentoPedidoFim.Value);

            if (filtros.DataEnvioEscritorioInicio.HasValue && filtros.DataEnvioEscritorioFim.HasValue)
                query = query.Where(p => p.LotesLancamento.Any(ll =>
                            ll.LancamentoProcesso.DataEnvioEscritorio >= filtros.DataEnvioEscritorioInicio.Value &&
                            ll.LancamentoProcesso.DataEnvioEscritorio <= filtros.DataEnvioEscritorioFim.Value)
                );

            if (filtros.ValorTotalLoteInicio.HasValue && filtros.ValorTotalLoteFim.HasValue)
                query = query.Where(p => p.Valor >= Convert.ToDouble(filtros.ValorTotalLoteInicio.Value) &&  p.Valor <= Convert.ToDouble(filtros.ValorTotalLoteFim.Value));

            if (filtros.IdsPedidosSAP?.Count() > 0)
                query = query.Where(p => filtros.IdsPedidosSAP.Contains(p.NumeroPedidoSAP));

            if (filtros.IdsEmpresasGrupo?.Count() > 0)
                query = query.Where(p => filtros.IdsEmpresasGrupo.Contains(p.CodigoParte));

            if (filtros.IdsFornecedores?.Count() > 0)
                query = TratarListaComMaisDeMilRegistrosOracle(filtros.IdsFornecedores, query); //query.Where(p => filtros.IdsFornecedores.Contains(p.CodigoFornecedor));

            if (filtros.IdsCentroCustos?.Count() > 0)
                query = query.Where(p => filtros.IdsCentroCustos.Contains(p.CodigoCentroCusto));

            if (filtros.IdsStatusPagamentos?.Count() > 0)
                query = query.Where(p => filtros.IdsStatusPagamentos.Contains(p.CodigoStatusPagamento));

            if (filtros.tipoProcesso > 0)
                query = query.Where(p => p.CodigoTipoProcesso == filtros.tipoProcesso);

            query = query.OrderBy(o => o.Id);

            if (filtros.Pagina >= 1 && filtros.Quantidade > 0)
            {
                query = query
                .Skip((filtros.Pagina - 1) * filtros.Quantidade)
                .Take(filtros.Quantidade);
            }
            return query;
        }

        private IQueryable<Lote> TratarListaComMaisDeMilRegistrosOracle(IEnumerable<long?> lista, IQueryable<Lote> query)
        {
            query = from lotes in query
                         where lista.Skip(0).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(1000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(2000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(3000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(4000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(5000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(5000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(6000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(7000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(8000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(9000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(10000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(20000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(30000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(40000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(50000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(60000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(70000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(80000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(90000).Take(1000).Contains(lotes.CodigoFornecedor) ||
                            lista.Skip(20000).Take(1000).Contains(lotes.CodigoFornecedor)
                         select lotes;
            return query;
        }

        public async Task<TotaisLoteResultadoDTO> TotalFiltroConsultaLote(LoteFiltroDTO filtros)
        {
            IQueryable<Lote> query = IQueryableFiltroConsultaLote(filtros);
            TotaisLoteResultadoDTO totaisLoteResultadoDTO = new TotaisLoteResultadoDTO();

            var result = await query.Where(l => l.LotesLancamento.Any()).Select(lote => new LoteResultadoDTO()
            {
                Id = lote.Id,
                ValorLote = lote.Valor
            }).ToListAsync();

            var listacodigoLote = result.Select(p => p.Id).ToList();
            totaisLoteResultadoDTO.TotalValorLotes = result.Sum(l => l.ValorLote);
            totaisLoteResultadoDTO.QuantidadesLancamentos = await lancamentoProcessoRepository.ObterTotalLancamentosDoLote(listacodigoLote);
            totaisLoteResultadoDTO.Total = result.Count();
            totaisLoteResultadoDTO.TotalLotes = result.Count();
            return totaisLoteResultadoDTO;
        }

        public async Task<IEnumerable<LoteResultadoDTO>> FiltroConsultaLote(LoteFiltroDTO filtros)
        {

            IQueryable<Lote> query = IQueryableFiltroConsultaLote(filtros);

            var result = await query.Where(l => l.LotesLancamento.Any()).Select(lote => new LoteResultadoDTO()
            {
                Id = lote.Id,
                FormaPagamento = lote.FormaPagamento.DescricaoFormaPagamento,
                NomeUsuario = lote.Usuario.Nome,
                DescricaoLote = lote.DescricaoLote,
                NumeroLoteBB = lote.NumeroLoteBB,
                CodigoStatusPagamento = lote.CodigoStatusPagamento,
                DataCriacao = lote.DataCriacao.HasValue ? lote.DataCriacao.Value.ToString("dd/MM/yyyy HH:mm:ss") : "",
                StatusPagamento = lote.StatusPagamento.Descricao,
                NumeroPedidoSAP = lote.NumeroPedidoSAP,
                ExisteBordero = lote.Borderos.Any(),
                CodigoParte = lote.CodigoParte,
                NomeEmpresaGrupo = lote.Parte.Nome,
                DataCriacaoPedido = string.Empty,//data baseada no log por causa do status do pagamento
                ValorLote = lote.Valor
                
            }).OrderBy(o => o.Id).ToListAsync();

            return result.Select(p => PreencheDadosComplementares(p)).ToList();
        }

        private LoteResultadoDTO PreencheDadosComplementares(LoteResultadoDTO lote)
        {
            var ultimoLog = logLoteProcessoRepository.RecuperarUltimoLog(lote.Id);
            lote.DataCriacaoPedido = ultimoLog.Result?.DataLog.Value.ToString("dd/MM/yyyy HH:mm:ss");
            return lote;
        }

        public async Task<long> ObterCodigoProcesso(long CodigoLote)
        {
            var result = await context.Set<LoteLancamento>()
              .Where(filtro => filtro.Id.Equals(CodigoLote))
              .Select(p => p.CodigoProcesso).FirstAsync();


            return result;
        }

        public async Task<IEnumerable<LoteExportarDTO>> ExportarConsultaLote(LoteFiltroDTO loteFiltroDTO)
        {
            IQueryable<Lote> query = IQueryableFiltroConsultaLote(loteFiltroDTO);

            var result = await query.Select(lote => new LoteExportarDTO()
            {
                Id = lote.Id,
                DataCriacaoLote = lote.DataCriacao.HasValue ? lote.DataCriacao.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty,
                DescricaoLote = lote.DescricaoLote,
                DescricaoStatusPagamento = lote.StatusPagamento.Descricao,
                DataCriacaoPedido = lote.DataCriacaoPedido.HasValue ? lote.DataCriacaoPedido.Value.ToString("dd/MM/yyyy") : string.Empty,
                NumeroPedido = lote.NumeroPedidoSAP.HasValue ? lote.NumeroPedidoSAP.Value.ToString("G") : string.Empty,
                DescricaoEmpresaGrupo = lote.Parte.Nome,
                DescricaoFornecedor = lote.Fornecedor.NomeFornecedor,
                DescricaoCentroCusto = lote.CentroCusto.Descricao,
                ValorLote = lote.Valor == 0 ? string.Empty : lote.Valor.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR")),
                QuantidadeLancamento = lote.LotesLancamento.Count().ToString(),
                DataGeracaoArquivoBB = lote.DataGeracaoArquivoBB.HasValue ? lote.DataGeracaoArquivoBB.Value.ToString("dd/MM/yyyy") : string.Empty,
                DataRetornoBB = lote.DataRetornoBB.HasValue ? lote.DataRetornoBB.Value.ToString("dd/MM/yyyy") : string.Empty,
                NumeroLoteBB = lote.NumeroLoteBB.HasValue ? lote.NumeroLoteBB.Value.ToString("G") : string.Empty,
                DataErroProcessamento = lote.DataErro.HasValue ? lote.DataErro.Value.ToString("dd/MM/yyyy") : string.Empty
            }).ToListAsync();

            return result;
        }

        public async Task VinculoLancamento(Lote lote, IList<DadosLoteCriacaoLancamentoDTO> dadosLancamentoDTOs)
        {
            foreach (var lancamentoProcessoDTO in dadosLancamentoDTOs)
            {
                LoteLancamento loteLancamento = new LoteLancamento()
                {
                    Id = lote.Id,
                    CodigoProcesso = lancamentoProcessoDTO.CodigoProcesso,
                    CodigoLancamento = lancamentoProcessoDTO.CodigoLancamento
                };

                await base.Inserir(loteLancamento);
            }
        }

        public async Task<LancamentoProcesso> RetornaLancamentoDoLote(long codigoLote)
        {
            return await context.Set<LoteLancamento>()
                .Include(i => i.LancamentoProcesso)
                .Where(i => i.Id == codigoLote)
                .Select(l=>l.LancamentoProcesso)
                .FirstAsync();
        }
        public async Task<List<LancamentoProcesso>> RetornaLancamentosDoLote(long codigoLote)
        {
            return await context.Set<LoteLancamento>()
                .Include(i => i.LancamentoProcesso)
                .Where(i => i.Id == codigoLote)
                .Select(l => l.LancamentoProcesso)
                .ToListAsync();
        }
    }

}
