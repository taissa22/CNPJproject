using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposAudiencias
{
    public class RegistrarTipoAudienciaCommand : ICommand
    {
        public RegistrarTipoAudienciaCommand() { }

        public RegistrarTipoAudienciaCommand(string descricao, string sigla, TipoProcessoEnum? tipoProcesso, bool estaAtivo) 
        {
            Descricao = descricao;
            Sigla = sigla;
            EstaAtivo = estaAtivo;
            TipoProcesso = tipoProcesso;
        }

        public string Descricao { get; set; }

        public string Sigla { get; set; }

        public bool EstaAtivo { get; set; }

        public TipoProcessoEnum? TipoProcesso { get; set; }  
    }
}
