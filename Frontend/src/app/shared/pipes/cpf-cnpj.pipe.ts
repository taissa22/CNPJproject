import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'cpfCnpj'
})
export class CpfCnpjPipe implements PipeTransform {


  transform(value: string, ...args: any[]): any {
    if(value){
      if (value.length === 11) {
        return value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, '\$1.\$2.\$3\-\$4');
      } else if (value.length === 14){
        return value.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d+)/, " $1.$2.$3/$4-$5");
      }}
      return value;
    }


}
