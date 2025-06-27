import { Component, OnInit } from '@angular/core';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BsModalRef } from 'ngx-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subscription, BehaviorSubject } from 'rxjs';
import { take, distinctUntilChanged } from 'rxjs/operators';
import { textosValidacaoFormulario } from '@shared/utils';
import { TipoAudienciaService } from 'src/app/core/services/sap/TipoAudiencia.service';
import { TipoAudienciaCrudServiceService } from './../services/tipoAudienciaCrudService.service';
import { TipoAudienciaServiceService } from './../services/tipoAudienciaService.service';
import { TipoAudienciaDTO } from '@shared/interfaces/TipoAudienciaDTO';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';



@Component({
  selector: 'app-cadastros-tipoaudiencia-modal',
  templateUrl: './cadastros-tipoaudiencia-modal.component.html',
  styleUrls: ['./cadastros-tipoaudiencia-modal.component.scss']
})
export class CadastrosTipoaudienciaModalComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef,
    private fb: FormBuilder,
    private service: TipoAudienciaCrudServiceService,
    public tipoprocessosservice: TipoProcessoService) { }

  public currentValueComboTipoProcessoSubject = new BehaviorSubject<any>(null);
  public comboboxTipoProcessoSubject = new BehaviorSubject<TipoProcesso[]>([]);


  comboTipoProcesso: TipoProcesso[] = [];
  //#region Subscriptions
  comboTipoProcessoSubscription: Subscription;

  titulo = 'Inclusão';
  registerForm: FormGroup;
  subscription: Subscription;


  ngOnInit() {

    this.registerForm = this.service.inicializarForm();
    this.comboTipoProcessoSubscription = this.getTipoProcesso()
      .subscribe(comboboxItens => { this.comboTipoProcesso = comboboxItens; });

    this.service.fecharModal.subscribe(item => {
      if (item) {
        this.bsModalRef.hide();
      }
    });

    this.service.valoresEdicaoFormulario.pipe(distinctUntilChanged()).subscribe(
      item => {
        if (item) {
          this.titulo = 'Alteração Tipo de Audiência';
          this.registerForm.patchValue(item);
        } else {
          this.titulo = 'Incluir Tipo de Audiência';
        }
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
    this.registerForm.get('codTipoProcesso').setValue(parseInt(index))
  }


  validacaoTextos(nomeControl: string, nomeCampo: string, isFeminino: boolean) {
    return textosValidacaoFormulario(this.registerForm, nomeControl, nomeCampo, isFeminino);
  }

  pegarNumero(numero: any) {
    // console.log("ManutençãoModalComponent -> pegarNumero -> numero", numero)
    this.registerForm.get('valorJuros').setValue(numero)

  }

  setData(data){
    // console.log(data)

    data && this.registerForm.get('dataVigencia').setValue(data)
  }

  salvarAlteracao() {

    this.service.salvar(this.registerForm);
  }

}
