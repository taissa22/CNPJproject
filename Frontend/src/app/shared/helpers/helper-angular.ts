import Swal, { SweetAlertIcon, SweetAlertResult } from 'sweetalert2';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { stringify } from 'querystring';

@Injectable({
  providedIn: 'root'
})
export class HelperAngular {

  result = new BehaviorSubject<boolean>(false);

  MsgBox(msg: string, titulo: string) {

    Swal.fire({
      title: titulo,
      html: msg,
      icon: 'info',
      confirmButtonColor: '#6F62B2',
      confirmButtonText: 'OK',

    });
  }

  MsgBox2(msg: string, titulo: string, types?: SweetAlertIcon, confirmar?: string,
          cancelar?: string , funcao?, alinhamento?: string
  ) : Promise<SweetAlertResult> {
    return Swal.fire({
      title: titulo,
      html: msg,
      icon: types,
      confirmButtonColor: '#6F62B2',
      confirmButtonText: confirmar,
      cancelButtonText: cancelar,
      showCancelButton: cancelar ? true : false,
      showConfirmButton: confirmar ? true : false,
      customClass: {
        title: 'title-popup',
        content: alinhamento || '',
        header: 'header-class',
        icon: 'icon-class',
        actions: 'actions-class',
        confirmButton: 'confirm-button-class'
      }

    });
  }


  promptHtmlBox(html: string, title: string,
                types?: SweetAlertIcon, textoSim?: string,
                textoNao?: string) {
    return Swal.fire({
      title: title,
      html: html,
      icon: types,
      showCloseButton: true,
      showCancelButton: true,
      focusConfirm: true,
      confirmButtonText: textoSim || 'Sim',
      cancelButtonText: textoNao || 'Não',
      customClass: {
        confirmButton: 'btn btn-success btn-popup-confirm',
        cancelButton: 'btn btn-danger btn-popup-cancel',
        actions: 'footer-popup',
        content: 'content-popup',
        popup: 'popup-popup-estorno',
        title: 'title-popup-estorno',
        header: 'header-popup',
        closeButton: 'close-popup'
      },
      reverseButtons: true,
      buttonsStyling: false
    });
  }
}
