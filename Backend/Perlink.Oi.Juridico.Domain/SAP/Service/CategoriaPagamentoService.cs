using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Resultados;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class CategoriaPagamentoService : BaseCrudService<CategoriaPagamento, long>, ICategoriaPagamentoService
    {
        private readonly ICategoriaPagamentoRepository repository;
        private readonly ITipoProcessoRepository tipoProcessoRepository;
        private readonly ITipoLancamentoRepository tipoLancamentoRepository;
        private readonly IClassesGarantiasRepository classesGarantiasRepository;
        private readonly IGrupoCorrecoesGarantiasRepository grupoCorrecoesGarantiasRepository;
        public CategoriaPagamentoService(ICategoriaPagamentoRepository repository,
                                         ITipoProcessoRepository tipoProcessoRepository,
                                         ITipoLancamentoRepository tipoLancamentoRepository,
                                         IClassesGarantiasRepository classesGarantiasRepository,
                                         IGrupoCorrecoesGarantiasRepository grupoCorrecoesGarantiasRepository) : base(repository)
        {
            this.repository = repository;
            this.tipoProcessoRepository = tipoProcessoRepository;
            this.tipoLancamentoRepository = tipoLancamentoRepository;
            this.classesGarantiasRepository = classesGarantiasRepository;
            this.grupoCorrecoesGarantiasRepository = grupoCorrecoesGarantiasRepository;
        }

        public async Task<IEnumerable<ComboboxDTO>> RecuperarComboBoxTipoProcesso()
        {
            List<long> Ids = new List<long>();
            ICollection<ComboboxDTO> result = new List<ComboboxDTO>();
            
            Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelConsumidor));
            Ids.Add(Convert.ToInt32(TipoProcessoEnum.CivelEstrategico));
            Ids.Add(Convert.ToInt32(TipoProcessoEnum.JuizadoEspecial));
            Ids.Add(Convert.ToInt32(TipoProcessoEnum.Trabalhista));
            Ids.Add(Convert.ToInt32(TipoProcessoEnum.TributarioAdministrativo));
            Ids.Add(Convert.ToInt32(TipoProcessoEnum.TributarioJudicial));
            Ids.Add(Convert.ToInt32(TipoProcessoEnum.Administrativo));
            Ids.Add(Convert.ToInt32(TipoProcessoEnum.Procon));
            Ids.Add(Convert.ToInt32(TipoProcessoEnum.Pex));

            foreach (var item in Ids) {
                var tipoProcesso = await tipoProcessoRepository.RecuperarPorId(item);

                ComboboxDTO comboboxDTO = new ComboboxDTO() {
                    Descricao = tipoProcesso.Descricao,
                    Id = tipoProcesso.Id
                };

                result.Add(comboboxDTO);
            }

            return result; //await tipoProcessoRepository.RecuperarTodosSAPCombobox(Ids.ToArray());
        }

        public async Task<IEnumerable<ComboboxDTO>> RecuperarComboboxTipoLancamento(long tipoProcesso)
        {
            return await tipoLancamentoRepository.RecuperarTodosSAP(tipoProcesso);
        }
        public async Task<bool> ExisteTipoMovimentoGarantia(long codigoCategoriaPagamento)
        {
            return await repository.ExisteTipoMovimentoGarantia(codigoCategoriaPagamento);
        }

        public async Task ExcluirCategoria(long codigoCategoriaPagamento)
        {
            await repository.RemoverPorId(codigoCategoriaPagamento);
        }

        public async Task<CategoriaPagamento> CadastrarCategoria(CategoriaPagamento categoriaPagamento)
        {
            return await repository.CadastrarCategoria(categoriaPagamento);
        }

        public async Task<ICollection<CategoriaDePagamentoResultadoDTO>> BuscarCategoriasPagamento(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO)
        {
            return await repository.BuscarCategoriasPagamento(categoriaPagamentoFiltroDTO);
        }

        public async Task<IEnumerable<ComboboxDTO>> RecuperarComboboxGrupoCorrecao(long tipoProcesso)
        {
            return await grupoCorrecoesGarantiasRepository.RecuperarComboboxGrupoCorrecao(tipoProcesso);
        }

        public IEnumerable<ComboboxDTO> RecuperarComboboxFornecedoresPermitidos()
        {
            return new List<ComboboxDTO>()
            {
                    new ComboboxDTO() { Id = 1, Descricao = "Banco" },
                    new ComboboxDTO() { Id = 2, Descricao = "Escritório/Profissional" },
                    new ComboboxDTO() { Id = 3, Descricao = "Ambos" }
            };
        }

        public IEnumerable<ComboboxDTO> RecuperarComboboxPagamentoA()
        {
            return new List<ComboboxDTO>()
            {
                    new ComboboxDTO() { Id = 1, Descricao = "Autor" },
                    new ComboboxDTO() { Id = 2, Descricao = "Advogado do Autor" }
            };
        }

        public async Task<IEnumerable<ComboboxDTO>> RecuperarComboboxClasseGarantia(long tipoLancamento)
        {
            return await classesGarantiasRepository.RecuperarClassesGarantias(tipoLancamento);
        }

        public async Task<ICollection<CategoriaPagamentoExportacaoDTO>> ExportarCategoriasPagamento(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO)
        {
            return await repository.ExportarCategoriaPagamento(categoriaPagamentoFiltroDTO);
        }



        public async Task<CategoriaPagamento> AlterarCategoriaPagamento(CategoriaPagamento categoriaPagamento)
        {
            return await repository.AlterarCategoriaPagamento(categoriaPagamento);
        }

        public async Task<string> EnvioSapIsValido(CategoriaPagamento categoria)
        {
            return await repository.EnvioSapIsValido(categoria);
        }

        public async Task<string> ValidaHistorica(CategoriaPagamento categoria)
        {
            return await repository.ValidaHistorica(categoria);
        }

        public async Task<bool> ExibeNotificacaoAoEditar(CategoriaPagamento categoria)
        {
            categoria.ResponsabilidadeOi = categoria.ResponsabilidadeOi > 100 ? categoria.ResponsabilidadeOi / 100 : categoria.ResponsabilidadeOi;
            return await repository.ExibeNotificacaoAoEditar(categoria);
        }

        public async Task<string> PagamentoAIsValido(CategoriaPagamento categoria)
        {
            return await repository.PagamentoAIsValido(categoria);
        }

        

        public async Task<ICollection<CategoriaDePagamentoEstrategicoDTO>> BuscarCategoriasPagamentoEstrategico(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO)
        {
            return await repository.BuscarCategoriasPagamentoEstrategico(categoriaPagamentoFiltroDTO);
        }

        public async Task<ICollection<CategoriaPagamentoExtrategicoExportacaoDTO>> ExportarCategoriasPagamentoEstrategico(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO)
        {
            return await repository.ExportarCategoriasPagamentoEstrategico(categoriaPagamentoFiltroDTO);
        }

        public async Task<IEnumerable<CategoriaDePagamentoDTO>> RecuperarComboboxEstrategico()
        {
            return await repository.RecuperarComboboxEstrategico();
        }

        public async Task<IEnumerable<CategoriaDePagamentoDTO>> RecuperarComboboxConsumidor()
        {
            return await repository.RecuperarComboboxConsumidor();
        }

        public async Task<ICollection<CategoriaDePagamentoConsumidorDTO>> BuscarCategoriasPagamentoConsumidor(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO)
        {
            return await repository.BuscarCategoriasPagamentoConsumidor(categoriaPagamentoFiltroDTO);
        }

        public async Task<ICollection<CategoriaPagamentoConsumidorExportacaoDTO>> ExportarCategoriasPagamentoConsumidor(CategoriaPagamentoFiltroDTO categoriaPagamentoFiltroDTO)
        {
            return await repository.ExportarCategoriasPagamentoConsumidor(categoriaPagamentoFiltroDTO);
        }
    }
}

