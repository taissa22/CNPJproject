import { isNullOrUndefined } from 'util';
import { List } from 'linqts';

export const queryStringGenerator = (parametros: Array<string>, valores: Array<any>): string => {
  const listaParametros: List<string> = new List(parametros);
  const tamanho = listaParametros.Count() - 1;
  let url: string = '?';

  listaParametros.ForEach((p, i) => {
    const ehTipoObject = typeof(valores[i]) === 'object';

    if (!isNullOrUndefined(valores[i]) || valores[i] !== '') {
      if (i === tamanho)
        url += ehTipoObject ? `${p}=${JSON.stringify(valores[i])}` : `${p}=${valores[i]}`;

      if (i !== tamanho)
        url += ehTipoObject ? `${p}=${JSON.stringify(valores[i])}&` : `${p}=${valores[i]}&`;
    }
  });


  return url;
};
