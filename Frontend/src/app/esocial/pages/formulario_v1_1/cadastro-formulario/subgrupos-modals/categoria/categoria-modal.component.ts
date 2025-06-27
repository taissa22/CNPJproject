import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { MudancaCategoria } from '@esocial/models/subgrupos/mudancaCategoria';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { MudancaCategoriaService } from '@esocial/services/formulario_v1_1/subgrupos/mudanca-categoria.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { BsLocaleService } from 'ngx-bootstrap';

@Component({
  selector: 'app-categoria-modal',
  templateUrl: './categoria-modal.component.html',
  styleUrls: ['./categoria-modal.component.scss']
})
export class CategoriaModalComponent implements OnInit {
  categoria: MudancaCategoria = null;

  listaCategoria: any[] = [];
  listaNaturezaAtividade: any[] = [];
  idf2500 = null;
  contratoId: number;
  temPermissaoEsocialBlocoABCDEFHI: boolean = false;

  tooltipCategoria: string = "Preencher com o código da categoria do trabalhador.";
  tooltipNatAtividade: string = "Preencher com a Natureza da Atividade.";
  tooltipDataMundanca: string = "Preencher com a data a partir da qual foi reconhecida a nova categoria e/ou a nova natureza da atividade.";

  IdFormControl: FormControl = new FormControl(null);
  IdFormularioFormControl: FormControl = new FormControl(null);
  DataOperacaoFormControl: FormControl = new FormControl(null);
  logCodUsuarioFormControl: FormControl = new FormControl(null);
  codigoCategoriaFormControl: FormControl = new FormControl(null, [Validators.required]);
  naturezaAtividadeFormControl: FormControl = new FormControl(null);
  dataMudancaCategoriaFormControl: FormControl = new FormControl(null, [Validators.required]);

  formGroup: FormGroup = new FormGroup({
    idF2500: this.IdFormularioFormControl,
    logDataOperacao: this.DataOperacaoFormControl,
    logCodUsuario: this.logCodUsuarioFormControl,
    mudcategativCodcateg: this.codigoCategoriaFormControl,
    mudcategativNatatividade: this.naturezaAtividadeFormControl,
    mudcategativDtmudcategativ: this.dataMudancaCategoriaFormControl,
    idEsF2500Mudcategativ: this.IdFormControl
  });
  dialog: any;

  constructor(
    private service: MudancaCategoriaService,
    private dialogService: DialogService,
    private modal: NgbActiveModal,
    private configLocalizacao: BsLocaleService,
    private listaService: ESocialListaFormularioService,
  ) {
    this.configLocalizacao.use('pt-BR');
  }

  ngOnInit() {
    this.obterListaCategoria();
    this.obterListaNaturezaAtividade();

    this.iniciarForm();
  }

  iniciarForm() {
    if (this.categoria) {
      this.IdFormularioFormControl.setValue(this.categoria.idF2500);
      this.DataOperacaoFormControl.setValue(this.categoria.logDataOperacao);
      this.logCodUsuarioFormControl.setValue(this.categoria.logCodUsuario);
      this.codigoCategoriaFormControl.setValue(
        this.categoria.mudcategativCodcateg
      );
      this.naturezaAtividadeFormControl.setValue(
        this.categoria.mudcategativNatatividade
      );
      this.dataMudancaCategoriaFormControl.setValue(
        this.categoria.mudcategativDtmudcategativ ? new Date(this.categoria.mudcategativDtmudcategativ) : null
      );
      this.IdFormControl.setValue(this.categoria.idEsF2500Mudcategativ);

      if (!this.temPermissaoEsocialBlocoABCDEFHI) {
        this.IdFormularioFormControl.disable();
        this.DataOperacaoFormControl.disable();
        this.logCodUsuarioFormControl.disable();
        this.codigoCategoriaFormControl.disable();
        this.naturezaAtividadeFormControl.disable();
        this.dataMudancaCategoriaFormControl.disable();
        this.IdFormControl.disable();
      }
    }
    else {
      this.naturezaAtividadeFormControl.setValue(1);
    }
  }

  async obterListaCategoria() {
    const resposta = await this.listaService.obterCodigoCategoriaAsync();
    if (resposta) {
      this.listaCategoria = resposta.map(categoria => {
        return ({
          id: categoria.id,
          descricao: categoria.descricao,
          descricaoConcatenada: `${categoria.id} - ${categoria.descricao}`
        })
      });
    }
  }

  async obterListaNaturezaAtividade() {
    const respostaNatureza =
      await this.listaService.obterNaturezaAtividadeAsync();
    if (respostaNatureza) {
      this.listaNaturezaAtividade = respostaNatureza.map(naturezaAtividade => {
        return {
          id: naturezaAtividade.id,
          descricao: naturezaAtividade.descricao,
          descricaoConcatenada: `${naturezaAtividade.id} - ${naturezaAtividade.descricao}`
        };
      });
    }
  }

  async salvar() {
    let Operacao = this.categoria ? 'Alteração' : 'Inclusão';

    try {
      if (this.categoria) {
        let obj = this.formGroup.value;
        obj.mudcategativDtmudcategativ = new Date(this.dataMudancaCategoriaFormControl.value);
        await this.service.alterar(this.idf2500, this.contratoId, obj);
      } else {
        let obj = this.formGroup.value;
        obj.idF2500 = this.idf2500;
        await this.service.incluir(this.idf2500, this.contratoId, this.formGroup.value);
      }

      await this.dialogService.alert(`${Operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.err(
        `Desculpe, não foi possivel a ${Operacao}`,
        mensagem
      );
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  static exibeModalAlterar(idf2500: number, contratoId: number, categoria: MudancaCategoria, temPermissaoEsocialBlocoABCDEFHI: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      CategoriaModalComponent,
      {
        windowClass: 'modal-categoria',
        centered: true,
        size: 'sm',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.idf2500 = idf2500;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.categoria = categoria;
    modalRef.componentInstance.temPermissaoEsocialBlocoABCDEFHI = temPermissaoEsocialBlocoABCDEFHI;
    return modalRef.result;
  }



  static exibeModalIncluir(idf2500: number, contratoId: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      CategoriaModalComponent,
      {
        windowClass: 'modal-categoria',
        centered: true,
        size: 'sm',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.idf2500 = idf2500;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.temPermissaoEsocialBlocoABCDEFHI = true;
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

}
