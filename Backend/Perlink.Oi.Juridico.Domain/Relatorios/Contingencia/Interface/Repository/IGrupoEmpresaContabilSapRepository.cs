using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.Interface.Repository
{
    public interface IGrupoEmpresaContabilSapRepository : IBaseCrudRepository<GrupoEmpresaContabilSap, long>
    {
        Task<IList<GrupoEmpresaContabilSap>> ListarGrupoEmpresaContabilSap();

        bool NomeGrupoJaCadastrado(string nome);

        bool EmpresaEstaEmUso(long empresaId, string[] nomesGrupos);

        void SaveChanges();

        GrupoEmpresaContabilSap RecuperarGrupoPorNome(string nome);

        void CriarGrupo(GrupoEmpresaContabilSap g);

        void CriarGrupoXEmpresa(IList<GrupoEmpresaContabilSapParte> ge);

        void ExcluirGrupo(GrupoEmpresaContabilSap id);

        void AtualizarGrupo(GrupoEmpresaContabilSap grp);
    }
}