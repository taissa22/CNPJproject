using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.DTO;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.AlteracaoBloco.Interface.Service
{
    public interface IAlteracaoEmBlocoService : IBaseCrudService<AlteracaoEmBloco, long>
    {
        bool ValidarArquivoDaPastaTemporaria(IFormFile file);
        bool ValidarTamanhoDoArquivo(string caminhoDoArquivoTemporario, Parametro modelParametro);
        bool ValidarDadosDoArquivo(string caminhoDoArquivoTemporario, long codigoTipoProcesso);
        bool ValidarColunasDoArquivo(string caminhoDoArquivoTemporariom, long codigoTipoProcesso);
        string GravarArquivoNaPastaTemporaria(IFormFile file);
        string GravarArquivoNoNas(string caminhoDoArquivoTemporario, IFormFile arquivo, Parametro modelParametro);
        Task<AlteracaoEmBlocoDTO> BaixarPlanilha(AlteracaoEmBloco model, Parametro parametroPastaDestinoArquivo);       
        void RemoverAgendamento(long id, Parametro parametro, string nomeDoArquivo);
        Task<IEnumerable<AlteracaoEmBlocoRetornoDTO>> ListarAgendamentos(int index, int count);
        Task<int> ObterQuantidadeTotal();
        Task ExpurgoAlteracaoEmBloco(ILogger logger);
        Task ProcessarAgendamentos(ILogger logger);
    }
}
