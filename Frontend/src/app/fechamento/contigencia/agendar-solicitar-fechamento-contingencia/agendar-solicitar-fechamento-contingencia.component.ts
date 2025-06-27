import { AfterViewInit, Component, EventEmitter, OnInit, Output, ChangeDetectorRef } from '@angular/core';
import { Permissoes } from '@permissoes';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BotaoGridState } from '@shared/interfaces/botao-grid-state';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { combineLatest, Subscription } from 'rxjs';
import { TrabalhistaResultadoService } from 'src/app/agenda/agendaDeAudiencias/trabalhista-filtros/trabalhista-resultado/services/trabalhistaResultado.service';
import { PermissoesService } from 'src/app/permissoes/permissoes.service';
import { AgendarSolicitarFechamentoContingenciaService } from '../../services/agendar-solicitar-fechamento-contingencia.service';
import { AgendarSolicitarFechamentoContingenciaModalComponent } from '../modals/agendar-solicitar-fechamento-contingencia-modal/agendar-solicitar-fechamento-contingencia-modal.component';
import { FechamentoCcMediaComponent } from '../modals/fechamento-cc-media/fechamento-cc-media.component';
import { FechamentoCcComponent } from '../modals/fechamento-cc/fechamento-cc.component';
import { FechamentoCivelEstrategicoComponent } from '../modals/fechamento-civel-estrategico/fechamento-civel-estrategico.component';
import { FechamentoJecComponent } from '../modals/fechamento-jec/fechamento-jec.component';
import { FechamentoPexMediaComponent } from '../modals/fechamento-pex-media/fechamento-pex-media.component';
import { FechamentoTrabMediaComponent } from '../modals/fechamento-trab-media/fechamento-trab-media.component';

@Component({
  selector: 'app-agendar-solicitar-fechamento-contingencia',
  templateUrl: './agendar-solicitar-fechamento-contingencia.component.html',
  styleUrls: ['./agendar-solicitar-fechamento-contingencia.component.scss']
})
export class AgendarSolicitarFechamentoContingenciaComponent implements OnInit, AfterViewInit {

