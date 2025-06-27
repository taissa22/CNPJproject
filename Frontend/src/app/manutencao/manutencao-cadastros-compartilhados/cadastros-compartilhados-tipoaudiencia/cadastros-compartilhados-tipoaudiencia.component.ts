import { Component, OnInit,OnDestroy,Input } from '@angular/core';
import { TipoAudienciaServiceService } from './services/tipoAudienciaService.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { TipoAudienciaCrudServiceService } from './services/tipoAudienciaCrudService.service';
import { ManutencaoCommonComponent } from './../../../sap/shared/components/manutencao-common-component';
import { Subscription, BehaviorSubject } from 'rxjs';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { take } from 'rxjs/operators';
import { CadastrosTipoaudienciaModalComponent }  from './cadastros-tipoaudiencia-modal/cadastros-tipoaudiencia-modal.component'


interface buscarModel {
  codTipoProcesso?: number;
  descricao:string;
  isExportMethod:boolean;
}


@Component({
  selector: 'app-cadastros-compartilhados-tipoaudiencia',
  templateUrl: './cadastros-compartilhados-tipoaudiencia.component.html',
  styleUrls: ['./cadastros-compartilhados-tipoaudiencia.component.scss']
})
export class CadastrosCompartilhadosTipoaudienciaComponent extends ManutencaoCommonComponent implements OnInit, OnDestroy {


  constructor(private modalService: BsModalService,
    public service: TipoAudienciaServiceService,
    public tipoprocessosservice: TipoProcessoService,
    private crudService: TipoAudienciaCrudServiceService
  ) {
    super(service);
  }

  filtroBuscaCad: buscarModel = {
    codTipoProcesso: null,
    descricao: '',
    isExportMethod: false
  }

  comboTipoProcesso: TipoProcesso[] = [];
  //#region Subscriptions
  comboTipoProcessoSubscription: Subscription;

  public textoPage = "Selecione o Tipo de processo para incluir e buscar os Tipos de Audiência."

  public bsModalRef: BsModalRef;
  public currentValueComboTipoProcessoSubject = new BehaviorSubject<any>(null);
  public comboboxTipoProcessoSubject = new BehaviorSubject<TipoProcesso[]>([]);

  /** Coloca a coluna padrão da tabela */
  headerSemBusca = ['Codigo', 'Sigla', 'Descrição', 'Tipo de Processo', 'Ativo'];


  subscription: Subscription;


  description: { descricao: string } = { descricao: null };


  ngOnInit() {
   
    this.comboTipoProcessoSubscription = this.getTipoProcesso()
    .subscribe(comboboxItens => { this.comboTipoProcesso = comboboxItens; });

  this.headersToRemove = ['CodTipoAudiencia'];
  this.ordemColunas = ['Sigla', 'Descricao', 'TipoProcesso', 'EstaAtivo'];
  //this.defineModel({filtro:this.filtroBuscaCad ,ordenacao: 'descricao', ascendente: true });
  this.subscription = this.crudService.valores.subscribe(data => {
    this.setData(data)
  }

  

  );

  }

  //#region API CALLERS
  getTipoProcesso() {
    this.tipoprocessosservice.getTiposProcesso('manutencaoTipoAudiencia')
      .pipe(take(1))
      .subscribe(response => this.setComboTipoProcesso(response));

    return this.comboboxTipoProcessoSubject;
  }

  //#region Setters
  setComboTipoProcesso(tipoProcessoCombo: TipoProcesso[]) {
    this.comboboxTipoProcessoSubject.next(tipoProcessoCombo);
  }

  setCurrentValueComboTipoProcesso(index) {
    this.currentValueComboTipoProcessoSubject.next(index);
  }

  onChangeComboTipoProcesso(index) {
    this.setCurrentValueComboTipoProcesso(parseInt(index));
  }

  pesquisar(description: string = null) {
    this.description = { descricao: description };
    this.refresh({ descricao: description, ordenacao: 'descricao', ascendente: true });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  adicionar(e) {
    this.crudService.adicionar();
    this.bsModalRef = this.modalService.show(CadastrosTipoaudienciaModalComponent);
  }

  editar(valor) {
    this.crudService.editar(valor);
    this.bsModalRef = this.modalService.show(CadastrosTipoaudienciaModalComponent);
  }

}
