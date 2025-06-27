import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ImagemGuiaService {
  constructor(private router: Router) {}

  inicializar(guia) {
    const newRelativeUrl = this.router.createUrlTree([
      'sap/interfaceBB/importacao/resultado/detalhe/imagemGuia/',
      guia.codigoProcesso,
      guia.codigoLancamento
    ]);
    const baseUrl = window.location.href.replace(this.router.url, '');

    window.open(
      baseUrl + newRelativeUrl,
      '_blank',
      'min-height=500,width=728, scrollbars=1'
    );
  }
}
