using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;
using Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.EFRepositories;
using Shared.Domain.Interface;
using System.Linq;

namespace Perlink.Oi.Juridico.Data.Manutencao.EFRepositories
{
    public class BaseCalculoRepository : IBaseCalculoRepository
    {
        private readonly JuridicoContext _context;

        public BaseCalculoRepository(JuridicoContext context, IAuthenticatedUser user)
        {
            _context = context;
        }

        public BaseCalculo Get(long id) => _context.BaseCalculos.Find(id);

        public void Remove(BaseCalculo baseCalculo) => _context.BaseCalculos.Remove(baseCalculo);

        public void Save(BaseCalculo baseCalculo) => _context.BaseCalculos.Add(baseCalculo);

        public void Update(BaseCalculo baseCalculo) => _context.Entry(baseCalculo).State = EntityState.Modified;

        public BaseCalculo ObterPorBaseInicial() => _context.BaseCalculos.Where(x => x.EhBaseInicial == true).FirstOrDefault();
    }
}