  constructor(
    private modalService: BsModalService,
    public servicePaginacao: TrabalhistaResultadoService,
    private messageService: HelperAngular,
    private service: AgendarSolicitarFechamentoContingenciaService,
    private permissaoService: PermissoesService,
    private breadcrumbsService: BreadcrumbsService,
    private changeDetection: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.optCC = true;
    this.optEstrategico = true;
    this.optJEC = true;
    this.optPEX = true;
    this.optTrab = true;
    this.pagina = 1;
    this.verificarPermissao();
    this.multiSelect();
    this.novaBusca();
  }
  
  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_AGENDAR_SOLICITAR_FECHAMENTO_CONTINGENCIA);
  }

  temPermissaoCivelConsumidor = false;
  temPermissaoCivelEstrategica = false;
  temPermissaoJEC = false;
  temPermissaoTRAB = false;
  temPermissaoPex = false;

  @Output() stateChange = new EventEmitter<BotaoGridState>();
  isActive: boolean = false;
  fechamento: boolean = false;
  descricao: boolean = false;
  proxExecucao: boolean = false;
  usuario: boolean = false;
  solicitacao: boolean = false;
  empresa: boolean = false
  ordemCrescente: boolean = true;

  subscriptions = [];

  breadcrumb: any;

  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};
  moduloSalvo: number = null;

  fechamentos: any[] = [];

  optCC: boolean;
  optEstrategico: boolean;
  optJEC: boolean;
  optPEX: boolean;
  optTrab: boolean;

  //#region PAGINAÇÃO
  pagina: number;
  quantidadePagina = 10;
  totalFechamentos: number = this.fechamentos.length;
  //#endregion

  public bsModalRef: BsModalRef;

  modal(editar?, fechamento?) {
    let civelConsumidor = FechamentoCcComponent;
    let civelConsumidorMedia = FechamentoCcMediaComponent;
    let civelEstrategico = FechamentoCivelEstrategicoComponent;
    let juizadoEspecial = FechamentoJecComponent;
    let trabalhistaMedia = FechamentoTrabMediaComponent;
    let pexMedia = FechamentoPexMediaComponent;

    let _combine;

    _combine = combineLatest(
      this.modalService.onHide,
      this.modalService.onHidden
    ).subscribe(() => this.changeDetection.markForCheck());

    this.subscriptions.push(
      this.modalService.onHide.subscribe((reason: string | any) => {
        if (typeof reason !== 'string') {
          if (this.bsModalRef.content.resultado == 1)
          this.moduloSalvo = this.bsModalRef.content.novoModulo;
          this.novaBusca();
          this.fazerBusca();
        }
        else {
          console.log(reason)
        }
      })
    );

    this.subscriptions.push(
      this.modalService.onHidden.subscribe((reason: string | any) => {
        this.unsubscribe();
      })
    );

    if (_combine) {
      this.subscriptions.push(_combine);
    }

    this.bsModalRef = this.modalService.show(AgendarSolicitarFechamentoContingenciaModalComponent);
    this.bsModalRef.setClass('modal-agend-fechamento-contingencia');
    this.bsModalRef.content.modulos = this.selectedItems;

    editar ? this.bsModalRef.content.editar(fechamento) : [
      civelConsumidor.prototype.editou = false,
      civelConsumidorMedia.prototype.editou = false,
      civelEstrategico.prototype.editou = false,
      juizadoEspecial.prototype.editou = false,
      trabalhistaMedia.prototype.editou = false,
      pexMedia.prototype.editou = false
    ]
  }

  unsubscribe() {
    this.subscriptions.forEach((subscription: Subscription) => {
      subscription.unsubscribe();
    });
    this.subscriptions = [];
  }

  multiSelect() {
    if(this.moduloSalvo != null) {return}
    if (this.temPermissaoCivelConsumidor) {
      this.dropdownList.push(
        { item_id: 1, item_text: 'Cível Consumidor' },
        { item_id: 50, item_text: 'Cível Consumidor por Média' }
      )
    }
    if (this.temPermissaoCivelEstrategica) {
      this.dropdownList.push(
        { item_id: 6, item_text: 'Cível Estratégico' }
      )
    }
    if (this.temPermissaoJEC) {
      this.dropdownList.push(
        { item_id: 49, item_text: 'Juizado Especial' }
      )
    }
    if (this.temPermissaoTRAB) {
      this.dropdownList.push(
        { item_id: 7, item_text: 'Trabalhista por Média' },
      )
    }
    if (this.temPermissaoPex) {
      this.dropdownList.push(
        { item_id: 51, item_text: 'Pex por Média' }
      )
    }
    for (let i = 0; i < this.dropdownList.length; i++) {
      this.selectedItems.push(this.dropdownList[i].item_id)
    }    

    this.fazerBusca()
  }

  verificarPermissao() {
    this.temPermissaoCivelConsumidor = this.permissaoService.temPermissaoPara(Permissoes.F_SOLICITAR_CONSULTAR_FECH_CONTING_CC);
    this.temPermissaoCivelEstrategica = this.permissaoService.temPermissaoPara(Permissoes.F_SOLICITAR_CONSULTAR_FECH_CONTING_CE);
    this.temPermissaoJEC = this.permissaoService.temPermissaoPara(Permissoes.F_SOLICITAR_CONSULTAR_FECH_CONTING_JEC);
    this.temPermissaoTRAB = this.permissaoService.temPermissaoPara(Permissoes.F_SOLICITAR_CONSULTAR_FECH_CONTING_TRAB);
    this.temPermissaoPex = this.permissaoService.temPermissaoPara(Permissoes.F_SOLICITAR_CONSULTAR_FECH_CONTING_PEX);
  }

  selecionar(item) {
    var x: boolean;

    switch (item) {
      case 'optCC':
        x = this.optCC = !this.optCC;
        break;
      case 'optEstrategico':
        x = this.optEstrategico = !this.optEstrategico;
        break;
      case 'optJEC':
        x = this.optJEC = !this.optJEC;
        break;
      case 'optPEX':
        x = this.optPEX = !this.optPEX;
        break;
      case 'optTrab':
        x = this.optTrab = !this.optTrab;
        break;
    }

    let btn = document.getElementById(item);

    if (x == false) {
      btn.classList.add('unchecked');
    }
    else {
      btn.classList.remove('unchecked');
    }

    this.fazerBusca()
  }

  async fazerBusca() {
    await this.service.obterAgendamentos(this.pagina - 1, this.selectedItems).subscribe(result => {
      this.fechamentos = result.ordenado,
        this.totalFechamentos = result.total
    });
  }

  async atualizarPaginaInicial(event) {
    this.servicePaginacao.paginatorService.setPage(event);
    this.pagina = event;
    this.fazerBusca()
  }

  excluirFechamento(id) {
    this.messageService.MsgBox2('Deseja excluir o agendamento?', 'Excluir Agendamento',
      'question', 'Sim', 'Não').then(resposta => {
        if (resposta.value) {
          this.service.excluirAgendamento(id).subscribe(() => {
            this.fazerBusca();
          })
        }
      }
      );
  }

  async onChangeOrdenacao(ordem: string, ordenacao: BotaoGridState) {
    const { isActive, ordemCrescente } = ordenacao;

    this.fechamento = false;
    this.empresa = false;
    this.descricao = false;
    this.proxExecucao = false;
    this.usuario = false;
    this.solicitacao = false;

    switch (ordem) {
      case 'Fechamento':
        this.fechamento = true;
        break;
      case 'Empresas':
        this.empresa = true;
        break;
      case 'Descricao':
        this.descricao = true;
        break;
      case 'ProxExecucao':
        this.proxExecucao = true;
        break;
      case 'Usuario':
        this.usuario = true;
        break;
      case 'Solicitacao':
        this.solicitacao = true;
        break;
    }

    await this.service.obterAgendamentos(this.pagina - 1, this.selectedItems, ordem, ordemCrescente).subscribe(result => {
      this.fechamentos = result.ordenado,
        this.totalFechamentos = result.total
    });

  }
  
  novaBusca(){    
    if( !this.selectedItems.find(s => s == this.moduloSalvo) && this.moduloSalvo != null ){      
      this.selectedItems.push(this.moduloSalvo);
      let novoValor = this.selectedItems;
      let valorFinal = []
      for(let i = 0; i < novoValor.length; i++){
        valorFinal.push(novoValor[i])
      }
      this.selectedItems = valorFinal
      return this.selectedItems;
    }
  }
}
