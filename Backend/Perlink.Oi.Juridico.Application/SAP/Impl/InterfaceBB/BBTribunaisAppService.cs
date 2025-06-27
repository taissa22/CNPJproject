using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Data;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB.BBTribunaisViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.Impl.InterfaceBB
{
    public class BBTribunaisAppService : BaseCrudAppService<BBTribunaisViewModel, BBTribunais, long>, IBBTribunaisAppService
    {
        private readonly IBBTribunaisService service;
        private readonly IMapper mapper;
        private readonly IBBOrgaosService orgaosService;

        public BBTribunaisAppService(IBBTribunaisService service, IBBOrgaosService orgaosService, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.orgaosService = orgaosService;
        }

        public async Task<IPagingResultadoApplication<ICollection<BBTribunaisViewModel>>> ConsultarBBTribunais(FiltrosDTO filtros)
        {
            var result = new PagingResultadoApplication<ICollection<BBTribunaisViewModel>>();

            try
            {
                var listaOrdenada = await service.ObterPorFiltroPaginado(filtros);
                         
                result.DefinirData(mapper.Map<ICollection<BBTribunaisViewModel>>(listaOrdenada));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
                result.Total = await service.ObterQuantidadeTotalPorFiltro(filtros);
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarBBTribunais(FiltrosDTO filtros)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {                
                filtros.Pagina = 0;
                filtros.Quantidade = 0;

                var lista = await service.ObterPorFiltroPaginado(filtros);
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
                    csv.WriteRecords(mapper.Map<ICollection<BBTribunaisExportarViewModel>>(lista));
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

        public async Task<IResultadoApplication> ExcluirBBTribunais(long Id)
        {
            {
                var resultado = new ResultadoApplication();
                try
                {
                    if (await orgaosService.OrgaoBBAssociadocomTribunais(Id))
                        throw new Exception("Não será possível excluir o Tribunal BB selecionado, pois se encontra relacionado com Orgão BB.");
                    await service.RemoverPorId(Id);
                    service.Commit();

                    resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
                    resultado.ExecutadoComSuccesso();                    
                }
                catch (Exception ex)
                {
                    resultado.ExibirMensagem(ex.Message);
                    resultado.ExecutadoComErro(ex);
                    resultado.ExibeNotificacao = true;
                }
                return resultado;
            }
        }

        public async Task<IResultadoApplication> SalvarBBTribunais(BBTributarioInclusaoEdicaoDTO tributarioInclusaoEdicaoDTO)
        {
            var result = new ResultadoApplication();

            try
            {
                var entidade = mapper.Map<BBTribunais>(tributarioInclusaoEdicaoDTO);

                if (await service.VerificarDuplicidadeTribunalnalBB(entidade))
                    throw new Exception("O Código Tribunal BB já está cadastrado em outro registro.");

                Validar(entidade);
                if (entidade.Id == 0)
                    await service.Inserir(entidade);
                else
                    await service.Atualizar(entidade);

                service.Commit();
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        private void Validar(BBTribunais objCategoriaPagamento)
        {
            if (!objCategoriaPagamento.Validar().IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var error in objCategoriaPagamento.Validar().Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }

                throw new Exception(stringBuilder.ToString());
            }
        }
    }
}