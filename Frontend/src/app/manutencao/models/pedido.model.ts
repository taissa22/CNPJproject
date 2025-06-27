export class Pedido {

  /*
  constructor(id: number, descricao: string, ativo: boolean,
              riscoPerda: any, provavelZero: boolean, proprioTerceiro: any) {
    this.id = id;
    this.descricao = descricao;
    this.ativo = ativo;
    this.riscoPerda = riscoPerda;
    this.provavelZero = provavelZero;
    this.proprioTerceiro = proprioTerceiro;
  }
  */

  id: number;
  idMigracao: number;
  descricao: string;
  audiencia: boolean;
  ativo: boolean;
  riscoPerda: any;
  provavelZero: boolean;
  proprioTerceiro: any;
  descricaoPara: string;
  ativoPara :boolean;

  static fromJson(p: Pedido): Pedido {
    const pedido = new Pedido();
    pedido.id = p.id;
    pedido.idMigracao = p.idMigracao;
    pedido.descricao = p.descricao;
    pedido.audiencia = p.audiencia;
    pedido.ativo = p.ativo;
    pedido.riscoPerda = p.riscoPerda;
    pedido.provavelZero = p.provavelZero;
    pedido.proprioTerceiro = p.proprioTerceiro;
    pedido.descricaoPara = p.descricaoPara ? p.descricaoPara +  (p.ativoPara ? '' : ' [INATIVO]') : '';  
    return pedido;
  }

  get riscoPerdaEmTexto(): string {
    return this.riscoPerda !== null ? this.riscoPerda.descricao : '';
  }

  get provavelZeroEmTexto(): string {
    return this.provavelZero ? 'Sim' : 'Não';
  }

  get proprioTerceiroEmTexto(): string {
    return this.proprioTerceiro !== null ? this.proprioTerceiro.valor : '';
  }
  
  get audienciaEmTexto(): string {
    return this.audiencia ? 'Sim' : 'Não';
  }

  get ativoEmTexto(): string {
    return this.ativo ? 'Sim' : 'Não';
  }

  get descricaoTratada(): string {
    if (this.descricao.length <= 30) return this.descricao;
    return this.descricao.substring(0, 30) + '...';
  }

  get dscPara(): string{
    if(this.descricaoPara !== null){
      if (this.descricaoPara.length <= 22) return this.descricaoPara;
      return this.descricaoPara.substring(0, 22) + '...';
    }
  }
}
