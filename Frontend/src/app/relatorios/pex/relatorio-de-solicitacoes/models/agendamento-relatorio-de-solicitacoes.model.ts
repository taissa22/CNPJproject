import { DiasSemanaEnum } from "../enuns/DiasSemanaRelatorio.enum";
import { TipoExecucaoRelatorioEnum } from "../enuns/TipoExecucaoRelatorio.enum";

export class AgendamentoRelatorioSolicitacoesModel{
    public idAgendamento: string = ""; 
    public nomeDoRelatorio:string = "";
    public descricao:string = "";
    public tipoExecucao: number = 1;
    public diaSemana: number = 1;
    public dataCriacao : string;
    public diaMes: number  =  0;
    public dataProxExecucao : string = "";
    public datUltExecucao : string = "";
    public dataIniAgendamento:string = "";
    public dataFimAgendamento : string = "";
    public somenteEmDiasUteis:boolean = false;
    public dataIniSolicitacao:string = "";
    public dataFimSolicitacao:string = "";
    public dataIniVencimento:string = "";
    public dataFimVencimento:string = "";
    public usuario : string = ""; 
    public datUltAlteracao : string = ""; 
    public tempoDecorrido : string = "";
    public duracaoExecucao : string = "";
    public nomeArquivoGerado : string = "";
    public colunasRelatorioSelecionadas: number[] = []; 
    public escritoriosSelecionados: number[] = []; 
    public tiposDeLancamentoSelecionados: number[] = []; 
    public ufsSelecionadas: string[] = []; 
    public statusSolicitacoesSelecionados: number[] = [];
    
    public static retornaObj(obj : any){
        let agendamentoModel: AgendamentoRelatorioSolicitacoesModel =  new AgendamentoRelatorioSolicitacoesModel();
        
         agendamentoModel.nomeDoRelatorio = obj.nomeDoRelatorio;
         agendamentoModel.descricao = obj.descricao;
         agendamentoModel.tipoExecucao =  obj.tipoExecucao ; 
         agendamentoModel.diaSemana  =  obj.diaSemana ; 
         agendamentoModel.diaMes = obj.diaMes;  
         agendamentoModel.dataIniAgendamento = obj.dataIniAgendamento;
         agendamentoModel.dataFimAgendamento   =obj.dataFimAgendamento;
         agendamentoModel.somenteEmDiasUteis =obj.somenteEmDiasUteis;
         agendamentoModel.dataIniSolicitacao =obj.dataIniSolicitacao;
         agendamentoModel.dataFimSolicitacao =obj.dataFimSolicitacao;
         agendamentoModel.dataIniVencimento =obj.dataInivencimento;
         agendamentoModel.dataFimVencimento =obj.dataFimvencimento;
         agendamentoModel.colunasRelatorioSelecionadas =obj.colunasRelatorioSelecionadas;
         agendamentoModel.escritoriosSelecionados =obj.escritoriosSelecionados;
         agendamentoModel.tiposDeLancamentoSelecionados= obj.tiposDeLancamentoSelecionados;
         agendamentoModel.ufsSelecionadas  =obj.ufsSelecionadas; 
         agendamentoModel.statusSolicitacoesSelecionados  =obj.statusSolicitacoesSelecionados; 
         agendamentoModel.tempoDecorrido = obj.tempoDecorrido;
         agendamentoModel.duracaoExecucao = obj.duracaoExecucao;
         agendamentoModel.dataProxExecucao = obj.dataProxExecucao;
         agendamentoModel.datUltExecucao = obj.datUltExecucao;
         agendamentoModel.datUltAlteracao  =  obj.datUltAlteracao;
         agendamentoModel.usuario  =  obj.usuario;
         agendamentoModel.dataCriacao = obj.datCriacao;
         agendamentoModel.idAgendamento = obj.idAgendamento;
         agendamentoModel.nomeArquivoGerado = obj.nomeArquivoGerado;
        return agendamentoModel;
    }
}