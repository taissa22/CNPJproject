using AutoMapper;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Shared.Application.Conversores;

namespace Perlink.Oi.Juridico.Application
{
    public class ControleDeAcessoConfigurationProfile : Profile
    {
        public ControleDeAcessoConfigurationProfile()
        {
            Configuracao.Registrar(this);
            AllowNullCollections = true;

            //colocar em ordem alfabetica para facilitar a busca
            ParametroViewModel.Mapping(this);
        }
    }
}