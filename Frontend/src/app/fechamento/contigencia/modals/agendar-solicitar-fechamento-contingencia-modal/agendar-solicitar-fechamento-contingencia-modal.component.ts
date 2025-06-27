import { Component, OnInit, Output } from '@angular/core';
import { Permissoes } from '@permissoes';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BsModalRef } from 'ngx-bootstrap';
import { AgendarSolicitarFechamentoContingenciaService } from 'src/app/fechamento/services/agendar-solicitar-fechamento-contingencia.service';
import { PermissoesService } from 'src/app/permissoes/permissoes.service';
import { AgendarSolicitarFechamentoContingenciaComponent } from '../../agendar-solicitar-fechamento-contingencia/agendar-solicitar-fechamento-contingencia.component';
import { FechamentoCcMediaComponent } from '../fechamento-cc-media/fechamento-cc-media.component';
import { FechamentoCcComponent } from '../fechamento-cc/fechamento-cc.component';
import { FechamentoCivelEstrategicoComponent } from '../fechamento-civel-estrategico/fechamento-civel-estrategico.component';
import { FechamentoJecComponent } from '../fechamento-jec/fechamento-jec.component';
import { FechamentoPexMediaComponent } from '../fechamento-pex-media/fechamento-pex-media.component';
import { FechamentoTrabMediaComponent } from '../fechamento-trab-media/fechamento-trab-media.component';

@Component({
  selector: 'app-agendar-solicitar-fechamento-contingencia-modal',
  templateUrl: './agendar-solicitar-fechamento-contingencia-modal.component.html',
  styleUrls: ['./agendar-solicitar-fechamento-contingencia-modal.component.scss']
})
export class AgendarSolicitarFechamentoContingenciaModalComponent implements OnInit {

  @Output() resultado: number;
  @Output() novoModulo: number;

  constructor(
    private bsModalRef: BsModalRef,
    private messageService: HelperAngular,
    private service: AgendarSolicitarFechamentoContingenciaService,
    private permissaoService: PermissoesService
  ) { }

  ngOnInit() {
    this.execucaoImediata = true;
    this.dataEspecifica = false;
    this.diaria = false;
    this.semanal = false;
    this.mensal = false;
    this.verificarPermissao();
  }

  ngOnDestroy() {
    // location.reload();
  }

  execucaoImediata: boolean;
  dataEspecifica: boolean;
  diaria: boolean;
  semanal: boolean;
  mensal: boolean;

  modulos: any;

  temPermissaoCivelConsumidor = false;
  temPermissaoCivelEstrategica = false;
  temPermissaoJEC = false;
  temPermissaoTRAB = false;
  temPermissaoPex = false;

  diariaPreviaInicio: Date;
  diariaPreviaFim: Date;

  tipoFechamentoSelecionado = "";
  opcaoSemanalSelecionada = "";
  opcaoMensalSelecionada = ""
  dias = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31"]

  execucaoCheck(item) {

    this.execucaoImediata = false;
    this.dataEspecifica = false;
    this.diaria = false;
    this.semanal = false;
    this.mensal = false;

    switch (item) {
      case 'execucaoImediata':
        this.execucaoImediata = true;
        break
      case 'dataEspecifica':
        this.dataEspecifica = true;
        break
      case 'diaria':
        this.diaria = true;
        break
      case 'semanal':
        this.semanal = true;
        break
      case 'mensal':
        this.mensal = true;
        break
    }
  }

  verificarPermissao() {
    this.temPermissaoCivelConsumidor = this.permissaoService.temPermissaoPara(Permissoes.F_SOLICITAR_CONSULTAR_FECH_CONTING_CC);
    this.temPermissaoCivelEstrategica = this.permissaoService.temPermissaoPara(Permissoes.F_SOLICITAR_CONSULTAR_FECH_CONTING_CE);
    this.temPermissaoJEC = this.permissaoService.temPermissaoPara(Permissoes.F_SOLICITAR_CONSULTAR_FECH_CONTING_JEC);
    this.temPermissaoTRAB = this.permissaoService.temPermissaoPara(Permissoes.F_SOLICITAR_CONSULTAR_FECH_CONTING_TRAB);
    this.temPermissaoPex = this.permissaoService.temPermissaoPara(Permissoes.F_SOLICITAR_CONSULTAR_FECH_CONTING_PEX);
  }


  salvar() {
    this.tipoFechamentoSelecionado == '' ? this.messageService.MsgBox2('Tipo de fechamento não informado.', 'Não foi possível fazer o agendamento', 'warning', 'Ok') : this.tipoFechamentoSelecionado;
  }

  close() {
    this.resultado = 0;
    this.bsModalRef.hide();
  }

  confirm(modulo: number) {
    this.novoModulo = modulo;
    this.resultado = 1;
    this.bsModalRef.hide();
  }

  editar(fechamento) {
    let civelConsumidor = FechamentoCcComponent;
    let civelConsumidorMedia = FechamentoCcMediaComponent;
    let civelEstrategico = FechamentoCivelEstrategicoComponent;
    let juizadoEspecial = FechamentoJecComponent;
    let trabalhistaMedia = FechamentoTrabMediaComponent;
    let pexMedia = FechamentoPexMediaComponent;

    // if (fechamento.tipoFechamento == "Cível Consumidor") {
    //   fechamento.tipoFechamento = "Cível Consumidor por Média"
    // }

    this.bsModalRef.content.tipoFechamentoSelecionado = 'Fechamento ' + fechamento.tipoFechamento;

    if (fechamento.tipoFechamento == "Cível Consumidor") {
      civelConsumidor.prototype.editou = true;
      civelConsumidor.prototype.id = fechamento.id,
        civelConsumidor.prototype.fechamentoEditado = fechamento
    }
    if (fechamento.tipoFechamento == "Cível Consumidor por Média") {
      civelConsumidorMedia.prototype.editou = true;
      civelConsumidorMedia.prototype.id = fechamento.id,
        civelConsumidorMedia.prototype.fechamentoEditado = fechamento
    }
    if (fechamento.tipoFechamento == "Cível Estratégico") {
      civelEstrategico.prototype.editou = true;
      civelEstrategico.prototype.id = fechamento.id,
        civelEstrategico.prototype.fechamentoEditado = fechamento
    }
    if (fechamento.tipoFechamento == "Juizado Especial") {
      juizadoEspecial.prototype.editou = true;
      juizadoEspecial.prototype.id = fechamento.id,
        juizadoEspecial.prototype.fechamentoEditado = fechamento
    }
    if (fechamento.tipoFechamento == "Trabalhista por Média") {
      trabalhistaMedia.prototype.editou = true;
      trabalhistaMedia.prototype.id = fechamento.id,
        trabalhistaMedia.prototype.fechamentoEditado = fechamento
    }
    if (fechamento.tipoFechamento == "PEX por Média") {
      pexMedia.prototype.editou = true;
      pexMedia.prototype.id = fechamento.id,
        pexMedia.prototype.fechamentoEditado = fechamento
    }

  }
}
