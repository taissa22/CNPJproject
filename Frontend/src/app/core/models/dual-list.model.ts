export interface DualListModel {
  id: any;
  idCount?:any;
  label: string;
  selecionado?: boolean;
  marcado?: boolean;
  somenteLeitura?: boolean;
  ativo?:boolean;
  codigoChave?:number;
  clone?:boolean;
  dados?:any[];
  caminho?:string;
}
