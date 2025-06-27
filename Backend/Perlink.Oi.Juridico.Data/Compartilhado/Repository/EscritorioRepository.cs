using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class EscritorioRepository : BaseCrudRepository<Profissional, long>, IEscritorioRepository
    {
        private readonly JuridicoContext dbContext;
        public EscritorioRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
            dbContext = context;
        }
        public async Task<ICollection<EscritorioDTO>> RecuperarAreaCivilConsumidor()
        {
            var resultado = await context.Set<Profissional>()
                 .Where(a => a.IndicadorEscritorio == true && a.IndicadorAreaCivel == true && a.IndicadorAtivo == true)
                 .AsNoTracking()
                 .OrderBy(x => x.Id)
                 .Select(dto => new EscritorioDTO()
                 {
                     Id = dto.Id,
                     Descricao = dto.NomeProfissional
                 }).ToListAsync();

            return resultado;
        }

        public async Task<ICollection<EscritorioDTO>> RecuperarAreaCivelEstrategico()
        {
            var resultado = await context.Set<Profissional>()
                 .Where(a => a.IndicadorEscritorio == true && a.IndicadorCivelEstrategico == true && a.IndicadorAtivo == true)
                 .AsNoTracking()
                 .OrderBy(x => x.Id)
                  .Select(dto => new EscritorioDTO()
                  {
                      Id = dto.Id,
                      Descricao = dto.NomeProfissional
                  }).ToListAsync();
            ;

            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }
        public async Task<ICollection<EscritorioDTO>> RecuperarAreaProcon()
        {
            var resultado = await context.Set<Profissional>()
                 .Where(a => a.IndicadorEscritorio == true && a.IndicadorProcom == true && a.IndicadorAtivo == true)
                 .AsNoTracking()
                 .OrderBy(x => x.Id)
                  .Select(dto => new EscritorioDTO()
                  {
                      Id = dto.Id,
                      Descricao = dto.NomeProfissional
                  }).ToListAsync(); ;

            return resultado;
        }

        public async Task<ICollection<EscritorioDTO>> RecuperarEscritorioFiltro(long codigoTipoProcesso)
        {
            var resultado = await context.Set<Profissional>()
                    .Where(e => e.IndicadorEscritorio == true)
                    .Where(e => (codigoTipoProcesso == (int)TipoProcessoEnum.CivelConsumidor && e.IndicadorAreaCivel == true) ||
                                ((codigoTipoProcesso == (int)TipoProcessoEnum.Trabalhista || codigoTipoProcesso == (int)TipoProcessoEnum.TrabalhistaAdministrativo) && e.IndicadorAreaTrabalhista == true) ||
                                (codigoTipoProcesso == (int)TipoProcessoEnum.Administrativo && e.IndicadorAreaRegulatoria == true) ||
                                (codigoTipoProcesso == (int)TipoProcessoEnum.Pex && e.IndicadorPex == true) ||
                                ((codigoTipoProcesso == (int)TipoProcessoEnum.TributarioAdministrativo || codigoTipoProcesso == (int)TipoProcessoEnum.TributarioJudicial) && e.IndicadorAreaTributaria == true) ||
                                ((codigoTipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial || codigoTipoProcesso == (int)TipoProcessoEnum.Expressinho) && e.IndicadorAreaJuizado == true) ||
                                (codigoTipoProcesso == (int)TipoProcessoEnum.CivelEstrategico && e.IndicadorCivelEstrategico == true)
                    )
                    .AsNoTracking()
                    .Select(dto => new EscritorioDTO()
                    {
                        Id = dto.Id,
                        Descricao = dto.NomeProfissional
                    })
                    .ToListAsync();
            resultado.Add(new EscritorioDTO()
            {
                Id = 0,
                Descricao = "Sem Escritório"
            });
            var resultadoOrdenado = resultado.OrderBy(e => e.Descricao).ToList();

            return resultadoOrdenado;
        }

        public async Task<ICollection<EscritorioDTO>> RecuperarEscritorioTrabalhistaFiltro()
        {
            //TODO: Union no Linq
            string sql = @"SELECT
                         p.cod_profissional Id,
                           p.nom_profissional || case
                             when p.ind_ativo is null then
                              'S'
                             when p.ind_ativo = '' then
                              ' [Inativo]'
                           end as Descricao
                      FROM jur.profissional p
                      left join (SELECT jur.profissional.cod_profissional Id,
                                        jur.profissional.nom_profissional || ' [Inativo]' as Descricao
                                   FROM jur.profissional
                                   join jur.audiencia_processo aud_proc on jur.profissional.cod_profissional in
                                                                           (aud_proc.adves_cod_profissional,
                                                                            aud_proc.adves_cod_profissional_acomp)
                                   join jur.processo proc on proc.cod_processo = aud_proc.cod_processo and proc.cod_tipo_processo = 2
                                  WHERE (jur.profissional.ind_escritorio = 'S')
                                    AND (jur.profissional.ind_area_trabalhista = 'N')) a on a.Id =
                                                                                            p.cod_profissional
                     WHERE p.ind_escritorio = 'S'
                       AND p.ind_area_trabalhista = 'S'
                     order by Descricao";

            return await this.context.Query<EscritorioDTO>().FromSql(sql).ToListAsync();
        }

        public async Task<ICollection<EscritorioDTO>> RecuperarEscritorioTrabalhistaFiltroUsuarioEscritorio(string codUsuario)
        {
            string sql = @"SELECT p.cod_profissional Id,
                                  p.nom_profissional || case
                                   when p.ind_ativo is null then
                                    'S'
                                   when p.ind_ativo = '' then
                                    ' [Inativo]' end as Descricao
                            FROM jur.profissional p
                            inner join jur.aca_usuario_escritorio uses on uses.cod_profissional = p.cod_profissional
                            inner join jur.aca_usuario us on us.cod_usuario = uses.cod_usuario
                            left join (SELECT jur.profissional.cod_profissional Id,
                                              jur.profissional.nom_profissional || ' [Inativo]' as Descricao
                                         FROM jur.profissional
                                         join jur.audiencia_processo aud_proc on jur.profissional.cod_profissional in
                                                                                 (aud_proc.adves_cod_profissional,
                                                                                  aud_proc.adves_cod_profissional_acomp)
                                         join jur.processo proc on proc.cod_processo = aud_proc.cod_processo and proc.cod_tipo_processo = 2
                                        WHERE (jur.profissional.ind_escritorio = 'S')
                                          AND (jur.profissional.ind_area_trabalhista = 'N')) a on a.Id =
                                                                                                  p.cod_profissional
                            WHERE p.ind_escritorio = 'S'
                             AND p.ind_area_trabalhista = 'S'
                             and us.cod_usuario = '" + codUsuario + @"'
                            order by Descricao";

            return await this.context.Query<EscritorioDTO>().FromSql(sql).ToListAsync();
        }

        public async Task<ICollection<EscritorioDTO>> RecuperarEscritorioTrabalhista()
        {
            var escritorio = await dbContext.Profissional
                .Where(p => p.IndicadorEscritorio == true
                && p.IndicadorAreaTrabalhista == true)
                .Select(s => new EscritorioDTO
                {
                    Id = s.Id,
                    Descricao = (s.IndicadorAtivo == false ? string.Format("{0} {1}", s.NomeProfissional, "[Inativo]") : s.NomeProfissional)
                }
                ).ToListAsync();

            return escritorio;
        }
    }
}
