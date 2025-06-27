import { Injectable } from '@angular/core';
import { ModalidadeProdutoBB } from '@shared/interfaces/modalidade-produto-BB';
import { BaseService } from '../base.service';

@Injectable({
  providedIn: 'root'
})
export class BbModalidadeProdutoService extends BaseService<ModalidadeProdutoBB, number>{


  endpoint = 'BBModalidade';
}
