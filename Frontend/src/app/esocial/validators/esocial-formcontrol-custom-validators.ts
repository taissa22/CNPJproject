import { Injectable } from "@angular/core";
import { AbstractControl, Validators } from "@angular/forms";

@Injectable({
  providedIn: 'root'
})

export class EsocialFormcontrolCustomValidators {
  nrInscricaoDuplicado(nrInscricaoEntrada: string) {

    return (control: AbstractControl): Validators => {
      const re = /\D/g;
      let nrInscricaoControle = control.value;
      let nrInscricaoComparacao = nrInscricaoEntrada;

      if (nrInscricaoControle === null || nrInscricaoControle === '') {
        return null;
      }

      if (nrInscricaoComparacao === null || nrInscricaoComparacao === '') {
        return null;
      }

      control.setErrors(null);

      nrInscricaoControle = nrInscricaoControle.replace(re, '');
      nrInscricaoComparacao = nrInscricaoComparacao.replace(re, '');

      if (nrInscricaoControle == nrInscricaoComparacao) {
        return {nrInscricaoDuplicado: true}
      }

      return null;
    };
  }

  preenchimentoNaturezaProibido(codigoCategoria: number) {

    return (control: AbstractControl): Validators => {
      const re = /\D/g;
      let naturezaControle = control.value;


      if (naturezaControle === null || naturezaControle === '') {
        return null;
      }

      if (codigoCategoria === null ) {
        return null;
      }

      control.setErrors(null);

      if (codigoCategoria == 721
        || codigoCategoria == 722
        || codigoCategoria == 771
        || codigoCategoria == 901
        && naturezaControle != null) {
          return {preenchimentoProibido: true}
        }

      if(codigoCategoria == 104 && naturezaControle == 2){
        return {preenchimentoProibido104: true}
      }
      
      if(codigoCategoria == 102 && naturezaControle == 1){
        return {preenchimentoProibido102: true}
      }
      
      return null;
    };
  }

  tipoContratoTempoParcial(valor : number){
    return (control: AbstractControl): Validators => {
      if ((valor != 1) && (control.value >= 0) && (control.value != null)) {
        {return { tipoInvalido: true }; }
      }
      else{
        return null;
      }

    }

  }

  tipoObrigatorio(valor : number, valorEsperado : number){
    return (control: AbstractControl): Validators => {
      if (valor != valorEsperado) {
        {return { tipoInvalido: true }; }
      }
      else{
        return null;
      }

    }

  }

  validDateTransf(dtStart:Date, dtEnd:Date)
  {
      return (control: AbstractControl): Validators => {
        if (dtEnd <= dtStart ) {
          {return { invalidDate: true }; }
        }
        else{
            return null;
          
        }  
      }
  }


  validacaoSucessaoVinculo(tipoInscricao : number, dataTransferencia :Date){

    const dataMinimaCGC : Date = new Date('1999-06-30');
    const dataMinimaCEI : Date = new Date('2011-12-31');

    return (control: AbstractControl): Validators => {
      dataMinimaCGC.setDate(dataMinimaCGC.getDate() + 1);
      if (tipoInscricao == 5 && dataTransferencia > dataMinimaCGC ) {
        {return { erroTipoCGC: true }; }
      }
      else {
        if (tipoInscricao == 6 && dataTransferencia > dataMinimaCEI) {
          {return { erroTipoCEI: true }; }
        }
        else{
          return null;
        }
      }
      return null;
    }

  }


  onlyNumber(){
    return (control: AbstractControl): Validators => {
      if (/^\d+$/.test(control.value)  ) {
        return null;
      }
      else{
        {return { onlyNumber: true }; }
      }

    }

  }


  validaocaoDataTransferencia(data :Date){
    return (control: AbstractControl): Validators => {
      if (control.value != null && control.value != '' && control.value > Date.now  ) {
        {return { dataMinimaAtual : true }; }
      }
      else{
        if (control.value != null && control.value != '' && control.value > data  ) {
          {return { dataMinimaTrabalhador: true }; }
        }
        else{

          return null;
        }
      }

    }
  }




}
