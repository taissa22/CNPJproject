using Oi.Juridico.Contextos.V2.RelatorioSolicitacaoLancamentoPexContext.Entities;
using Oi.Juridico.Shared.V2.Tools;
using Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Enums;

namespace Oi.Juridico.WebApi.V2.Areas.RelatorioSolicitacaoLancamentoPex.Model
{
    public class AgendamentoRelatorioSolicitacoes
    {

        public string IdAgendamento { get; set; } = "0";

        public string NomeDoRelatorio { get; set; } = "";

        public string? Descricao { get; set; } = "";

        public string TipoExecucao { get; set; } = "N";

        public string DiaSemana { get; set; } = "";

        public string DiaMes { get; set; } = "";

        public string ExecutarNaData { get; set; } = "";

        public string DataIniAgendamento { get; set; } = "";

        public string DataFimAgendamento { get; set; } = "";

        public string? SomenteEmDiasUteis { get; set; } = "N";

        public string DataIniSolicitacao { get; set; } = "";

        public string DataFimSolicitacao { get; set; } = "";

        public string DataInivencimento { get; set; } = "";

        public string DataFimvencimento { get; set; } = "";

        public string DatUltExecucao { get; set; } = "";

        public string DatCriacao { get; set; } = "";

        public string DatUltAlteracao { get; set; } = "";

        public string DataProxExecucao { get; set; } = "";
                
        public string DuracaoExecucao { get; set; } = "";
                
        public string TempoDecorrido { get; set; } = "";

        public string NomeArquivoGerado { get; set; } = "";

        public int[] ColunasRelatorioSelecionadas { get; set; } = Array.Empty<int>();

        public int[] EscritoriosSelecionados { get; set; } = Array.Empty<int>();

        public int[] TiposDeLancamentoSelecionados { get; set; } = Array.Empty<int>();

        public int[] StatusSolicitacoesSelecionados { get; set; } = Array.Empty<int>();

        public string[] UfsSelecionadas { get; set; } = Array.Empty<string>();

        public string? Usuario { get; set; } = "PERLINK";

        public string ErroEmAtributos { get; set; } = "";

        public AgendamentoRelatorioSolicitacoes()
        {

        }

        public AgendamentoRelatorioSolicitacoes(AgPexRelSolic agendamento)
        {

            IdAgendamento = agendamento.Id.ToString(); ;

            TipoExecucao = agendamento.TipAgendamento.ToString().Trim();

            NomeDoRelatorio = agendamento.NomAgendamento.Trim();
            
            Descricao = agendamento.DscAgendamento;

            DiaSemana = agendamento.NumSemanaAgendamento != null ? agendamento.NumSemanaAgendamento.Value.ToString() : "";

            DiaMes = agendamento.NumMesAgendamento != null ? agendamento.NumMesAgendamento.Value.ToString() : "";

            DataIniAgendamento = agendamento.DatAgendamentoIni != null ?
                                 agendamento.DatAgendamentoIni.Value.ToString("yyyy-MM-dd") : "";

            DataFimAgendamento = agendamento.DatAgendamentoFim != null ?
                                 agendamento.DatAgendamentoFim.Value.ToString("yyyy-MM-dd") : "";

            DatUltExecucao = agendamento.DatUltExecucao != null ?
                             agendamento.DatUltExecucao.Value.ToString("yyyy-MM-dd hh:mm") : "";


            SomenteEmDiasUteis = agendamento.IndExecutarDiaUtil;

            Usuario = agendamento.UsrCodUsuario;

            DatCriacao = agendamento.DatCriacao.ToString("yyyy-MM-dd");

            DatUltAlteracao = agendamento.DatUltAlteracao.ToString("yyyy-MM-dd");

            DataProxExecucao = agendamento.DataProxExecucao.ToString("yyyy-MM-dd");

            DuracaoExecucao = CalculaDuracaoExecucao(agendamento.DatFimExecucao, agendamento.DatInicioExecucao);

            TempoDecorrido = CalculaTempoDecorrido(agendamento.DatFimExecucao);

            NomeArquivoGerado = agendamento.NomArqGerado;

        }


