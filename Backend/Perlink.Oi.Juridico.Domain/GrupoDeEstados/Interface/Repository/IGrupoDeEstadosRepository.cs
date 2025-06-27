using Perlink.Oi.Juridico.Domain.GrupoDeEstados;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Repository
{
    public interface IGrupoDeEstadosRepository : IBaseCrudRepository<Entity.GrupoDeEstados, long>
    {
        Task<IList<Entity.GrupoDeEstados>> ListarGrupoDeEstados();

        bool NomeGrupoJaCadastrado(string nome);

        bool EstadoEstaEmUso(string estadoId, string[] nomesGrupos);

        void SaveChanges();

        Entity.GrupoDeEstados RecuperarGrupoPorNome(string nome);

        void CriarGrupo(Entity.GrupoDeEstados entity);

        void CriarGrupoEstado(IList<GrupoDeEstados.Entity.GrupoEstados> grupoEstado);

        void ExcluirGrupo(Entity.GrupoDeEstados entity);

        void AtualizarGrupo(Entity.GrupoDeEstados entity);

    }
}
