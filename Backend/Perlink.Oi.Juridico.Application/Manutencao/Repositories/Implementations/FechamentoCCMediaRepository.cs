using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class FechamentoCCMediaRepository : IFechamentoCCMediaRepository
    {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IFechamentoCCMediaRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        public FechamentoCCMediaRepository(IDatabaseContext databaseContext, ILogger<IFechamentoCCMediaRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<PaginatedQueryResult<FechamentoCCMedia>> ObterPaginado(DateTime? dataInicial, DateTime? dataFinal, int pagina)
        {
            string logName = "Relatório Cível Consumidor Média";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_RELATORIO_CIVEL_CONS_MEDIA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_RELATORIO_CIVEL_CONS_MEDIA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<FechamentoCCMedia>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(dataInicial, dataFinal);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(10, total, pagina);

            var resultado = new PaginatedQueryResult<FechamentoCCMedia>()
            {
                Total = total,
                Data = MontarFechamentos(listaBase, skip)
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<FechamentoCCMedia>>.Valid(resultado);
        }

        public CommandResult<byte[]> Baixar(long id, ref string nomeArquivo)
        {
            bool arquivoLocalizado = false;                
            string entity = "Relatório CC Média ";
            string command = $"Download {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try
            {
                Logger.LogInformation("Verificando Id do relatório");
                if (id <= 0)
                {
                    return CommandResult<byte[]>.Invalid("Id do relatório não informado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entity));
                var indetificacaoDoArquivo = DatabaseContext
                                 .FechamentosCiveisConsumidoresMedia
                                 .AsNoTracking()
                                 .FirstOrDefault(a => a.Id == id);

                if (indetificacaoDoArquivo is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entity, (int)id));
                    return CommandResult<byte[]>.Invalid("Relatório não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
                FileInfo relatorio = this.RecuperarArquivosDownload(indetificacaoDoArquivo, ref arquivoLocalizado).FirstOrDefault();               

                if (!arquivoLocalizado)
                    return CommandResult<byte[]>.Invalid("Arquivo não encontrado ou mais de um arquivo foi localizado.");            
                else
                {
                    nomeArquivo = relatorio.Name;
                    return CommandResult<byte[]>.Valid(File.ReadAllBytes(Path.Combine(relatorio.FullName)));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(command));
                return CommandResult<byte[]>.Invalid(ex.Message);
            }
        }

        private FileInfo[] RecuperarArquivosDownload(FechamentoCCMedia fechamento, ref bool arquivoLocalizado)
        {            
            DirectoryInfo diretorioRelatorios = new DirectoryInfo(DatabaseContext.ParametrosJuridicos
                                                     .FirstOrDefault(x => x.Parametro.Equals("FECH_CCPM_DIR_NAS")).Conteudo);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo($"Tentando localizar arquivo: {fechamento.Id}"));
            FileInfo[] relatorio = diretorioRelatorios.GetFiles($"{fechamento.DataFechamento.ToString("yyyyMMdd")}_Consumidor_{fechamento.SolicitacaoFechamentoContingencia}_*.*").ToArray();

            arquivoLocalizado = relatorio.Length == 1;
            return relatorio;
        }

        private IQueryable<FechamentoCCMedia> ObterBase(DateTime? dataInicial, DateTime? dataFinal)
        {
            if (dataInicial is null)
                dataInicial = DateTime.Now.AddDays(-30);
            if (dataFinal is null)
                dataFinal = DateTime.Now;


            var query = DatabaseContext.FechamentosCiveisConsumidoresMedia
                                             .Include(x => x.Usuario)
                                             .Include(x => x.EmpresaCentralizadora)
                                             .AsNoTracking()
                                             .Where(x => x.DataFechamento.Date >= dataInicial.Value.Date
                                             && x.DataFechamento.Date <= dataFinal.Value.Date)
                                             .GroupBy(p => new { p.SolicitacaoFechamentoContingencia, p.DataFechamento })
                                             .Where(x => x.All(y => y.IndBaseGerada))
                                             .Select(z => z.First());

            return query;
        }

        private FechamentoCCMedia[] MontarFechamentos(IQueryable<FechamentoCCMedia> query, int skip)
        {
            bool arquivoGerado = false;
            var fechamentos = query.OrderByDescending(x => x.DataFechamento)
                                   .ThenByDescending(x => x.DataGeracao)
                                   .Skip(skip)
                                   .Take(10)
                                   .ToArray();

            foreach (var fechamento in fechamentos)
            {
                this.RecuperarArquivosDownload(fechamento, ref arquivoGerado);
                fechamento.FechamentoGerado = arquivoGerado;

                fechamento.EmpresasParticipantes = string.Join(", ", DatabaseContext.FechamentosCiveisConsumidoresMedia
                    .AsNoTracking()
                    .Where(x => x.SolicitacaoFechamentoContingencia == fechamento.SolicitacaoFechamentoContingencia && x.DataFechamento == fechamento.DataFechamento)
                    .Select(x => x.EmpresaCentralizadora.Nome)
                    .ToArray());
            }

            return fechamentos;
        }
    }
}