        public AgendamentoRelatorioSolicitacoes(AgPexRelSolicHist agendamento)
        {

            IdAgendamento = agendamento.Id.ToString(); 

            TipoExecucao = agendamento.TipAgendamento.ToString();

            NomeDoRelatorio = agendamento.NomAgendamento.Trim();

            Descricao = agendamento.DscAgendamento.Trim();

            DiaSemana = agendamento.NumSemanaAgendamento != null ? agendamento.NumSemanaAgendamento.Value.ToString() : "";

            DiaMes = agendamento.NumMesAgendamento != null ? agendamento.NumMesAgendamento.Value.ToString() : "";

            DataIniAgendamento = agendamento.DatAgendamentoIni != null ?
                                 agendamento.DatAgendamentoIni.Value.ToString("yyyy-MM-dd") : "";

            DataFimAgendamento = agendamento.DatAgendamentoFim != null ?
                                 agendamento.DatAgendamentoFim.Value.ToString("yyyy-MM-dd") : "";

            DatUltExecucao = agendamento.DatUltExecucao != null ?
                             agendamento.DatUltExecucao.Value.ToString("yyyy-MM-dd hh:mm") : "";


            SomenteEmDiasUteis = agendamento.IndExecutarDiaUtil;

            Usuario = agendamento.UsrCodUsuario;

            DatCriacao = agendamento.DatCriacao.ToString("yyyy-MM-dd");

            DatUltAlteracao = agendamento.DatUltAlteracao.ToString("yyyy-MM-dd");

            DataProxExecucao = agendamento.DataProxExecucao.ToString("yyyy-MM-dd");

            DuracaoExecucao = CalculaDuracaoExecucao(agendamento.DatFimExecucao, agendamento.DatInicioExecucao);

            TempoDecorrido = CalculaTempoDecorrido(agendamento.DatFimExecucao);

            NomeArquivoGerado = agendamento.NomArqGerado;

        }



        public AgPexRelSolic AdicionarAgendamento(IQueryable<AgPexRelSolic> agendSolic, string usuario)
        {

            var agendamento = new AgPexRelSolic();

            try
            {
                agendamento.NomAgendamento = NomeDoRelatorio.Trim();
                
                if (!CritcaEmNomeAgendamento(agendSolic)) return new AgPexRelSolic(); 

                agendamento.DscAgendamento = Descricao.Trim();

                agendamento.NumSemanaAgendamento = DiaSemana != "" ? byte.Parse(DiaSemana) : null;

                agendamento.NumMesAgendamento = DiaMes != "" ? short.Parse(DiaMes) : null;

                ErroEmAtributos = "Data de Agendamento Inicial inválida";
                agendamento.DatAgendamentoIni = DataIniAgendamento != "" ? DateTime.Parse(DataIniAgendamento) : null;

                ErroEmAtributos = "Data de Agendamento Final inválida";
                agendamento.DatAgendamentoFim = DataFimAgendamento != "" ? DateTime.Parse(DataFimAgendamento) : null;

                agendamento.IndExecutarDiaUtil = TrataValorCampoSomenteEmDiasUteis();
                
                agendamento.UsrCodUsuario = usuario;

                agendamento.DatInicioExecucao = null;

                agendamento.DatFimExecucao = null;
                               
                agendamento.StatusExecucao = 0;

                agendamento.MsgErro = "";

                agendamento.DatCriacao = DateTime.Now;
                                
                ErroEmAtributos = "Tipo de Agendamento deve ser um valor inteiro entre '0' e '9'.";
                agendamento.TipAgendamento = short.Parse(TipoExecucao);

                agendamento.DataProxExecucao = DateTime.Now.Date;

                agendamento.DatUltAlteracao = DateTime.Now;

                agendamento.DatUltExecucao = null;


                ErroEmAtributos = "";

                return agendamento;

            }
            catch (Exception)
            {
                
                return new AgPexRelSolic();
            }

        }


