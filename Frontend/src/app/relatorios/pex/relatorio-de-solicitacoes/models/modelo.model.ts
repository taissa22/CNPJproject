export class ModeloModel {
    public id : number = 0;
    public nome: string = "";
    public descricao: string = "";
    public dataIniSolicitacao: string = "";
    public dataFimSolicitacao: string = "";
    public dataIniVencimento: string = "";
    public dataFimVencimento: string = "";
    public colunasRelatorioSelecionadas: number[] = [];
    public escritoriosSelecionados: number[] = [];
    public tiposDeLancamentoSelecionados: number[] = [];
    public ufsSelecionadas: string[] = [];
    public statusSolicitacoesSelecionados: number[] = [];
    public dataDeCadastro: string = "";
    public dataDeAlteracao: string = "";

    public static fromObj(obj: any):ModeloModel {
        var modelo = new ModeloModel();
        modelo.id = obj.id;
        modelo.nome = obj.nome;
        modelo.descricao = obj.descricao;
        modelo.dataIniSolicitacao = obj.dataIniSolicitacao;
        modelo.dataFimSolicitacao = obj.dataFimSolicitacao;
        modelo.dataIniVencimento = obj.dataIniVencimento;
        modelo.dataFimVencimento = obj.dataFimVencimento;
        modelo.colunasRelatorioSelecionadas = obj.colunasRelatorioSelecionadas;
        modelo.escritoriosSelecionados = obj.escritoriosSelecionados;
        modelo.tiposDeLancamentoSelecionados = obj.tiposDeLancamentoSelecionados;
        modelo.ufsSelecionadas = obj.ufsSelecionadas;
        modelo.statusSolicitacoesSelecionados = obj.statusSolicitacoesSelecionados;
        modelo.dataDeAlteracao = obj.dataDeAlteracao;
        modelo.dataDeCadastro = obj.dataDeCadastro;

        return modelo;
    }
}