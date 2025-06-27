using AutoMapper;
using Perlink.Oi.Juridico.Application.AlteracaoBloco.ViewModel;
using Shared.Application.Conversores;

namespace Perlink.Oi.Juridico.Application.AlteracaoBloco
{
    public class AlteracaoEmBlocoConfigurationProfile : Profile
    {
        public AlteracaoEmBlocoConfigurationProfile()
        {
            Configuracao.Registrar(this);
            AllowNullCollections = true;

            AlteracaoEmBlocoViewModel.Mapping(this);
            AlteracaoEmBlocoDownloadViewModel.Mapping(this);
        }
    }
}
