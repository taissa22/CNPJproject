using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.External.Interface;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class ExportacaoPrePosRJService : BaseCrudService<ExportacaoPrePosRJ, long>, IExportacaoPrePosRJService
    {
        private readonly IExportacaoPrePosRJRepository repository;
        private readonly INasService nasService;
        private readonly IParametroRepository parametroRepository;


        public ExportacaoPrePosRJService(IExportacaoPrePosRJRepository repository, INasService nasService, IParametroRepository parametroRepository) : base(repository)
        {
            this.repository = repository;
            this.nasService = nasService;
            this.parametroRepository = parametroRepository;
        }

        public async Task<ExportacaoPrePosRJ> InserirDados(ExportacaoPrePosRJ model)
        {
            var validate = model.Validar();

            if (validate.IsValid)
                await repository.Inserir(model);

            return model;
        }

        public async Task<IEnumerable<ExportacaoPrePosRJ>> ListarExportacaoPrePosRj(DateTime? dataExtracao, int pagina, int qtd)
        {
            var resultado = await repository.ListarExportacaoPrePosRj(dataExtracao, pagina, qtd);
            return resultado;
        }

        public async Task<int> ObterQuantidadeTotal(DateTime? dataExtracao)
        {
            var quantidade = await repository.QuantidadeTotal(dataExtracao);
            return quantidade;
        }

        public async Task<DownloadExportacaoPrePosRjDTO> DownloadExportacaoPrePosRj(long idExtracao, ICollection<string> tiposProcessos)
        {
            var path = parametroRepository.RecuperarPorNome("EXPORTACAOBASEPREPOSNAS").Conteudo;
            var model = await repository.RecuperarPorId(idExtracao);

            var lista = await BuscaArquivosSelecionados(tiposProcessos, model);

            var criandoNomePastaZip = $"Bases_Pre_Pos_{Convert.ToDateTime(model.DataExecucao).ToString("ddMMyyyy", CultureInfo.InvariantCulture)}.zip";

            ExcluindoZip(path, criandoNomePastaZip);

            CriandoZip(path, lista, criandoNomePastaZip);

            var zip = await nasService.GetFileFromNAs(criandoNomePastaZip, "EXPORTACAOBASEPREPOSNAS");

            var dto = new DownloadExportacaoPrePosRjDTO()
            {
                NomeArquivo = criandoNomePastaZip,
                Arquivo = zip
            };

            ExcluindoZip(path, criandoNomePastaZip);

            return dto;
        }

        private static void ExcluindoZip(string path, string criandoNomePastaZip)
        {
            if (File.Exists(string.Concat(path, criandoNomePastaZip)))
            {
                File.Delete(string.Concat(path, criandoNomePastaZip));
            }
        }

        private static void CriandoZip(string path, List<DownloadExportacaoPrePosRjDTO> lista, string criandoNomePastaZip)
        {
            using (var stream = File.OpenWrite(string.Concat(path, "\\", criandoNomePastaZip)))
            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                foreach (var item in lista)
                {
                    archive.CreateEntryFromFile(String.Concat(path, "\\", item.NomeArquivo), item.NomeArquivo, CompressionLevel.Optimal);
                }
            }
        }

        private async Task<List<DownloadExportacaoPrePosRjDTO>> BuscaArquivosSelecionados(ICollection<string> tiposProcessos, ExportacaoPrePosRJ model)
        {
            var lista = new List<DownloadExportacaoPrePosRjDTO>();
            DownloadExportacaoPrePosRjDTO dto;

            foreach (var item in tiposProcessos)
            {
                switch (item)
                {
                    case "Administrativo":
                        dto = new DownloadExportacaoPrePosRjDTO()
                        {
                            NomeArquivo = model.ArquivoAdministrativo,
                            Arquivo = await nasService.GetFileFromNAs(model.ArquivoAdministrativo, "EXPORTACAOBASEPREPOSNAS")
                        };
                        lista.Add(dto);
                        break;
                    case "CivelConsumidor":
                        dto = new DownloadExportacaoPrePosRjDTO()
                        {
                            NomeArquivo = model.ArquivoCivelConsumidor,
                            Arquivo = await nasService.GetFileFromNAs(model.ArquivoCivelConsumidor, "EXPORTACAOBASEPREPOSNAS")
                        };
                        lista.Add(dto);
                        break;
                    case "CivelEstrategico":
                        dto = new DownloadExportacaoPrePosRjDTO()
                        {
                            NomeArquivo = model.ArquivoCivelEstrategico,
                            Arquivo = await nasService.GetFileFromNAs(model.ArquivoCivelEstrategico, "EXPORTACAOBASEPREPOSNAS")
                        };
                        lista.Add(dto);
                        break;
                    case "JuizadoEspecial":
                        dto = new DownloadExportacaoPrePosRjDTO()
                        {
                            NomeArquivo = model.ArquivoJec,
                            Arquivo = await nasService.GetFileFromNAs(model.ArquivoJec, "EXPORTACAOBASEPREPOSNAS")
                        };
                        lista.Add(dto);
                        break;
                    case "Pex":
                        dto = new DownloadExportacaoPrePosRjDTO()
                        {
                            NomeArquivo = model.ArquivoPex,
                            Arquivo = await nasService.GetFileFromNAs(model.ArquivoPex, "EXPORTACAOBASEPREPOSNAS")
                        };
                        lista.Add(dto);
                        break;
                    case "Trabalhista":
                        dto = new DownloadExportacaoPrePosRjDTO()
                        {
                            NomeArquivo = model.ArquivoTrabalhista,
                            Arquivo = await nasService.GetFileFromNAs(model.ArquivoTrabalhista, "EXPORTACAOBASEPREPOSNAS")
                        };
                        lista.Add(dto);
                        break;
                    case "TributarioJudicial":
                        dto = new DownloadExportacaoPrePosRjDTO()
                        {
                            NomeArquivo = model.ArquivoTributarioJudicial,
                            Arquivo = await nasService.GetFileFromNAs(model.ArquivoTributarioJudicial, "EXPORTACAOBASEPREPOSNAS")
                        };
                        lista.Add(dto);
                        break;
                }
            }
            return lista;
        }
    }
}
