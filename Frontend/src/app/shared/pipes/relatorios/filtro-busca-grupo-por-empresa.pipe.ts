import { Pipe, PipeTransform } from '@angular/core';
import { GrupoEmpresaContabilSapModel } from 'src/app/relatorios/models/grupo-empresa-contabil-sap.model';

@Pipe({ name: 'appFiltroBuscaEmpresa' })
export class FiltroBuscaGrupoPorEmpresa implements PipeTransform {
  /**
   * Transform
   *
   * @param {GrupoEmpresaContabilSapModel[]} empresas
   * @param {string} nomeEmpresa
   * @returns {GrupoEmpresaContabilSapModel[]}
   */
  transform(grupos: GrupoEmpresaContabilSapModel[], nomeEmpresa: string): GrupoEmpresaContabilSapModel[] {
    if (!grupos) {
      return [];
    }
    if (!nomeEmpresa) {
      return grupos;
    }
    var gruposEncontrados =  new Array<GrupoEmpresaContabilSapModel>();
    nomeEmpresa = nomeEmpresa.toLocaleUpperCase();

    for (let i = 0; i < grupos.length; i++) {
      for (let k = 0; k < grupos[i].empresasGrupo.length; k++) {
        var empresaEncontrada = grupos[i].empresasGrupo.filter(x => x.nome.includes(nomeEmpresa))

        if (empresaEncontrada && empresaEncontrada.length > 0) {
          if (!gruposEncontrados.find(grupo => grupo.nome === grupos[i].nome))
            gruposEncontrados.push(grupos[i]);
        }
      }
    }
    return gruposEncontrados;
  }
}
