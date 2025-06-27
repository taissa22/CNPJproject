using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.DTO;
using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Entity;
using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Service
{
    public class FechamentoCCPorMediaService : BaseCrudService<FechamentoCivelConsumidorPorMedia, long>, IFechamentoCCPorMediaService
    {
        private readonly IFechamentoCCPorMediaRepository _repositoryFechamento;
 
        public FechamentoCCPorMediaService(IFechamentoCCPorMediaRepository repositoryFechamento) : base(repositoryFechamento) 
        { 
            _repositoryFechamento = repositoryFechamento; 
        }

        private static string GeraDataUltimoMes(string dataInicio) 
        { 
            if (dataInicio == null) 
                dataInicio = DateTime.Today.AddDays(-30).ToString("dd/MM/yyyy"); 

            return dataInicio; 
        }

        private static string GeraDataHoje(string dataFim) 
        { 
            if (dataFim == null) 
                dataFim = DateTime.Today.ToString("dd/MM/yyyy"); 

            return dataFim; 
        }

        public async Task<IEnumerable<FechamentoContingenciaCCPorMediaDTO>> ListarFechamentos(string dataInicio, string dataFim, int quantidade, int pagina) 
        {
            /*if (dataInicio == null || dataFim == null) 
            {                 
                dataInicio = DateTime.Today.AddDays(-30).ToString("dd/MM/yyyy"); 
                dataFim = DateTime.Today.ToString("dd/MM/yyyy"); 
            }
 
            var lista = await _repositoryFechamento.ListarFechamentos(dataInicio + " 00:00:00", dataFim + " 23:59:59", total, pagina);
            foreach (var item in lista) 
            { 
                var dataExecucaoNova = await _repositoryFechamento.MaxDataExecucao(item.Id); 
                item.DataExecucao = dataExecucaoNova;
                //.ToString("dd/MM/yyyy HH:mm:ss- HH:mm").Replace("-", "às");
            }

            return lista;*/

            var lista = await _repositoryFechamento.ListarFechamentos(GeraDataUltimoMes(dataInicio), 
                GeraDataHoje(dataFim), quantidade, pagina); 
            foreach (var item in lista) 
            { 
                var dataExecucaoNova = await _repositoryFechamento.MaxDataExecucao(item.Id);
                item.DataGeracao = dataExecucaoNova;
                    //.ToString("dd/MM/yyyy - HH:mm").Replace("-", "às"); 
            }
            return lista;

        }

        public async Task<int> TotalFechamentos(string dataInicio, string dataFim) 
        { 
            return await _repositoryFechamento.TotalFechamentos(GeraDataUltimoMes(dataInicio), GeraDataHoje(dataFim)); 
        }

    }
}
