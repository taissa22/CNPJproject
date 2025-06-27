export class InfoDep {
    constructor(idEsF2501Infodep : number,
                idF2500 : number,
                infodepCpfdep : string,
                infodepDtnascto : string,
                infodepNome : string,
                infodepDepirrf : string,
                infodepTpdep : string,
                infodepDescrdep : string,
                logDataOperacao: Date,
                logCodUsuario: string,
                descricaoTpDep: string){
  
      this.idEsF2501Infodep = idEsF2501Infodep,
      this.idF2500 = idF2500,
      this.infodepCpfdep = infodepCpfdep,
      this.infodepDtnascto = infodepDtnascto != null ? new Date(infodepDtnascto) : null,
      this.infodepNome = infodepNome,
      this.infodepDepirrf = infodepDepirrf,
      this.infodepTpdep= infodepTpdep,
      this.infodepDescrdep = infodepDescrdep,
      this.logDataOperacao = logDataOperacao,
      this.logCodUsuario = logCodUsuario;
      this.infodepDtnasctoTexto = infodepDtnascto != null ? infodepDtnascto.split('T')[0].split('-').reverse().join('/') : null;
      this.descricaoTpDep = descricaoTpDep
    }
  
    readonly idEsF2501Infodep : number;
    readonly idF2500 : number;
    readonly infodepCpfdep : string;
    readonly infodepDtnascto : Date;
    readonly infodepNome : string;
    readonly infodepDepirrf : string;
    readonly infodepTpdep : string;
    readonly infodepDescrdep : string
    readonly logDataOperacao : Date;
    readonly logCodUsuario: string;
    readonly infodepDtnasctoTexto : string;
    readonly descricaoTpDep : string;
  
  
    static fromObj(item: any){
      return new InfoDep(item.idEsF2501Infodep ,item.idF2500 , item.infodepCpfdep , item.infodepDtnascto ,
                            item.infodepNome ,item.infodepDepirrf , item.infodepTpdep, item.infodepDescrdep, item.logDataOperacao, item.logCodUsuario, item.descricaoTpdep  );
    }
  
  }