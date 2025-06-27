import { Fornecedor } from './../../../../shared/interfaces/fornecedor';
import { Injectable, OnInit } from '@angular/core';
import { ManutencaoFornecedoresService } from './manutencao-fornecedores.service';
import { FiltroFornecedoresService } from './filtro-fornecedores.service';
import { BsModalRef } from 'ngx-bootstrap';
import { FormBuilder } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalFornecedoresService {

  public tipoFornecedor: any[] = [];

  public editar = false;

  public editarFornecedor = new BehaviorSubject<boolean>(false)

  constructor(private manutencaoService: ManutencaoFornecedoresService,
    private filtroFornecedorService: FiltroFornecedoresService) { }



  tirarNumerosListaFornecedores() {
    this.tipoFornecedor = [];
    this.filtroFornecedorService.listaTipoFornecedor.map(
      item => {
        item.descricao = item.descricao.substring(3);
        this.tipoFornecedor.push(item);
      }
    );
    return this.tipoFornecedor;
  }


  cadastrarFornecedor(Fornecedor: any) {
    this.manutencaoService.cadastrarFornecedores(Fornecedor)
  }


}
