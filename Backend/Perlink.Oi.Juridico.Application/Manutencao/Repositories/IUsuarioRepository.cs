using Perlink.Oi.Juridico.Application.Manutencao.Results.Usuario;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IUsuarioRepository
    {
        CommandResult<PaginatedQueryResult<Usuario>> ObterTodos();

        IEnumerable<UsuarioCommandResult> ObterAtivos();
        IEnumerable<UsuarioCommandResult> ObterAtivosEInativos();

        IEnumerable<UsuarioCommandResult> ObterPrepostosAtivos();
        IEnumerable<UsuarioCommandResult> ObterTodosPrepostos();
    }
}
