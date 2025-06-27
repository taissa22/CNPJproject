using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.Processos;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository.Processos
{
    public class AdvogadoEscritorioRepository : BaseCrudRepository<AdvogadoEscritorio, long>, IAdvogadoEscritorioRepository
    {
        private readonly JuridicoContext dbContext;

        public AdvogadoEscritorioRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AdvogadoEscritorioDTO>> RecuperarAdvogadoEscritorio()
        {     

            var advogados = await dbContext.AdvogadoEscritorio
                .Join(dbContext.Profissional, 
                      advogado => advogado.CodigoProfissional,
                      profissional => profissional.Id,
                     (advogado, profissional) => new { advogado, profissional })
                .Where(w => w.profissional.IndicadorAreaTrabalhista == true)
                .Select(s => new AdvogadoEscritorioDTO
                {
                    Id = s.advogado.Id,                    
                    CodigoInterno = s.advogado.CodigoProfissional,
                    Descricao = (string.Format("{0}{1}{2}{3}", s.advogado.NomeAdvogado, " (", s.profissional.NomeProfissional, ")"))
                    
                })
                .OrderBy(o => o.Descricao)
                .AsNoTracking()
                .ToListAsync();

            return advogados;
        }

        public async Task<IEnumerable<AdvogadoEscritorioDTO>> RecuperarAdvogadoEscritorioUsuarioEscritorio(string codUsuario) {
            string sql = @"SELECT (trim(advogado.NOM_ADVOGADO)||' ('||trim(profissional.NOM_PROFISSIONAL)||')') as Descricao, 
                            advogado.cod_profissional as Id, advogado.COD_INTERNO as CodigoInterno
                            FROM JUR.ADVOGADO_ESCRITORIO advogado
                            INNER JOIN JUR.PROFISSIONAL profissional ON advogado.COD_PROFISSIONAL = profissional.COD_PROFISSIONAL
                            inner join jur.aca_usuario_escritorio uses on uses.cod_profissional = profissional.cod_profissional
                            inner join jur.aca_usuario us on us.cod_usuario = uses.cod_usuario
                            WHERE profissional.IND_AREA_TRABALHISTA = 'S'
                            and us.cod_usuario = '" + codUsuario + @"'
                            order by Descricao asc";

            return await this.context.Query<AdvogadoEscritorioDTO>().FromSql(sql).ToListAsync();
        }

        public async Task<IEnumerable<AdvogadoEscritorioDTO>> RecuperarAdvogadoEscritorioPorCodigoProfissional(long codigoEscritorio)
        {
            var Advogado = await dbContext.AdvogadoEscritorio
              .Where(a => a.CodigoProfissional == codigoEscritorio)
              .Select(s => new AdvogadoEscritorioDTO
              {
                  Id = s.Id,                  
                  Descricao = s.NomeAdvogado,
                  CodigoInterno = s.CodigoProfissional,
              })
              .OrderBy(o => o.Descricao)
              .ToListAsync();

            return Advogado;
        }
    }
}
