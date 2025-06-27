using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using Flunt.Validations;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Immutable;
using System.IO;
using System.Xml;
using Z.EntityFramework.Plus;

namespace Oi.Juridico.WebApi.V2.Services
{
    #region Methods
    public class ChangeUploadTXTtoXML
    {
        public static ImmutableArray<EsEmpresaAgrupadora> ObtemEmpresasAgrupadoras(ESocialDbContext eSocialDbContext)
        {
            var empresas = eSocialDbContext.EsEmpresaAgrupadora
                .AsNoTracking()
                .ToImmutableArray();

            return empresas;
        }

        public static bool ConvertToXml(string inputFilePath, string outputFilePath, List<string> linesText, ESocialDbContext eSocialDbContext)
        {
            //var nomeArquivoOrigem = "PROC_TRAB_{0}.xml";
            //var numeroProcessoTrabalhista = "";
            //var _tipoArquivo = "";
            //var _dataProcessamento = "";

            //#region Se veio do arquivo ele trata pelo ReadAllLines, se não ele pega a lista de linha pela variavel linesText enviada
            //List<string> lines;
            //if (linesText == null)
            //    lines = File.ReadAllLines(inputFilePath).ToList();
            //else
            //    lines = linesText;
            //#endregion

            //#region Definir nome do arquivo de origem e dados do formulário
            //if (lines != null)
            //{
            //    var result = lines.Where(x => x.Contains("NÚMERO PROCESSO TRABALHISTA: ")).FirstOrDefault();
            //    if (result != null)
            //    {
            //        numeroProcessoTrabalhista = result.Replace("NÚMERO PROCESSO TRABALHISTA: ", "");
            //        numeroProcessoTrabalhista = numeroProcessoTrabalhista.Replace("\r", "");
            //    }

            //    //Nome do arquivo
            //    var TipoArquivo = lines.Where(x => x.Contains("TAREFA DE PROCESSAMENTO:")).FirstOrDefault();
            //    if (TipoArquivo != null)
            //    {
            //        if (TipoArquivo.Contains("P2500"))
            //        {
            //            _tipoArquivo = "F2500";
            //        }
            //        else
            //        {
            //            _tipoArquivo = "F2501";
            //        }
            //    }

            //    //Data do processamento
            //    _dataProcessamento = lines.Where(x => x.Contains("DATA E HORA DE PROCESSAMENTO: ")).FirstOrDefault();
            //    if (_dataProcessamento != null)
            //    {
            //        _dataProcessamento = _dataProcessamento.Replace("DATA E HORA DE PROCESSAMENTO: ", "");
            //        _dataProcessamento = _dataProcessamento.Replace("\r", "");
            //    }
            //}

            //var empresas = ObtemEmpresasAgrupadoras(eSocialDbContext);
            //var empresa = empresas.First(x => x.NomEmpresaAgrupadora == "OI"); //Default, caso não encontre

            //var queryF2500 = eSocialDbContext.EsF2500
            //    .Include(x => x.CodP).ThenInclude(x => x.CodProcessoNavigation).ThenInclude(x => x.CodParteEmpresaNavigation).ThenInclude(x => x.IdEsEmpresaAgrupadoraNavigation)
            //.Where(x => x.InfoprocessoNrproctrab == numeroProcessoTrabalhista)
            //.Select(s => new InfoContracts()
            //{
            //    IdF2500 = s.IdF2500,
            //    IdetrabCpftrab = s.IdetrabCpftrab,
            //    NomeArquivoEnviado = s.NomeArquivoEnviado,
            //    IdeempregadorNrinsc = s.IdeempregadorNrinsc,
            //    IdeempregadorTpinsc = s.IdeempregadorTpinsc,
            //    DirXmlRetornoNaoProcessado = (s.CodP.CodProcessoNavigation == null || s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation == null ||
            //            s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation.IdEsEmpresaAgrupadoraNavigation == null)
            //                        ? empresa.DirXmlRetornoNaoProcessado :
            //                            s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation.IdEsEmpresaAgrupadoraNavigation.DirXmlRetornoNaoProcessado,
            //    CodParteNavigation = s.CodP.CodParteNavigation
            //}).ToList();


            //var queryF2501 = eSocialDbContext.EsF2501
            //    .Include(x => x.CodP).ThenInclude(x => x.CodProcessoNavigation).ThenInclude(x => x.CodParteEmpresaNavigation).ThenInclude(x => x.IdEsEmpresaAgrupadoraNavigation)
            //.Where(x => x.IdeprocNrproctrab == numeroProcessoTrabalhista)
            //.Select(s => new InfoContracts()
            //{
            //    IdF2500 = s.IdF2501,
            //    IdetrabCpftrab = s.IdetrabCpftrab,
            //    NomeArquivoEnviado = s.NomeArquivoEnviado,
            //    IdeempregadorNrinsc = s.IdeempregadorNrinsc,
            //    IdeempregadorTpinsc = s.IdeempregadorTpinsc,
            //    DirXmlRetornoNaoProcessado = (s.CodP.CodProcessoNavigation == null || s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation == null ||
            //            s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation.IdEsEmpresaAgrupadoraNavigation == null)
            //                        ? empresa.DirXmlRetornoNaoProcessado :
            //                            s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation.IdEsEmpresaAgrupadoraNavigation.DirXmlRetornoNaoProcessado,
            //    CodParteNavigation = s.CodP.CodParteNavigation
            //}).ToList();

            ////var diretorioFPW = Path.Combine(empresa.DirXmlRetornoNaoProcessado, Path.GetFileNameWithoutExtension(item.NomeArquivoEnviado));
            //#endregion

            //#region Lendo os dados do arquivo Texto
            //var _tarefa = "";
            //var _numeroProcessoTrabalhista = "";
            //var _cPFTrabalhador = "";
            //var _dataSentenca = "";
            //var _codigo = "";
            //var _localizacao = "";
            //var _tipo = "";
            //var _mensagem = "";

            //var lstAnexos = new List<Anexo>();
            //foreach (string line in lines)
            //{
            //    if (line.Contains("TAREFA DE PROCESSAMENTO:")) _tarefa = line.Split(':')[1].Trim();

            //    if (line.Contains("NÚMERO PROCESSO TRABALHISTA:")) _numeroProcessoTrabalhista = line.Split(':')[1].Trim();
            //    if (line.Contains("CPF TRABALHADOR:")) _cPFTrabalhador = line.Split(':')[1].Trim();
            //    if (line.Contains("DATA DA SENTENÇA/CONCILIAÇÃO:")) _dataSentenca = line.Split(':')[1].Trim();

            //    if (line.Contains("CÓDIGO:")) _codigo = line.Split(':')[1].Trim();
            //    if (line.Contains("LOCALIZAÇÃO:")) _localizacao = line.Split(':')[1].Trim();
            //    if (line.Contains("TIPO:")) _tipo = line.Split(':')[1].Trim();
            //    if (line.Contains("MENSAGEM:"))
            //    {
            //        _mensagem = line.Split(':')[1].Trim();
            //        lstAnexos.Add(new Anexo(_tarefa, _codigo, _cPFTrabalhador, _dataSentenca, _localizacao, _mensagem, _numeroProcessoTrabalhista, _tipo));
            //    }
            //}
            //#endregion

            //#region Gerando arquivoS XMLs
            //var lstAnexosDistinctsByCPF = lstAnexos.DistinctBy(d => d.CPFTrabalhador).Select(s => s.CPFTrabalhador).ToList();
            //foreach (var _cpf in lstAnexosDistinctsByCPF)
            //{
            //    var queryFilter = (_tipoArquivo == "F2500" ?
            //                        queryF2500.Where(w => w.IdetrabCpftrab == string.Concat(_cpf.Where(Char.IsDigit))).FirstOrDefault() :
            //                        queryF2501.Where(w => w.IdetrabCpftrab == string.Concat(_cpf.Where(Char.IsDigit))).FirstOrDefault());

            //    /*var queryFilter = (_tipoArquivo == "F2500" ?
            //                        queryF2500.Where(w => w.IdetrabCpftrab == _cpf.Replace("-", "").Replace(".", "")).FirstOrDefault() :
            //                        queryF2501.Where(w => w.IdetrabCpftrab == _cpf.Replace("-", "").Replace(".", "")).FirstOrDefault());

            //    */
            //    //Se não tiver mais informações sobre o envio, precisa esperar o reenvio
            //    if (queryFilter == null || string.IsNullOrEmpty(queryFilter.NomeArquivoEnviado))
            //    {
            //        return false;
            //    }
            //    XmlDocument xmlDoc = new XmlDocument();

            //    XmlDeclaration xmlDecl;
            //    xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            //    XmlElement root = xmlDoc.DocumentElement;
            //    xmlDoc.InsertBefore(xmlDecl, root);

            //    XmlElement rootElement = xmlDoc.CreateElement("InconsistenciaImportacaoArquivo");
            //    xmlDoc.AppendChild(rootElement);

            //    XmlElement identificacaoEmpregradorElement = xmlDoc.CreateElement("IdentificadorDoEmpregador");
            //    rootElement.AppendChild(identificacaoEmpregradorElement);

            //    XmlElement tpInscElement = xmlDoc.CreateElement("tpInsc");
            //    tpInscElement.InnerText = queryFilter == null ? "1" : queryFilter.IdeempregadorTpinsc.ToString() ?? "1";
            //    identificacaoEmpregradorElement.AppendChild(tpInscElement);

            //    XmlElement nrInscElement = xmlDoc.CreateElement("nrInsc");
            //    nrInscElement.InnerText = (queryFilter == null ? "1" : queryFilter.IdeempregadorNrinsc);

            //    identificacaoEmpregradorElement.AppendChild(nrInscElement);

            //    XmlElement tipoEventoElement = xmlDoc.CreateElement("TipoDoEvento");
            //    tipoEventoElement.InnerText = "Processo Trabalhista";
            //    rootElement.AppendChild(tipoEventoElement);

            //    XmlElement nomeArquivoElement = xmlDoc.CreateElement("NomeDoArquivoOrigem");
            //    nomeArquivoElement.InnerText = (queryFilter == null ? "1" : queryFilter.NomeArquivoEnviado);
            //    rootElement.AppendChild(nomeArquivoElement);

            //    XmlElement dataHoraDoProcessamentoElement = xmlDoc.CreateElement("DataHoraDoProcessamento");
            //    dataHoraDoProcessamentoElement.InnerText = _dataProcessamento;
            //    rootElement.AppendChild(dataHoraDoProcessamentoElement);

            //    XmlElement inconsistenciasElement = xmlDoc.CreateElement("Inconsistencias");
            //    rootElement.AppendChild(inconsistenciasElement);
            //    List<XmlElement> inconsistencias = new List<XmlElement>();
            //    foreach (var line in lstAnexos.Where(w => w.CPFTrabalhador == _cpf))
            //    {
            //        XmlElement inconsistenciaElement = xmlDoc.CreateElement("Inconsistencia");
            //        inconsistenciasElement.AppendChild(inconsistenciaElement);
            //        inconsistencias.Add(inconsistenciaElement);

            //        XmlElement nomeElement = xmlDoc.CreateElement("LinhaDoArquivo");
            //        nomeElement.InnerText = "1";
            //        inconsistenciaElement.AppendChild(nomeElement);

            //        XmlElement enderecoElement = xmlDoc.CreateElement("MensagemInconsistencia");
            //        enderecoElement.InnerText = line.Mensagem;
            //        inconsistenciaElement.AppendChild(enderecoElement);
            //    }

            //    var diretorioFPW = Path.Combine(queryFilter.DirXmlRetornoNaoProcessado, Path.GetFileNameWithoutExtension(queryFilter.NomeArquivoEnviado));
            //    Directory.CreateDirectory(diretorioFPW);

            //    xmlDoc.Save(diretorioFPW + "\\Inconsistencias_" + _cpf + ".XML");
            //}
            //#endregion

            return true;
        }

