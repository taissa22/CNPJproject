export class itemSelecaoMultiSelectList{
   id: string;
   descricao: string; 
   marcado : boolean;
   hide: boolean;
   constructor(id:string,descricao:string){
    this.id = id.toString();
    this.descricao = descricao;
   }
   
}