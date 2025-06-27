export class FielDepositario {

  // constructor(id: number, cpf: string, nome: string) {
  //   this.id = id;
  //   this.cpf = cpf;
  //   this.nome = nome;
  // }

  id: number;
  nome: string;
  cpf: string;

  static fromJson(f: FielDepositario): FielDepositario {
    const fielDepositario = new FielDepositario();
    fielDepositario.id = f.id;
    fielDepositario.nome = f.nome;
    fielDepositario.cpf = f.cpf;

    return fielDepositario;
  }

  get nomeTratado(): string {
    if (this.nome.length <= 30) return this.nome;
    return this.nome.substring(0, 30) + '...';
  }

  get CPF(): string {
    if (this.cpf !== null && this.cpf !== '') {
        const cpf = this.cpf.replace(/\D/g, '');
        return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, '\$1.\$2.\$3-\$4');
    }
    return '';
}
}

