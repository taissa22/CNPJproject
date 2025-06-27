using DocumentFormat.OpenXml.Drawing;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Repositories;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.RequestDTOs;
using Perlink.Oi.Juridico.Application.Security;
using System.Collections.Generic;
using System.Linq;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Services
{
    public class ESocialDashboardService
    {
        private readonly ESocialDashboardRepository _dashboardRepository;

        public ESocialDashboardService(ESocialDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<(ESocialDashboardDTO? dadosDashboad, string? mensagemErro)>  ObtemDadosDashboard(ESocialDashboardRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                var naoFiltrarEmpresas = requestDTO.IdsEmpresaAgrupadoras == null || requestDTO.IdsEmpresaAgrupadoras.Count == 0;
                var filtroEmpresas = requestDTO.IdsEmpresaAgrupadoras != null ? requestDTO.IdsEmpresaAgrupadoras : new List<int> { };

                var naoFiltrarEstados = requestDTO.UFs == null || requestDTO.UFs.Count == 0;
                var filtroEstados = requestDTO.UFs != null ? requestDTO.UFs : new List<string> { };

                var anoAtual = DateTime.Today.Year;
                var mesAtual = DateTime.Today.Month;

                var mesAnterior = mesAtual - 1;

                var dataPeriodoVigenteInicial = DateTime.Today.Day > 15 ? new DateTime(anoAtual, mesAtual, 1) : new DateTime(anoAtual, mesAnterior, 1);
                var dataPeriodoVigenteFinal = DateTime.Today.Day > 15 ? new DateTime(anoAtual, mesAtual, DateTime.DaysInMonth(anoAtual, mesAtual)) : new DateTime(anoAtual, mesAnterior, DateTime.DaysInMonth(anoAtual, mesAnterior));

                var dadosDashboard = await _dashboardRepository.ObtemDadosDashboard(dataPeriodoVigenteInicial, dataPeriodoVigenteFinal, naoFiltrarEmpresas, filtroEmpresas, naoFiltrarEstados, filtroEstados, ct);

                return (dadosDashboard, null);           

            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }     

        public async Task<(int? quantidadeErrosEnvio, string? mensagemErro)> ObetemQuantidadeErrosEnvio(ESocialDashboardRequestDTO requestDTO, CancellationToken ct)
        {
            try
            {
                var naoFiltrarEmpresas = requestDTO.IdsEmpresaAgrupadoras == null || requestDTO.IdsEmpresaAgrupadoras.Count == 0;
                var filtroEmpresas = requestDTO.IdsEmpresaAgrupadoras ?? (new List<int> { });

                var naoFiltrarEstados = requestDTO.UFs == null || requestDTO.UFs.Count == 0;
                var filtroEstados = requestDTO.UFs ?? (new List<string> { });


                var quantidadeErrosEnvio = await _dashboardRepository.ObetemQuantidadeErrosEnvio(naoFiltrarEmpresas, filtroEmpresas, naoFiltrarEstados, filtroEstados, ct);

                return (quantidadeErrosEnvio, null);

            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }


        public async Task<(List<RetornoListaDefaultDTO>? listaEmpresas, string? mensagemErro)> ObtemFiltroEmpresaAgrupadora(string loginUsuario, CancellationToken ct)
        {
            try
            {
                List<RetornoListaDefaultDTO>? listaEmpresas = await _dashboardRepository.ObtemFiltroEmpresaAgrupadora(loginUsuario, ct);
                return (listaEmpresas, null);
            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }

        public async Task<(List<RetornoListaUFDTO>? listaUFs, string? mensagemErro)> ObtemFiltroUF(CancellationToken ct)
        {
            try
            {
                List<RetornoListaUFDTO>? listaUFs = await _dashboardRepository.ObtemFiltroUF(ct);
                return (listaUFs, null);

            }
            catch (Exception e)
            {
                return (null, e.Message);
            }
        }

    }
}
