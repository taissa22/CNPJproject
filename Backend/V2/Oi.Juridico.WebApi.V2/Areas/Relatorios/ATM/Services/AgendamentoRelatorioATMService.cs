using Microsoft.AspNetCore.Http.HttpResults;
using Oi.Juridico.Contextos.V2.AgendamentoRelatorioNegociacaoContext.Data;
using Oi.Juridico.Contextos.V2.AgendamentoRelatorioNegociacaoContext.Entities;
using Oi.Juridico.Contextos.V2.AtmCCContext.Data;
using Oi.Juridico.Contextos.V2.AtmCCContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.DTOs;
using Oi.Juridico.WebApi.V2.Areas.Relatorios.ATM.DTOs;
using Perlink.Oi.Juridico.Application.Security;

namespace Oi.Juridico.WebApi.V2.Areas.Relatorios.ATM.Services
{
    public class AgendamentoRelatorioATMService
    {
        private readonly AtmCCContext _db;
        public AgendamentoRelatorioATMService(AtmCCContext db)
        {
            _db = db;
        }
       

        public async Task<(List<AgendRelatorioAtmCc>? dadosAgendamento, int total, string? mensagemErro)> Listar(int pagina, int quantidade, CancellationToken ct)
        {
            try
            {
                var query = _db.AgendRelatorioAtmCc.Include(x => x.AgendRelatAtmCcIndiceUf).OrderByDescending(x => x.DatSolicitacao);

                var total = query.Count();

                var agendamentos = await query.Skip(Pagination.PagesToSkip(quantidade, total, pagina))
                                              .Take(quantidade)
                                              .ToListAsync();
                return (agendamentos, total, null);
            }
            catch (Exception e)
            {
                return (null, 0, e.Message);
            }
        }

        public async Task<string> SalvarAgendamentoAsync(AgendamentoDTO requestDto, string user, CancellationToken ct)
        {
            if (user is null)
            {
                return "Usuário não informado";
            }

            try
            {
                _db.Database.BeginTransaction();

                var novo = new AgendRelatorioAtmCc();

                novo.CodFechContCcMedia = requestDto.CodFechContCcMedia;
                novo.MesAnoContabil = requestDto.MesAnoContabil;
                novo.IndFechMensal = requestDto.IndFechMensal.ToUpper(); 
                novo.DatFechamento = requestDto.DatFechamento;
                novo.EmpresaCentralizadora = requestDto.empresas;
                novo.NumeroMeses = requestDto.NumeroMeses;
                novo.Status = (byte)StatusAgendamentoRelatorioATMCCEnum.Agendado;
                novo.Mensagem = ProcessarMensagem(StatusAgendamentoRelatorioATMCCEnum.Agendado,novo);
                novo.UsrCodUsuario = user;  
                novo.DatSolicitacao =  DateTime.Now;  

                _db.Add(novo);
                _db.SaveChanges();

                foreach (var item in requestDto.UFs)
                {
                    var novoIndice = new AgendRelatAtmCcIndiceUf();

                    novoIndice.Uf = item.UF;
                    novoIndice.CodIndice = item.Indice;
                    novoIndice.CodAgendRelatorioAtm = novo.CodAgendRelatorioAtm;
                    _db.Add(novoIndice);
                }

                _db.SaveChanges();
                _db.Database.CommitTransaction();
                return "OK";
            }
            catch (Exception e)
            {
                _db.Database.RollbackTransaction();
                return e.Message;
            }
        }

        private string ProcessarMensagem(StatusAgendamentoRelatorioATMCCEnum status, AgendRelatorioAtmCc agendamento )
        {
            switch (status)
            {
                case StatusAgendamentoRelatorioATMCCEnum.Agendado:
                    return "Este agendamento será processado em breve. Por favor, aguarde.";
                case StatusAgendamentoRelatorioATMCCEnum.Processando:
                    return $"Este agendamento está sendo processado. Por favor, aguarde.";
                case StatusAgendamentoRelatorioATMCCEnum.Processado:
                    return $"Execução iniciada em " + agendamento.DatIniExec + " e finalizada em : " + agendamento.DatFimExec;
                case StatusAgendamentoRelatorioATMCCEnum.Erro:
                    return $"Falha na execução do relatório.";
                default:
                    return "";
            }

        }

        public async Task<(bool excluido, string? mensagem)> RemoveAgendamentoAsync(int codigo, CancellationToken ct)
        {
            try
            {
                var agendamento = _db.AgendRelatorioAtmCc.FirstOrDefault(x => x.CodAgendRelatorioAtm == codigo);

                if (agendamento == null)
                {
                    return (false, "Não foi possivel encontrar o agendamento.");
                }

                var indices = _db.AgendRelatAtmCcIndiceUf.Where(x => x.CodAgendRelatorioAtm == codigo);

                _db.AgendRelatAtmCcIndiceUf.RemoveRange(indices);
                _db.AgendRelatorioAtmCc.Remove(agendamento);
                await _db.SaveChangesAsync(ct);

                return (true, "Agendamento excluído com sucesso.");

            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }


    }
}
