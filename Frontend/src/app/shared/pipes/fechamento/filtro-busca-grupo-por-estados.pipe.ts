import { Pipe, PipeTransform } from '@angular/core';
import { GrupoDeEstadosModel } from '../../../fechamento/models/parametrizacao-contingencia-pex-grupo-estados.model';

@Pipe({ name: 'appFiltroBusca' })
export class FiltroBuscaGrupoPorEstado implements PipeTransform {
  /**
   * Transform
   *
   * @param {GrupoDeEstadosModel[]} estados
   * @param {string} nomeEstado
   * @returns {GrupoDeEstadosModel[]}
   */
  transform(grupos: GrupoDeEstadosModel[], nomeEstado: string): GrupoDeEstadosModel[] {
    if (!grupos) {
      return [];
    }
    if (!nomeEstado) {
      return grupos;
    }
    var gruposEncontrados =  new Array<GrupoDeEstadosModel>();
    nomeEstado = nomeEstado.toLocaleUpperCase();

    for (let i = 0; i < grupos.length; i++) {
      for (let k = 0; k < grupos[i].estadosGrupo.length; k++) {
        var empresaEncontrada = grupos[i].estadosGrupo.filter(x => x.descricao.includes(nomeEstado))

        if (empresaEncontrada && empresaEncontrada.length > 0) {
          if (!gruposEncontrados.find(grupo => grupo.nome === grupos[i].nome))
            gruposEncontrados.push(grupos[i]);
        }
      }
    }
    return gruposEncontrados;
  }
}
