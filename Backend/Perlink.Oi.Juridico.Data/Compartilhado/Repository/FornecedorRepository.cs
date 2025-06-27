using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Shared.Data;
using Shared.Domain.Interface;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class FornecedorRepository : BaseCrudRepository<Fornecedor, long>, IFornecedorRepository
    {
        private readonly ParametroRepository parametroRepository;

        public FornecedorRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
            parametroRepository = new ParametroRepository(context, user);
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        private IQueryable<Fornecedor> IQueryableFiltroConsultaFornecedor(FornecedorFiltroDTO filtros)
        {
            IQueryable<Fornecedor> query = context.Set<Fornecedor>();

            if (!string.IsNullOrEmpty(filtros.NomeFornecedor))
                query = query.Where(p => p.NomeFornecedor.ToUpper().Trim().Contains(filtros.NomeFornecedor.ToUpper().Trim()));

            if (!string.IsNullOrEmpty(filtros.CodigoFornecedorSAP))
                query = query.Where(p => p.CodigoFornecedorSAP == filtros.CodigoFornecedorSAP);

            if (filtros.CodigoBanco.HasValue && filtros.CodigoBanco > 0)
                query = query.Where(p => p.CodigoBanco == filtros.CodigoBanco);

            if (filtros.CodigoEscritorio.HasValue && filtros.CodigoEscritorio > 0)
                query = query.Where(p => p.CodigoEscritorio == filtros.CodigoEscritorio);

            if (filtros.CodigoProfissional.HasValue && filtros.CodigoProfissional > 0)
                query = query.Where(p => p.CodigoProfissional == filtros.CodigoProfissional);

            if (filtros.CodigoTipoFornecedor.HasValue && filtros.CodigoTipoFornecedor > 0)
                query = query.Where(p => p.CodigoTipoFornecedor == filtros.CodigoTipoFornecedor);

            query = query.Where(p => p.CodigoTipoFornecedor != 4);

            return query;
        }

        public async Task<ICollection<FornecedorExportarDTO>> ExportarFornecedores(FornecedorFiltroDTO fornecedorFiltroDTO)
        {
            IQueryable<Fornecedor> query = IQueryableFiltroConsultaFornecedor(fornecedorFiltroDTO);

            var result = await query.Select(fornecedor => new FornecedorExportarDTO()
            {
                Banco = fornecedor.Banco.NomeBanco,
                Id = fornecedor.Id,
                CodigoFornecedorSap = fornecedor.CodigoFornecedorSAP,
                Escritorio = fornecedor.Escritorio.NomeProfissional,
                profissional = fornecedor.Profissional.NomeProfissional,
                NomeFornecedor = fornecedor.NomeFornecedor,
                TipoFornecedor = DescricaoFornecedor(fornecedor.CodigoTipoFornecedor)
            }).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<FornecedorResultadoDTO>> FiltroConsultaFornecedor(FornecedorFiltroDTO filtros)
        {
            IQueryable<Fornecedor> query = IQueryableFiltroConsultaFornecedor(filtros);

            var result = query.Select(fornecedor => new FornecedorResultadoDTO()
            {
                NomeBanco = fornecedor.Banco.NomeBanco,
                Id = fornecedor.Id,
                CodigoBanco = fornecedor.CodigoBanco,
                CodigoEscritorio = fornecedor.CodigoEscritorio,
                CodigoProfissional = fornecedor.CodigoProfissional,
                CodigoTipoFornecedor = fornecedor.CodigoTipoFornecedor,
                CodigoFornecedorSap = fornecedor.CodigoFornecedorSAP,
                NomeEscritorio = fornecedor.Escritorio.NomeProfissional,
                NomeProfissional = fornecedor.Profissional.NomeProfissional,
                NomeFornecedor = fornecedor.NomeFornecedor,
                NomeTipoFornecedor = DescricaoFornecedor(fornecedor.CodigoTipoFornecedor)
            });

            result = result
                    .OrdenarPorPropriedade(filtros.Ascendente, filtros.Ordenacao, "NomeFornecedor")
                    .Paginar(filtros.Pagina, filtros.Quantidade);

            return await result.ToListAsync();
        }

        private string DescricaoFornecedor(long codigoTipoFornecedor)
        {
            switch (codigoTipoFornecedor)
            {
                case (int)TipoFornecedorEnum.Banco:
                    return TipoFornecedorEnum.Banco.Descricao();

                case (int)TipoFornecedorEnum.Escritorio:
                    return TipoFornecedorEnum.Escritorio.Descricao();

                case (int)TipoFornecedorEnum.Profissional:
                    return TipoFornecedorEnum.Profissional.Descricao();

                default:
                    return "";
            }
        }

        public async Task<Fornecedor> FornecedorComCodigoSAPJaCadastrado(string codigoSAP)
        {
            var fornecedor = await context.Set<Fornecedor>()
               .Where(filtro => filtro.CodigoFornecedorSAP.Equals(codigoSAP))
               .AsNoTracking()
               .FirstOrDefaultAsync();

            return fornecedor;
        }

        public async Task<Fornecedor> CadastrarFornecedor(Fornecedor fornecedor)
        {
            await base.Inserir(fornecedor);
            return fornecedor;
        }

        public async Task<Fornecedor> AtualizarFornecedor(Fornecedor modelo)
        {
            await base.Atualizar(modelo);
            return modelo;
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(FornecedorFiltroDTO fornecedorFiltroDTO)
        {
            return await IQueryableFiltroConsultaFornecedor(fornecedorFiltroDTO).CountAsync();
        }

        public async Task<Fornecedor> VerificarCodigoSap(string codigoSAP, long CodigoFornecedor)
        {
            var fornecedor = await context.Set<Fornecedor>()
               .Where(filtro => filtro.CodigoFornecedorSAP.Equals(codigoSAP) && filtro.Id.Equals(CodigoFornecedor))
               .AsNoTracking()
               .FirstOrDefaultAsync();

            return fornecedor;
        }

        public async Task<IEnumerable<FornecedorDTOFiltro>> RecuperarFornecedorParaFiltroLote()
        {
            Parametro parametroExcecao  = await parametroRepository.RecuperarPorId("TP_FORNECEDOR_SAP");
            
            var fornecedor = await context.Set<Fornecedor>()
              .Where(f =>  f.CodigoTipoFornecedor != Convert.ToInt32(parametroExcecao.Conteudo))
              .AsNoTracking()
              .Select(dto => new FornecedorDTOFiltro()
              {
                  Id = dto.Id,
                  Descricao = dto.NomeFornecedor,
                  CodigoFornecedorSAP = dto.CodigoFornecedorSAP
              }).OrderBy(p => p.Descricao).ToListAsync();
            return fornecedor;
        }
    }
}