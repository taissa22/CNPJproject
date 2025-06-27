using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.Compartilhado.Repository;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;
using Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.EFRepositories;
using Shared.Domain.Interface;

namespace Perlink.Oi.Juridico.Data.Manutencao.EFRepositories
{
    public class TipoAudienciaRepository : ITipoAudienciaRepository
    {
        private readonly JuridicoContext _context;

        public TipoAudienciaRepository(JuridicoContext context, IAuthenticatedUser user)
        {
            _context = context;
        }

        public TipoAudiencia Get(long id) => _context.TiposAudiencias.Find(id);

        public void Remove(TipoAudiencia tipoAudiencia) => _context.TiposAudiencias.Remove(tipoAudiencia);

        public void Save(TipoAudiencia tipoAudiencia) => _context.TiposAudiencias.Add(tipoAudiencia);

        public void Update(TipoAudiencia tipoAudiencia) => _context.Entry(tipoAudiencia).State = EntityState.Modified;
    }
}
