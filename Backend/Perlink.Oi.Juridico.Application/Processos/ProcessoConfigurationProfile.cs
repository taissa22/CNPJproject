using AutoMapper;
using Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Shared.Application.Conversores;

namespace Perlink.Oi.Juridico.Application
{
    public class ProcessoConfigurationProfile : Profile
    {
        public ProcessoConfigurationProfile()
        {
            Configuracao.Registrar(this);
            AllowNullCollections = true;
            //colocar em ordem alfabetica para facilitar a busca

            AgendaAudienciaFiltrosViewModel.Mapping(this);
            AgendaAudienciaPartesViewModel.Mapping(this);
            AgendaAudienciaPedidosViewModel.Mapping(this);
            AgendaAudienciaComboEdicaoViewModel.Mapping(this);
            AgendaAudienciaAdvogadoViewModel.Mapping(this);
            AudienciaProcessoUpdateVM.Mapping(this);
        }
    }
}
