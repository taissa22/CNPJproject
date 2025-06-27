using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Enum;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.External.Interface;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Service {
    public class AgendarApuracaoOutlierService : BaseCrudService<AgendarApuracaoOutliers, long>, IAgendarApuracaoOutlierService {
        public readonly IAgendarApuracaoOutlierRepository repository;
        private readonly IParametroRepository parametroRepository;
        private readonly INasService nasService;


        public AgendarApuracaoOutlierService(IAgendarApuracaoOutlierRepository repository, INasService nasService) : base(repository) {
            this.repository = repository;
            this.nasService = nasService;
        }

        public async Task<AgendarApuracaoOutliers> AgendarApuracaoOutliers(AgendarApuracaoOutliers obj)
        {
            return await repository.AgendarApuracaoOutliers(obj);
        }

        public async Task<IEnumerable<ListarAgendamentosApuracaoOutliersDTO>> CarregarAgendamento(int pagina, int qtd) {
            return await repository.CarregarAgendamento(pagina, qtd);
        }       

        public async Task<int> ObterQuantidadeTotal() {
            return await repository.ObterQuantidadeTotal();
        }

        public void RemoverAgendamento(AgendarApuracaoOutliers agendamento) {
            repository.RemoverAgendamento(agendamento);
        }

        public async Task<ApuracaoOutliersDownloadArquivoDTO> BaixarArquivosResultado(AgendarApuracaoOutliers agendar, Parametro parametroPastaDestinoArquivo)
        {
            var conteudo = await nasService.GetFileFromNAs(agendar.ArquivoResultado, parametroPastaDestinoArquivo.Id);
     
            var dto = new ApuracaoOutliersDownloadArquivoDTO()
            {
                NomeArquivo = agendar.ArquivoResultado,
                Arquivo = conteudo
            };

            return dto;
        }

        public async Task<ApuracaoOutliersDownloadArquivoDTO> DownloadBaseFechamento(int id)
        {
            var model = await repository.RecuperarPorId(id);

            var conteudo = await nasService.GetFileFromNAs(model.ArquivoBaseFechamento, "DIR_NAS_CALC_OUTLIERS_JEC");

            var dto = new ApuracaoOutliersDownloadArquivoDTO()
            {
                NomeArquivo = model.ArquivoBaseFechamento,
                Arquivo = conteudo
            };
        
            return dto;
        }

        #region Executor
        public async Task TratandoAgendamentosInterrompidos() {
            var agendamentosProcessando = repository.Pesquisar()
                .Where(a => a.Status == AgendarApuracaoOutliersStatusEnum.Processando)
                .ToList();

            foreach (var item in agendamentosProcessando) {
                item.Status = AgendarApuracaoOutliersStatusEnum.Erro;
                item.DataFinalizacao = DateTime.Now;
                item.MgsStatusErro = "Executor encerrado manualmente.";
                await repository.CommitAsync();
            }
        }

        public async Task<ICollection<AgendarApuracaoOutliers>> ObterAgendados() {
            return await repository.ObterAgendados();            
        }

        public async Task<ApuracaoOutliersDownloadResultadoDTO> ObterResultadoDoAgendamento(long id)
        {
            var resultado = await repository.ObterResultadoDoAgendamento(id);
            return resultado;
        }

        public async Task<AgendarApuracaoOutliers> RealizarCalculos(AgendarApuracaoOutliers agendamento) {
            return await repository.RealizarCalculos(agendamento);
        }
        #endregion Executor
    }
}
