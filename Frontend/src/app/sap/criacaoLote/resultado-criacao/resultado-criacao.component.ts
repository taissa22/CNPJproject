import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { IEmpresaCentralizadora } from '../interfaces/EmpresasCentralizadoras';
import { distinctUntilChanged } from 'rxjs/operators';
import { CriacaoService } from '../criacao.service';
import { LotesContainerService } from './lotes-container/lotes-container.service';
@Component({
  selector: 'app-resultado-criacao',
  templateUrl: './resultado-criacao.component.html',
  styleUrls: ['./resultado-criacao.component.scss']
})
export class ResultadoCriacaoComponent implements OnInit, OnDestroy {

  /** Lista das empresas centralizadoras da tela
   * @example Oi, Telemar, Oi S/A
   */
  empresasCentralizadoras: IEmpresaCentralizadora[] = [];

  nomeProcessoSelecionado: string = this.criacaoService.nomeProcesso;

  /**
   *@description Index da empresa que foi aberta
   */
  private _indexEmpresaSelecionada: number;

  private  _subscription: Subscription;

  constructor(private criacaoService: CriacaoService,
              private lotesContainerService: LotesContainerService) { }


  ngOnInit() {
    

    //#region Atualizar o quantitativo de lotes ao gerar um lote
   this._subscription = this.lotesContainerService.lotes.pipe(distinctUntilChanged())
   .subscribe(lotes =>  this.updatelotes(lotes));
    //#endregion
   this._subscription.add(this.criacaoService.listaEmpresaCentralizadoras.subscribe(
      listaEmpresa => {
        this.empresasCentralizadoras = listaEmpresa;
        if (this.empresasCentralizadoras) {
          this.empresasCentralizadoras.forEach(e => { e.isOpen = false; e.isEmpty = false; });
        }
      }
    ));

  this._subscription.add(this.lotesContainerService.isEmpty.subscribe(EmptyModel => {
                          if (this.empresasCentralizadoras) {
                            const resultadoIndex = this.empresasCentralizadoras.
                            findIndex(resultado => resultado.codigoEmpresaCentralizadora === EmptyModel.parentId);
                            if (resultadoIndex !== -1) {
                              this.empresasCentralizadoras[resultadoIndex].isEmpty = EmptyModel.isEmpty;
                            }
                          }
                        })
                       );
  }

  ngOnDestroy(): void {
    this._subscription.unsubscribe();
  }

  onOpen(indexEmpresaSelecionada) {
    //#region Toda vez que abrir uma nova fechar as antigas
    
    this.empresasCentralizadoras.map(item => item.isOpen = false );
    //#endregion

    //#region Abrir a empresa escolhida
    this.empresasCentralizadoras[indexEmpresaSelecionada].isOpen = true;
    //#endregion

    this._indexEmpresaSelecionada = indexEmpresaSelecionada;
  }

  /** Atualiza a quantidade de lotes
   * @example
   * 10 lotes na empresa centralizadora Oi, gerei 1 lote, a empresa atualiza para 9
   */
  updatelotes(lotes) {
    if (lotes.length > 0) {
      this.empresasCentralizadoras[this._indexEmpresaSelecionada].totalLote = lotes.length;
    }
  }

  manterDados() {
    this.criacaoService.manterDados = true;
    }

}