        public static bool RecebeXML(List<string> linesText, ESocialDbContext eSocialDbContext)
        {
            //#region Lendo os dados do arquivo
            //var _nrInsc = "";
            //var _nomeArquivoOrigen = "";
            //var _dirXmlRetornoNaoProcessado = "";

            //var lstAnexos = new List<Anexo>();
            //foreach (string line in linesText)
            //{
            //    if (line.Contains("<nrInsc>")) _nrInsc = line.Replace("<nrInsc>", "").Replace("</nrInsc>", "").Trim();
            //    if (line.Contains("<NomeDoArquivoOrigem>")) _nomeArquivoOrigen = line.Replace("<NomeDoArquivoOrigem>", "").Replace("</NomeDoArquivoOrigem>", "").Trim();
            //}
            //#endregion

            //#region Buscar na base no formulário para saber informações da pasta onde gravar o retorno
            //var empresas = ObtemEmpresasAgrupadoras(eSocialDbContext);
            //var empresa = empresas.First(x => x.NomEmpresaAgrupadora == "OI"); //Default, caso não encontre
            //if (!string.IsNullOrEmpty(_nrInsc) && !string.IsNullOrEmpty(_nomeArquivoOrigen))
            //{
            //    var queryFilter = new List<InfoContracts>();

            //    if (_nomeArquivoOrigen.Contains("F2500"))
            //    {
            //        var queryF2500 = eSocialDbContext.EsF2500
            //        .Include(x => x.CodP).ThenInclude(x => x.CodProcessoNavigation).ThenInclude(x => x.CodParteEmpresaNavigation).ThenInclude(x => x.IdEsEmpresaAgrupadoraNavigation)
            //        .Where(x => x.NomeArquivoEnviado == _nomeArquivoOrigen)
            //        .Select(s => new InfoContracts()
            //        {
            //            IdF2500 = s.IdF2500,
            //            IdetrabCpftrab = s.IdetrabCpftrab,
            //            NomeArquivoEnviado = s.NomeArquivoEnviado,
            //            IdeempregadorNrinsc = s.IdeempregadorNrinsc,
            //            IdeempregadorTpinsc = s.IdeempregadorTpinsc,
            //            DirXmlRetornoNaoProcessado = (s.CodP.CodProcessoNavigation == null || s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation == null ||
            //                    s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation.IdEsEmpresaAgrupadoraNavigation == null)
            //                                ? empresa.DirXmlRetornoNaoProcessado :
            //                                    s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation.IdEsEmpresaAgrupadoraNavigation.DirXmlRetornoNaoProcessado,
            //            CodParteNavigation = s.CodP.CodParteNavigation
            //        }).ToList();
            //        queryFilter = queryF2500;

            //    }
            //    else
            //    {
            //        var queryF2501 = eSocialDbContext.EsF2501
            //            .Include(x => x.CodP).ThenInclude(x => x.CodProcessoNavigation).ThenInclude(x => x.CodParteEmpresaNavigation).ThenInclude(x => x.IdEsEmpresaAgrupadoraNavigation)
            //        .Where(x => x.NomeArquivoEnviado == _nomeArquivoOrigen)
            //        .Select(s => new InfoContracts()
            //        {
            //            IdF2500 = s.IdF2501,
            //            IdetrabCpftrab = s.IdetrabCpftrab,
            //            NomeArquivoEnviado = s.NomeArquivoEnviado,
            //            IdeempregadorNrinsc = s.IdeempregadorNrinsc,
            //            IdeempregadorTpinsc = s.IdeempregadorTpinsc,
            //            DirXmlRetornoNaoProcessado = (s.CodP.CodProcessoNavigation == null || s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation == null ||
            //                    s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation.IdEsEmpresaAgrupadoraNavigation == null)
            //                                ? empresa.DirXmlRetornoNaoProcessado :
            //                                    s.CodP.CodProcessoNavigation.CodParteEmpresaNavigation.IdEsEmpresaAgrupadoraNavigation.DirXmlRetornoNaoProcessado,
            //            CodParteNavigation = s.CodP.CodParteNavigation
            //        }).ToList();
            //        queryFilter = queryF2501;
            //    }
            //    _dirXmlRetornoNaoProcessado = queryFilter.FirstOrDefault().DirXmlRetornoNaoProcessado;
            //}
            //else
            //{
            //    return false;
            //}

            //#endregion

            //#region Salvar arquivo na pasta de não_processados
            //var diretorioFPW = Path.Combine(_dirXmlRetornoNaoProcessado, Path.GetFileNameWithoutExtension(_nomeArquivoOrigen));
            //Directory.CreateDirectory(diretorioFPW);
            //File.WriteAllLinesAsync(diretorioFPW + "\\Inconsistencias_" + _nrInsc + ".XML", linesText);
            //#endregion

            return true;
        }
    }
    #endregion

