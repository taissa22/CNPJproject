import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  OnDestroy,
} from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { BehaviorSubject } from "rxjs";
import { TipoProcessoService } from "src/app/core/services/sap/tipo-processo.service";

@Component({
  selector: "app-cpn-filtro",
  templateUrl: "./filtro-relatorios.component.html",
  styleUrls: ["./filtro-relatorios.component.scss"],
})
export class FiltroRelatoriosComponent implements OnInit, OnDestroy {
  //public enabled = true;
  @Input() btnclear = false;
  @Input() enabled = true;
  @Input() listaFiltro: any[] = [];
  @Input() button: string;
  @Output() funcBtn: EventEmitter<any[]> = new EventEmitter<any[]>();
  @Output() funcbtnclear: EventEmitter<any[]> = new EventEmitter<any[]>();
  // tslint:disable-next-line: no-output-native
  @Output() listaSelecionados: EventEmitter<any[]> = new EventEmitter<any[]>();
  @Input() textNomeFiltro: string;

  public subscription;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private tipoProcessoService: TipoProcessoService
  ) {}

  decisoesDeObjeto(objeto: any) {
    this.listaFiltro.forEach((item) => {
      if (item.id === objeto.id && !item.desativado) {
        item.ativo = true;
      } else {
        item.ativo = false;
      }
      if (item.desativado) {
        item.ativo = false;
        item.selecionado = false;
      }
    });
    if (this.enabled) {
      objeto.ativo = true;
    }
    
    this.listaSelecionados.emit(this.listaFiltro);
  }

  ngOnInit() {}

  ngOnDestroy(): void {
    this.enabled = true;
  }

  enviarBtn(event) {
    if (this.enabled) {
      this.funcBtn.emit(event);
    }
  }

  limparBtn(event) {
    this.funcbtnclear.emit(event);
  }
}
