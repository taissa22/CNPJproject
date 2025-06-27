import { TemplateDefinitionBuilder } from "@angular/compiler/src/render3/view/template";
import { EmailValidator } from "@angular/forms";
import { Sweetalert2ModuleConfig } from "@sweetalert2/ngx-sweetalert2";

export class Advogado {
  private constructor(
    id: number,
    nome: string,
    estadoOAB : string,
    numeroOAB : string,
    telefone : string,
    telefoneDDD : string,
    ehContato : boolean,
    email : string

) {
  this.id = id;
  this.nome = nome;
  this.estadoOAB = estadoOAB;
  this.numeroOAB = numeroOAB;
  this.telefone = telefone;
  this.telefoneDDD = telefoneDDD;
  this.ehContato = ehContato;
  this.email = email;

}

  readonly id: number;
  readonly nome: string;
  readonly estadoOAB : string;
  readonly numeroOAB : string;
  readonly telefone : string;
  readonly telefoneDDD : string;
  readonly ehContato : boolean;
  readonly email : string;


  static fromObj(obj: any): Advogado {
    
    return ({
      id: obj.id,
      nome: obj.nome,
      estadoOAB : obj.estadoId,
      numeroOAB : obj.numeroOAB,
      telefone :  obj.celular,
      telefoneDDD :  obj.celularDDD,
      ehContato : obj.ehContato,
      email : obj.email,

    });
  }

}
