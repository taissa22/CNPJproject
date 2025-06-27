using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposAudiencias
{
    public class ExcluirTipoAudienciaCommand : ICommand
    {
        public ExcluirTipoAudienciaCommand() { }

        public ExcluirTipoAudienciaCommand(long id)
        {
            CodigoTipoAudiencia = id;
        }

        public long CodigoTipoAudiencia { get; set; }  
    }
}
