using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.DTO;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Entity;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Interface.EFRepository
{
    public interface IJuroCorrecaoProcessoRepository : IBaseCrudRepository<JuroCorrecaoProcesso, long>
    {
        Task<ICollection<JuroCorrecaoProcessoDTO>> PesquisarComTipoProcesso(long? codTipoProcesso,
                                                                            DateTime? dataInicio,
                                                                            DateTime? dataFim,
                                                                            bool ascendente, string ordenacao,
                                                                            int pagina, int quantidade);

        Task<ICollection<JuroCorrecaoProcessoDTO>> PesquisarParaExportacaoComTipoProcesso(long? codTipoProcesso,
                                                                                          DateTime? dataInicio,
                                                                                          DateTime? dataFim);

        Task<JuroCorrecaoProcesso> ObterPorChavesCompostas(long codTipoProcesso, DateTime dataVigencia);

        bool VerificarSeDataInseridaEMenorQueACadastrada(long codTipoProcesso, DateTime dataVigencia);
    }
}
