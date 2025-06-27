using Perlink.Oi.Juridico.Domain.ContingenciaPex.DTO;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Entity;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ContingenciaPex.Service
{
    public class FechamentoPexMediaService : BaseCrudService<FechamentoPexMedia, long>, IFechamentoPexMediaService
    {
        private readonly IFechamentoPexMediaRepository _repositoryFechamento;
        public FechamentoPexMediaService(IFechamentoPexMediaRepository repositoryFechamento) : base(repositoryFechamento)
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
            if(dataFim == null)
                dataFim = DateTime.Today.ToString("dd/MM/yyyy");
            
            return dataFim;
        }
        public async Task<IEnumerable<FechamentoContingenciaPexMediaDTO>> ListarFechamentos(string dataInicio, string dataFim, int quantidade, int pagina)
        {           
            var lista = await _repositoryFechamento.ListarFechamentos(GeraDataUltimoMes(dataInicio), GeraDataHoje(dataFim), quantidade, pagina);           
            return lista;
        }

        public async Task<int> TotalFechamentos(string dataInicio, string dataFim)
        {
            return await _repositoryFechamento.TotalFechamentos(GeraDataUltimoMes(dataInicio), GeraDataHoje(dataFim));
        }
    }
}
