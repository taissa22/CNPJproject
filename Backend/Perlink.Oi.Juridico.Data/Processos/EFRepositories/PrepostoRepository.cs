using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.Processos;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository.Processos
{
    public class PrepostoRepository : BaseCrudRepository<Preposto, long>, IPrepostoRepository
    {
        private readonly JuridicoContext dbContext;

        public PrepostoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
            dbContext = context;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Esse método retorna os Prepostos/Prepostos Acompanhamtes
        /// não são trabalhistas mas possuem audiência trabalhista,
        /// e também os que são trabalhistas
        /// </summary>
        public async Task<IEnumerable<PrepostoDTO>> RecuperarPrepostoTrabalhista()
        {
            string sql = @"SELECT jur.preposto.cod_preposto Id  ,               
                          case when (jur.preposto.ind_preposto_ativo = 'S' and jur.preposto.ind_preposto_trabalhista = 'S') then 
                          jur.preposto.nom_preposto
                          else
                          jur.preposto.nom_preposto ||  ' [Inativo]'
                          end  Descricao                         
                          FROM jur.preposto 
                        full outer join
                        (SELECT jur.preposto.cod_preposto Id,
                               jur.preposto.nom_preposto,
                               jur.preposto.ind_preposto_trabalhista preposto_trabalhista
                          FROM jur.preposto
                          join jur.audiencia_processo aud_proc on aud_proc.cod_preposto in
                                                                  (nvl(jur.preposto.cod_preposto, 0),
                                                                   nvl(jur.preposto.cod_preposto,0))
                          join jur.processo proc on proc.cod_processo = aud_proc.cod_processo
                                                and proc.cod_tipo_processo = 2
                         WHERE jur.preposto.ind_preposto_trabalhista = 'N') a on a.Id = jur.preposto.cod_preposto
 
 
                         WHERE jur.preposto.ind_preposto_trabalhista = 'S' or a.preposto_trabalhista = 'N'
                         order by Descricao";

            return await this.context.Query<PrepostoDTO>().FromSql(sql).ToListAsync();

        }

        /// <summary>
        /// Esse método retorna os Prepostos/Prepostos Acompanhamtes  ativos  
        /// ele pode ou não ser filtrado por tipo de processo
        /// </summary>
        public async Task<IEnumerable<PrepostoDTO>> ListarPreposto(long? tipoProcesso)
        {
            var preposto = dbContext.Preposto.AsQueryable();

            IEnumerable<PrepostoDTO> result;

            if (tipoProcesso == (long)TipoProcessoEnum.Trabalhista)
            {
                preposto = preposto.Where(r => r.IndicaPrepostoTrabalhista == true).OrderBy(p => p.NomePreposto);
            }

            result = await preposto.Select(p => new PrepostoDTO
            {
                Id = p.Id,
                Descricao = p.IndicaPrepostoAtivo == false ? string.Format("{0} {1}", p.NomePreposto, "[Inativo]") : string.Format("{0}", p.NomePreposto)
            }).ToListAsync();
           
            return result;
        }
    }
}
