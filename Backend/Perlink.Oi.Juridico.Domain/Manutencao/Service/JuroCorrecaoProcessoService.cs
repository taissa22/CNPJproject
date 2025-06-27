using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.DTO;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Entity;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Interface.EFRepository;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Service
{
    public class JuroCorrecaoProcessoService : BaseCrudService<JuroCorrecaoProcesso, long>, IJuroCorrecaoProcessoService
    {
        private readonly IJuroCorrecaoProcessoRepository repository;

        public JuroCorrecaoProcessoService(IJuroCorrecaoProcessoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<ICollection<JuroCorrecaoProcessoDTO>> PesquisarComTipoProcesso(long? codTipoProcesso,
                                                                                         DateTime? dataInicio,
                                                                                         DateTime? dataFim,
                                                                                         bool ascendente, string ordenacao,
                                                                                         int pagina, int quantidade)
        {
            return await repository.PesquisarComTipoProcesso(codTipoProcesso, dataInicio, dataFim, ascendente, 
                                                             ordenacao, pagina, quantidade);
        }

        public async Task<ICollection<JuroCorrecaoProcessoDTO>> PesquisarParaExportacaoComTipoProcesso(long? codTipoProcesso,
                                                                                                       DateTime? dataInicio,
                                                                                                       DateTime? dataFim)
        {
            return await repository.PesquisarParaExportacaoComTipoProcesso(codTipoProcesso, dataInicio, dataFim);
        }

        public async Task<JuroCorrecaoProcesso> ObterPorChavesCompostas(long codTipoProcesso, DateTime dataVigencia)
        {
            return await repository.ObterPorChavesCompostas(codTipoProcesso, dataVigencia);
        }

        public bool VerificarSeDataInseridaEMenorQueACadastrada(long codTipoProcesso, DateTime dataVigencia)
        {
            return repository.VerificarSeDataInseridaEMenorQueACadastrada(codTipoProcesso, dataVigencia);
        }
    }
}
