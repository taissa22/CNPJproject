using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;
using Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.EFRepositories;
using Shared.Domain.Interface;

namespace Perlink.Oi.Juridico.Data.Manutencao.EFRepositories
{
    public class TipoParticipacaoRepository : ITipoParticipacaoRepository
    {
        private readonly JuridicoContext _context;

        public TipoParticipacaoRepository(JuridicoContext context, IAuthenticatedUser user)
        {
            _context = context;
        }

        public TipoParticipacao Get(long id) => _context.TipoParticipacoes.Find(id);

        public void Remove(TipoParticipacao tipoParticipacao) => _context.TipoParticipacoes.Remove(tipoParticipacao);

        public void Save(TipoParticipacao tipoParticipacao) => _context.TipoParticipacoes.Add(tipoParticipacao);

        public void Update(TipoParticipacao tipoParticipacao) => _context.Entry(tipoParticipacao).State = EntityState.Modified;
    }
}
