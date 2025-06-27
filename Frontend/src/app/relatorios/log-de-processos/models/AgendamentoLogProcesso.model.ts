import { OperacoesEnum } from "../enuns/operacoes.enum";
import { StatusEnum } from "../enuns/status.enum";
import { TipoLogEnum } from "../enuns/tipoLog.enum";

export class AgendamentoLogProcessoModel {

    public id   : number = 0;
    public dataIni : string = "";
    public dataFim : string = "";
    public dataDeAgendamento : string = "";
    public dataDeCadastro : string = "";
    public usuarioId : string = "";
    public operacao : string =  OperacoesEnum["T"];
    public tipoLog : string = TipoLogEnum["0"];
    public status :string = "";
    public nomeDoArquivo : string = "";
    public mensagemErro : string = ""
    public dataIniExecucao : string = "";
    public dataFimExecucao :string = "";


    public static fromObj(obj:any){
        let agendamento = new AgendamentoLogProcessoModel();
        agendamento.id   = obj.id ;
        agendamento.dataIni =  obj.dataIni ,
        agendamento.dataFim =  obj.dataFim ;
        agendamento.dataDeAgendamento =  obj.dataDeAgendamento ;
        agendamento.dataDeCadastro =  obj.dataDeCadastro ;
        agendamento.usuarioId =  obj.usuarioId ;
        agendamento.operacao = Object.keys(OperacoesEnum)[Object.values(OperacoesEnum).indexOf(obj.operacao as unknown as OperacoesEnum)];
        agendamento.tipoLog = Object.keys(TipoLogEnum)[Object.values(TipoLogEnum).indexOf(obj.tipoLog.toString() as unknown as TipoLogEnum)];
        agendamento.status = Object.keys(StatusEnum)[Object.values(StatusEnum).indexOf(obj.status as unknown as StatusEnum)] ;
        agendamento.nomeDoArquivo =  obj.nomeDoArquivo ;
        agendamento.mensagemErro = obj.mensagemErro ;
        agendamento.dataIniExecucao =  obj.dataIniExecucao ;
        agendamento.dataFimExecucao=  obj.dataFimExecucao ;
        return agendamento;
    }

}
