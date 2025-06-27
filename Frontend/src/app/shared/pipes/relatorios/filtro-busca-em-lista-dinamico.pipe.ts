import { EmpresaModel } from 'src/app/relatorios/models/empresa.model';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'appFiltroBusca' })
export class FiltroBuscaEmListaDinamico implements PipeTransform {
  /**
   * Transform
   *
   * @param {EmpresaModel[]} empresas
   * @param {string} nomeEmpresa
   * @returns {EmpresaModel[]}
   */
  transform(empresas: EmpresaModel[], nomeEmpresa: string): EmpresaModel[] {
    if (!empresas) {
      return [];
    }
    if (!nomeEmpresa) {
      return empresas;
    }
    nomeEmpresa = nomeEmpresa.toLocaleLowerCase();

    return empresas.filter(it => {
      return it.nome.toLocaleLowerCase().includes(nomeEmpresa);
    });
  }
}
