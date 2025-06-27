

import { Pipe, PipeTransform } from "@angular/core";
import { DualListModel } from "@core/models/dual-list.model";

@Pipe({ name: 'filterDualList' })
export class FilterDualList implements PipeTransform {

  transform(dualListPermissoes: DualListModel[], filtroNaoSelecionados: string, filtroSelecionados: string) {
    if (filtroNaoSelecionados === '' && filtroSelecionados === '')
      return dualListPermissoes;

    filtroNaoSelecionados = filtroNaoSelecionados.trim().toLowerCase();
    filtroSelecionados = filtroSelecionados.trim().toLowerCase();

    var labels = dualListPermissoes.filter(function (dualListPermissao) {

      
      var itemAtual = dualListPermissao.label.toLowerCase() + " ";
      itemAtual += (dualListPermissao.caminho === null || dualListPermissao.caminho === undefined) ? " " : dualListPermissao.caminho.toLowerCase();
      console.log(itemAtual+"\n");
      if (itemAtual.includes(filtroNaoSelecionados) && dualListPermissao.selecionado == false || itemAtual.includes(filtroSelecionados) && dualListPermissao.selecionado == true)
        return dualListPermissao;

    })
    return labels;
  }
}



