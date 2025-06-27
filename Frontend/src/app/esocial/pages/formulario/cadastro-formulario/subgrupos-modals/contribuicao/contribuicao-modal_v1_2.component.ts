import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Contribuicao } from '@esocial/models/subgrupos/v1_2/contribuicao';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { ContribuicaoService } from '@esocial/services/formulario/subgrupos/contribuicao.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-contribuicao-modal-v1-2',
  templateUrl: './contribuicao-modal_v1_2.component.html',
  styleUrls: ['./contribuicao-modal_v1_2.component.scss']
})
export class ContribuicaoModal_v1_2_Component implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    private service: ContribuicaoService,
    private serviceList: ESocialListaFormularioService,
    private dialogService: DialogService,
  ) { }

  ngAfterViewInit(): void {
    this.obterCodigoReceita();
  }

  temPermissaoBlocoCeDeE: boolean = false;
  titulo: string;
  codF2501: number;
  codCalTrib: number;
  codInfoContrib: number;
  contribuicao: Contribuicao;
  codigoReceitaList = [];
  regex12_2 = /^([0-9]{1,10}\,[0-9]{2})/g;

  //#region MÉTODOS
  async obterContribuicao() {
    const resposta = await this.service.obterContribuicao(this.codCalTrib, this.codInfoContrib);
    if (resposta) {
      this.contribuicao = resposta;
      this.iniciarForm();
    }
  }

  async obterCodigoReceita() {
    const resposta = await this.serviceList.obterCodigoReceitaAsync();
    if (resposta) {
      this.codigoReceitaList = resposta.map((cr) => {
        return(
          {
            id: cr.id,
            descricao: cr.descricao,
            descricaoConcatenada: `${cr.id} - ${cr.descricao}`
          }
        );
      });
      this.iniciarForm();
    }
  }

  async salvar() {
    let operacao = this.titulo == 'Alterar' ? 'Alteração' : 'Inclusão';
    let valueSubmit = this.formGroup.value
    valueSubmit.calctribPerref = Number(valueSubmit.calctribPerref);
    valueSubmit.calctribVrbccpmensal = Number(valueSubmit.calctribVrbccpmensal);

    try {

      if (this.titulo == "Alterar") {
        await this.service.atualizar(this.codF2501, this.codCalTrib, this.codInfoContrib, valueSubmit);
      } else {
        await this.service.incluir(this.codF2501, this.codCalTrib, valueSubmit);
      }

      await this.dialogService.alert('Operação realizada!', `${operacao} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
        await this.dialogService.err(
          `Desculpe, não foi possivel a ${operacao}`,
          mensagem
        );
    }
  }

  //#endregion


  static exibeModalAlterar(codF2501: number, codCalTrib: number, codInfoContrib: number, temPermissaoBlocoCeDeE: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ContribuicaoModal_v1_2_Component,
      { windowClass: 'modal-contribuicao', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codCalTrib = codCalTrib;
    modalRef.componentInstance.codInfoContrib = codInfoContrib;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE ? 'Alterar' : 'Consultar';
    modalRef.componentInstance.obterContribuicao();
    return modalRef.result;
  }

  static exibeModalIncluir(codF2501: number, codCalTrib: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ContribuicaoModal_v1_2_Component,
      { windowClass: 'modal-contribuicao',centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codCalTrib = codCalTrib;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = true;
    modalRef.componentInstance.titulo = 'Incluir';
    modalRef.componentInstance.iniciarForm();
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#region FORMCONTROL

  infocrcontribTpcrFormControl: FormControl = new FormControl(null, [Validators.required]);
  infocrcontribVrcrFormControl: FormControl = new FormControl(null, [Validators.required]);

  formGroup: FormGroup = new FormGroup({
    infocrcontribTpcr: this.infocrcontribTpcrFormControl,
    infocrcontribVrcr: this.infocrcontribVrcrFormControl
  });

  iniciarForm() {
    if(this.codInfoContrib > 0){
      this.infocrcontribTpcrFormControl.setValue(this.contribuicao.infocrcontribTpcr);
      this.infocrcontribVrcrFormControl.setValue(this.contribuicao.infocrcontribVrcr);
      if (!this.temPermissaoBlocoCeDeE) {
        this.infocrcontribTpcrFormControl.disable();
        this.infocrcontribVrcrFormControl.disable();
      }
    }else{
      this.infocrcontribTpcrFormControl.setValue(null);
      this.infocrcontribVrcrFormControl.setValue(null);
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  //#endregion

}
