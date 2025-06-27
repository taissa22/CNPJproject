using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.Compartilhado.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class Empresas_SapRepository : BaseCrudRepository<Empresas_Sap, long>, IEmpresas_SapRepository
    {
        private readonly JuridicoContext dbContext;

        public Empresas_SapRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<EmpresaSapDTO>> ExportarEmpresasSap(Empresas_SapFiltroDTO filtroDTO)
        {
            IQueryable<Empresas_Sap> query = WhereEmpresaSap(filtroDTO);
            var result = await query

               .Select(obj => new EmpresaSapDTO()
               {
                   Id = obj.Id,
                   Nome = obj.Nome,
                   Sigla = obj.Sigla,
                   CodigoOrganizacaocompra = obj.CodigoOrganizacaoCompra,
                   IndicaAtivo = obj.IndicaAtivo,
                   IndicaEnvioArquivoSolicitacao = obj.IndicaEnvioArquivoSolicitacao
               }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<EmpresaSapDTO>> RecuperarEmpresasPorFiltro(Empresas_SapFiltroDTO filtros)
        {
            IQueryable<Empresas_Sap> query = WhereEmpresaSap(filtros);
            var result = await query

               .Select(obj => new EmpresaSapDTO()
               {
                   Id = obj.Id,
                   Nome = obj.Nome,
                   Sigla = obj.Sigla,
                   CodigoOrganizacaocompra = obj.CodigoOrganizacaoCompra,
                   IndicaAtivo = obj.IndicaAtivo,
                   IndicaEnvioArquivoSolicitacao = obj.IndicaEnvioArquivoSolicitacao
               }).ToListAsync();
            

            var resultado = result.OrderBy(ct => ct.Nome)
                                  .AsQueryable()
                                  .OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "Nome")
                                  .Paginar(filtros.Pagina, filtros.Quantidade)
                                  .ToList();

            return resultado;
        }

        public async Task<bool> EmpresaComSiglaJaCadastrada(Empresas_Sap empresa)
        {
            var ret = await context.Set<Empresas_Sap>()
                           .AsNoTracking()
                           .FirstOrDefaultAsync(emp => emp.Sigla.ToUpper().Equals(empresa.Sigla.ToUpper()));
            //caso esteja atualizando e o objeto retornado seja o mesmo da model
            if (empresa.Id > 0 && ret != null && empresa.Id == ret.Id)
                return false;

            return ret != null;
        }

        public async Task CadastrarEmpresa(Empresas_Sap model)
        {
            await base.Inserir(model);
        }

        public async Task<Empresas_Sap> AtualizarEmpresa(Empresas_Sap model)
        {
            await base.Atualizar(model);

            return model;
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(Empresas_SapFiltroDTO filtroDTO)
        {
            IQueryable<Empresas_Sap> query = WhereEmpresaSap(filtroDTO);
            var resultado = await query.CountAsync();

            return resultado;
            
        }

        public async Task<bool> EmpresaAssociadaNaEmpresaDoGrupo(Empresas_Sap entidade)
        {
            //método utilizado somente na alteração de empresa Sap
            var resultado = await context.Set<Parte>()
                .AsNoTracking()
                .Include(p => p.Empresa_Sap)
                .FirstOrDefaultAsync(l => l.CodigoEmpresaSap == entidade.Id &&
                 l.TipoParte == "E");
            var ret = resultado != null && resultado.Empresa_Sap.Sigla != entidade.Sigla;
            return ret;
        }

        private IQueryable<Empresas_Sap> WhereEmpresaSap(Empresas_SapFiltroDTO filtros)
        {
            IQueryable<Empresas_Sap> query = dbContext.Empresas_sap;

            if (!string.IsNullOrEmpty(filtros.Descricao))
                query = query.Where(q => q.Nome.ToUpper().Trim().Contains(filtros.Descricao.ToUpper().Trim()));

            return query;
        }

    }
}