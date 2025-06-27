import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'orgaosBBalias'
})
export class InterfacebbOrgaosBbAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      id: 'Código',
      nome: 'Nome do Órgão BB',
      codigo: 'Órgão BB',
      nomeBBTribunal: 'Tribunal BB',
      nomeBBComarca: 'Comarca BB',
    }[key];
  }

}
