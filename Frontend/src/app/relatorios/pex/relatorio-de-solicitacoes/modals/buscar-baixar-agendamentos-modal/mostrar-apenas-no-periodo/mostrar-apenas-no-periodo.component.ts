import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { NgbActiveModal, NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StaticInjector } from '@relatorios/pex/relatorio-de-solicitacoes/static-injector';
import { RelatorioDeSolicitacoesService } from '@relatorios/services/relatorios-de-solicitacoes.service';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { DialogService } from '@shared/services/dialog.service';
import moment from 'moment';

@Component({
  selector: 'app-mostrar-apenas-no-periodo',
  templateUrl: './mostrar-apenas-no-periodo.component.html',
  styleUrls: ['./mostrar-apenas-no-periodo.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]

})
export class MostrarApenasNoPeriodoComponent implements OnInit {

  dataIniSolicitacaoFormControl: FormControl = new FormControl('');
  dataFimSolicitacaoFormControl: FormControl = new FormControl('');

  constructor(public modal: NgbActiveModal,public dialog :DialogService, public service : RelatorioDeSolicitacoesService) { }

  ngOnInit() {
    let modals:any = document.getElementsByClassName("modal"); 
    modals[0].style.setProperty("z-index", "100", "important");
  }
  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(this, { centered: true, size: 'sg', backdrop: 'static' });
    // modalRef.componentInstance.comarca = comarca;
    return modalRef.result;
  }

  /// mascaras ---------

  mascaraDataKeyUp(formControl:FormControl, id :string){  
    if(typeof(formControl.value) != "string") return false; 
    let el:any =  document.getElementById(id);
    if(formControl.value.length == 2){ 
      el.value = formControl.value+"/";
    }
    else if(formControl.value.length == 5){
      el.value = formControl.value+"/";
    } 
  }
  // ------- 

  verificarDatas(dataIni:FormControl,dataFim:FormControl,formControlClicado: FormControl,focusout:boolean = false){ 
    if( typeof(dataIni.value) == "string" || typeof(dataFim.value) == "string") return false;
    if( (dataIni.value.getFullYear() < 1000 ||  dataFim.value.getFullYear() < 1000) && !focusout ) return false;
    if ( moment(dataFim.value).format(moment.HTML5_FMT.DATE) < moment(dataIni.value).format(moment.HTML5_FMT.DATE) ) {
      this.dialog.err(
        'A data final não pode ser maior que a data inicial'
      ); 
      formControlClicado.reset();
    }
  } 
  resetarFormControlInvalido(formControl : FormControl) { 
    if (formControl.invalid) {
      formControl.reset();
    }
  }

  pesquisar(){ 
    this.service.pesquisarPelasDatas$.next({
      dataIni : this.dataIniSolicitacaoFormControl.value ? moment(this.dataIniSolicitacaoFormControl.value).format(moment.HTML5_FMT.DATE) : "" ,
      dataFim : this.dataFimSolicitacaoFormControl.value ? moment(this.dataFimSolicitacaoFormControl.value).format(moment.HTML5_FMT.DATE) : "" 
    });
    this.close();
    
  }

 close(){
  this.modal.close();
  let modals:any = document.getElementsByClassName("modal"); 
  modals[0].style.setProperty("z-index", "9000", "important");
 }

}
