import { Estado } from './estado.model';
export abstract class Estados {
  static readonly AC = new Estado('AC', 'ACRE');
  static readonly AL = new Estado('AL', 'ALAGOAS');
  static readonly AM = new Estado('AM', 'AMAZONAS');
  static readonly AP = new Estado('AP', 'AMAPÁ');
  static readonly BA = new Estado('BA', 'BAHIA');
  static readonly CE = new Estado('CE', 'CEARÁ');
  static readonly DF = new Estado('DF', 'DISTRITO FEDERAL');
  static readonly ES = new Estado('ES', 'ESPÍRITO SANTO');
  static readonly GO = new Estado('GO', 'GOIÁS');
  static readonly MA = new Estado('MA', 'MARANHÃO');
  static readonly MG = new Estado('MG', 'MINAS GERAIS');
  static readonly MS = new Estado('MS', 'MATO GROSSO DO SUL');
  static readonly MT = new Estado('MT', 'MATO GROSSO');
  static readonly PA = new Estado('PA', 'PARÁ');
  static readonly PB = new Estado('PB', 'PARAÍBA');
  static readonly PE = new Estado('PE', 'PERNAMBUCO');
  static readonly PI = new Estado('PI', 'PIAUÍ');
  static readonly PR = new Estado('PR', 'PARANÁ');
  static readonly RJ = new Estado('RJ', 'RIO DE JANEIRO');
  static readonly RN = new Estado('RN', 'RIO GRANDE DO NORTE');
  static readonly RO = new Estado('RO', 'RONDÔNIA');
  static readonly RR = new Estado('RR', 'RORAIMA');
  static readonly RS = new Estado('RS', 'RIO GRANDE DO SUL');
  static readonly SC = new Estado('SC', 'SANTA CATARINA');
  static readonly SE = new Estado('SE', 'SERGIPE');
  static readonly SP = new Estado('SP', 'SÃO PAULO');
  static readonly TO = new Estado('TO', 'TOCANTINS');

  static obterUfs() {
    return [
      this.AC, this.AL, this.AM,
      this.AP, this.BA, this.CE,
      this.DF, this.ES, this.GO,
      this.MA, this.MG, this.MS,
      this.MT, this.PA, this.PB,
      this.PE, this.PI, this.PR,
      this.RJ, this.RN, this.RO,
      this.RR, this.RS, this.SC,
      this.SE, this.SP, this.TO,
    ];
  }
}
