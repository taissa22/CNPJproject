using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class EmpresaDoGrupoRepository : BaseCrudRepository<Parte, long>, IEmpresaDoGrupoRepository
    {
        public EmpresaDoGrupoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
        }

        public async Task<bool> ExisteEmpresaDoGrupoComFornecedor(long codigoFornecedor)
        {
            var resultado = await context.Set<Parte>()
                .Where(a => a.TipoParte.Equals("E"))
                .AnyAsync(l => l.Fornecedor == codigoFornecedor);

            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        private int orderByNumberFirst(string x)
        {
            int i = 0;
            return int.TryParse(x, out i) ? i : int.MaxValue;
        }

        public async Task<ICollection<EmpresaDoGrupoDTO>> RecuperarEmpresaDoGrupo()
        {
            return await context.Set<Parte>()
               .Where(a => a.TipoParte.Equals("E"))
               .AsNoTracking()
               .OrderBy(s => orderByNumberFirst(s.Nome))
            .ThenBy(s => s.Nome)
            .Select(dto => new EmpresaDoGrupoDTO()
            {
                Id = dto.Id,
                Nome = dto.Nome
            }).ToListAsync();
        }
    }
}