        public AgPexRelSolic AplicaRegraProximaExecucaoPorTipoAgendamento(AgPexRelSolic agendamento)
        {


            switch ((TipoExecucaoRelatorio)agendamento.TipAgendamento)
            {
                case TipoExecucaoRelatorio.Hoje:

                    agendamento.StatusExecucao = 0;
                    agendamento.DataProxExecucao = DateTime.Now.Date;

                    break;

                case TipoExecucaoRelatorio.Diariamente:

                    agendamento.StatusExecucao = 0;
                    agendamento.IndExecutarDiaUtil ??= "N";

                    if (agendamento.IndExecutarDiaUtil != "N")
                        foreach (DateTime day in Util.EachDay(agendamento.DatAgendamentoIni, agendamento.DatAgendamentoFim))
                        {
                            if (!Util.EhSabadoOuDomingo(day))
                            {
                                agendamento.DataProxExecucao = day;
                                break;
                            }
                        }
                    else agendamento.DataProxExecucao = agendamento.DatAgendamentoIni ?? DateTime.Now.Date;


                    break;
                case TipoExecucaoRelatorio.Semanalmente:

                    agendamento.StatusExecucao = 0;
                    agendamento.DataProxExecucao = Util.GetNextWeekday(agendamento.DatAgendamentoIni,
                                                                       agendamento.DatAgendamentoFim,
                                                                       agendamento.NumSemanaAgendamento,
                                                                       agendamento.IndExecutarDiaUtil);

                    break;
                case TipoExecucaoRelatorio.Mensalmente:

                    agendamento.StatusExecucao = 0;
                    agendamento.DataProxExecucao = Util.GetNextMonthday(agendamento.DatAgendamentoIni,
                                                                        agendamento.DatAgendamentoFim,
                                                                        agendamento.NumMesAgendamento,
                                                                        agendamento.IndExecutarDiaUtil);

                    break;
                case TipoExecucaoRelatorio.Na_Data:

                    agendamento.StatusExecucao = 0;
                    agendamento.DatAgendamentoIni ??= DateTime.Now.Date;

                    if (agendamento.IndExecutarDiaUtil != "N")
                        foreach (DateTime day in Util.EachDay(agendamento.DatAgendamentoIni, agendamento.DatAgendamentoFim))
                        {
                            if (!Util.EhSabadoOuDomingo(day))
                            {
                                agendamento.DataProxExecucao = day;
                                break;
                            }
                        }
                    else
                        //agendamento.DataProxExecucao = agendamento.DatAgendamentoIni ?? DateTime.Now.Date;
                        agendamento.DataProxExecucao = DateTime.Parse(DataProxExecucao);

                    break;
            }

            return agendamento;

        }

        // Métodos Ação 
        public AgPexRelSolicColunas AdicionarColunasRel(decimal idAgendamento, int codColuna)
        {
            var coluna = new AgPexRelSolicColunas();
            try
            {
                ErroEmAtributos = "Valor Inválido para o Id da Coluna. O valor deve ser um número.";
                coluna.AgPexRelSolicId = idAgendamento;
                coluna.CodColRelSolic = (decimal)codColuna;

                ErroEmAtributos = "";

                return coluna;
            }
            catch (Exception)
            {
                return new AgPexRelSolicColunas();
            }
        }

        public AgPexRelSolicData AdicionarDatasRel(decimal idAgendamento)
        {
            var datas = new AgPexRelSolicData();

            try
            {
                datas.AgPexRelSolicId = idAgendamento;

                ErroEmAtributos = "Data de Solicitação Inicial inválida";
                datas.DatSolicIni = DataIniSolicitacao != "" ? DateTime.Parse(DataIniSolicitacao) : null;

                ErroEmAtributos = "Data de Solicitação Inicial inválida";
                datas.DatSolicFim = DataFimSolicitacao != "" ? DateTime.Parse(DataFimSolicitacao) : null;

                ErroEmAtributos = "Data de Vencimento Inicial inválida";
                datas.DatVencimentoIni = DataInivencimento != "" ? DateTime.Parse(DataInivencimento) : null;

                ErroEmAtributos = "Data de Vencimento Inicial inválida";
                datas.DatVencimentoFim = DataFimvencimento != "" ? DateTime.Parse(DataFimvencimento) : null;

                ErroEmAtributos = "";

                return datas;
            }
            catch (Exception)
            {
                return new AgPexRelSolicData();
            }
        }