    #region Contracts
    public class Anexo
    {
        public Anexo(string tarefa, string codigo, string cPFTrabalhador, string dataSentenca, string localizacao,
                    string mensagem, string numeroProcessoTrabalhista, string tipo)
        {
            Tarefa = tarefa;
            Codigo = codigo;
            CPFTrabalhador = cPFTrabalhador;
            DataSentenca = dataSentenca;
            Localizacao = localizacao;
            Mensagem = mensagem;
            NumeroProcessoTrabalhista = numeroProcessoTrabalhista;
            Tipo = tipo;
        }

        public string Tarefa { get; set; }

        public string NumeroProcessoTrabalhista { get; set; }
        public string CPFTrabalhador { get; set; }
        public string DataSentenca { get; set; }

        public string Codigo { get; set; }
        public string Localizacao { get; set; }
        public string Tipo { get; set; }
        public string Mensagem { get; set; }
    }

    public class InfoContracts
    {
        public int IdF2500 { get; set; }
        public string IdetrabCpftrab { get; set; }
        public string NomeArquivoEnviado { get; set; }
        public string IdeempregadorNrinsc { get; set; }
        public byte? IdeempregadorTpinsc { get; set; }
        public string DirXmlRetornoNaoProcessado { get; set; }
        public virtual Parte CodParteNavigation { get; set; }
    }
    #endregion
}
