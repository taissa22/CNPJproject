import { ConsultaCriterioCivelConsumidorService } from 'src/app/sap/consulta/consulta-saldo-garantia/service/consulta-criterio-civel-consumidor.service';
import { SaldoGarantiaService } from './../../../core/services/sap/saldo-garantia.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CriteriosGeraisService } from '../../consultaLote/services/criterios-gerais.service';
import { FiltroModel } from 'src/app/core/models/filtro.model';
import { BehaviorSubject, Subscription } from 'rxjs';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { ConsultaSaldoGarantiaService } from './service/consulta-saldo-garantia.service';
import { ConsultaTipoProcessoService } from './service/consulta-tipo-processo.service';
import { ConsultaSaldoGarantiaBancoService } from './service/consulta-saldo-garantia-banco.service';
import { ConsultaSaldoGarantiaEstadoService } from './service/consulta-saldo-garantia-estado.service';
import { ConsultaFiltroProcessoService } from './service/consulta-filtro-processo.service';
import { ConsultaFiltroEmpresaGrupoService } from './service/consulta-filtro-empresa-grupo.service';
import { take } from 'rxjs/operators';
import { ListaFiltroSaldoGarantiaRadio } from '@shared/enums/lista-filtro-saldo-garantia-radio.enum';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { ModalAdicionarAgendamentoComponent } from './modal-adicionar-agendamento/modal-adicionar-agendamento.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../../sap.constants';

@Component({
  selector: 'app-consulta-saldo-garantia',
  templateUrl: './consulta-saldo-garantia.component.html',
  styleUrls: ['./consulta-saldo-garantia.component.scss']
})
export class ConsultaSaldoGarantiaComponent implements OnInit {

  constructor(private service: ConsultaSaldoGarantiaService,
              private tipoProcessoService: ConsultaTipoProcessoService,
              private route: ActivatedRoute,
              private router: Router,
              private criteriosGeraisService: ConsultaCriterioCivelConsumidorService,
              private processoService: ConsultaFiltroProcessoService,
              private saldoGarantiaService: SaldoGarantiaService,
              private empresaService: ConsultaFiltroEmpresaGrupoService,
              private bancoService: ConsultaSaldoGarantiaBancoService,
              private estadoService: ConsultaSaldoGarantiaEstadoService,
              private modalService: BsModalService,
              private breadcrumbsService: BreadcrumbsService
              ) { }



  // FontAwesome

  public bsModalRef: BsModalRef;


  public tipoSelecionado = new BehaviorSubject<number>(null);
  public tiposProcesso: TipoProcesso[];
  public btnResponse: string;
  public resultado = false;
  public nomeProcesso = '';
  public isDisabled = true;
  subscription: Subscription;

  // dualList




  listaFiltro: Array<FiltroModel> = [];
  tituloPagina = 'Agendamento de Consulta Saldo de Garantias';
  caminhoPaginaSap: string;
  filtroSap = 'Selecione o tipo de processo e os critérios de seleção dos lotes a serem considerados';

  ngOnInit() {
    this.tipoProcessoService.LimparTipoProcesso();
    this.isDisabled = false;

    //#region   Resolver do tipo de processo
    this.subscription = this.route.data.subscribe(info => {
      this.tiposProcesso = info.tipoProcesso;
    });
    //#endregion


    this.tipoProcessoService.tipoProcessoTracker.subscribe(processo => {
      this.tipoSelecionado.next(processo)
      this.alterarTitulos();
      this.pegarListas(processo);

      switch (processo) {
        case TipoProcessoEnum.civelEstrategico: {
          this.listaFiltro = this.service.getFiltros().filter(item => item.id != 2);
          break;
        }
        case TipoProcessoEnum.tributarioJudicial: {
          this.listaFiltro = this.service.getFiltros().filter(item => item.id != 2);
          break;
        }
        case TipoProcessoEnum.tributarioAdministrativo: {
          this.listaFiltro = this.service.getFiltros().filter(
            item => item.id != ListaFiltroSaldoGarantiaRadio.bancos
          );
          break;
          }
        default: {
          this.listaFiltro = this.service.getFiltros();
          break;
        }

      }

      if (processo) {
        this.isDisabled = true;
        this.listaFiltro.map(item => {
          item.ativo = false;
          if (item.id === 1) {
            item.ativo = true;
          }
        });
      } else {
        this.isDisabled = false;
        this.listaFiltro.map(item => {
          item.ativo = false;
        });
        this.goToInicio();
      }

    });

    if (this.tipoSelecionado.value > 0) {
      this.filtroSap = 'Selecione os critérios de seleção dos lotes a serem considerados';
    }

    this.goToInicio();

    this.service.limparConsulta.subscribe(limpar => {
      if(limpar){
        this.limpar();
        
        this.service.limparConsulta.emit(false)
      }
    })
  }

  async ngAfterViewInit(): Promise<void> {
    this.caminhoPaginaSap = await this.breadcrumbsService.nomeBreadcrumb(role.menuConsultaSaldoGarantia);
  }

  pegarListas(processo: number) {

    if (processo > 0) {
      this.saldoGarantiaService.carregarFiltros(processo).pipe(take(1)).subscribe(listas => {
        this.limpar();

        listas.data.listaBancos.map(val => {
          this.bancoService.listaBanco.push({
            id: val.id,
            label: val.descricao,
            selecionado: false,
            marcado: false,
            somenteLeitura: false,
            ativo: val.ativo
          });
        });

        listas.data.listaEmpresaDoGrupo.map(val => {
          this.empresaService.listaEmpresaGrupo.push({
            id: val.id,
            label: val.descricao,
            selecionado: false,
            marcado: false,
            somenteLeitura: false,
            ativo: val.ativo
          });
        });

        listas.data.listaEstados.map(val => {
          this.estadoService.listaEstado.push({
            id: val.id,
            label: val.descricao,
            selecionado: false,
            marcado: false,
            somenteLeitura: false,
            ativo: val.ativo
          });
        });
      });
    }

  }


  onTypeSelect(tipoProcessoId) {
    this.limpar();
    this.tipoProcessoService.tipoProcessoTracker.next(parseInt(tipoProcessoId));
    this.service.json.tipoProcesso = tipoProcessoId;
    this.filtroSap = 'Selecione os critérios de seleção dos lotes a serem considerados';
    this.listaFiltro.map(item => {
      item.ativo = false;
      if (item.id === 1) {
        item.ativo = true;
      }
    });

    this.goToInicio();
  }

  limpar() {
    this.criteriosGeraisService.limparDados();
    this.processoService.limparProcesso();
    this.empresaService.limpar();
    this.bancoService.limpar();
    this.estadoService.limpar();
  }

  goToInicio() {
    this.router.navigate(['criteriosGeraisGuia'], { relativeTo: this.route });
  }



  onClickbtn(e: string) {
    //If de gambiarra para verificar se a agencia e a conta estão com valor.
    if((this.criteriosGeraisService.agencia == "" && this.criteriosGeraisService.conta == "")
    ||( this.criteriosGeraisService.agencia != "" && this.criteriosGeraisService.conta != ""))
    {
      this.bsModalRef = this.modalService.show(ModalAdicionarAgendamentoComponent)
    }


  }

  alterarTitulos() {
    this.nomeProcesso = this.tipoProcessoService.getnomeProcesso();
    if (this.nomeProcesso) {
      this.tituloPagina = 'Agendamento de Consulta Saldo de Garantias - ' + this.nomeProcesso;
    }
  }

}
