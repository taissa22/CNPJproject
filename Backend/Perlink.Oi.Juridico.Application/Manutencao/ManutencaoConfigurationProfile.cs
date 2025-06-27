using AutoMapper;
using Perlink.Oi.Juridico.Application.Manutencao.ViewModel.JurosCorrecaoProcesso;
using Shared.Application.Conversores;

namespace Perlink.Oi.Juridico.Application
{
    public class ManutencaoConfigurationProfile : Profile
    {
        public ManutencaoConfigurationProfile()
        {
            Configuracao.Registrar(this);
            AllowNullCollections = true;
            //colocar em ordem alfabetica para facilitar a busca

            JuroCorrecaoProcessoViewModel.Mapping(this);
            JuroCorrecaoProcessoInputViewModel.Mapping(this);
        }
    }
}
