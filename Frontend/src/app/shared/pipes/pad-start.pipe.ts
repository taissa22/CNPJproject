import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'padStartPipe'
})
export class PadStartPipe implements PipeTransform {

  transform(value: any, ...args: any[]): any {
    let tamanho = args[0]; 
    let caracter = args[1] ;
    if(!value) return'';
    let valorStr: string =  value.toString();
    let aux : string= '';
    while((valorStr.length + aux.length)< tamanho){
      aux += caracter;
    }
    return aux+valorStr ; 
  }

}
