export class PautaJuizadoComposicaoCommandModel {

    public porJuizado: string;
    public codParteEmpresa: string;
    public codComarca: number;
    public codTipoVara: number;
    public codVara: number;
    public dataAudiencia: string; 
    public codGrupoJuizado: number;
    public codPreposto: number[] = [];
    public codPrepostoPrincipal: number;

    // public static retornaObj(obj : any){
    //     let agendamentoModel: PautaJuizadoComposicaoCommandModel =  new PautaJuizadoComposicaoCommandModel();
        
    //      agendamentoModel.dataAudiencia = obj.dataAudiencia;
    //      agendamentoModel.codComarca = obj.codComarca;
    //      agendamentoModel.codVara =  obj.codVara ; 
    //      agendamentoModel.codTipoVara  =  obj.codTipoVara ; 
    //      agendamentoModel.codParteEmpresa = obj.codParteEmpresa;  
         
    //     return agendamentoModel;
    // }
    
    // public static retornaObj(obj : any){
    //     let agendamentoModel: AgendamentoRelatorioSolicitacoesModel =  new AgendamentoRelatorioSolicitacoesModel();
        
    //      agendamentoModel.nomeDoRelatorio = obj.nomeDoRelatorio;
    //      agendamentoModel.descricao = obj.descricao;
    //      agendamentoModel.tipoExecucao =  obj.tipoExecucao ; 
    //      agendamentoModel.diaSemana  =  obj.diaSemana ; 
    //      agendamentoModel.diaMes = obj.diaMes;  
    //      agendamentoModel.dataIniAgendamento = obj.dataIniAgendamento;
    //      agendamentoModel.dataFimAgendamento   =obj.dataFimAgendamento;
    //      agendamentoModel.somenteEmDiasUteis =obj.somenteEmDiasUteis;
    //      agendamentoModel.dataIniSolicitacao =obj.dataIniSolicitacao;
    //      agendamentoModel.dataFimSolicitacao =obj.dataFimSolicitacao;
    //      agendamentoModel.dataIniVencimento =obj.dataInivencimento;
    //      agendamentoModel.dataFimVencimento =obj.dataFimvencimento;
    //      agendamentoModel.colunasRelatorioSelecionadas =obj.colunasRelatorioSelecionadas;
    //      agendamentoModel.escritoriosSelecionados =obj.escritoriosSelecionados;
    //      agendamentoModel.tiposDeLancamentoSelecionados= obj.tiposDeLancamentoSelecionados;
    //      agendamentoModel.ufsSelecionadas  =obj.ufsSelecionadas; 
    //      agendamentoModel.statusSolicitacoesSelecionados  =obj.statusSolicitacoesSelecionados; 
    //      agendamentoModel.tempoDecorrido = obj.tempoDecorrido;
    //      agendamentoModel.duracaoExecucao = obj.duracaoExecucao;
    //      agendamentoModel.dataProxExecucao = obj.dataProxExecucao;
    //      agendamentoModel.datUltExecucao = obj.datUltExecucao;
    //      agendamentoModel.datUltAlteracao  =  obj.datUltAlteracao;
    //      agendamentoModel.usuario  =  obj.usuario;
    //      agendamentoModel.dataCriacao = obj.datCriacao;
    //      agendamentoModel.idAgendamento = obj.idAgendamento;
    //      agendamentoModel.nomeArquivoGerado = obj.nomeArquivoGerado;
    //     return agendamentoModel;
    // }
}