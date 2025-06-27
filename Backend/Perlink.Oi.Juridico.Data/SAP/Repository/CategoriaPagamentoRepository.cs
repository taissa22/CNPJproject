using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.Compartilhado.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Resultados;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using Shared.Tools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class CategoriaPagamentoRepository : BaseCrudRepository<CategoriaPagamento, long>, ICategoriaPagamentoRepository
    {
        private readonly JuridicoContext dbContext;

        public CategoriaPagamentoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public async Task<CategoriaPagamento> CadastrarCategoria(CategoriaPagamento categoriaPagamento)
        {
            await base.Inserir(categoriaPagamento);
            return categoriaPagamento;
        }

        #region Pesquisar Categoria Pagamento
        public async Task<ICollection<CategoriaDePagamentoResultadoDTO>> BuscarCategoriasPagamento(CategoriaPagamentoFiltroDTO filtros)
        {
            var lista = dbContext.Set<CategoriaPagamento>()
                .WhereIf(filtros.TipoLancamento != null, p => p.CodigoTipoLancamento == filtros.TipoLancamento)
                .WhereIf(filtros.TipoProcesso != null, p => p.IndicadorCivel == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelConsumidor) &&
                                                            p.IndicadorTrabalhista == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Trabalhista) &&
                                                            p.IndicadorAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Administrativo) &&
                                                            p.IndicadorTributarioAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioAdministrativo) &&
                                                            p.IndicadorTributarioJudicial == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioJudicial) &&
                                                            p.IndicadorJuizado == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.JuizadoEspecial) &&
                                                            p.IndicadorCivelEstrategico == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelEstrategico) &&
                                                            p.IndicadorProcon == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Procon) &&
                                                            p.IndicadorPex == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Pex))
                .WhereIf(filtros.Codigo != null, p => p.Id == filtros.Codigo)
                .Select(CategoriaPagamento => new CategoriaDePagamentoResultadoDTO()
                {
                    Codigo = CategoriaPagamento.Id,
                    TipoLancamento = CategoriaPagamento.CodigoTipoLancamento,
                    Descricao = CategoriaPagamento.DescricaoCategoriaPagamento,
                    Descricaoclassegarantia = CategoriaPagamento.ClasseGarantia.Descricao,
                    CodigoMaterialSAP = CategoriaPagamento.CodigoMaterialSap,
                    indAtivo = CategoriaPagamento.IndicadorAtivo,
                    indEnvioSap = CategoriaPagamento.IndicadorEnvioSap,
                    ClgarCodigoClasseGarantia = CategoriaPagamento.ClgarCodigoClasseGarantia,
                    IndicadorNumeroGuia = CategoriaPagamento.IndicadorRequerimentoNumeroGuia,
                    TipoFornecedorPermitido = CategoriaPagamento.TipoFornecedorPermitido,
                    FornecedoresPermitidos = DescricaoFornecedor(CategoriaPagamento.TipoFornecedorPermitido),
                    IndEscritorioSolicitaLan = CategoriaPagamento.IndicadorEscritorioSolicitaLancamento == null ? CategoriaPagamento.IndicadorEscritorioSolicitaLancamento == false : CategoriaPagamento.IndicadorEscritorioSolicitaLancamento.Value,
                    GrpcgIdGrupoCorrecaoGar = CategoriaPagamento.GrpcgIdGrupoCorrecaoGar,
                    GrupoCorrecao = CategoriaPagamento.GrupoCorrecao.Descricao,
                    IndEncerraProcessoContabil = CategoriaPagamento.IndicadorEncerraProcessoContabilmente == null ? CategoriaPagamento.IndicadorEncerraProcessoContabilmente == false : CategoriaPagamento.IndicadorEncerraProcessoContabilmente.Value,
                    IndComprovanteSolicitacao = CategoriaPagamento.IndicadorRequerComprovanteSolicitacao == null ? CategoriaPagamento.IndicadorRequerComprovanteSolicitacao == false : CategoriaPagamento.IndicadorRequerComprovanteSolicitacao.Value,
                    IndicadorRequerDataVencimento = CategoriaPagamento.IndicadorRequerDataVencimento == null ? CategoriaPagamento.IndicadorRequerDataVencimento == false : CategoriaPagamento.IndicadorRequerDataVencimento.Value,
                    IndicadorContingencia = CategoriaPagamento.IndicadorInfluenciaContingencia,
                    IndicadorCivelEstrategico = CategoriaPagamento.IndicadorCivelEstrategico,
                    IndicadorCivelConsumidor = CategoriaPagamento.IndicadorCivel,
                    IndicadorTrabalhista = CategoriaPagamento.IndicadorTrabalhista,
                    IndicadorTributarioJudicial = CategoriaPagamento.IndicadorTributarioJudicial,
                    Ind_TributarioAdministrativo = CategoriaPagamento.IndicadorTributarioAdministrativo,
                    IndicadorBaixaGarantia = CategoriaPagamento.IndicadorBaixaGarantia,
                    indBaixaPagamento = CategoriaPagamento.IndicadorBaixaGarantia,
                    IndicadorBloqueioDeposito = CategoriaPagamento.IndicadorBloqueioDeposito,
                    IndicadorJuizado = CategoriaPagamento.IndicadorJuizado,
                    IndicadorAdministrativo = CategoriaPagamento.IndicadorAdministrativo,
                    IndicadorHistorico = CategoriaPagamento.IndicadorHistorico,
                    TmgarCodigoMovicadorGarantia = CategoriaPagamento.TmgarCodigoTipoMovicadorGarantia,
                    IndicadorFinalizacaoContabil = CategoriaPagamento.IndicadorFinalizacaoContabil,
                    IndicadorProcon = CategoriaPagamento.IndicadorProcon,
                    IndicadorPex = CategoriaPagamento.IndicadorPex,
                    DescricaoJustificativa = CategoriaPagamento.DescricaoJustificativaNaoInfluenciaContigencia,
                    PagamentoA = CategoriaPagamento.CodPagamentoA,
                    ResponsabilidadeOi = (CategoriaPagamento.ResponsabilidadeOi.HasValue && CategoriaPagamento.ResponsabilidadeOi.Value > 0)  ? StringExtensions.FormataValor(CategoriaPagamento.ResponsabilidadeOi.Value): null
                });

            var result = await lista.ToListAsync();

            var resultado = result.OrderBy(ct => ct.Descricao).AsQueryable().OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "Descricao").ToList();

            return resultado;
        }
        private string DescricaoFornecedor(long? codigoTipoFornecedor)
        {
            switch (codigoTipoFornecedor)
            {
                case (int)TipoFornecedorEnumCategoriaPagamento.Banco:
                    return TipoFornecedorEnumCategoriaPagamento.Banco.Descricao();

                case (int)TipoFornecedorEnumCategoriaPagamento.EscritorioProfissional:
                    return TipoFornecedorEnumCategoriaPagamento.EscritorioProfissional.Descricao();

                case (int)TipoFornecedorEnumCategoriaPagamento.Todos:
                    return TipoFornecedorEnumCategoriaPagamento.Todos.Descricao();

                default:
                    return "";
            }
        }
        #endregion

        #region Validações externas
        public bool CodigoMaterialSAPValido(long CodigoCategoriaPagamento)
        {
            var resultado = dbContext.Set<CategoriaPagamento>()
              .Any(c => c.Id == CodigoCategoriaPagamento &&
                       (c.CodigoMaterialSap.HasValue &&
                        c.CodigoMaterialSap > 0));

            return resultado;
        }
        #endregion

        public async Task<ICollection<CategoriaDePagamentofiltroDTO>> ListaCategoriaPagamento(long codigoTipoProcesso)
        {
            var tipoLancamentos = RecuperarTipoLancamento();
            var categoriasPagamentos = await dbContext.Set<CategoriaPagamento>()
                .Where(cp => cp.IndicadorCivel == TipoProcessoEnum.CivelConsumidor.GetHashCode().Equals((int)codigoTipoProcesso))
                .Where(cp => cp.IndicadorTrabalhista == TipoProcessoEnum.Trabalhista.GetHashCode().Equals((int)codigoTipoProcesso))
                .Where(cp => cp.IndicadorTributarioJudicial == TipoProcessoEnum.TributarioJudicial.GetHashCode().Equals((int)codigoTipoProcesso))
                .Where(cp => cp.IndicadorCivelEstrategico == TipoProcessoEnum.CivelEstrategico.GetHashCode().Equals((int)codigoTipoProcesso))
                .Where(cp => cp.IndicadorPex == TipoProcessoEnum.Pex.GetHashCode().Equals((int)codigoTipoProcesso))
                .AsNoTracking()
                .ToListAsync();
            foreach (var tipoLancamento in tipoLancamentos)
            {
                var listaCategorias = new Collection<CategoriaDePagamentoDTO>();
                foreach (var categoria in categoriasPagamentos.Where(p => p.CodigoTipoLancamento == tipoLancamento.Id))
                {
                    var categoriaDTO = new CategoriaDePagamentoDTO()
                    {
                        Id = categoria.Id,
                        Descricao = categoria.IndicadorAtivo ? $"{categoria.DescricaoCategoriaPagamento}" :
                                                            $"{categoria.DescricaoCategoriaPagamento} (Inativo)",
                        Ativo = categoria.IndicadorAtivo
                    };
                    listaCategorias.Add(categoriaDTO);
                }
                tipoLancamento.Dados = listaCategorias.OrderBy(cp => cp.Descricao).ToList();
            }
            return tipoLancamentos;
        }

        private List<CategoriaDePagamentofiltroDTO> RecuperarTipoLancamento()
        {
            var Tipolancamento = dbContext.Set<TipoLancamento>()
                .AsNoTracking()
                .Select(dto => new CategoriaDePagamentofiltroDTO()
                {
                    Id = dto.Id,
                    Titulo = dto.Descricao
                }).ToList();

            return Tipolancamento;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<CategoriaPagamentoExportacaoDTO>> ExportarCategoriaPagamento(CategoriaPagamentoFiltroDTO filtros)
        {
            var result = dbContext.Set<CategoriaPagamento>()
                .WhereIf(filtros.TipoLancamento != null, p => p.CodigoTipoLancamento == filtros.TipoLancamento)
                .WhereIf(filtros.TipoProcesso != null, p => p.IndicadorCivel == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelConsumidor) &&
                                                            p.IndicadorTrabalhista == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Trabalhista) &&
                                                            p.IndicadorAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Administrativo) &&
                                                            p.IndicadorTributarioAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioAdministrativo) &&
                                                            p.IndicadorTributarioJudicial == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioJudicial) &&
                                                            p.IndicadorJuizado == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.JuizadoEspecial) &&
                                                            p.IndicadorCivelEstrategico == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelEstrategico) &&
                                                            p.IndicadorProcon == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Procon) &&
                                                            p.IndicadorPex == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Pex))
                .WhereIf(filtros.Codigo != null, p => p.Id == filtros.Codigo)
                .Select(catpag => new CategoriaPagamentoExportacaoDTO() {
                    Codigo = catpag.Id,
                    TipoLancamento = catpag.CodigoTipoLancamento,
                    Descricao = catpag.DescricaoCategoriaPagamento,
                    Descricaoclassegarantia = catpag.ClasseGarantia.Descricao,
                    CodigoMaterialSAP = catpag.CodigoMaterialSap,
                    indAtivo = catpag.IndicadorAtivo,
                    indEnvioSap = catpag.IndicadorEnvioSap,
                    ClgarCodigoClasseGarantia = catpag.ClgarCodigoClasseGarantia,
                    IndicadorNumeroGuia = catpag.IndicadorRequerimentoNumeroGuia,
                    TipoFornecedorPermitido = catpag.TipoFornecedorPermitido,
                    FornecedoresPermitidos = DescricaoFornecedor(catpag.TipoFornecedorPermitido),
                    IndEscritorioSolicitaLan = catpag.IndicadorEscritorioSolicitaLancamento,
                    GrpcgIdGrupoCorrecaoGar = catpag.GrpcgIdGrupoCorrecaoGar,
                    GrupoCorrecao = catpag.GrupoCorrecao.Descricao,
                    IndEncerraProcessoContabil = catpag.IndicadorEncerraProcessoContabilmente,
                    IndComprovanteSolicitacao = catpag.IndicadorRequerComprovanteSolicitacao,
                    IndicadorRequerDataVencimento = catpag.IndicadorRequerDataVencimento,
                    ReponsabilidadeOi = (catpag.ResponsabilidadeOi.HasValue && catpag.ResponsabilidadeOi.Value > 0) ? catpag.ResponsabilidadeOi : null,
                    IndicadorContingencia = catpag.IndicadorInfluenciaContingencia,
                    IndicadorCivelEstrategico = catpag.IndicadorCivelEstrategico,
                    IndicadorCivelConsumidor = catpag.IndicadorCivel,
                    IndicadorTrabalhista = catpag.IndicadorTrabalhista,
                    IndicadorTributarioJudicial = catpag.IndicadorTributarioJudicial,
                    Ind_TributarioAdministrativo = catpag.IndicadorTributarioAdministrativo,
                    IndicadorBaixaGarantia = catpag.IndicadorBaixaGarantia,
                    IndicadorBloqueioDeposito = catpag.IndicadorBloqueioDeposito,
                    IndicadorJuizado = catpag.IndicadorJuizado,
                    indBaixaPagamento = catpag.IndicadorBaixaGarantia,
                    IndicadorAdministrativo = catpag.IndicadorAdministrativo,
                    IndicadorHistorico = catpag.IndicadorHistorico,
                    TmgarCodigoMovicadorGarantia = catpag.TmgarCodigoTipoMovicadorGarantia,
                    IndicadorFinalizacaoContabil = catpag.IndicadorFinalizacaoContabil,
                    IndicadorProcon = catpag.IndicadorProcon,
                    IndicadorPex = catpag.IndicadorPex,
                    DescricaoJustificativa = catpag.DescricaoJustificativaNaoInfluenciaContigencia,
                });
            var resultado = await result.ToListAsync();
            return resultado.OrderBy(ct => ct.Descricao).AsQueryable().OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "Descricao").ToList();
        }

        public async Task<CategoriaPagamento> AlterarCategoriaPagamento(CategoriaPagamento categoriaPagamento)
        {
            {
                await base.Atualizar(categoriaPagamento);
                return categoriaPagamento;
            }
        }

        public async Task<bool> ExisteTipoMovimentoGarantia(long codigoCategoriaPagamento)
        {
            var resultado = await dbContext.Set<CategoriaPagamento>()
              .AnyAsync(c => c.Id == codigoCategoriaPagamento &&
                        c.TmgarCodigoTipoMovicadorGarantia != null);

            return resultado;
        }

        public async Task<string> EnvioSapIsValido(CategoriaPagamento categoria)
        {
            var categoriaBanco = await dbContext.CategoriaPagamentos
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(cp => cp.Id == categoria.Id); 
            var mudouParaSim = categoriaBanco != null && (categoriaBanco.IndicadorEnvioSap == false && categoria.IndicadorEnvioSap == true);
            var mudouParaNao = categoriaBanco != null && (categoriaBanco.IndicadorEnvioSap == true && categoria.IndicadorEnvioSap == false);
            
            if (mudouParaNao)
            {
                var existeAssociaoComLancamento = await dbContext.LancamentoProcessos
                                                .AsNoTracking()
                                                .AnyAsync(c => c.CodigoCatPagamento == categoria.Id);
                if (existeAssociaoComLancamento)
                    return "A indicação Envio SAP não pode ser alterada, pois existem lançamentos registrados em pelo menos um processo para esta categoria.";
              
                   
            }
         
            if (mudouParaSim && categoriaBanco.TmgarCodigoTipoMovicadorGarantia.HasValue)
                return "A alteração Envio para o SAP não pode ser marcada, pois esta informação está sendo usada em tipo de movimento de garantia.";
            
            return "";
        }

        public async Task<string> PagamentoAIsValido(CategoriaPagamento categoria)
        {
            var categoriaBanco = await dbContext.CategoriaPagamentos
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(cp => cp.Id == categoria.Id);

            if (categoriaBanco != null && categoriaBanco.CodPagamentoA != categoria.CodPagamentoA)
            {
                var existeAssociaoComLancamento = await dbContext.LancamentoProcessos
                                                 .AsNoTracking()
                                                 .AnyAsync(c => c.CodigoCatPagamento == categoria.Id);
                if (existeAssociaoComLancamento)
                    return "Alteração não permitida! Existem pagamentos cadastrados com essa categoria de pagamento.";
            }

            return "";
        }


        public async Task<bool> ExibeNotificacaoAoEditar(CategoriaPagamento categoria)
        {
            if (categoria.Id != 0)
            {
                var categoriaBanco = await dbContext.CategoriaPagamentos
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(cp => cp.Id == categoria.Id);
                var mudouParaSim = categoriaBanco != null && (categoriaBanco.IndicadorEnvioSap == false && categoria.IndicadorEnvioSap == true);
                if (mudouParaSim)
                {
                    var existeAssociaoComLancamento = await dbContext.LancamentoProcessos
                                                    .AsNoTracking()
                                                    .AnyAsync(c => c.CodigoCatPagamento == categoria.Id);
                    return existeAssociaoComLancamento;
                }
            }
            return false;
        }

        public async Task<string> ValidaHistorica(CategoriaPagamento categoria)
        {
            var existeAssociaoComLancamento = await dbContext.LancamentoProcessos
                                                .AsNoTracking()
                                                .AnyAsync(c => c.CodigoCatPagamento == categoria.Id);
            if (existeAssociaoComLancamento)
            {
                var categoriaBanco = await dbContext.CategoriaPagamentos
                       .AsNoTracking()
                       .FirstOrDefaultAsync(cp => cp.Id == categoria.Id);
                return categoriaBanco.IndicadorHistorico != categoria.IndicadorHistorico ? 
                     "Histórica não pode ser alterada, pois esta Categoria de Pagamento tem associação com Lançamento." : 
                     "";
            }
            else
                return "";
        }

        public async Task<ICollection<CategoriaDePagamentoEstrategicoDTO>> BuscarCategoriasPagamentoEstrategico(CategoriaPagamentoFiltroDTO filtros)
        {

            var listaCategoriaEstrategicoMigracao = dbContext.CategoriaPagamentos.Where(x => x.IndicadorCivelEstrategico).Select(y => new { y.Id, y.DescricaoCategoriaPagamento, y.IndicadorAtivo }).AsNoTracking().ToArray();

            var lista = from a in dbContext.CategoriaPagamentos.AsNoTracking()
                .WhereIf(filtros.TipoLancamento != null, p => p.CodigoTipoLancamento == filtros.TipoLancamento)
               .WhereIf(filtros.TipoProcesso != null, p => p.IndicadorCivel == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelConsumidor) &&
                                                           p.IndicadorTrabalhista == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Trabalhista) &&
                                                           p.IndicadorAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Administrativo) &&
                                                           p.IndicadorTributarioAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioAdministrativo) &&
                                                           p.IndicadorTributarioJudicial == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioJudicial) &&
                                                           p.IndicadorJuizado == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.JuizadoEspecial) &&
                                                           p.IndicadorCivelEstrategico == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelEstrategico) &&
                                                           p.IndicadorProcon == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Procon) &&
                                                           p.IndicadorPex == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Pex))
               .WhereIf(filtros.Codigo != null, p => p.Id == filtros.Codigo)
                        join ma in dbContext.MigracaoCategoriaPagamento on a.Id equals ma.CodCategoriaPagamentoCivel into LeftJoinMa
                        from ma in LeftJoinMa.DefaultIfEmpty()
                        join cg in dbContext.ClassesGarantias on a.ClgarCodigoClasseGarantia equals cg.Id into LeftJoinCg
                        from cg in LeftJoinCg.DefaultIfEmpty()
                        select new CategoriaDePagamentoEstrategicoDTO(                      
                            a.Id,
                            a.DescricaoCategoriaPagamento,
                            a.CodigoTipoLancamento,
                            a.IndicadorAtivo,
                            a.CodigoMaterialSap,
                            a.IndicadorEnvioSap,
                            a.ClgarCodigoClasseGarantia,
                            cg.Descricao,
                            a.IndicadorRequerimentoNumeroGuia,
                            a.TipoFornecedorPermitido,
                            DescricaoFornecedor(a.TipoFornecedorPermitido),
                            a.IndicadorEscritorioSolicitaLancamento == null ? a.IndicadorEscritorioSolicitaLancamento == false : a.IndicadorEscritorioSolicitaLancamento.Value,
                            a.GrpcgIdGrupoCorrecaoGar,
                            a.GrupoCorrecao.Descricao,
                            a.IndicadorEncerraProcessoContabilmente == null ? a.IndicadorEncerraProcessoContabilmente == false : a.IndicadorEncerraProcessoContabilmente.Value,
                            a.IndicadorRequerComprovanteSolicitacao == null ? a.IndicadorRequerComprovanteSolicitacao == false : a.IndicadorRequerComprovanteSolicitacao.Value,
                            a.IndicadorRequerDataVencimento == null ? a.IndicadorRequerDataVencimento == false : a.IndicadorRequerDataVencimento.Value,
                            a.IndicadorInfluenciaContingencia,
                            a.IndicadorCivel,
                            a.IndicadorCivelEstrategico,
                            a.IndicadorTrabalhista,
                            a.IndicadorTributarioJudicial,
                            a.IndicadorTributarioAdministrativo,
                            a.IndicadorBaixaGarantia,
                            a.IndicadorBaixaGarantia,
                            a.IndicadorBloqueioDeposito,
                            a.IndicadorJuizado,
                            a.IndicadorAdministrativo,
                            a.IndicadorHistorico,
                            a.IndicadorProcon,
                            a.IndicadorPex,
                            a.TmgarCodigoTipoMovicadorGarantia,
                            a.IndicadorFinalizacaoContabil,
                            a.DescricaoJustificativaNaoInfluenciaContigencia,
                            a.CodPagamentoA,
                            (a.ResponsabilidadeOi.HasValue && a.ResponsabilidadeOi.Value > 0) ? StringExtensions.FormataValor(a.ResponsabilidadeOi.Value) : null,
                            (long?)ma.CodCategoriaPagamentoEstra == null ? false : listaCategoriaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoEstra) != null ? listaCategoriaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoEstra).IndicadorAtivo : false,
                            (long?)ma.CodCategoriaPagamentoEstra,
                            (long?)ma.CodCategoriaPagamentoEstra == null ? null : listaCategoriaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoEstra).DescricaoCategoriaPagamento
                        );

            var result = await lista.ToListAsync();

            var resultado = result.OrderBy(ct => ct.Descricao).AsQueryable().OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "Descricao").ToList();

            return resultado;
        }

        public async Task<ICollection<CategoriaPagamentoExtrategicoExportacaoDTO>> ExportarCategoriasPagamentoEstrategico(CategoriaPagamentoFiltroDTO filtros)
        {
            var listaCategoriaEstrategicoMigracao = dbContext.CategoriaPagamentos.Where(x => x.IndicadorCivelEstrategico).Select(y => new { y.Id, y.DescricaoCategoriaPagamento, y.IndicadorAtivo }).AsNoTracking().ToArray();

            var lista = from a in dbContext.Set<CategoriaPagamento>().AsNoTracking()
                .WhereIf(filtros.TipoLancamento != null, p => p.CodigoTipoLancamento == filtros.TipoLancamento)
               .WhereIf(filtros.TipoProcesso != null, p => p.IndicadorCivel == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelConsumidor) &&
                                                           p.IndicadorTrabalhista == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Trabalhista) &&
                                                           p.IndicadorAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Administrativo) &&
                                                           p.IndicadorTributarioAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioAdministrativo) &&
                                                           p.IndicadorTributarioJudicial == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioJudicial) &&
                                                           p.IndicadorJuizado == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.JuizadoEspecial) &&
                                                           p.IndicadorCivelEstrategico == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelEstrategico) &&
                                                           p.IndicadorProcon == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Procon) &&
                                                           p.IndicadorPex == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Pex))
               .WhereIf(filtros.Codigo != null, p => p.Id == filtros.Codigo)
                        join ma in dbContext.Set<MigracaoCategoriaPagamento>() on a.Id equals ma.CodCategoriaPagamentoCivel into LeftJoinMa
                        from ma in LeftJoinMa.DefaultIfEmpty()


                        select new CategoriaPagamentoExtrategicoExportacaoDTO(
                    a.Id,
                    a.DescricaoCategoriaPagamento,
                    a.CodigoTipoLancamento,
                    a.IndicadorAtivo,
                    a.CodigoMaterialSap,
                    a.IndicadorEnvioSap,
                    a.ClgarCodigoClasseGarantia,
                    a.ClasseGarantia.Descricao,
                    a.IndicadorRequerimentoNumeroGuia,
                    a.TipoFornecedorPermitido,
                    DescricaoFornecedor(a.TipoFornecedorPermitido),
                    a.IndicadorEscritorioSolicitaLancamento,
                    a.GrpcgIdGrupoCorrecaoGar,
                    a.GrupoCorrecao.Descricao,
                    a.IndicadorEncerraProcessoContabilmente,
                    a.IndicadorRequerComprovanteSolicitacao,
                    a.IndicadorRequerDataVencimento,
                    a.ResponsabilidadeOi.HasValue && a.ResponsabilidadeOi.Value > 0 ? a.ResponsabilidadeOi : null,
                    a.IndicadorInfluenciaContingencia,
                    a.IndicadorCivel,
                    a.IndicadorCivelEstrategico,
                    a.IndicadorTrabalhista,
                    a.IndicadorTributarioJudicial,
                    a.IndicadorTributarioAdministrativo,
                    a.IndicadorBaixaGarantia,
                    a.IndicadorBaixaGarantia,
                    a.IndicadorBloqueioDeposito,
                    a.IndicadorJuizado,
                    a.IndicadorAdministrativo,
                    a.IndicadorHistorico,
                    a.IndicadorProcon,
                    a.IndicadorPex,
                    a.TmgarCodigoTipoMovicadorGarantia,
                    a.IndicadorFinalizacaoContabil,
                    a.DescricaoJustificativaNaoInfluenciaContigencia,
                    (long?)ma.CodCategoriaPagamentoEstra == null ? false : listaCategoriaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoEstra) != null ? listaCategoriaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoEstra).IndicadorAtivo : false,
                    (long)ma.CodCategoriaPagamentoEstra,
                    (long?)ma.CodCategoriaPagamentoEstra == null ? null : listaCategoriaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoEstra).DescricaoCategoriaPagamento);

            var result = await lista.ToListAsync();

            var resultado = result.OrderBy(ct => ct.Descricao).AsQueryable().OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "Descricao").ToList();

            return resultado;
        }

        public async Task<IEnumerable<CategoriaDePagamentoDTO>> RecuperarComboboxEstrategico()
        {
            return await dbContext.CategoriaPagamentos.Where(x => x.IndicadorCivelEstrategico)
                .Select(x => new CategoriaDePagamentoDTO(x.Id, x.DescricaoCategoriaPagamento, x.IndicadorAtivo, x.CodigoTipoLancamento)).ToArrayAsync();
        }

        public async Task<IEnumerable<CategoriaDePagamentoDTO>> RecuperarComboboxConsumidor()
        {
            return await dbContext.CategoriaPagamentos.Where(x=> x.IndicadorCivel)
                .Select(x => new CategoriaDePagamentoDTO(x.Id, x.DescricaoCategoriaPagamento, x.IndicadorAtivo, x.CodigoTipoLancamento)).ToArrayAsync();
        }

        public async Task<ICollection<CategoriaDePagamentoConsumidorDTO>> BuscarCategoriasPagamentoConsumidor(CategoriaPagamentoFiltroDTO filtros)
        {
            var listaCategoriaConsumidorMigracao = dbContext.CategoriaPagamentos.Where(x => x.IndicadorCivel).Select(y => new { y.Id, y.DescricaoCategoriaPagamento, y.IndicadorAtivo }).AsNoTracking().ToArray();

            var lista = from a in dbContext.CategoriaPagamentos.AsNoTracking()
               .WhereIf(filtros.TipoLancamento != null, p => p.CodigoTipoLancamento == filtros.TipoLancamento)
              .WhereIf(filtros.TipoProcesso != null, p => p.IndicadorCivel == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelConsumidor) &&
                                                          p.IndicadorTrabalhista == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Trabalhista) &&
                                                          p.IndicadorAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Administrativo) &&
                                                          p.IndicadorTributarioAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioAdministrativo) &&
                                                          p.IndicadorTributarioJudicial == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioJudicial) &&
                                                          p.IndicadorJuizado == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.JuizadoEspecial) &&
                                                          p.IndicadorCivelEstrategico == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelEstrategico) &&
                                                          p.IndicadorProcon == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Procon) &&
                                                          p.IndicadorPex == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Pex))
              .WhereIf(filtros.Codigo != null, p => p.Id == filtros.Codigo)
                        join ma in dbContext.MigracaoCategoriaPagamentoEstrategico on a.Id equals ma.CodCategoriaPagamentoEstra into LeftJoinMa
                        from ma in LeftJoinMa.DefaultIfEmpty()
                        join cg in dbContext.ClassesGarantias on a.ClgarCodigoClasseGarantia equals cg.Id into LeftJoinCg
                        from cg in LeftJoinCg.DefaultIfEmpty()
                        select new CategoriaDePagamentoConsumidorDTO(
                            a.Id,
                            a.DescricaoCategoriaPagamento,
                            a.CodigoTipoLancamento,
                            a.IndicadorAtivo,
                            a.CodigoMaterialSap,
                            a.IndicadorEnvioSap,
                            a.ClgarCodigoClasseGarantia,
                            cg.Descricao,
                            a.IndicadorRequerimentoNumeroGuia,
                            a.TipoFornecedorPermitido,
                            DescricaoFornecedor(a.TipoFornecedorPermitido),
                            a.IndicadorEscritorioSolicitaLancamento == null ? a.IndicadorEscritorioSolicitaLancamento == false : a.IndicadorEscritorioSolicitaLancamento.Value,
                            a.GrpcgIdGrupoCorrecaoGar,
                            a.GrupoCorrecao.Descricao,
                            a.IndicadorEncerraProcessoContabilmente == null ? a.IndicadorEncerraProcessoContabilmente == false : a.IndicadorEncerraProcessoContabilmente.Value,
                            a.IndicadorRequerComprovanteSolicitacao == null ? a.IndicadorRequerComprovanteSolicitacao == false : a.IndicadorRequerComprovanteSolicitacao.Value,
                            a.IndicadorRequerDataVencimento == null ? a.IndicadorRequerDataVencimento == false : a.IndicadorRequerDataVencimento.Value,
                            a.IndicadorInfluenciaContingencia,
                            a.IndicadorCivel,
                            a.IndicadorCivelEstrategico,
                            a.IndicadorTrabalhista,
                            a.IndicadorTributarioJudicial,
                            a.IndicadorTributarioAdministrativo,
                            a.IndicadorBaixaGarantia,
                            a.IndicadorBaixaGarantia,
                            a.IndicadorBloqueioDeposito,
                            a.IndicadorJuizado,
                            a.IndicadorAdministrativo,
                            a.IndicadorHistorico,
                            a.IndicadorProcon,
                            a.IndicadorPex,
                            a.TmgarCodigoTipoMovicadorGarantia,
                            a.IndicadorFinalizacaoContabil,
                            a.DescricaoJustificativaNaoInfluenciaContigencia,
                            a.CodPagamentoA,
                            (a.ResponsabilidadeOi.HasValue && a.ResponsabilidadeOi.Value > 0) ? StringExtensions.FormataValor(a.ResponsabilidadeOi.Value) : null,
                            (long?)ma.CodCategoriaPagamentoCivel == null ? false : listaCategoriaConsumidorMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoCivel) != null ? listaCategoriaConsumidorMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoCivel).IndicadorAtivo : false,
                            (long?)ma.CodCategoriaPagamentoCivel,
                            (long?)ma.CodCategoriaPagamentoCivel == null ? null : listaCategoriaConsumidorMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoCivel).DescricaoCategoriaPagamento
                        );

            var result = await lista.ToListAsync();

            var resultado = result.OrderBy(ct => ct.Descricao).AsQueryable().OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "Descricao").ToList();

            return resultado;


        }

        public async Task<ICollection<CategoriaPagamentoConsumidorExportacaoDTO>> ExportarCategoriasPagamentoConsumidor(CategoriaPagamentoFiltroDTO filtros)
        {
            var listaCategoriaConsumidorMigracao = dbContext.CategoriaPagamentos.Where(x => x.IndicadorCivel).Select(y => new { y.Id, y.DescricaoCategoriaPagamento, y.IndicadorAtivo }).AsNoTracking().ToArray();

            var lista = from a in dbContext.Set<CategoriaPagamento>().AsNoTracking()
                .WhereIf(filtros.TipoLancamento != null, p => p.CodigoTipoLancamento == filtros.TipoLancamento)
               .WhereIf(filtros.TipoProcesso != null, p => p.IndicadorCivel == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelConsumidor) &&
                                                           p.IndicadorTrabalhista == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Trabalhista) &&
                                                           p.IndicadorAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Administrativo) &&
                                                           p.IndicadorTributarioAdministrativo == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioAdministrativo) &&
                                                           p.IndicadorTributarioJudicial == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.TributarioJudicial) &&
                                                           p.IndicadorJuizado == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.JuizadoEspecial) &&
                                                           p.IndicadorCivelEstrategico == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.CivelEstrategico) &&
                                                           p.IndicadorProcon == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Procon) &&
                                                           p.IndicadorPex == filtros.TipoProcesso.Equals((long)TipoProcessoEnum.Pex))
               .WhereIf(filtros.Codigo != null, p => p.Id == filtros.Codigo)
                        join ma in dbContext.Set<MigracaoCategoriaPagamentoEstrategico>() on a.Id equals ma.CodCategoriaPagamentoEstra into LeftJoinMa
                        from ma in LeftJoinMa.DefaultIfEmpty()


                        select new CategoriaPagamentoConsumidorExportacaoDTO(
                    a.Id,
                    a.DescricaoCategoriaPagamento,
                    a.CodigoTipoLancamento,
                    a.IndicadorAtivo,
                    a.CodigoMaterialSap,
                    a.IndicadorEnvioSap,
                    a.ClgarCodigoClasseGarantia,
                    a.ClasseGarantia.Descricao,
                    a.IndicadorRequerimentoNumeroGuia,
                    a.TipoFornecedorPermitido,
                    DescricaoFornecedor(a.TipoFornecedorPermitido),
                    a.IndicadorEscritorioSolicitaLancamento,
                    a.GrpcgIdGrupoCorrecaoGar,
                    a.GrupoCorrecao.Descricao,
                    a.IndicadorEncerraProcessoContabilmente,
                    a.IndicadorRequerComprovanteSolicitacao,
                    a.IndicadorRequerDataVencimento,
                    a.ResponsabilidadeOi.HasValue && a.ResponsabilidadeOi.Value > 0 ? a.ResponsabilidadeOi : null,
                    a.IndicadorInfluenciaContingencia,
                    a.IndicadorCivel,
                    a.IndicadorCivelEstrategico,
                    a.IndicadorTrabalhista,
                    a.IndicadorTributarioJudicial,
                    a.IndicadorTributarioAdministrativo,
                    a.IndicadorBaixaGarantia,
                    a.IndicadorBaixaGarantia,
                    a.IndicadorBloqueioDeposito,
                    a.IndicadorJuizado,
                    a.IndicadorAdministrativo,
                    a.IndicadorHistorico,
                    a.IndicadorProcon,
                    a.IndicadorPex,
                    a.TmgarCodigoTipoMovicadorGarantia,
                    a.IndicadorFinalizacaoContabil,
                    a.DescricaoJustificativaNaoInfluenciaContigencia,
                    (long?)ma.CodCategoriaPagamentoCivel == null ? false : listaCategoriaConsumidorMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoCivel) != null ? listaCategoriaConsumidorMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoCivel).IndicadorAtivo : false,
                    (long)ma.CodCategoriaPagamentoCivel,
                    (long?)ma.CodCategoriaPagamentoCivel == null ? null : listaCategoriaConsumidorMigracao.FirstOrDefault(z => z.Id == ma.CodCategoriaPagamentoCivel).DescricaoCategoriaPagamento);

            var result = await lista.ToListAsync();

            var resultado = result.OrderBy(ct => ct.Descricao).AsQueryable().OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "Descricao").ToList();

            return resultado;
        }
    }
}