        public List<AgPexRelSolicColunas> AdicionarListasColunasRel(decimal idAgendamento)
        {

            var lstColunas = new List<AgPexRelSolicColunas>();
            try
            {

                ErroEmAtributos = "Valor Inválido para o Id da Coluna. O valor deve ser um número.";

                foreach (var item in ColunasRelatorioSelecionadas)
                {
                    var coluna = new AgPexRelSolicColunas
                    {
                        AgPexRelSolicId = idAgendamento,
                        CodColRelSolic = (decimal)item
                    };
                    lstColunas.Add(coluna);
                }

                ErroEmAtributos = "";

                return lstColunas;
            }
            catch (Exception)
            {
                return new List<AgPexRelSolicColunas>();
            }
        }

        public List<AgPexRelSolicEscrit> AdicionarListasEscritoriosRel(decimal idAgendamento)
        {
            var lstEscritorio = new List<AgPexRelSolicEscrit>();

            try
            {

                ErroEmAtributos = "Valor Inválido para o Id de Escritorio. O valor deve ser um número.";

                foreach (var item in EscritoriosSelecionados)
                {
                    var escritorio = new AgPexRelSolicEscrit
                    {
                        AgPexRelSolicId = idAgendamento,
                        CodEscRelSolic = (decimal)item,
                        NomEscRelSolic = " "
                    };
                    lstEscritorio.Add(escritorio);
                }

                ErroEmAtributos = "";

                return lstEscritorio;
            }
            catch (Exception)
            {
                return new List<AgPexRelSolicEscrit>();
            }


        }

        public List<AgPexRelSolicStatus> AdicionarStatusSolcitacaoRel(decimal idAgendamento)
        {
            var lstSolicStatus = new List<AgPexRelSolicStatus>();

            try
            {

                ErroEmAtributos = "Valor Inválido para o Id de Solicitação Status. O valor deve ser um número.";

                foreach (var item in StatusSolicitacoesSelecionados)
                {
                    var status = new AgPexRelSolicStatus
                    {
                        AgPexRelSolicId = idAgendamento,
                        CodStaRelSolic = (decimal)item,
                        NomStaRelSolic = " "
                    };
                    lstSolicStatus.Add(status);
                }

                ErroEmAtributos = "";

                return lstSolicStatus;
            }
            catch (Exception)
            {
                return new List<AgPexRelSolicStatus>();
            }
        }

        public List<AgPexRelSolicTpLan> AdicionarTipoLancamentoRel(decimal idAgendamento)
        {
            var lstTipoLancamento = new List<AgPexRelSolicTpLan>();

            try
            {

                ErroEmAtributos = "Valor Inválido para o Id de Tipo de Lancamento. O valor deve ser um número.";

                foreach (var item in TiposDeLancamentoSelecionados)
                {
                    var status = new AgPexRelSolicTpLan
                    {
                        AgPexRelSolicId = idAgendamento,
                        CodTplRelSolic = (decimal)item,
                        NomTplRelSolic = " "
                    };
                    lstTipoLancamento.Add(status);
                }

                ErroEmAtributos = "";

                return lstTipoLancamento;
            }
            catch (Exception)
            {
                return new List<AgPexRelSolicTpLan>();
            }
        }

        public List<AgPexRelSolicUf> AdicionarUfRel(decimal idAgendamento)
        {
            var lstUf = new List<AgPexRelSolicUf>();

            try
            {

                ErroEmAtributos = "Valor Inválido para o Id de Tipo de Lancamento. O valor deve ser um número.";

                foreach (var item in UfsSelecionadas)
                {
                    var uf = new AgPexRelSolicUf
                    {
                        AgPexRelSolicId = idAgendamento,
                        CodUfRelSolic = item,
                        NomUfRelSolic = " "
                    };
                    lstUf.Add(uf);
                }

                ErroEmAtributos = "";

                return lstUf;
            }
            catch (Exception)
            {
                return new List<AgPexRelSolicUf>();
            }
        }

