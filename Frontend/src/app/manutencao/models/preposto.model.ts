//import { EstadoEnum } from ".";

export class Preposto {
  private constructor(
    id: number,
    nome: string,
    //estadoId: string,
    //estado: EstadoEnum,
    ativo: boolean,
    ehCivelEstrategico: boolean,
    ehCivel: boolean,    
    ehTrabalhista : boolean,
    ehJuizado : boolean,
    usuarioId : string,
    nomeUsuario : string,
    usuarioAtivo : boolean,
    ehProcon : boolean,
    ehPex : boolean,
    ehEscritorio : boolean,
    matricula: string

) {
  this.id = id;  
  this.nome = nome;
  //this.estadoId = estadoId;
  //this.estado  =  estado;
  this.ativo  = ativo;
  this.ehCivelEstrategico  =  ehCivelEstrategico;
  this.ehCivel  = ehCivel;
  this.ehTrabalhista = ehTrabalhista;
  this.ehJuizado = ehJuizado;
  this.usuarioId = usuarioId;
  this.nomeUsuario = nomeUsuario;  
  this.usuarioAtivo = usuarioAtivo;
  this.ehProcon = ehProcon;
  this.ehPex = ehPex;
  this.ehEscritorio = ehEscritorio;  
  this.matricula = matricula;
}

  readonly id: number;
  readonly nome: string;
 // readonly estadoId: string;
 // readonly estado: EstadoEnum;
  readonly ativo: boolean;
  readonly ehCivelEstrategico: boolean;
  readonly ehCivel: boolean;    
  readonly ehTrabalhista : boolean;
  readonly ehJuizado : boolean;
  readonly usuarioId : string;
  readonly nomeUsuario : string;
  readonly usuarioAtivo : boolean;
  readonly ehProcon : boolean;
  readonly ehPex : boolean;
  readonly ehEscritorio : boolean;
  readonly matricula: string;

  static fromObj(obj: any): Preposto {
    return ({
      id: obj.id,
      nome: obj.nome,
//      estadoId: obj.estadoId,
//      estado: obj.estado,
      ativo: obj.ativo,
      ehCivelEstrategico: obj.ehCivelEstrategico,
      ehCivel: obj.ehCivel,
      ehTrabalhista : obj.ehTrabalhista,
      ehJuizado : obj.ehJuizado,
      usuarioId : obj.usuarioId,
      nomeUsuario : obj.nomeUsuario,
      usuarioAtivo : obj.usuarioAtivo,
      ehProcon : obj.ehProcon,
      ehPex : obj.ehPex,
      ehEscritorio : obj.ehEscritorio,
      matricula: obj.matricula     
    });
  }



}
