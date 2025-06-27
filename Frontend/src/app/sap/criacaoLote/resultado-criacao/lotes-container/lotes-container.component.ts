import { Component, OnInit, Input } from '@angular/core';
import { CriacaoService } from '../../criacao.service';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { AbasContentService } from './detalhamento-lancamento/abasContent/abas-content.service';
import { LotesContainerService } from './lotes-container.service';
import { TextareaLimitadoService } from '../../services/textarea-limitado.service';
import { take, distinctUntilChanged } from 'rxjs/operators';
import { IFiltroCriacaoLote } from '../../interfaces/IFiltroCriacaoLote';
import { Subscription } from 'rxjs';
import { IEmpresaGrupo } from '../../interfaces/IEmpresaGrupo';

interface InfoEmpresaCentralizadora{
  codigo: number;
  totalLotes: number;
}

@Component({
  selector: 'app-lotes-container',
  templateUrl: './lotes-container.component.html',
  styleUrls: ['./lotes-container.component.scss']
})
export class LotesContainerComponent implements OnInit {

  /** Verifica qual a empresa centralizadora foi aberta */
  empresaCentralizadoraAnterior = null;

  /** Valor dos lotes trago pela API */
  lotes: Array<IEmpresaGrupo> = [];

  /** Valores do filtro de criação */
  filtro: IFiltroCriacaoLote = this.criacaoService.filtro;

  /** Index do lote aberto pelo usuario */
  private _indexLoteEscolhido: number = null;

  /** Informações da empresa centralizadora que é o componente pai da empresa do grupo */
  @Input() infoEmpresaCentralizadora: InfoEmpresaCentralizadora;

  subscription: Subscription;

  constructor(private criacaoService: CriacaoService,
              private abasContentService: AbasContentService,
              private service: LotesContainerService,
              private textAreaLimitadoService: TextareaLimitadoService,
              private lotesService: LoteService) { }

  ngOnInit() {

    this.carregarLancamentos();

    this.subscription = this.abasContentService.lancamentos.pipe(distinctUntilChanged()).subscribe(lancamentos => {
      if (lancamentos) {
        this.updateLancamentos(lancamentos);
        this.updateEmptyTotal();
      }
    });
  }

  ngOnDestroy(): void {
    if (this.lotes){
      this.lotes.map(item => item.isOpen = false);
    }

    this.subscription.unsubscribe();
  }

  /**  Filtra somente os lotes que possuem lançamentos mais que 0 */
  updateEmptyTotal() {
  this.lotesService.sucessoCriarLote.pipe(take(1)).subscribe(sucesso => {
      if (sucesso === true) {
        let dados = this.lotes.filter(item => item.totalLancamneto != 0);
        this.service.lotes.next(dados);
      }
    });
  }

  /** Atualiza os lançamentos ao gerar um lote e possuir mais de um lançamento  */
  private updateLancamentos(lancamentos: any[]) {
    if (this.lotes[this._indexLoteEscolhido]) {
      this.lotes[this._indexLoteEscolhido].totalLancamneto = lancamentos.length;
      this.service.isEmpty.next({
        parentId: this.infoEmpresaCentralizadora.codigo,
        isEmpty: this.lotes.every(e => e.totalLancamneto <= 0)
      });
    }
  }



  onLoteOpen(index) {
    this._indexLoteEscolhido = index;
    //Fechar todos os lotes ao abrir um.
    this.lotes.map(lote => lote.isOpen = false);
    //Abrir o lote escolhido.
    this.lotes[index].isOpen = true;
    var filtro = {...this.lotes[index], ...this.filtro};

    this.criacaoService.filtroLoteEscolhido.next(filtro);
  }

  carregarLancamentos() {

    this.criacaoService.obterLotesEmpresaGrupo({
      ...this.filtro,
      codigoEmpresaCentralizadora: this.infoEmpresaCentralizadora.codigo
    }).pipe(distinctUntilChanged()).subscribe(response => this.lotes = response.data);

      // verifica se a empresa possui algum lote antes de chamar o endpoint
      // if (this.infoEmpresaCentralizadora.codigo)      {      }
    }


}

