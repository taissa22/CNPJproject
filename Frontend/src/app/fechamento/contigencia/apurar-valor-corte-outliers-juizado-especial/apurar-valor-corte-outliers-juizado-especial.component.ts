import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HelperAngular } from '@shared/helpers/helper-angular';

import { take } from 'rxjs/operators';

import { AgendarApuracaoOutliersModel } from '../../models/contigencia/apurar-valor-corte-outliers/agendar-apuracao-outliers';
import { CorStatusApuracoesAgendadas } from '../../models/contigencia/apurar-valor-corte-outliers/cor-status-apuracoes-agendadas';
import { ApurarValorCorteOutliersService } from '../../services/apurar-valor-corte-outliers.service';
@Component({
  selector: 'app-apurar-valor-corte-outliers-juizado-especial',
  templateUrl: './apurar-valor-corte-outliers-juizado-especial.component.html',
  styleUrls: ['./apurar-valor-corte-outliers-juizado-especial.component.scss']
})
export class ApurarValorCorteOutliersJuizadoEspecialComponent implements OnInit {

  public listaAgendamentos = [];
  public listaBaseFechamento = [];
 
  public vermais = true;
  public popupAgendamento = false;
 
  public form: FormGroup;
  public valorSelect;
  public valorAnteriorFatorDesvioPadrao = '1,5';
  public valorFatorDesvioPadrao = '1,5';
  
  public agendarApuracaoOutliers: AgendarApuracaoOutliersModel;
  public codEmpCentralizadora;
  public mesAnoFechamento;
  public required = false;

  constructor(
    private service: ApurarValorCorteOutliersService,
    private fb: FormBuilder,
    private messageService: HelperAngular,
  ) { }

  ngOnInit() {
    this.atualizarLista();
    this.listarBaseFechamento();
    this.formulario();
  }

  formulario() {
    this.form = this.fb.group({
      baseFechamentoForm: [this.valorSelect],
      desvioPadraoForm: [this.valorFatorDesvioPadrao,[Validators.required, Validators.max(99.99)]],
      observacaoForm: ['']
    });
  }

  listarBaseFechamento() {
    this.service.listarBaseFechamento().subscribe(res => {
      this.listaBaseFechamento = res.data;
      this.valorSelect = this.listaBaseFechamento[0].dataFechamento;
    });
  }
  
  atualizarLista() {
    this.service.limpar();
    this.service.consultar()
    .pipe(take(1))
    .subscribe(
      agendamentos => {
        this.listaAgendamentos = agendamentos
        if( this.listaAgendamentos.length != 0){
          this.valorFatorDesvioPadrao = this.listaAgendamentos[0].fatorDesvioPadrao;
          this.valorAnteriorFatorDesvioPadrao = this.listaAgendamentos[0].fatorDesvioPadrao;        
          }
          
          if (this.service.paginacaoVerMais.value['total'] == this.listaAgendamentos.length || this.listaAgendamentos.length == 0)
          this.vermais = false;
          else this.vermais = true;
        }
        );
  }

  verificarCorStatusAgendamento(status) {
    if (status)
      return CorStatusApuracoesAgendadas[status]
  }
  
  verMais() {
    this.service.consultarMais().subscribe(
      agendamentos => {
        this.listaAgendamentos.push.apply(this.listaAgendamentos, agendamentos)
        if (this.service.paginacaoVerMais.value['total'] == this.listaAgendamentos.length || this.listaAgendamentos == [])
          this.vermais = false;
        else this.vermais = true;
      }
    );
  }
  
  validarCampoObservacao() {
    
    if (this.valorAnteriorFatorDesvioPadrao != this.valorFatorDesvioPadrao) {
      if (this.form.value.observacaoForm == null || this.form.value.observacaoForm == '') {
        this.required = true;
      } else {
        this.required = false;
      }
    } else {
      this.required = false;
    }        
  }

  preenchendoFormulario() {
    this.listarBaseFechamento();
  
    this.form.value.observacaoForm = '';
    (document.getElementById('obs') as HTMLInputElement).value = '';
  
    if(this.listaAgendamentos.length == 0){
      this.valorFatorDesvioPadrao = '1,5';
      this.valorAnteriorFatorDesvioPadrao = '1,5';
      this.form.value.desvioPadraoForm = '1,5';
      (document.getElementById('fator') as HTMLInputElement).value = '1,5';
    }else{
      this.form.value.desvioPadraoForm = this.valorFatorDesvioPadrao;
      (document.getElementById('fator') as HTMLInputElement).value = this.valorAnteriorFatorDesvioPadrao;
    }
  }

  abrirPopupAgendamento() { 
    this.atualizarLista();
    this.preenchendoFormulario();
    this.required = false;   
    this.popupAgendamento = true;
  }

  fecharPopupAgendamento() {
    this.popupAgendamento = false;
    this.form.reset();    
  }

  pegarDadosBaseFechamentoSelecionada(form, select) {
    this.listaBaseFechamento.forEach(e => {
      if (e.dataFechamento.match(form) || e.dataFechamento.match(select)) {
        this.codEmpCentralizadora = e.codigoEmpresaCentralizadora;
        this.mesAnoFechamento = e.mesAnoFechamento;
      }
    });
  }

  fazerAgendamento() {
    this.agendarApuracaoOutliers = new AgendarApuracaoOutliersModel();

    if (this.form.value.baseFechamentoForm == null || this.form.value.baseFechamentoForm == undefined)
      this.agendarApuracaoOutliers.dataFechamento = this.valorSelect;
    else
      this.agendarApuracaoOutliers.dataFechamento = this.form.value.baseFechamentoForm;

    this.agendarApuracaoOutliers.dataSolicitacao = formatDate(new Date(), 'dd/MM/yyyy HH:mm:ss', 'pt_BR');
    this.agendarApuracaoOutliers.fatorDesvioPadrao = this.form.value.desvioPadraoForm;
    this.agendarApuracaoOutliers.observacao = this.form.value.observacaoForm;
    this.agendarApuracaoOutliers.status = "0";

    this.pegarDadosBaseFechamentoSelecionada(this.agendarApuracaoOutliers.dataFechamento, this.valorSelect);
    this.agendarApuracaoOutliers.codEmpresaCentralizadora = this.codEmpCentralizadora;
    this.agendarApuracaoOutliers.mesAnoFechamento = this.mesAnoFechamento;
  
    this.service.agendarApuracaoOutliers(this.agendarApuracaoOutliers).subscribe(res => {
      if (res['sucesso']) {
        this.messageService.MsgBox2('', 'Agendamento realizado com sucesso', 'success', 'Ok');
        this.fecharPopupAgendamento();
        this.atualizarLista();
      } else {
        if (this.agendarApuracaoOutliers.fatorDesvioPadrao == 0) {
          this.messageService.MsgBox2('Fator de desvio padrão não pode ser igual a 0 ', 'Não foi possível fazer o agendamento', 'warning', 'Ok');
        } else {
          this.messageService.MsgBox2(res['mensagem'], 'Não foi possível fazer o agendamento', 'warning', 'Ok');
        }
      }
    });
  }

  public deletarApuracoesAgendadas(id) {
    this.messageService.MsgBox2('Deseja excluir o agendamento?', 'Excluir Agendamento',
      'question', 'Sim', 'Não').then(resposta => {
        if (resposta.value) {
          this.service.removerApuracoesAgendadas(id).subscribe(() => {
            this.atualizarLista();
          });
        }
      });
  }

  download(nomeArquivo) {
    this.service.downloadArquivos(nomeArquivo);
  }
}
