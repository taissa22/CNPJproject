using CsvHelper;
using CsvHelper.TypeConversion;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Service;
using Shared.Application.Impl;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Service {
    public class BaseFechamentoJecCompletaService : BaseCrudService<BaseFechamentoJecCompleta, long>, IBaseFechamentoJecCompletaService
    {

        public readonly IBaseFechamentoJecCompletaRepository repository;

        public BaseFechamentoJecCompletaService(IBaseFechamentoJecCompletaRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<FechamentoJecDisponivelDTO>> CarregarFechamentosDisponiveisParaAgendamento()
        {
            return await repository.CarregarFechamentosDisponiveisParaAgendamento();
        }
        public async Task<ICollection<ApuracaoOutliersDownloadBaseFechamentoDTO>> ListarBaseFechamento(long codEmpCentralizadora, DateTime mesAnoFechamento, DateTime dataFechamento)
        {
            var resultado = await repository.ListarBaseFechamento(codEmpCentralizadora, mesAnoFechamento, dataFechamento);
            return resultado;
        }

        public async Task<ICollection<ListaProcessosBaseFechamentoJecDTO>> ListarProcessosResultado(long codEmpCentralizadora, DateTime mesAnoFechamento, DateTime dataFechamento)
        {
            var resultado = await repository.ListarProcessosResultado(codEmpCentralizadora, mesAnoFechamento, dataFechamento);
            return resultado;
        }
    }
}
