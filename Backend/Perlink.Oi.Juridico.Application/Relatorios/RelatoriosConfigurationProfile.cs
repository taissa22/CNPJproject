using AutoMapper;
using Shared.Application.Conversores;

namespace Perlink.Oi.Juridico.Application.Relatorios
{
    public class RelatoriosConfigurationProfile : Profile
    {
        public RelatoriosConfigurationProfile()
        {
            Configuracao.Registrar(this);
            AllowNullCollections = true;
        }
    }
}