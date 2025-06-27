using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using Oi.Juridico.AddOn.Extensions.IEnumerable;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs.CsvHelperMap;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.RequestDTOs;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Services
{
    public class ESocialDownloadRetornoService
    {
        public bool ExibirHistorioco(ESocialDbContext context, long? codigoFormulario, string NomeArquivoEnviado)
        {
            //var qtd = 0;

            //var nomeArquivo = context.EsInconsistenciasF2500F2501.AsNoTracking().Where(r => r.NomeArquivoEnviado.Contains(NomeArquivoEnviado) && (r.IdF2500 == codigoFormulario || r.IdF2501 == codigoFormulario)).Select(x => x.IdExecucao).Distinct().ToList();
            //qtd = nomeArquivo.Count();
            var qtd = context.EsInconsistenciasF2500F2501.AsNoTracking().Where(r => (r.IdF2500 == codigoFormulario || r.IdF2501 == codigoFormulario)).Select(x => x.IdExecucao).Distinct().Count();

            return qtd > 1;
        }
        public bool ExibirRetorno(ESocialDbContext context, long? codigoFormulario, string NomeArquivoEnviado, string tipoFormulario)
        {
            var nomeArquivo = context.EsInconsistenciasF2500F2501.AsNoTracking().Any(r => (r.NomeArquivoEnviado != null && r.NomeArquivoEnviado.Contains(NomeArquivoEnviado)) && ((tipoFormulario == "F_2500" && r.IdF2500 == codigoFormulario) || (tipoFormulario == "F_2501" && r.IdF2501 == codigoFormulario)));

            return nomeArquivo;
        }

        public async Task<DownloadRetornoDTO> ExportarRetorno(ESocialDbContext context, int codigoFormulario, bool f2500, CancellationToken ct)
        {
            var DownloadRetorno = new DownloadRetornoDTO();
            using var scope = await context.Database.BeginTransactionAsync(ct);
            context.PesquisarPorCaseInsensitive();
            var vAcompanhamento = new VEsAcompanhamento();

            if (f2500)
            {
                vAcompanhamento = await context.VEsAcompanhamento.AsNoTracking().FirstOrDefaultAsync(x => x.IdFormulario == codigoFormulario && x.TipoFormulario == "F_2500", ct);

                var nomeArquivo = string.Empty;
                if (vAcompanhamento!.StatusFormulario == EsocialStatusFormulario.Exclusao3500NaoOk.ToByte())
                {
                    nomeArquivo = vAcompanhamento!.NomeArquivoRetornado;
                }
                else
                {
                    nomeArquivo = vAcompanhamento.NomeArquivoEnviado;
                }

                var maxId = context.EsInconsistenciasF2500F2501.Where(r => r.IdF2500 == codigoFormulario && r.NomeArquivoEnviado == nomeArquivo).Max(m => m.IdExecucao);

                var query = from a in context.EsInconsistenciasF2500F2501.AsNoTracking().Where(r => r.IdF2500 == codigoFormulario && r.NomeArquivoEnviado == nomeArquivo && (r.IdExecucao == maxId || r.IdExecucao == null)).OrderByDescending(x => x.LogDataOperacao)
                            select new AcompanhamentoEnvioDTO
                            {
                                CodProcesso = a.CodProcesso,
                                Contador = vAcompanhamento!.NomContador,
                                Cpf = vAcompanhamento!.CpfParte,
                                EmpreseGrupo = vAcompanhamento!.NomParteEmpresa,
                                Escritorio = vAcompanhamento!.NomEscritorio,
                                Formulario = vAcompanhamento!.TipoFormulario,
                                LogDataOperacao = a.LogDataOperacao,
                                NomeReclamante = vAcompanhamento!.NomParte,
                                NumProcesso = vAcompanhamento.InfoprocessoNrproctrab,
                                Ocorrencia = a.Inconsistencia.Replace("\n", "").Replace("\r", ""),
                                Original_retificacao = vAcompanhamento.IdeeventoIndretif == 2 ? "Retificado" : "Original",
                                Acao = a.Acao.Replace("\n", "").Replace("\r", ""),
                                Codigo = a.Codigo,
                                Localizacao = a.Localizacao,
                                IdExecucao = a.IdExecucao

                            };

                var ListaAcompanhamentoEnvio = await query.ToListAsync(ct);

                var csv = ListaAcompanhamentoEnvio.ToCsvByteArray(typeof(ExportaAcompanhamentoEnvioMap), sanitizeForInjection: false);
                var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();
                DownloadRetorno = new DownloadRetornoDTO
                {
                    CpfParte = vAcompanhamento!.CpfParte,
                    InfoprocessoNrproctrab = vAcompanhamento!.InfoprocessoNrproctrab,
                    StatusFormulario = vAcompanhamento!.StatusFormulario,
                    TipoFormulario = vAcompanhamento!.TipoFormulario,
                    Dados = bytes
                };
            }
            else
            {
                vAcompanhamento = await context.VEsAcompanhamento.AsNoTracking().FirstOrDefaultAsync(x => x.IdFormulario == codigoFormulario && x.TipoFormulario == "F_2501", ct);
                var _periodoApuracao = !string.IsNullOrEmpty(vAcompanhamento!.IdeprocPerapurpgto) ? vAcompanhamento!.IdeprocPerapurpgto.Substring(4, 2) + "/" + vAcompanhamento!.IdeprocPerapurpgto.Substring(0, 4) : string.Empty;

                var nomeArquivo = string.Empty;
                if (vAcompanhamento!.StatusFormulario == EsocialStatusFormulario.Exclusao3500NaoOk.ToByte())
                {
                    nomeArquivo = vAcompanhamento!.NomeArquivoRetornado;
                }
                else
                {
                    nomeArquivo = vAcompanhamento.NomeArquivoEnviado;
                }

                var maxId = context.EsInconsistenciasF2500F2501.Where(r => r.IdF2501 == codigoFormulario && r.NomeArquivoEnviado == nomeArquivo).Max(m => m.IdExecucao);
                var query = from a in context.EsInconsistenciasF2500F2501.AsNoTracking().Where(r => r.IdF2501 == codigoFormulario && r.NomeArquivoEnviado == nomeArquivo && (r.IdExecucao == maxId || r.IdExecucao == null)).OrderByDescending(x => x.LogDataOperacao)
                            select new AcompanhamentoEnvio2501DTO
                            {
                                CodProcesso = a.CodProcesso,
                                Contador = vAcompanhamento!.NomContador,
                                Cpf = vAcompanhamento!.CpfParte,
                                EmpreseGrupo = vAcompanhamento!.NomParteEmpresa,
                                Escritorio = vAcompanhamento!.NomEscritorio,
                                Formulario = vAcompanhamento!.TipoFormulario,
                                LogDataOperacao = a.LogDataOperacao,
                                NomeReclamante = vAcompanhamento!.NomParte,
                                NumProcesso = vAcompanhamento.InfoprocessoNrproctrab,
                                Ocorrencia = a.Inconsistencia.Replace("\n", "").Replace("\r", ""),
                                Original_retificacao = vAcompanhamento.IdeeventoIndretif == 2 ? "Retificado" : "Original",
                                Acao = a.Acao.Replace("\n", "").Replace("\r", ""),
                                Codigo = a.Codigo,
                                Localizacao = a.Localizacao,       
                                IdExecucao = a.IdExecucao,
                                PeriodoApuracao = !string.IsNullOrEmpty(vAcompanhamento!.IdeprocPerapurpgto) ? vAcompanhamento!.IdeprocPerapurpgto.Substring(4, 2) + "/" + vAcompanhamento!.IdeprocPerapurpgto.Substring(0, 4) : string.Empty

            };

                var ListaAcompanhamentoEnvio = await query.ToListAsync(ct);

                var csv = ListaAcompanhamentoEnvio.ToCsvByteArray(typeof(ExportaAcompanhamentoEnvio2501Map), sanitizeForInjection: false);
                var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();
                DownloadRetorno = new DownloadRetornoDTO
                {
                    CpfParte = vAcompanhamento!.CpfParte,
                    InfoprocessoNrproctrab = vAcompanhamento!.InfoprocessoNrproctrab,
                    StatusFormulario = vAcompanhamento!.StatusFormulario,
                    TipoFormulario = vAcompanhamento!.TipoFormulario,
                    Dados = bytes
                };
            }

            
            return DownloadRetorno;
        }

        public async Task<DownloadRetornoDTO> ExportarHistoricoRetorno(ESocialDbContext context, int codigoFormulario, bool f2500, CancellationToken ct)
        {
            var DownloadRetorno = new DownloadRetornoDTO();
            using var scope = await context.Database.BeginTransactionAsync(ct);
            context.PesquisarPorCaseInsensitive();
            var vAcompanhamento = new VEsAcompanhamento();


            if (f2500)
            {
                vAcompanhamento = await context.VEsAcompanhamento.AsNoTracking().FirstOrDefaultAsync(x => x.IdFormulario == codigoFormulario && x.TipoFormulario == "F_2500", ct);
                var query = from a in context.EsInconsistenciasF2500F2501.AsNoTracking().Where(r => r.IdF2500 == codigoFormulario).OrderByDescending(x => x.LogDataOperacao)
                            select new AcompanhamentoEnvioDTO
                            {
                                CodProcesso = a.CodProcesso,
                                Contador = vAcompanhamento!.NomContador,
                                Cpf = vAcompanhamento!.CpfParte,
                                EmpreseGrupo = vAcompanhamento!.NomParteEmpresa,
                                Escritorio = vAcompanhamento!.NomEscritorio,
                                Formulario = vAcompanhamento!.TipoFormulario,
                                LogDataOperacao = a.LogDataOperacao,
                                NomeReclamante = vAcompanhamento!.NomParte,
                                NumProcesso = vAcompanhamento.InfoprocessoNrproctrab,
                                Ocorrencia = a.Inconsistencia,
                                Original_retificacao = vAcompanhamento.IdeeventoIndretif == 2 ? "Retificado" : "Original",
                                Codigo = a.Codigo,
                                Localizacao = a.Localizacao,
                                IdExecucao = a.IdExecucao,

                            };

               var ListaAcompanhamentoEnvio = await query.ToListAsync(ct);

                var csv = ListaAcompanhamentoEnvio.ToCsvByteArray(typeof(ExportaAcompanhamentoEnvioMap), sanitizeForInjection: false);
                var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

                DownloadRetorno = new DownloadRetornoDTO
                {
                    CpfParte = vAcompanhamento!.CpfParte,
                    InfoprocessoNrproctrab = vAcompanhamento!.InfoprocessoNrproctrab,
                    StatusFormulario = vAcompanhamento!.StatusFormulario,
                    TipoFormulario = vAcompanhamento!.TipoFormulario,
                    Dados = bytes
                };
            }
            else
            {
                vAcompanhamento = await context.VEsAcompanhamento.AsNoTracking().FirstOrDefaultAsync(x => x.IdFormulario == codigoFormulario && x.TipoFormulario == "F_2501", ct);

                var query = from a in context.EsInconsistenciasF2500F2501.AsNoTracking().Where(r => r.IdF2501 == codigoFormulario).OrderByDescending(x => x.LogDataOperacao)
                            select new AcompanhamentoEnvio2501DTO
                            {
                                CodProcesso = a.CodProcesso,
                                Contador = vAcompanhamento!.NomContador,
                                Cpf = vAcompanhamento!.CpfParte,
                                EmpreseGrupo = vAcompanhamento!.NomParteEmpresa,
                                Escritorio = vAcompanhamento!.NomEscritorio,
                                Formulario = vAcompanhamento!.TipoFormulario,
                                LogDataOperacao = a.LogDataOperacao,
                                NomeReclamante = vAcompanhamento!.NomParte,
                                NumProcesso = vAcompanhamento.InfoprocessoNrproctrab,
                                Ocorrencia = a.Inconsistencia,
                                Original_retificacao = vAcompanhamento.IdeeventoIndretif == 2 ? "Retificado" : "Original",
                                Codigo = a.Codigo,
                                Localizacao = a.Localizacao,
                                IdExecucao = a.IdExecucao,
                                PeriodoApuracao = !string.IsNullOrEmpty(vAcompanhamento!.IdeprocPerapurpgto) ? vAcompanhamento!.IdeprocPerapurpgto.Substring(4, 2) + "/" + vAcompanhamento!.IdeprocPerapurpgto.Substring(0, 4) : string.Empty

                            };

               var ListaAcompanhamentoEnvio = await query.ToListAsync(ct);

                var csv = ListaAcompanhamentoEnvio.ToCsvByteArray(typeof(ExportaAcompanhamentoEnvio2501Map), sanitizeForInjection: false);
                var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

                DownloadRetorno = new DownloadRetornoDTO
                {
                    CpfParte = vAcompanhamento!.CpfParte,
                    InfoprocessoNrproctrab = vAcompanhamento!.InfoprocessoNrproctrab,
                    StatusFormulario = vAcompanhamento!.StatusFormulario,
                    TipoFormulario = vAcompanhamento!.TipoFormulario,
                    Dados = bytes
                };
            }

            
            return DownloadRetorno;
        }

        public async Task<byte[]> ExportarCriticasRetorno(ESocialDbContext context, List<long?> resultado, EsF2500AcompanhamentoRequestDTO requestDTO, CancellationToken ct)
        {
            using var scope = await context.Database.BeginTransactionAsync(ct);
            context.PesquisarPorCaseInsensitive();

            var dados = await (from a in context.EsInconsistenciasF2500F2501.AsNoTracking()
                               join va in context.VEsAcompanhamento.AsNoTracking() on a.CodProcesso equals va.CodProcesso
                               where
                                (requestDTO.tipoFormulario == 1 ? va.TipoFormulario == "F_2500" : requestDTO.tipoFormulario == 2 ? va.TipoFormulario == "F_2501" : va.TipoFormulario == "F_2500" || va.TipoFormulario == "F_2501")
                                && va.NomeArquivoEnviado == a.NomeArquivoEnviado
                               select new AcompanhamentoCriticasRetornoDTO
                               {
                                   CodProcesso = a.CodProcesso,
                                   Contador = va!.NomContador,
                                   Cpf = va!.CpfParte,
                                   EmpreseGrupo = va!.NomParteEmpresa,
                                   Escritorio = va!.NomEscritorio,
                                   Formulario = va!.TipoFormulario,
                                   LogDataOperacao = a.LogDataOperacao,
                                   NomeReclamante = va!.NomParte,
                                   NumProcesso = va.InfoprocessoNrproctrab,
                                   Ocorrencia = a.Inconsistencia,
                                   Original_retificacao = va.IdeeventoIndretif == 2 ? "Retificado" : "Original",
                                   StatusFormulario = va!.StatusFormulario,
                                   IdFormulario = va!.IdFormulario,
                                   Acao = a.Acao,
                                   Codigo = a.Codigo,
                                   Localizacao = a.Localizacao,
                                   IdExecucao = a.IdExecucao,
                                   PeriodoApuracao = !string.IsNullOrEmpty(va!.IdeprocPerapurpgto) ? va!.IdeprocPerapurpgto.Substring(4, 2) + "/" + va!.IdeprocPerapurpgto.Substring(0, 4) : string.Empty


                               }).Where(x => resultado.Contains(x.IdFormulario) && (x.StatusFormulario == EsocialStatusFormulario.RetornoESocialNaoOk.ToByte() || x.StatusFormulario == EsocialStatusFormulario.PendenteAcaoFPW.ToByte())).ToListAsync(ct);

            var ListaAcompanhamentoEnvio = dados;

            var csv = ListaAcompanhamentoEnvio.ToCsvByteArray(typeof(ExportaAcompanhamentoCriticasRetornoMap), sanitizeForInjection: false);
            var bytes = Encoding.UTF8.GetPreamble().Concat(csv).ToArray();

            return bytes;
        }


        #region Configuracao
        private async Task<Stream?> Download(string url)
        {
            try
            {
                var filePath = url;
                if (!File.Exists(filePath))
                {
                    return null;
                }
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return memory;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private string GetContentType(string file)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(file, out string contentType))
                contentType = "application/octet-stream";
            return contentType;
        }
        #endregion


    }
}
