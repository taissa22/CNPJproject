using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos;
using Perlink.Oi.Juridico.Data;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Impl.InterfaceBB
{
    public class BBComarcaAppService : BaseCrudAppService<BBComarcaViewModel, BBComarca, long>, IBBComarcaAppService
    {
        private readonly IBBComarcaService service;
        private readonly IBBOrgaosService bbOrgaoBBService;
        private readonly IComarcaService comarcaService;

        private readonly IMapper mapper;

        public BBComarcaAppService(IBBComarcaService service, IMapper mapper,
                    IBBOrgaosService bbOrgaoBBService,
                    IComarcaService comarcaService) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.bbOrgaoBBService = bbOrgaoBBService;
            this.comarcaService = comarcaService;
        }

        public async Task<IPagingResultadoApplication<ICollection<BBComarcaViewModel>>> ConsultarBBComarca(FiltrosDTO filtroDTO)
        {
            var result = new PagingResultadoApplication<ICollection<BBComarcaViewModel>>();
             
            try
            {

                var model = await service.ConsultarBBComarca(filtroDTO);
                result.Total = await service.TotalBBComarca(filtroDTO);
                result.DefinirData(mapper.Map<ICollection<BBComarcaViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
               
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<ICollection<ComboboxViewModel<int>>>> ConsultarComarcaPorEstado(string codigoEstado)
        {
            var result = new ResultadoApplication<ICollection<ComboboxViewModel<int>>>();

            try
            {
                var listaOrdenada = await service.Pesquisar()
                        .Where(e => e.CodigoEstado == codigoEstado)
                        .OrderBy(e => e.Descricao)
                        .ToListAsync();
                result.DefinirData(mapper.Map<ICollection<ComboboxViewModel<int>>>(listaOrdenada));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication> AlterarBBComarca(BBComarcaViewModel objeto)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<BBComarca>(objeto);
            try
            {
                await ValidarAsync(entidade);
                if (!objeto.ConfirmaCardastro && !(await service.UFIsValid(objeto.CodigoEstado)))
                {
                    result.ExibeNotificacao = true;
                    result.ExibirMensagem("O código da UF informado não é válido. Deseja continuar assim mesmo?");
                    return result;
                }
                await service.Atualizar(entidade);
                service.Commit();
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Alteracao);
                result.ExecutadoComSuccesso();
            }
            catch (Exception e)
            {
                result.ExecutadoComErro(e);
            }
            return result;
        }

        public async Task<IResultadoApplication> CadastrarBBComarca(BBComarcaViewModel objeto)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<BBComarca>(objeto);
            try
            {
                await ValidarAsync(entidade);
                if (!objeto.ConfirmaCardastro && !(await service.UFIsValid(objeto.CodigoEstado)))
                {
                    result.ExibeNotificacao = true;
                    result.ExibirMensagem("O código da UF informado não é válido. Deseja continuar assim mesmo?");
                    return result;
                }
                await service.Inserir(entidade);
                service.Commit();
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
                result.ExecutadoComSuccesso();
            }
            catch (Exception e)
            {
                result.ExecutadoComErro(e);
            }
            return result;
        }

        public async Task<IResultadoApplication> ExcluirBBComarca(long codigo)
        {
            var resultado = new ResultadoApplication();
            try
            {
                if (await bbOrgaoBBService.ExisteBBComarcaAssociadoBBOrgao(codigo))
                {
                    throw new Exception("Não será possível excluir a Comarca BB\n selecionada, pois se encontra relacionada com\n Órgão BB.");
                }
                if (await comarcaService.ExisteBBComarcaAssociadoComarca(codigo))
                {
                    throw new Exception("Não será possível excluir a Comarca BB\n selecionada, pois se encontra relacionada com\n Comarca.");
                }

                await service.RemoverPorId(codigo);
                service.Commit();

                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExibirMensagem(ex.Message);
                resultado.ExecutadoComErro(ex);
            }
            return resultado;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarBBComarca(FiltrosDTO filtroDTO)
        {
            var result = new ResultadoApplication<byte[]>();

            try
            {
                var model = await service.ExportarBBComarca(filtroDTO);
                byte[] dados;
                var csvConfiguration = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
                {
                    Delimiter = ";",
                    TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true
                };

                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var csv = new CsvWriter(streamWriter, csvConfiguration))
                {
                    csv.WriteRecords(mapper.Map<ICollection<BBComarcaExportarViewModel>>(model));
                    streamWriter.Flush();
                    dados = memoryStream.ToArray();
                }

                result.DefinirData(dados);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        private async Task ValidarAsync(BBComarca obj)
        {
            if (!obj.Validar().IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var error in obj.Validar().Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }

                throw new Exception(stringBuilder.ToString());
            }
            if (await service.CodigoBBJaExiste(obj))
                throw new Exception("O Código Comarca BB já está cadastrado em outro registro.");
        }
    }
}