        public string TrataValorCampoSomenteEmDiasUteis()
        {
            SomenteEmDiasUteis ??= "N";

            if (SomenteEmDiasUteis.Trim().ToUpper() == "FALSE")
                return "N";

            if (SomenteEmDiasUteis.Trim().ToUpper() == "TRUE")
                return "S";

            return SomenteEmDiasUteis;

        }

        //Metodos Verificação

        public bool VerificaValores(IQueryable<AgPexRelSolic> agendSolic)
        {

            try
            {
                ErroEmAtributos = "ID do Agendamento Inválido";

                var id = int.Parse(IdAgendamento);

                if (id == 0) return false;

                ErroEmAtributos = "Dia Semana Inválido";
                byte? diaSem = DiaSemana != "" ? byte.Parse(DiaSemana) : null;

                ErroEmAtributos = "Dia Mês Inválido";
                short? diaMes = DiaMes != "" ? short.Parse(DiaMes) : null;

                ErroEmAtributos = "Data Inicial Agendamento Inválido";
                DateTime? datAgendamentoIni = DataIniAgendamento != "" ? DateTime.Parse(DataIniAgendamento) : null;

                ErroEmAtributos = "Data de Agendamento Final inválida";
                DateTime? datAgendamentoFim = DataFimAgendamento != "" ? DateTime.Parse(DataFimAgendamento) : null;

                ErroEmAtributos = "Tipo de Agendamento deve ser um valor inteiro entre '0' e '9'.";
                short? tipExe = short.Parse(TipoExecucao);

                return CritcaEmNomeAgendamento(agendSolic);
            }
            catch (Exception)
            {

                return false;
            }

        }        

        private bool CritcaEmNomeAgendamento(IQueryable<AgPexRelSolic> agendSolic)
        {
            if (NomeDoRelatorio.Trim() == "")
            {
                ErroEmAtributos = "O nome do Agendamento é obrigatório.";
                return false;
            }
            if (agendSolic.ToList().Count > 0)
            {
                ErroEmAtributos = "O nome do Agendamento deve ser diferente de um já existente.";
                return false;
            }

            return true;

        }

        public bool ListaEscritoriosTemItens(List<AgPexRelSolicEscrit> lista)
        { return ListaTemItens<AgPexRelSolicEscrit>(lista); }

        public bool ListaColunasTemItens(List<AgPexRelSolicColunas> lista)  
        { return ListaTemItens<AgPexRelSolicColunas>(lista); }

        public bool ListaStatusTemItens(List<AgPexRelSolicStatus> lista)
        { return ListaTemItens<AgPexRelSolicStatus>(lista); }

        public bool ListaTipoLancamentoTemItens(List<AgPexRelSolicTpLan> lista)
        { return ListaTemItens<AgPexRelSolicTpLan>(lista); }

        public bool ListaUfTemItens(List<AgPexRelSolicUf> lista)
        { return ListaTemItens<AgPexRelSolicUf>(lista); }


        //Métodos Auxiliares
        public bool ErroAoAdicionar()
        {
            if (ErroEmAtributos == "")
            {
                return false;
            }

            return true;
        }

        public string RetornaErro()
        {
                return ErroEmAtributos;
        }

        private bool ListaTemItens<T>(List<T> lista)
        {
            if (lista == null)
            {
                return false;
            }
            if (lista.Count > 0)
            {
                return true;
            }

            return false;


        }

        public static string CalculaDuracaoExecucao(DateTime? data1, DateTime? data2)
        {
            if (data1 == null || data2 == null) return "00:00";

            return data1.Value.Subtract(data2.Value).ToString();

        }

        public static string CalculaTempoDecorrido(DateTime? data)
        {
            if (data == null) return "";

            return (DateTime.Now - data.Value).RelativeTime();

        }

    }
}
