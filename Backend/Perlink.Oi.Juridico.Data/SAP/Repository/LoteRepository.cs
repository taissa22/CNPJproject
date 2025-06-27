
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.Compartilhado.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class LoteRepository : BaseCrudRepository<Lote, long>, ILoteRepository
    {
        private readonly JuridicoContext dbContext;
        private readonly LoteLancamentoRepository loteLancamentoRepository;
        private readonly SequencialRepository sequencialRepository;

        public LoteRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
            loteLancamentoRepository = new LoteLancamentoRepository(dbContext, user);
            sequencialRepository = new SequencialRepository(dbContext, user);
        }

        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            return await dbContext.Set<Lote>()
                    .AsNoTracking()
                    .OrderBy(x => x.DataCriacao)
                    .ToDictionaryAsync(x => x.Id.ToString(),
                                       x => x.NumeroLoteBB.ToString());
        }

        public async Task<string> RecuperarPorCodigoSAP(long CodigoSap)
        {
            var Lote = await dbContext.Lotes
              .Where(filtro => filtro.NumeroPedidoSAP.Equals(CodigoSap))
              .AsNoTracking()
              .Select(p => p.NumeroPedidoSAP)
              .FirstOrDefaultAsync();
            if (Lote.HasValue)
                return Lote.Value.ToString();
            else
                return "";
        }

        public async Task<ICollection<Lote>> RecuperarPorParteCodigoSAP(long CodigoSapParte)
        {
            return await dbContext.Lotes
              .Where(filtro => filtro.NumeroPedidoSAP.ToString().Contains(CodigoSapParte.ToString()))
              .AsNoTracking()
              .ToListAsync();
        }

        public async Task<IEnumerable<Lote>> RecuperarPorDataCriacaoLote(DateTime DataCriacaoMin, DateTime DataCriacaoMax)
        {
            return await dbContext.Lotes
                                    .Where(lote => lote.DataCriacao >= DataCriacaoMin)
                                    .Where(lote => lote.DataCriacao <= DataCriacaoMax)
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<IEnumerable<Lote>> RecuperarPorDataCriacaoPedidoLote(DateTime DataCriacaoPedidoMin, DateTime DataCriacaoPedidoMax)
        {
            var Lote = await dbContext.Lotes
                                    .Where(lote => lote.DataCriacaoPedido >= DataCriacaoPedidoMin)
                                    .Where(lote => lote.DataCriacaoPedido <= DataCriacaoPedidoMax)
                                    .AsNoTracking()
                                    .ToListAsync();

            return Lote;
        }

        public async Task<string> RecuperarPorNumeroSAPCodigoTipoProcesso(long NumeroPedidoSAP, long CodigoTipoProcesso)
        {
            var retorno = await dbContext.Lotes
                                    .Where(lote => lote.NumeroPedidoSAP.Equals(NumeroPedidoSAP) &&
                                                   lote.CodigoTipoProcesso.Equals(CodigoTipoProcesso))
                                    .AsNoTracking()
                                    .Select(p => p.NumeroPedidoSAP)
                                    .FirstOrDefaultAsync();

            if (retorno.HasValue)
                return retorno.Value.ToString();
            else
                return "";
        }

        public async Task<LoteDetalhesDTO> RecuperarDetalhes(long codigoLote)
        {

            var retorno = await dbContext.Lotes
                                   .Include(p => p.Parte)
                                   .Include(p => p.CodigoCentroCusto)
                                   .Include(p => p.Fornecedor)
                                   .Include(p => p.LotesLancamento)
                                   .ThenInclude(p => p.LancamentoProcesso)
                                   .Where(p => p.Id == codigoLote)
                                    .Select(lote => new LoteDetalhesDTO()
                                    {
                                        Id = lote.Id,
                                        NomeEmpresaGrupo = lote.Parte.Nome,
                                        Fornecedor = lote.Fornecedor.NomeFornecedor,
                                        FormaPagamento = lote.FormaPagamento.DescricaoFormaPagamento,
                                        CentroCusto = lote.CentroCusto.Descricao,
                                        NumeroLoteBB = lote.NumeroLoteBB,
                                        Valor = lote.Valor,
                                        DataRetornoBB = lote.DataRetornoBB.HasValue ? lote.DataRetornoBB.Value.ToString("dd/MM/yyyy") : "",
                                        DataGeracaoArquivoBB = lote.DataGeracaoArquivoBB.HasValue ? lote.DataGeracaoArquivoBB.Value.ToString("dd/MM/yyyy") : "",
                                        DataEnvioEscritorio = string.Empty,
                                        exibirLoteBB = !lote.DataRetornoBB.HasValue && lote.DataGeracaoArquivoBB.HasValue
                                    }).FirstOrDefaultAsync();
                                        
            retorno.QuantLancamento = ObterQuantidadeLancamentoDoLote(retorno.Id);            

            return retorno;
        }

        public async Task<long?> AtualizarNumeroLoteBBAsync(long codigolote)
        {
            var retorno = await dbContext.Lotes
                .Where(p => p.Id == codigolote).FirstOrDefaultAsync();
            await sequencialRepository.AtualizarSequence("LOTE_BB");
            retorno.NumeroLoteBB = await sequencialRepository.RecuperarSequencialAsync("LOTE_BB");
            await base.Atualizar(retorno);
            base.Commit();
            return retorno.NumeroLoteBB;
        }

        private long ObterQuantidadeLancamentoDoLote(long CodigoLote)
        {
            return loteLancamentoRepository.ObterTotalLancamentosDoLote(CodigoLote).Result;
        }

        public async Task<Lote> CriacaoLote(Lote lote)
        {
            await base.Inserir(lote);

            return lote;
        }

        public async Task<bool> ExisteLoteComFornecedor(long codigoFornecedor)
        {
            var resultado = await dbContext.Lotes
                .AnyAsync(l => l.CodigoFornecedor == codigoFornecedor);

            return resultado;
        }

        public async Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto)
        {
            var result = await dbContext.Lotes.AsNoTracking()
               .AnyAsync(l => l.CodigoCentroCusto == codigoCentroCusto);
            return result;
        }
        public async Task<bool> ExisteLoteComFormaPagamento(long codigoFormaPagamento)
        {
            var resultado = await dbContext.Lotes.AnyAsync(lote => lote.CodigoFormaPagamento.Equals(codigoFormaPagamento));

            return resultado;
        }

        public async Task<long> ValidarNumeroLote(long numeroLote, long codigoTipoProcesso) {
            return await dbContext.Lotes.AnyAsync(lote => lote.Id.Equals(numeroLote) && lote.CodigoTipoProcesso == codigoTipoProcesso) ? numeroLote : 0;
            
        }
    }
}