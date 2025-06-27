using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.GrupoDeEstados.Repository
{
    public class GrupoDeEstadosRepository : BaseCrudRepository<Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity.GrupoDeEstados, long>, IGrupoDeEstadosRepository
    {
        private readonly IGrupoEstadosRepository repository;
        public GrupoDeEstadosRepository(JuridicoContext context, IAuthenticatedUser user, IGrupoEstadosRepository _repository)
            : base(context, user)
        {
            repository = _repository;
        }

        public void AtualizarGrupo(Domain.GrupoDeEstados.Entity.GrupoDeEstados entity)
        {
            context.Update(entity);
        }

        public void CriarGrupo(Domain.GrupoDeEstados.Entity.GrupoDeEstados entity)
        {
            context.Add(entity);
        }

        public void CriarGrupoEstado(IList<GrupoEstados> grupoEstado)
        {
            context.AddRange(grupoEstado);
        }

        public bool EstadoEstaEmUso(string estadoId, string[] nomesGrupos)
        {
            var existe = context.Set<GrupoEstados>()
                           .Any(x => x.EstadoId == estadoId && !nomesGrupos.Contains(x.GrupoDeEstados.NomeGrupo));

            return existe;
        }

        public void ExcluirGrupo(Domain.GrupoDeEstados.Entity.GrupoDeEstados entity)
        {
            repository.ExcluirGrupoEstado(entity.Id);
            context.Remove(entity);
        }

        public async Task<IList<Domain.GrupoDeEstados.Entity.GrupoDeEstados>> ListarGrupoDeEstados()
        {
            var teste = await context.Set<Domain.GrupoDeEstados.Entity.GrupoDeEstados>()
                          .AsNoTracking()
                          .Include(x => x.GrupoEstados)
                          .ThenInclude(x => x.Estado)
                          .OrderBy(x => x.NomeGrupo)
                          .ThenBy(x => x.Id)
                          .ToArrayAsync();
            return teste;

        }

        public bool NomeGrupoJaCadastrado(string nome)
        {
            return context.Set<Domain.GrupoDeEstados.Entity.GrupoDeEstados>()
                            .AsNoTracking()
                            .Any(x => x.NomeGrupo.Equals(nome));
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public Domain.GrupoDeEstados.Entity.GrupoDeEstados RecuperarGrupoPorNome(string nome)
        {
            return context.Set<Domain.GrupoDeEstados.Entity.GrupoDeEstados>()
                            .Include(x => x.GrupoEstados)                            
                            .FirstOrDefault(x => x.NomeGrupo.Equals(nome));
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
