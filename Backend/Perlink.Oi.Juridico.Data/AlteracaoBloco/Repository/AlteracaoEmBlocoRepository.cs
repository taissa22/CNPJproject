using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.DTO;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Entity;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Enum;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.AlteracaoBloco.Repository
{
    public class AlteracaoEmBlocoRepository : BaseCrudRepository<AlteracaoEmBloco, long>, IAlteracaoEmBlocoRepository
    {
        public AlteracaoEmBlocoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
        }

        public override async Task<AlteracaoEmBloco> RecuperarPorId(long id)
        {
            var resultado = await base.RecuperarPorId(Convert.ToInt64(id));
            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AlteracaoEmBlocoRetornoDTO>> ListarAgendamentos(int index, int count)
        {

            return await (from ab in context.Set<AlteracaoEmBloco>().AsNoTracking()
                                        join u in context.Set<Usuario>() on ab.CodigoDoUsuario equals u.Id
                                        orderby ab.Id descending
                                        select new AlteracaoEmBlocoRetornoDTO
                                        {
                                            Id = ab.Id,
                                            DataCadastro = ab.DataCadastro,
                                            DataExecucao = ab.DataExecucao,
                                            Arquivo = ab.Arquivo,
                                            Status = ab.Status,
                                            ProcessosAtualizados = ab.ProcessosAtualizados,
                                            ProcessosComErro = ab.ProcessosComErro,
                                            CodigoTipoProcesso = ab.CodigoTipoProcesso,
                                            CodigoDoUsuario = ab.CodigoDoUsuario,
                                            NomeUsuario = u.Nome 
                                        })
                                        .Skip((index - 1) * count)
                                        .Take(count)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<AlteracaoEmBloco>> ListarAgendamentosComStatusAgendado()
        {
            return await context.Set<AlteracaoEmBloco>()
                                .AsNoTracking()
                                .Where(c => c.Status == AlteracaoEmBlocoEnum.Agendado)
                                .OrderBy(c => c.DataCadastro)
                                .ToListAsync() ;

        }
        public async Task<IEnumerable<AlteracaoEmBlocoRetornoDTO>> ListarAgendamentosPorUsuario(int index, int count, string usuario)
        {
            return await (from ab in context.Set<AlteracaoEmBloco>().AsNoTracking()
                                        join u in context.Set<Usuario>() on ab.CodigoDoUsuario equals u.Id
                                        orderby ab.Id descending
                                        where ab.CodigoDoUsuario == usuario
                                        select new AlteracaoEmBlocoRetornoDTO
                                        {
                                            Id = ab.Id,
                                            DataCadastro = ab.DataCadastro,
                                            DataExecucao = ab.DataExecucao,
                                            Arquivo = ab.Arquivo,
                                            Status = ab.Status,
                                            ProcessosAtualizados = ab.ProcessosAtualizados,
                                            ProcessosComErro = ab.ProcessosComErro,
                                            CodigoTipoProcesso = ab.CodigoTipoProcesso,
                                            CodigoDoUsuario = ab.CodigoDoUsuario,
                                            NomeUsuario = u.Nome 
                                        })
                                        .Skip((index - 1) * count)
                                        .Take(count)
                                        .ToListAsync();

        }

        public async Task<int> QuantidadeTotal()
        {
            return await context.Set<AlteracaoEmBloco>()
                                 .AsNoTracking()
                                 .CountAsync();
        }

        public async Task<int> QuantidadeTotalPorUsuario(string usuario)
        {
            return await context.Set<AlteracaoEmBloco>()
                                 .AsNoTracking()
                                 .Where(c => c.CodigoDoUsuario == usuario)
                                 .CountAsync();
        }

        public async Task<IEnumerable<AlteracaoEmBloco>> ExpurgoAlteracaoEmBloco(int parametro)
        {
            var resultado = await context.Set<AlteracaoEmBloco>()
                .AsNoTracking()
                .Where(a => Convert.ToDateTime(a.DataCadastro).Date < DateTime.Now.Date.AddDays(parametro * -1))
                .ToListAsync();

            return resultado;
        }
    }
}
