import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PesquisaProcesso } from '@esocial/models/pesquisa-processo';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-parte-processo-busca-modal',
  templateUrl: './parte-processo-busca-modal.component.html',
  styleUrls: ['./parte-processo-busca-modal.component.scss']
})
export class ParteProcessoBuscaModalComponent implements OnInit {

  constructor(
    private modal: NgbActiveModal,
    private router: Router,
  ) { }
  processo: PesquisaProcesso;
  dadosProcessos: Array<PesquisaProcesso>
  codigoInternoSelecionado: number;
  ngOnInit() {
  }

  static exibeModalIncluir(dadosProcesso: Array<PesquisaProcesso>): Promise<PesquisaProcesso> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ParteProcessoBuscaModalComponent,
      { windowClass: 'modal-busca-parte-processo', centered: true, size: 'lg', backdrop: 'static' }
      );
      
    // modalRef.componentInstance.titulo = 'Inclusão';
    modalRef.componentInstance.dadosProcessos = dadosProcesso;
    
    return modalRef.result;
  }

  selecionarProcesso(codigoInterno: number){
    this.codigoInternoSelecionado = codigoInterno;
  }


  close(): void {
    this.modal.close(false);
  }

  buscar(){
    if (this.codigoInternoSelecionado) {
      this.processo = this.dadosProcessos.filter(pro => pro.codProcesso == this.codigoInternoSelecionado)[0];

      this.modal.close(this.processo);
    }else{
      localStorage.removeItem('buscaProcessoParteProcesso');
      this.modal.close(false);
    }
  }

}
