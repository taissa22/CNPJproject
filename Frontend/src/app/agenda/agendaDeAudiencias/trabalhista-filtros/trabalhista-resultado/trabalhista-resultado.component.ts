import { Component, OnInit } from "@angular/core";
import { BsModalService, BsModalRef } from "ngx-bootstrap";
import { ActivatedRoute, Router } from "@angular/router";
import { TrabalhistaResultadoService } from "./services/trabalhistaResultado.service";
import { TrabalhistaFiltrosService } from "../services/trabalhista-filtros.service";
import { HelperAngular } from "@shared/helpers/helper-angular";
import { Subscription } from "rxjs";
import { IAgendaProcessoResultadoPesquisar } from "@shared/interfaces/IAgendaProcessoPesquisar";
import { paginaInicial, quantidadePaginainicial } from "src/app/app.constants";
import { NgbTooltip } from "@ng-bootstrap/ng-bootstrap";
import { ModalAlteracaoAudienciaComponent } from "./modal/modalAlteracaoAudiencia/modalAlteracaoAudiencia.component";
import {
  IPedidos,
  IPedidosResultado,
} from "@core/services/processos/AgendaAudiencia/IPedidos";
import { pluck } from "rxjs/operators";
import { PedidosModalComponent } from "src/app/componentes/pedidos-modal/pedidos-modal.component";
import { ValorParteListas } from "src/app/componentes/partes-modal/IPartes";
import { PartesModalComponent } from "src/app/componentes/partes-modal/partes-modal.component";
import { OrdenacaoDragDropComponent } from "src/app/componentes/ordenacao-drag-drop/ordenacao-drag-drop.component";
import { SortOrderView } from "@shared/interfaces/Sort/SortOrderView";
import { BreadcrumbsService } from "@shared/services/breadcrumbs.service";
import { Permissoes } from "@permissoes";

@Component({
  selector: "app-trabalhista-resultado",
  templateUrl: "./trabalhista-resultado.component.html",
  styleUrls: ["./trabalhista-resultado.component.css"],
})
export class TrabalhistaResultadoComponent implements OnInit {
  breadcrumb: string;
  constructor(
    private modalService: BsModalService,
    private route: ActivatedRoute,
    public service: TrabalhistaResultadoService,
    private filterService: TrabalhistaFiltrosService,
    private router: Router,
    private messageService: HelperAngular,
    private trabalhistaService: TrabalhistaFiltrosService,
    private breadcrumbsService: BreadcrumbsService
  ) {}

  public bsModalRef: BsModalRef;
  subscription: Subscription;
  dadosTabela: IAgendaProcessoResultadoPesquisar;
  headerTabela: string[] = [];
  notFound = false;
  pagina = paginaInicial;
  quantidadePagina = quantidadePaginainicial;
  valoresCriterio: any;

  ordenacao: String[];

  config = {
    backdrop: false,
    ignoreBackdropClick: true,
    class: "modal-class",
  };

  ngOnInit() {
    //#region Resolver dos filtros
    this.subscription = this.service.trabalhistaFiltroSevice.valoresTabela$.subscribe(
      (pesquisa: IAgendaProcessoResultadoPesquisar) => {
        if (pesquisa.dado.length > 0) this.definirDados(pesquisa);
        else {
          this.router.navigate([
            "agenda/agendaAudienncia/trabalhista/criteriosGuia",
          ]);
          this.messageService.MsgBox2(
            "Nenhum resultado encontrado.",
            "Atenção!",
            "info",
            "Ok"
          );
        }
      }
    );
    //#endregion

    this.subscription.add(
      this.service.paginatorService.pageSubject.subscribe((value) => {
        this.pagina = value;
      })
    );

    this.subscription.add(
      this.service.quntityService.quantidadeSubject.subscribe((valor) => {
        this.quantidadePagina = valor;
      })
    );

    this.service.atualizardadosTabela.subscribe((valor) => {
      valor && this.refreshTabela();
    });

    this.valoresCriterio = this.trabalhistaService.getCriterios();

    this.ordenacao = this.trabalhistaService.ordemCriterios;
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_AGENDA_AUDIENCIA_TRABALHISTA);
  }

  abrirCriterio(tooltip: NgbTooltip) {
    tooltip.open();
  }
  fecharCriterio(tooltip: NgbTooltip) {
    tooltip.close();
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
    this.service.audienciaSelecionada = null;
    this.service.quntityService.quantidadeSubject.next(quantidadePaginainicial);
    this.service.paginatorService.pageSubject.next(paginaInicial);
  }

  exportar() {
    this.service.exportar();
  }
  
  exportarCompleto() {
    this.service.exportarCompleto();
  }

  download() {
    this.service.download();
  }

  modalEditar() {
    this.bsModalRef = this.modalService.show(ModalAlteracaoAudienciaComponent);
  }

  private pedidos: Array<IPedidos> = [];

  modalPedidos() {
    this.service
      .recuperarPedidos()
      .pipe(pluck("data"))
      .subscribe(
        (item: IPedidosResultado) => {
          this.pedidos = item.pedidos;
        },
        () => {},
        () => {
          this.bsModalRef = this.modalService.show(
            PedidosModalComponent,
            this.config
          );
          this.bsModalRef.content.pedidos = this.pedidos.map((item) => {
            return {
              descricao: item.descricaoPedido,
            };
          });
        }
      );
  }

  partes: ValorParteListas = { autores: [], reus: [] };

  modalPartes() {
    this.service
      .RecuperarPartes()
      .pipe(pluck("data"))
      .subscribe(
        (item: ValorParteListas) => {
          this.partes = item;
        },
        () => {},
        () => {
          this.bsModalRef = this.modalService.show(
            PartesModalComponent,
            this.config
          );
          this.bsModalRef.content.partes = this.partes;
          this.bsModalRef.content.tituloAutor = "Reclamantes";
          this.bsModalRef.content.tituloReu = "Reclamadas";
        }
      );
  }

  modalOrdenar() {
    this.bsModalRef = this.modalService.show(OrdenacaoDragDropComponent);
    this.bsModalRef.content.title = "Agenda de Audiências";
    this.bsModalRef.content.values = this.service.order;

    this.subscription.add(
      this.bsModalRef.content.orderedValues.subscribe(
        (order: SortOrderView[]) => {
          this.service.order = order;
          this.refreshTabela();
        }
      )
    );
  }

  voltar(e) {
    
    this.filterService.manter = e;
  }

  atualizarQuantidadePagina(quantidade) {
    this.service.quntityService.setQuantidade(quantidade);
    this.refreshTabela();
  }

  private definirDados(pesquisa: IAgendaProcessoResultadoPesquisar) {
    if (pesquisa.dado.length > 0) {
      this.dadosTabela = pesquisa;
      this.headerTabela = Object.keys(pesquisa.dado[0]).filter(
        (item) => item != "codProcesso" && item != "seqAudiencia"
      );
      this.notFound = false;
    } else this.notFound = true;
  }

  private refreshTabela() {
    this.service.atualizarTabela().subscribe((itens) => {
      var pesquisa = itens;
      this.definirDados(pesquisa);
      this.service.audienciaSelecionada = null;
    });
  }

  atualizarPagina(event) {
    this.service.paginatorService.setPage(event);
    this.refreshTabela();
  }

  onRowClick(dado) {
    if (this.service.audienciaSelecionada == dado)
      this.service.audienciaSelecionada = null;
    else this.service.audienciaSelecionada = dado;
  }
}
