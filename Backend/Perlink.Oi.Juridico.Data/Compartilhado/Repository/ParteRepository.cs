using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class ParteRepository : BaseCrudRepository<Parte, long>, IParteRepository
    {
        private readonly JuridicoContext dbContext;
        private readonly ParametroRepository parametroRepository;

        public ParteRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
            this.parametroRepository = new ParametroRepository(dbContext, user);
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExisteParteComEmpresaSap(long codigoEmpresaSap)
        {
            var resultado = await context.Set<Parte>()

                .AnyAsync(l => l.CodigoEmpresaSap == codigoEmpresaSap &&
                 l.TipoParte == "E")
                ;
            return resultado;
        }

        public async Task<ICollection<PartesDTO>> RecuperarPartesTrabalhistaPorCodigoProcesso(long codigoProcesso, bool autores)
        {
            var parte = dbContext.Parte;
            var parteProcesso = dbContext.ParteProcesso;
            var tipoParticipacao = dbContext.TipoParticipacoes;

            ICollection<PartesDTO> result;

            if (autores)
            {
                result = await parteProcesso.Where(pp =>
                                   pp.CodigoParte == pp.Parte.Id &&
                                   pp.Id == codigoProcesso &&
                                   pp.CodigoTipoParticipacao == (long)TipoParticipacaoEnum.Reclamante)
                                   .Select(pp => new PartesDTO
                                   {
                                       CodigoParte = pp.CodigoParte,
                                       NomeParte = pp.Parte.Nome,
                                       CgcParte = pp.Parte.Cgc,
                                       CPF = pp.Parte.Cpf,
                                       CarteiraTrabalhoParte = pp.Parte.CarteiraTrabalho
                                   }).OrderBy(item => item.NomeParte)
                                   .ToListAsync();
            }
            else
            {
                result = await parteProcesso.Where(pp =>
                                  pp.CodigoParte == pp.Parte.Id &&
                                  pp.Id == codigoProcesso &&
                                  pp.CodigoTipoParticipacao == (long)TipoParticipacaoEnum.Reclamada)
                                  .Select(pp => new PartesDTO
                                  {
                                      CodigoParte = pp.CodigoParte,
                                      NomeParte = pp.Parte.Nome,
                                      CgcParte = pp.Parte.Cgc,
                                      CPF = pp.Parte.Cpf,
                                      CarteiraTrabalhoParte = pp.Parte.CarteiraTrabalho
                                  }).OrderBy(item => item.NomeParte)
                                  .ToListAsync();
            }

            return result;
        }

        public async Task<ICollection<Reclamantes_ReclamadasDTO>> RecuperarReclamanteReclamadas(LancamentoEstornoFiltroDTO lancamentoEstornoFiltroDTO, bool reclamadas, long Id)
        {
            ICollection<Reclamantes_ReclamadasDTO> result;
            if (lancamentoEstornoFiltroDTO.TipoProcesso == 2)
            {
                if (reclamadas == false)
                {
                    string nomeparamentro = "TP_RECLAMANTE";
                    IQueryable<ParteProcesso> query = IQueryableConsultaTrabalhista(nomeparamentro, Id);
                    result = await query.Select(pp => new Reclamantes_ReclamadasDTO
                    {
                        CodigoProcesso = pp.Id,
                        CodigoParte = pp.CodigoParte,
                        CodigoTipoParticipacao = pp.CodigoTipoParticipacao.ToString(),
                        NomeParte = pp.Parte.Nome,
                        CGCParte = pp.Parte.Cgc,
                        CPF = pp.Parte.Cpf,
                        CarteiradeTrabalho = pp.Parte.CarteiraTrabalho
                    }).ToListAsync();
                }
                else
                {
                    string nomeparamentro = "TP_RECLAMADA";
                    IQueryable<ParteProcesso> query = IQueryableConsultaTrabalhista(nomeparamentro, Id);
                    result = await query.Select(pp => new Reclamantes_ReclamadasDTO
                    {
                        CodigoProcesso = pp.Id,
                        CodigoParte = pp.CodigoParte,
                        CodigoTipoParticipacao = pp.CodigoTipoParticipacao.ToString(),
                        NomeParte = pp.Parte.Nome,
                        CGCParte = pp.Parte.Cgc,
                        CPF = pp.Parte.Cpf,
                        CarteiradeTrabalho = pp.Parte.CarteiraTrabalho
                    }).ToListAsync();
                }
            }
            else
            {
                if (reclamadas == false)
                {
                    string nomeparamentro = "TP_AUTOR";
                    IQueryable<ParteProcesso> query = IQueryableConsulta(nomeparamentro, Id);
                    result = await query.Select(pp => new Reclamantes_ReclamadasDTO
                    {
                        CodigoProcesso = pp.Id,
                        CodigoParte = pp.CodigoParte,
                        CodigoTipoParticipacao = pp.CodigoTipoParticipacao.ToString(),
                        NomeParte = pp.Parte.Nome,
                        CGCParte = pp.Parte.Cgc,
                        CPF = pp.Parte.Cpf,
                        CarteiradeTrabalho = pp.Parte.CarteiraTrabalho
                    }).ToListAsync();
                }
                else
                {
                    string nomeparamentro = "TP_REU";
                    IQueryable<ParteProcesso> query = IQueryableConsulta(nomeparamentro, Id);

                    result = await query.Select(pp => new Reclamantes_ReclamadasDTO
                    {
                        CodigoProcesso = pp.Id,
                        CodigoParte = pp.CodigoParte,
                        CodigoTipoParticipacao = pp.CodigoTipoParticipacao.ToString(),
                        NomeParte = pp.Parte.Nome,
                        CGCParte = pp.Parte.Cgc,
                        CPF = pp.Parte.Cpf,
                        CarteiradeTrabalho = pp.Parte.CarteiraTrabalho
                    }).ToListAsync();
                }
            }
            return result.OrderBy(p => p.NomeParte).ToList();
        }

        private IQueryable<ParteProcesso> IQueryableConsultaTrabalhista(string nomeparamentro, long Id)
        {
            IQueryable<ParteProcesso> query = context.Set<ParteProcesso>();

            query = query.Where(pp =>
                            pp.CodigoParte == pp.Parte.Id &&
                            pp.Id == Id &&
                            pp.CodigoTipoParticipacao == Convert.ToInt64(RecuperarConteudoParametroJuridico(nomeparamentro))
                    );

            return query;
        }

        private IQueryable<ParteProcesso> IQueryableConsulta(string nomeparamentro, long Id)
        {
            IQueryable<ParteProcesso> query = context.Set<ParteProcesso>();

            query = query.Where(pp =>
                            pp.CodigoParte == pp.Parte.Id &&
                            pp.Id == Id &&
                            pp.CodigoTipoParticipacao == Convert.ToInt64(RecuperarConteudoParametroJuridico(nomeparamentro))
                    );

            return query;
        }

        private string RecuperarConteudoParametroJuridico(string nomeparamentro)
        {
            return parametroRepository.RecuperarConteudoParametroJuridico(nomeparamentro);
        }

        public async Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto)
        {
            return await dbContext.Parte.AsNoTracking()
                        .AnyAsync(p => p.CodigoCentroCusto == codigoCentroCusto);
        }

        public async Task<IList<Parte>> ListarEmpresasSapNaoAssociadas()
        {
            return await dbContext.Parte.AsNoTracking()
                            .Where(p => !p.GrupoEmpresaContabilSapParte.Any(x => x.Id != p.Id)
                             && p.TipoParte == "E")
                            .OrderBy(p => p.Nome)
                            .ThenBy(x => x.Id)
                            .ToListAsync();
        }
    }
}