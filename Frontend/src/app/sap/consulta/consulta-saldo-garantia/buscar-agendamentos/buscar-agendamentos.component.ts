import { Component, OnInit } from '@angular/core';
import { BuscarAgendamentosService } from '../service/buscar-agendamentos.service';
import { ActivatedRoute } from '@angular/router';
import { take, pluck } from 'rxjs/operators';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { SaldoGarantiaCriteriosPesquisaService } from '../service/saldo-garantia-criterios-pesquisa.service';
import { SaldoGarantiaService } from 'src/app/core/services/sap/saldo-garantia.service';
import { ResultadoAgendamento } from '@shared/interfaces/ResultadoAgendamento';
import { statusAgendamento } from './buscar-agendamentos.constants';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from 'src/app/sap/sap.constants';

@Component({
  selector: 'app-buscar-agendamentos',
  templateUrl: './buscar-agendamentos.component.html',
  styleUrls: ['./buscar-agendamentos.component.scss']
})
export class BuscarAgendamentosComponent implements OnInit {

  agendamentos: ResultadoAgendamento[];
  breadcrumb: string;

  constructor(private service: BuscarAgendamentosService,
    private actr: ActivatedRoute,
    public criteriopesquisaService: SaldoGarantiaCriteriosPesquisaService,
    private criterioService: SaldoGarantiaCriteriosPesquisaService,
    private messageService: HelperAngular,
    private breadcrumbsService: BreadcrumbsService) {

    //RESOLVER DA TELA
    this.actr.data.subscribe(
      data => {
        this.agendamentos = data['agendamentos']

        if (this.service.filtrosSubject.value['total'] == this.agendamentos.length)
          this.vermais = false;
        else this.vermais = true;
      }
    );
  }

  vermais = true;

  ngOnInit() {


  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuConsultaSaldoGarantia);
  }

  onExcluir(index) {
    const item = this.agendamentos[index];
    this.messageService.MsgBox2('Deseja excluir o agendamento?', 'Excluir Agendamento',
      'question', 'Sim', 'Não').then(resposta => {
        if (resposta.value) {
          this.service.excluir(item.id)
            .subscribe(
              res => {
                if (res['sucesso']) {
                  this.agendamentos.splice(index, 1);
                }
                else this.messageService.MsgBox2(res['mensagem'], "Erro ao excluir agendamento", "warning", 'Ok');
              }
            );
        }
      })

  }

  onDownload(nomeArquivo, tipoProcesso) {
    this.service.download(nomeArquivo, tipoProcesso);
  }

  onVerMais() {
    this.service.consultarMais().subscribe(
      agendamentos => {
        this.agendamentos.push.apply(this.agendamentos, agendamentos)
        if (this.service.filtrosSubject.value['total'] == this.agendamentos.length)
          this.vermais = false;
        else this.vermais = true;
      }
    );
  }

  onAtualizar() {
    this.service.limpar();
    this.service.consultar()
      .pipe(take(1))
      .subscribe(
        agendamentos => {
          this.agendamentos = agendamentos
          if (this.service.filtrosSubject.value['total'] == this.agendamentos.length)
            this.vermais = false;
          else this.vermais = true;
        }
      );
  }

  exibirExcluir(status) {
    if (status == 'Erro' || status == 'Agendado') return true;
    else return false;

  }

  verificarCor(status) {
    if (status)
      return statusAgendamento[status.toLowerCase()]
  }

  valoresCriterio;

  eventModalFechou(event, t: NgbTooltip) {
    //Se clicar no fechar, fechar todas as tooltips, pois so pode ter uma tooltip por vez
    this.tooltips.forEach(item => item.close())
    t.close();
  }
/**Array com todas as tooltips da tela */
  tooltips: NgbTooltip[] = [];

  verificarID(tooltip: NgbTooltip, id: string) {
    this.tooltips.push(tooltip)
    if (tooltip.isOpen()) {
      //Se a tooltip ja está aberta, fechar todas
      this.tooltips.forEach(item => item.close())
      tooltip.close();
    } else {
      this.criterioService.valoresConvertidosCriterios(id)
        .subscribe(item =>
          this.valoresCriterio = this.criterioService.converterValores(item), () => { }, () => {
          //Antes de abrir qualquer tooltip fechar a anterior
            this.tooltips.forEach(item => item.close())
          tooltip.open();
        }
        )
    }
  }
}
