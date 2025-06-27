using Oi.Juridico.Contextos.V2.LogProcessoTrabalhistaContext.Data;
using Oi.Juridico.WebApi.V2.Areas.LogProcesso.Trabalhista.DTO;

namespace Oi.Juridico.WebApi.V2.Areas.LogProcesso.Trabalhista.Services
{
    public class LogProcessoTrabalhistaService
    {

        private readonly LogProcessoTrabalhistaContext _db;

        public LogProcessoTrabalhistaService(LogProcessoTrabalhistaContext db)
        {
            _db = db;
        }


        public async Task<LogProcessoTrabalhistaResponse[]?> ObterProcessoAudienciaPrepostoAsync(long codProcesso, CancellationToken ct)
        {
            using var scope = _db.Database.BeginTransaction();

            var logProcessoTrabalhista = await
                  (from log in _db.LogReclaPrepostoAudiencia
                   where log.CodProcesso == codProcesso
                   orderby log.DatUltAtualizacao ascending
                   select new LogProcessoTrabalhistaResponse
                   {
                       Operacao = log.TipoOperacao == "A" ? "Alteração" : log.TipoOperacao == "I" ? "Inclusão" : "Exclusão",
                       DatLog = log.DatUltAtualizacao.HasValue ? log.DatUltAtualizacao.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty,
                       CodUsuarioUltAlteracao = log.CodUsuarioUltAlteracao,
                       NomUsuario = log.NomUsuario,
                       IndUsuarioInternet = log.IndUsuarioInternet == "S" ? "Sim" : "Não",
                       DatAudiencia = log.DatAudiencia.HasValue ? log.DatAudiencia.Value.ToString("dd/MM/yyyy") : string.Empty,
                       HoraAudiencia = log.DatAudiencia.HasValue ? log.DatAudiencia.Value.ToString("HH:mm") : string.Empty,
                       NomReclamadaAntes = log.NomReclamadaA,
                       NomReclamadaDepois = log.NomReclamadaD,
                       NomPrepostoAntes = log.NomPrepostoA,
                       NomPrepostoDepois = log.NomPrepostoD
                   }).AsNoTracking().ToArrayAsync(ct);

            return logProcessoTrabalhista;
        }

    }
}
