using AutoMapper;
using Perlink.Oi.Juridico.Application.GrupoDeEstados.ViewModel;
using Shared.Application.Conversores;

namespace Perlink.Oi.Juridico.Application.GrupoDeEstados
{
    public class GrupoDeEstadosConfigurationProfile : Profile
    {
        public GrupoDeEstadosConfigurationProfile()
        {
            Configuracao.Registrar(this);
            AllowNullCollections = true;
        }
    }
}
