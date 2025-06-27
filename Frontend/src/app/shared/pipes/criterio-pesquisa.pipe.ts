import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'criterioPesquisa'
})
export class CriterioPesquisaPipe implements PipeTransform {

  transform(listaService, listaSelecionados: any[], hastitulo: boolean): string[] {
      let teste = [];
      if (!hastitulo) {
        teste = listaService.filter(item => listaSelecionados.includes(item.id))
          .map(nome => nome.label);
      }
      if (hastitulo) {
        listaService.forEach(item =>
          item.dados.forEach(dados => teste.push(dados))
        );

        teste = teste.filter(item => listaSelecionados.includes(item.id)).map(nome => nome.descricao)
      }

      return teste;
    }

  }

