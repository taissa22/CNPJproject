using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Fechamento.Entity;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.ApuracaoOutliers.Repository {
    public class BaseFechamentoJecCompletaRepository : BaseCrudRepository<BaseFechamentoJecCompleta, long>, IBaseFechamentoJecCompletaRepository {

        public BaseFechamentoJecCompletaRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user) {}

        public override Task<IDictionary<string, string>> RecuperarDropDown() {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FechamentoJecDisponivelDTO>> CarregarFechamentosDisponiveisParaAgendamento() {
            var lista = context.Set<FechamentosProcessosJEC>()
                 .AsNoTracking()
                 .Select(@base => new FechamentoJecDisponivelDTO() {
                    DataFechamento = @base.DataFechamento,
                    NumeroMeses = @base.NumeroMeses,
                    CodigoEmpresaCentralizadora = @base.CodigoEmpresaCentralizadoraAssociada,
                    MesAnoFechamento = @base.MesAnoFechamento,
                    IndFechamentoMensal = @base.IndicaFechamentoMes
                 })
                 .OrderBy(o => o.DataFechamento);

            return await lista.ToListAsync();
        }

        public async Task<ICollection<ApuracaoOutliersDownloadBaseFechamentoDTO>> ListarBaseFechamento(long codEmpCentralizadora, DateTime mesAnoFechamento, DateTime dataFechamento)
        {
            var resultado = (from baseF in context.Set<BaseFechamentoJecCompleta>()
                                   where baseF.CodigoEmpresaCentralizadora == codEmpCentralizadora && baseF.MesAnoFechamento == mesAnoFechamento && baseF.DataFechamento == dataFechamento
                                   select new ApuracaoOutliersDownloadBaseFechamentoDTO()
                                   {
                                       CodigoInterno = baseF.CodigoProcesso,
                                       NumeroProcesso = baseF.NumeroProcesso,
                                       Estado = baseF.CodigoEstado,
                                       CodigoComarca = baseF.CodigoComarca,
                                       NomeComarca = baseF.NomeComarca,
                                       CodigoVara = baseF.CodigoVara,
                                       CodigoTipoVara = baseF.CodigoTipoVara,
                                       NomeTipoVara = baseF.NomeTipoVara,
                                       CodigoEmpresaGrupo = baseF.CodigoEmpresaDoGrupo,
                                       NomeEmpresaGrupo = baseF.NomeEmpresaGrupo,                                       
                                       DataCadastro = baseF.DataCadastroProcesso,
                                       PrePos = baseF.PrePos,
                                       DataFinalizacaoContabil = baseF.DataFinalizacaoContabil,
                                       ProcInfluenciaContingencia = baseF.ProcInfluenciaContingencia,
                                       CodigoLancamento = baseF.CodigoLancamento,
                                       ValorLancamento = baseF.ValorLancamento ?? 0,
                                       DataRecebimentoFiscal = baseF.DataRecebimentoFiscal ?? (DateTime?)null,
                                       DataPagamento = baseF.DataPagamento ?? (DateTime?)null,
                                       CodigoCategoriaPagamento = baseF.CodigoCategoriaPagamento ?? 0,
                                       DescricaoCategoriaPagamento = baseF.DescricaoCategoriaPagamento,
                                       CatPagInfluenciaContingencia = baseF.CatPagInfluenciaContingencia,
                                       ParametroMediaMovel = baseF.ParametroMediaMovel
                                   });

            var teste = resultado.ToSql();

            return await resultado.ToListAsync();
        }
        public async Task<ICollection<ListaProcessosBaseFechamentoJecDTO>> ListarProcessosResultado(long codEmpCentralizadora, DateTime mesAnoFechamento, DateTime dataFechamento)
        {
            var resultado = await (from baseF in context.Set<BaseFechamentoJecCompleta>()
                                   where baseF.CodigoEmpresaCentralizadora == codEmpCentralizadora && baseF.MesAnoFechamento == mesAnoFechamento && baseF.DataFechamento == dataFechamento
                                   group baseF by baseF.CodigoProcesso into g
                                   select new ListaProcessosBaseFechamentoJecDTO() {
                                       CodigoProcesso = g.Key,
                                       TotalPagamentos = g.Sum(o => o.ValorLancamento ?? 0)
                                   }).ToListAsync();

            return resultado;
        }
    }
}
