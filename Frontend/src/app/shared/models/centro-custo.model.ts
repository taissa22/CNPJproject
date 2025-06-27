export class CentroCusto {
  id: number;
  nome: string;
  ativo: boolean;
  centroCustoSapId: string;

  constructor(id: number, nome: string, centroCustoSapId: string, ativo: boolean) {
    this.id = id;
    this.nome = nome;
    this.centroCustoSapId = centroCustoSapId;
    this.ativo = ativo;
  }

  get descricao() {
    const sufixoAtivo = this.ativo ? '' : ' [INATIVO]';
    const descricao = `${this.nome}${sufixoAtivo}`;
    return descricao;
  }
}
