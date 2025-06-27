import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusAtivoInativoAmbos'
})
export class StatusAtivoInativoAmbosPipe implements PipeTransform {

  transform(value?: any): any {
    return value === 1 || value === 'S'? ' Ativo' : value === 2 || value === 'N' ? ' Inativo' : ' Ativo e Inativo';
  }

}
