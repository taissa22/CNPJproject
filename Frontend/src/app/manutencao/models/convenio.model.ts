export class Convenio {
  estado: { id: string; nome: string };
  codigo: number;
  cnpj: string;
  mci: number;
  bancoDebito: number;
  agenciaDebito: number;
  digitoAgenciaDebito: string;
  contaDebito: string;
  agenciaDepositaria: number;
  digitoAgenciaDepositaria: string;

  get estadoId(): string {
    return this.estado ? this.estado.id : '';
  }

  get cnpjFormatado(): string {
    if (this.cnpj !== null && this.cnpj !== '') {
      const cnpj = this.cnpj.replace(/\D/g, '');
      return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, '\$1.\$2.\$3/\$4-\$5');
    }
    return '';
  }

  static fromJson(c: Convenio): Convenio {
    const convenio = new Convenio();
    convenio.estado = c.estado;
    convenio.codigo = c.codigo;
    convenio.cnpj = c.cnpj;
    convenio.mci = c.mci;
    convenio.bancoDebito = c.bancoDebito;
    convenio.agenciaDebito = c.agenciaDebito;
    convenio.digitoAgenciaDebito = c.digitoAgenciaDebito;
    convenio.contaDebito = c.contaDebito;
    convenio.agenciaDepositaria = c.agenciaDepositaria;
    convenio.digitoAgenciaDepositaria = c.digitoAgenciaDepositaria;
    return convenio;
  }
}
