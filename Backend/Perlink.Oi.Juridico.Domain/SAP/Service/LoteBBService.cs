using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using System.Linq;
using Shared.Domain.Impl.Service;
using System.IO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class LoteBBService : BaseCrudService<Lote, long>, ILoteBBService
    {


        private readonly ILoteBBRepository repository;
        private readonly IParametroRepository parametroRepository;
        private readonly ILancamentoProcessoRepository lancamentoProcessoRepository;

        public LoteBBService(ILoteBBRepository repository, IParametroRepository parametro, ILancamentoProcessoRepository lancamentoProcessoRepository) : base(repository)
        {
            this.repository = repository;
            parametroRepository = parametro;
            this.lancamentoProcessoRepository = lancamentoProcessoRepository;
        }

        public byte[] Gerar(ArquivoBBDTO arquivoBBDTO, ref string msgErro)
        {
            //GravarLogLote("Iniciando regerar arquivo de Lote BB");
            string[] MensagemsValidacao = repository.ValidaLancamentosLoteBB(arquivoBBDTO.NumeroLoteBB);

            if (MensagemsValidacao != null)
            {
                msgErro = "Existem lançamentos que não estão com as parametrizações efetuadas para geração do arquivo do BB. <br>";
                foreach (var mensagem in MensagemsValidacao)
                {
                    msgErro += $"{mensagem}<br>";
                }
                return new byte[0];
            }

            if (!ValidaEstado(arquivoBBDTO, ref msgErro))
                return new byte[0];

            VerificaAlteraDataGuia(arquivoBBDTO, ref msgErro);

            return CriarArquivo(arquivoBBDTO, ref msgErro);
        }

        private byte[] CriarArquivo(ArquivoBBDTO arquivoBBDTO, ref string msgErro)
        {            
            var caminhoDestino = arquivoBBDTO.EnviarServidor ? parametroRepository.RecuperarPorId("SAP_SERV_DIR_DESTINO_BB").Result.Conteudo : "";
            string ultimosTresCarac = arquivoBBDTO.NumeroLoteBB.ToString().Substring(arquivoBBDTO.NumeroLoteBB.ToString().Length - 3);
            arquivoBBDTO.NomeArquivo = $"DJO100{ultimosTresCarac}.REM";
            arquivoBBDTO.ConteudoArquivo = new StringBuilder();

            //GravarLogLote("Buscando dados para montar o arquivo BB");
            bool processou = repository.MontarHeader(arquivoBBDTO);
            //GravarLogLote("Header do arquivo finalizado");
            if(processou)
                processou = repository.MontarDetalheArquivo(arquivoBBDTO);
            //GravarLogLote("Detalhes do arquivo finalizado");
            if(processou)
                processou = repository.MontarTrailerArquivo(arquivoBBDTO);
            //GravarLogLote("Trailer do arquivo finalizado");
            
            byte[] arquivo = new byte[0];            
            if (!processou)
            {
                msgErro = "Ocorreu um erro ao tentar recuperar o conteúdo do arquivo.";
                return arquivo;
            }

            using (var memoryStream = new MemoryStream())
            using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                streamWriter.WriteLine(arquivoBBDTO.ConteudoArquivo.ToString());
                streamWriter.Flush();
                arquivo = memoryStream.ToArray();
            };
            //GravarLogLote("Arquivo gerado com sucesso");                
            if (arquivoBBDTO.EnviarServidor)
            {
                //GravarLogLote($"Criando arquivo no servidor: {caminhoDestino}");
                Directory.CreateDirectory(caminhoDestino);
                var destino = $"{caminhoDestino}\\{Path.GetFileName(arquivoBBDTO.NomeArquivo)}";
                File.WriteAllBytes(destino, arquivo); //escreve os byte no diretório serv
            }

            return arquivo;
        }

        private void VerificaAlteraDataGuia(ArquivoBBDTO arquivoBBDTO, ref string msgErro)
        {
            if (string.IsNullOrEmpty(msgErro) && arquivoBBDTO.DataGuia.HasValue)
            {
                //GravarLogLote("Iniciando alteração de Data da Guia.");
                var lancamentos = repository.ObterLancamentosPorLoteComAssociacao(arquivoBBDTO.CodigoLote).Result;
                if (lancamentos != null && lancamentos.Count() > 0)
                {
                    foreach (var lancamento in lancamentos)
                    {
                        lancamento.DataGuiaJudicial = arquivoBBDTO.DataGuia;
                        lancamentoProcessoRepository.Atualizar(lancamento);
                    }
                    lancamentoProcessoRepository.Commit();
                    //GravarLogLote("Data da Guia alterada com sucesso.");
                }
                else
                    msgErro = "Não foi encontrado nenhum lançamento para o lote durante a alteração da Data da Guia.";
            }
        }

        private bool ValidaEstado(ArquivoBBDTO arquivoBBDTO, ref string msgErro)
        {
            var estados = repository.RecuperarEstados(arquivoBBDTO.CodigoLote).Result;
            bool isDiferente = false;
            string codEstado;
            bool isValido;
            if (estados != null && estados.Count() > 0)
            {
                codEstado = estados.FirstOrDefault();
                foreach (var estadoAtual in estados.Skip(1))
                {
                    if (estadoAtual != codEstado)
                    {
                        isDiferente = true;
                        break;
                    }
                }
                if (isDiferente)
                    codEstado = "RJ";
            }
            else
            {
                msgErro = "Não foi possível recuperar os estados dos processos do lote.";
                return false;
            }

            if (!string.IsNullOrEmpty(codEstado))
            {
                arquivoBBDTO.CodigoEstado = codEstado;
                isValido = repository.ExisteParametroConvenio(arquivoBBDTO.NumeroLoteBB, codEstado);
                if (!isValido)
                {
                    msgErro = "Não existe parametrização de convênio para o Lote. Favor verificar.";
                    return false;
                }
            }
            return true;
        }
    }
}