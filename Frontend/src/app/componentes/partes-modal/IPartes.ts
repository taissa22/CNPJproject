
export interface ValorParteListas{
  autores : Array<IPartes>;
  reus: Array<IPartes>;
}

export interface IPartes{
  nomeParte: string;
  cpf?: string;
  cgcParte?: string;
  carteiraTrabalhoParte: string;
}
