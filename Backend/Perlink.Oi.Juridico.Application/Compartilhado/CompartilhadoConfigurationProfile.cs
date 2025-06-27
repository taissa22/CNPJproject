using AutoMapper;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Shared.Application.Conversores;

namespace Perlink.Oi.Juridico.Application.Compartilhado
{
    public class CompartilhadoConfigurationProfile : Profile
    {
        public CompartilhadoConfigurationProfile()
        {
            Configuracao.Registrar(this);
            AllowNullCollections = true;

            //colocar em ordem alfabetica para facilitar a busca
            EmpresaDoGrupoViewModel.Mapping(this);
            EscritorioViewModel.Mapping(this);
            EmpresaDoGrupoListaViewModel.Mapping(this);
            EscritorioListaViewModel.Mapping(this);
            ExportacaoPrePosRJViewModel.Mapping(this);
            ExportacaoPrePosRJListaViewModel.Mapping(this);
            EstadoViewModel.Mapping(this);
            LancamentoProcessoViewModel.Mapping(this);
            TipoProcessoViewModel.Mapping(this);
            StatusPagamentoViewModel.Mapping(this);
            LoteLancamentoViewModel.Mapping(this);
            LoteLancamentoCeViewModel.Mapping(this);
            LoteLancamentoTrabViewModel.Mapping(this);
            LoteLancamentoCcViewModel.Mapping(this);
            LoteLancamentoJecViewModel.Mapping(this);
            LoteLancamentoPexViewModel.Mapping(this);
            BancoViewModel.Mapping(this);
            ProfissionalDropDownViewModel.Mapping(this);
            ProcessoViewModel.Mapping(this);
            ProcessoFiltroViewModel.Mapping(this);
            DadosLancamentoEstornoViewModel.Mapping(this);
            DadosProcessoEstornoViewModel.Mapping(this);
        }
    }
}