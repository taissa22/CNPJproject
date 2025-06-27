using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFornecedores;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class FornecedorContigenciaRepository : BaseCrudRepository<Fornecedor, long>, IFornecedorContigenciaRepository
    {
        private readonly JuridicoContext dbContext;
        private readonly ParametroRepository parametroRepository;

        public FornecedorContigenciaRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
            parametroRepository = new ParametroRepository(dbContext, user);
        }
        private IQueryable<Fornecedor> QueryFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO, Parametro parametroExcecao)
        {
            
            IQueryable<Fornecedor> query = dbContext.Fornecedor;

            
            if (!string.IsNullOrEmpty(fornecedorContigenciaConsultaDTO.nome))
                query = query.Where(q => q.NomeFornecedor.ToUpper().Trim().Contains(fornecedorContigenciaConsultaDTO.nome.ToUpper().Trim()));
            if (!string.IsNullOrEmpty(fornecedorContigenciaConsultaDTO.codigo))
                query = query.Where(q => q.CodigoFornecedorSAP.Contains(fornecedorContigenciaConsultaDTO.codigo));
            if (!string.IsNullOrEmpty(fornecedorContigenciaConsultaDTO.cnpj))
                query = query.Where(q => q.NumeroCNPJ.Contains(fornecedorContigenciaConsultaDTO.cnpj));

            if (fornecedorContigenciaConsultaDTO.statusFornecedor == 1) 
            {
                query = query.Where(q => q.IndicaAtivoSAP == true);
            }
            else if (fornecedorContigenciaConsultaDTO.statusFornecedor == 2)
            {
                query = query.Where(q => q.IndicaAtivoSAP == false);
            }

            query = query.Where(p => p.CodigoTipoFornecedor == Convert.ToInt32(parametroExcecao.Conteudo));

            return query;
        }
        public async Task<ICollection<FornecedorContigenciaResultadoDTO>> ConsultarFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO)
        {
            Parametro parametroExcecao = await parametroRepository.RecuperarPorId("TP_FORNECEDOR_SAP");

            IQueryable<Fornecedor> query = QueryFornecedorContigencia(fornecedorContigenciaConsultaDTO, parametroExcecao);
            var result = await query

                .Select(q => new FornecedorContigenciaResultadoDTO()
                {
                    Id = q.Id,
                    Codigo = q.CodigoFornecedorSAP,
                    Nome = q.NomeFornecedor,
                    CNPJ = q.NumeroCNPJ,
                    DataVencimentoCartaFianca = q.DataCartaFianca,
                    ValorCartaFianca = q.ValorCartaFianca,
                    StatusFornecedor = q.IndicaAtivoSAP == true ? 1 : 2
                }).ToListAsync();

            return result.OrderBy(ct => ct.Nome).AsQueryable()
                         .OrdenarPorPropriedade(fornecedorContigenciaConsultaDTO.Ascendente, fornecedorContigenciaConsultaDTO.Ordenacao, "nome")
                         .Paginar(fornecedorContigenciaConsultaDTO.Pagina, fornecedorContigenciaConsultaDTO.Quantidade).ToList(); 
         }
        public async Task<ICollection<FornecedorContigenciaExportarDTO>> ExportarFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO)
        {
            Parametro parametroExcecao = await parametroRepository.RecuperarPorId("TP_FORNECEDOR_SAP");
            IQueryable<Fornecedor> query = QueryFornecedorContigencia(fornecedorContigenciaConsultaDTO, parametroExcecao);

            return await query
                .Select(q => new FornecedorContigenciaExportarDTO()
                {
                    Id = q.Id,
                    Codigo = q.CodigoFornecedorSAP,
                    Nome = q.NomeFornecedor,
                    CNPJ = q.NumeroCNPJ,
                    DataVencimentoCartaFianca = q.DataCartaFianca,
                    ValorCartaFianca = q.ValorCartaFianca.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR")),
                    StatusFornecedor = q.IndicaAtivoSAP == true ? "Ativo" : "Inativo"
                }).ToListAsync();

            
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<int> RecuperarTotalRegistros(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO)
        {
            Parametro parametroExcecao = await parametroRepository.RecuperarPorId("TP_FORNECEDOR_SAP");

            return await QueryFornecedorContigencia(fornecedorContigenciaConsultaDTO, parametroExcecao).CountAsync();
        }
    }
}