import { DialogService } from '@shared/services/dialog.service';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Documento } from './../models/documento.model';
import { CargaDeDocumentosService } from './../services/carga-de-documentos.service';
import { Component, OnInit } from '@angular/core';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  templateUrl: './carga-de-documentos.component.html',
  styleUrls: ['./carga-de-documentos.component.scss']
})
export class CargaDeDocumentosComponent implements OnInit {

  titulo: string = 'Carga de Documentos';
  breadcrumb: string = '';

  documentos: Array<Documento> = [];
  totalDocumentos: number = 0;
  pagina: number = 1;

  constructor(
    private service: CargaDeDocumentosService, 
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService) { }

  async ngOnInit(): Promise<void> {
    await this.obterPaginado();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_CARGA_DOCUMENTOS_PAGAMENTO_RJ);
  }

  async obterPaginado(): Promise<void> {
    try {
      const listaDocumentos = await this.service.obterPaginado(this.pagina);
      listaDocumentos.data.forEach((d: Documento) => this.documentos.push(Documento.fromJson(d)));
      this.totalDocumentos = listaDocumentos.total;
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async obterMaisComprovantes(): Promise<void> {
    this.pagina += 1;
    await this.obterPaginado();
  }

  async redefinirListagem(): Promise<void> {
    this.documentos = [];
    this.pagina = 1;

    await this.obterPaginado();
  }

  async excluir(id: number | string): Promise<void> {
    try {
      await this.service.excluir(id);
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      return;
    }
    await this.redefinirListagem();
  }

}
