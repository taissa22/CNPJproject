using AutoMapper;
using Perlink.Oi.Juridico.Application.ContingenciaPex.ViewModel;
using Shared.Application.Conversores;

namespace Perlink.Oi.Juridico.Application.ContingenciaPex
{
    public class ContingenciaPexConfigurationProfile : Profile
    {
        public ContingenciaPexConfigurationProfile()
        {
            Configuracao.Registrar(this);
            AllowNullCollections = true;

            FechamentoContingenciaPexMediaViewModel.Mapping(this);
        }
    }
}
