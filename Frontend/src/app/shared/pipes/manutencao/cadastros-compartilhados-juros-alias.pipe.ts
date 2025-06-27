import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'cadastrosCompartilhadosJurosAlias'
})
export class CadastrosCompartilhadosJurosAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      dataVigencia: 'Inicio de Vigêsncia',
      valorJuros: 'Valor da Taxa de Juros (%)',
      nomTipoProcesso: 'Tipo de Processo',
    }[key];
  }

}
