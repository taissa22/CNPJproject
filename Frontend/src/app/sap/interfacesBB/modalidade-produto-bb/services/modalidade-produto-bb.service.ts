import { Injectable } from '@angular/core';
import { ManutencaoCommonService } from 'src/app/sap/shared/services/manutencao-common.service';
import { ModalidadeProdutoBB } from '@shared/interfaces/modalidade-produto-BB';
import { BbModalidadeProdutoService } from 'src/app/core/services/sap/bbModalidadeProduto.service';

@Injectable({
  providedIn: 'root'
})
export class ModalidadeProdutoBbService  extends ManutencaoCommonService<ModalidadeProdutoBB, number>{

  constructor(protected modalidadeService: BbModalidadeProdutoService) {
    super(modalidadeService);
 }

}
