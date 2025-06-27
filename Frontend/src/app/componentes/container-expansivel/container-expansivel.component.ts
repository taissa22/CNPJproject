import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { IColoredMessage } from '@shared/interfaces/colored-message';
import { PermissoesSapService } from 'src/app/sap/permissoes-sap.service';


/** @deprecated Componente desatualizado, favor usar o Container-Expansivel-New */
@Component({
  selector: 'app-container-expansivel',
  templateUrl: './container-expansivel.component.html',
  styleUrls: ['./container-expansivel.component.scss']
})
export class ContainerExpansivelComponent implements OnInit, OnDestroy {

  // tslint:disable-next-line: no-inferrable-types
  @Input() id: number;
  @Input() isOpen = false;
  @Input() titulo: string;
  @Input() subtitulo: string;
  @Input() descricao: string;
  @Input() estado: IColoredMessage;
  @Input() estadoDetalhado: string;
  @Input() helperLabel: string;
  @Input() observacao: string;
  @Input() iconeObservacao: string;
  @Input() tituloBotaoCancelar: string;
  @Input() hasNumeroLoteBB: boolean;
  @Output() openSignal = new EventEmitter<boolean>();
  @Output() clicked = new EventEmitter<number>();
  @Output() cancelarClicked = new EventEmitter();
  @Output() regerarClicked = new EventEmitter();

  temPermissao: boolean;

  idSelecionado: any = null;

  constructor(public permissoesSapService: PermissoesSapService) { }

  ngOnInit() {

    this.idSelecionado = null;

    this.verificarPermissao();
  }

  ngOnDestroy() {
    this.idSelecionado = null;
  }

  verificarPermissao() {
    this.temPermissao = this.permissoesSapService.f_CancelarLotesCivelCons ||
      this.permissoesSapService.f_CancelarLotesCivelEstrat ||
      this.permissoesSapService.f_CancelarLotesJuizado ||
      this.permissoesSapService.f_CancelarLotesPex ||
      this.permissoesSapService.f_CancelarLotesTrabalhista;
  }

  onClickCancelar() {
    this.cancelarClicked.emit();
  }

  onClickRegerar(){
    this.regerarClicked.emit();
  }

  open(id) {

    if (this.idSelecionado === id && this.isOpen == true) {
      this.isOpen = false;

    } else {

      this.idSelecionado = id;
      this.isOpen = true;
    }
    this.openSignal.emit(this.isOpen);
    this.clicked.emit(this.id);
  }

}
