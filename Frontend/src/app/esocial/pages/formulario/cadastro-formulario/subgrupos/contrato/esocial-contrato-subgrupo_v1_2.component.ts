import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { Contrato } from '@esocial/models/subgrupos/v1_2/contrato';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { ContratoService } from '@esocial/services/formulario/subgrupos/contrato.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { ContratoModal_v1_2_Component } from '../../subgrupos-modals/contrato/contrato-modal_v1_2.component';

@Component({
  selector: 'app-esocial-contrato-subgrupo-v1-2',
  templateUrl: './esocial-contrato-subgrupo_v1_2.component.html',
  styleUrls: ['./esocial-contrato-subgrupo_v1_2.component.css']
})
export class EsocialContratoSubgrupo_v1_2_Component implements AfterViewInit {
  breadcrumb: string;
  constructor(
    private service: ContratoService,
    private dialog: DialogService,
    private serviceList: ESocialListaFormularioService,
  ) { }

  //#region DECORATORS
  @Input() formulario2500 : FormGroup;
  @Input() podeValidar: boolean = false;
  @Input() salvarRascunho: boolean = false;
  @Input() salvarEEnviar: boolean = false;
  @Input() finalizarEscritorio: boolean = false;
  @Input() finalizarContador: boolean = false;
  @Input() finalizadoEscritorio: boolean = false;
  @Input() finalizadoContador: boolean = false;
  @Input() temPermissaoEsocialBlocoABCDEFHI: boolean;
  @Input() temPermissaoEsocialBlocoGK: boolean;
  @Input() temPermissaoEsocialBlocoJDadosEstabelecimento: boolean;
  @Input() temPermissaoEsocialBlocoJValores: boolean;
  @Input() temPermissaoEsocialBlocoEPensaoAlimenticia: boolean;
  @Input() temPermissaoEnviarEsocial: boolean;
  @Input() contratoTerceiro: boolean = false;
  @Input() dataSentenca: Date = new Date();
  @Input() retornaStatusRascunho: boolean = false;

  @Output() aoValidarContratosSubgrupos = new EventEmitter<Array<{ idContrato: number, invalido: boolean }>>();
  @Output() aoSalvarRascunho = new EventEmitter<Array<{ idContrato: number, salvo: boolean, mensagemErro: string }>>();
  @Output() aoSalvarEEnviar = new EventEmitter<Array<{ idContrato: number, salvo: boolean, mensagemErro: string }>>();
  @Output() aoFinalizarEscritorio = new EventEmitter<Array<{ idContrato: number, validado: boolean, mensagemErro: string }>>();
  @Output() aoFinalizarContador = new EventEmitter<Array<{ idContrato: number, validado: boolean, mensagemErro: string }>>();
  @Output() aoRetornarQtdContratos = new EventEmitter();
  @Output() carregado = new EventEmitter();
  @Output() aoAbrirPopup = new EventEmitter();

  //#endregion

  async ngAfterViewInit(): Promise<void> {
    this.codigoFormularioFormControl.setValue(this.formularioId);
    await this.buscarTabela();
    await this.obterDados();
    this.carregado.emit(true);
  }

  //#region DECORATORS CONTRATO
  @Input() formularioId: number;

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  //#endregion

  //#region VARIAVEIS CONTRATO
  total: number = 0;
  dataSource: Array<Contrato> = [];
  contratoSelecionado = -1;

  retornoValidacoesContratos: Array<{ idContrato: number, invalido: boolean, cont: number }> = [];
  retornoSalvarRascunhoLista: Array<{ idContrato: number, salvo: boolean, mensagemErro: string }> = [];
  retornoSalvarEEnviarLista: Array<{ idContrato: number, salvo: boolean, mensagemErro: string }> = [];
  retornoFinalizarEscritorioLista: Array<{ idContrato: number, validado: boolean, mensagemErro: string }> = [];
  retornoFinalizarContadorLista: Array<{ idContrato: number, validado: boolean, mensagemErro: string }> = [];

  buscarDescricaoFormControl: FormControl = new FormControl(null);
  codigoFormularioFormControl: FormControl = new FormControl(null);
  //#endregion

  //#region CONTRATO
  @Input() ideempregadorTpinsc: any;

  //#region VARIAVEIS SUB-CONTRATO
  tipoRegimePrevidenciarioList: [];
  tipoContratoTempoParcialList: [];
  motivoDesligamentoList: [];
  repercussaoProcessoList: [];
  tipoRegimeTrabalhistaList: [];
  //#endregion

  //#region FUNÇÕES CONTRATO

  iniciaValoresDaView() {
    let sort: any =
      this.table === undefined || !this.table.sortColumn
        ? 'id'
        : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection:
        this.table === undefined || !this.table.sortDirection
          ? 'asc'
          : this.table.sortDirection,
      pageIndex:
        this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize
    };
  }

  async incluir(): Promise<void> {

    if (this.dataSource.length == 1) {
      const incluirNovo: boolean = await this.dialog.confirm(
        'Incluir Contrato',
        `Já existe um contrato de trabalho registrado para esse reclamante. Deseja realmente incluir um novo</b>?`
      );

      if (!incluirNovo) {
        return;
      }
    }
    if (this.dataSource.length > 1) {
      const excluir: boolean = await this.dialog.confirm(
        'Incluir Contrato',
        `Já existem contratos de trabalho registrados para esse reclamante. Deseja realmente incluir um novo</b>?`
      );

      if (!excluir) {
        return;
      }
    }

    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await ContratoModal_v1_2_Component.exibeModalIncluir(this.formularioId, this.contratoTerceiro);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async alterar(item: Contrato): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await ContratoModal_v1_2_Component.exibeModalAlterar(
      item
    );

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async consultar(item: Contrato): Promise<void> {
    await ContratoModal_v1_2_Component.exibeModalConsultar(item);
  }

  async onClearInputPesquisar() {
    if (!this.buscarDescricaoFormControl.value) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  selecionarContrato(expandir: string) {
    //return expandir == "true" ? false : true;
    return true;
  }

  async setRetornoValidacao(retornoValidacao: { idContrato: number, invalido: boolean, cont: number }) {

    if (this.retornoValidacoesContratos.length == 0) {
      retornoValidacao.cont = 1;
      this.retornoValidacoesContratos.push(retornoValidacao);
      return this.aoValidarContratosSubgrupos.emit(this.retornoValidacoesContratos);
    }

    const existe = this.retornoValidacoesContratos.some((contrato) => contrato.idContrato == retornoValidacao.idContrato);    

    if (!existe) {      
      retornoValidacao.cont = this.retornoValidacoesContratos[0].cont;
      this.retornoValidacoesContratos.push(retornoValidacao);
    } else {
      this.retornoValidacoesContratos.forEach((contrato) => {
        if (contrato.idContrato == retornoValidacao.idContrato) {
           contrato.cont += 1;
           contrato.invalido = retornoValidacao.invalido;
           return
        }
      })
    }

    return this.aoValidarContratosSubgrupos.emit(this.retornoValidacoesContratos);
  }

  async setRetornoSalvarRascunho(retornoSalvarRascunho: { idContrato: number, salvo: boolean, mensagemErro: string }) {
    if (this.retornoSalvarRascunhoLista.length == 0) {
      this.retornoSalvarRascunhoLista.push(retornoSalvarRascunho);
      return this.aoSalvarRascunho.emit(this.retornoSalvarRascunhoLista);
    }

    const existe = this.retornoSalvarRascunhoLista.some((contrato) => contrato.idContrato == retornoSalvarRascunho.idContrato)

    if (!existe) {
      this.retornoSalvarRascunhoLista.push(retornoSalvarRascunho);
    } else {
      this.retornoSalvarRascunhoLista.forEach((contrato) => {
        if (contrato.idContrato == retornoSalvarRascunho.idContrato) {
           contrato.salvo = retornoSalvarRascunho.salvo;
           contrato.mensagemErro = retornoSalvarRascunho.mensagemErro;
           return;
        }
      })
    }

    return this.aoSalvarRascunho.emit(this.retornoSalvarRascunhoLista);
  }

  async setRetornoSalvarEEnviar(retornoSalvarEEnviar: { idContrato: number, salvo: boolean, mensagemErro: string }) {
    if (this.retornoSalvarEEnviarLista.length == 0) {
      this.retornoSalvarEEnviarLista.push(retornoSalvarEEnviar);
      return this.aoSalvarEEnviar.emit(this.retornoSalvarEEnviarLista);
    }

    const existe = this.retornoSalvarEEnviarLista.some((contrato) => contrato.idContrato == retornoSalvarEEnviar.idContrato)

    if (!existe) {
      this.retornoSalvarEEnviarLista.push(retornoSalvarEEnviar);
    } else {
      this.retornoSalvarEEnviarLista.forEach((contrato) => {
        if (contrato.idContrato == retornoSalvarEEnviar.idContrato) {
          contrato.salvo = retornoSalvarEEnviar.salvo;
          contrato.mensagemErro = retornoSalvarEEnviar.mensagemErro;
          return;
        }
      })
    }

    return this.aoSalvarEEnviar.emit(this.retornoSalvarEEnviarLista);
  }

  async setRetornoFinalizarEscritorio(retornoFinalizarEscritorio: { idContrato: number, validado: boolean, mensagemErro: string }) {
    if (this.retornoFinalizarEscritorioLista.length == 0) {
      this.retornoFinalizarEscritorioLista.push(retornoFinalizarEscritorio);
      return this.aoFinalizarEscritorio.emit(this.retornoFinalizarEscritorioLista);
    }

    const existe = this.retornoFinalizarEscritorioLista.some((contrato) => contrato.idContrato == retornoFinalizarEscritorio.idContrato)

    if (!existe) {
      this.retornoFinalizarEscritorioLista.push(retornoFinalizarEscritorio);
    } else {
      this.retornoFinalizarEscritorioLista.forEach((contrato) => {
        if (contrato.idContrato == retornoFinalizarEscritorio.idContrato) {
          contrato.validado = retornoFinalizarEscritorio.validado;
          contrato.mensagemErro = retornoFinalizarEscritorio.mensagemErro;
          return;
        }
      })
    }

    return this.aoFinalizarEscritorio.emit(this.retornoFinalizarEscritorioLista);
  }

  async setRetornoFinalizarContador(retornoFinalizarContador: { idContrato: number, validado: boolean, mensagemErro: string }) {
    if (this.retornoFinalizarContadorLista.length == 0) {
      this.retornoFinalizarContadorLista.push(retornoFinalizarContador);
      return this.aoFinalizarContador.emit(this.retornoFinalizarContadorLista);
    }

    const existe = this.retornoFinalizarContadorLista.some((contrato) => contrato.idContrato == retornoFinalizarContador.idContrato)

    if (!existe) {
      this.retornoFinalizarContadorLista.push(retornoFinalizarContador);
    } else {
      this.retornoFinalizarContadorLista.forEach((contrato) => {
        if (contrato.idContrato == retornoFinalizarContador.idContrato) {
          contrato.validado = retornoFinalizarContador.validado;
          contrato.mensagemErro = retornoFinalizarContador.mensagemErro;
          return;
        }
      })
    }

    return this.aoFinalizarContador.emit(this.retornoFinalizarContadorLista);
  }

  //#endregion

  //#region MÉTODOS CONTRATO

  async buscarTabela(): Promise<void> {
    if (this.codigoFormularioFormControl.value > 0) {
      let view = this.iniciaValoresDaView();

      try {
        const data = await this.service.obterPaginado(
          this.codigoFormularioFormControl.value,
          view.pageIndex,
          view.sortColumn,
          view.sortDirection == 'asc'
        );

        this.total = data.total;
        this.dataSource = data.lista;

        this.aoRetornarQtdContratos.emit(this.total);
      } catch (error) {
        this.dialog.err(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
        );
      }
    }
  }

  async excluir(item: Contrato): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Contrato',
      `Deseja excluir o Contrato<br><b></br> ID: ${item.idEsF2500Infocontrato}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.idF2500, item.idEsF2500Infocontrato);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Contrato excluído!'
      );
      this.buscarDescricaoFormControl.setValue('');
      this.removeDadosRetorno(item.idEsF2500Infocontrato);
      await this.buscarTabela();
      this.carregado.emit(true);
    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('registrado')) {
        await this.dialog.info(
          `Exclusão não permitida`,
          (error as HttpErrorResult).messages.join('\n')
        );
        return;
      }
      await this.dialog.err(
        `Exclusão não realizada`,
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  removeDadosRetorno(idContrato: number){
    if (this.retornoSalvarEEnviarLista.length > 0) {
      this.retornoSalvarEEnviarLista = this.retornoSalvarEEnviarLista.filter(contrato => {contrato.idContrato != idContrato});
    }
    if (this.retornoSalvarRascunhoLista.length > 0) {
      this.retornoSalvarRascunhoLista = this.retornoSalvarRascunhoLista.filter(contrato => {contrato.idContrato != idContrato});
    }
    if (this.retornoValidacoesContratos.length > 0) {
      this.retornoValidacoesContratos = this.retornoValidacoesContratos.filter(contrato => {contrato.idContrato != idContrato});
    }
  }

  //#region MÉTODO PROMISE.ALL PARA FORMULARIO

  async obterDados() {
    try {
      const [
        tipoRegimePrevidenciarioList,
        tipoContratoTempoParcialList,
        motivoDesligamentoList,
        repercussaoProcessoList,
        tipoRegimeTrabalhistaList,
      ] = await Promise.all([
        this.serviceList.obterTipoRegimePrevidenciarioAsync(),
        this.serviceList.obterTipoContratoTempoParcialAsync(),
        this.serviceList.obterMotivoDesligamentoAsync(),
        this.serviceList.obterIndRepercussaoProcessoAsync(),
        this.serviceList.obterTipoRegimeTrabalhistaAsync(),
      ]);

      if (!tipoRegimePrevidenciarioList || tipoRegimePrevidenciarioList.length === 0) {
        throw new Error('Não foi possível obter a lista de tipos de regime previdenciário.');
      }
      if (!tipoContratoTempoParcialList || tipoContratoTempoParcialList.length === 0) {
        throw new Error('Não foi possível obter a lista de tipos de contrato de trabalho parcial.');
      }
      if (!motivoDesligamentoList || motivoDesligamentoList.length === 0) {
        throw new Error('Não foi possível obter a lista de motivos de desligamento.');
      }
      if (!repercussaoProcessoList || repercussaoProcessoList.length === 0) {
        throw new Error('Não foi possível obter a lista de repercussões de processos.');
      }
      if (!tipoRegimeTrabalhistaList || tipoRegimeTrabalhistaList.length === 0) {
        throw new Error('Não foi possível obter a lista de tipos de regime trabalhista.');
      }

      this.tipoRegimePrevidenciarioList = tipoRegimePrevidenciarioList.map(item =>
        {
          return ({
            id : item.id,
            descricao : item.descricao,
            descricaoConcatenada : `${item.id } - ${item.descricao}`
            })
        });;
      this.tipoContratoTempoParcialList = tipoContratoTempoParcialList.map(item =>
        {
          return ({
            id : item.id,
            descricao : item.descricao,
            descricaoConcatenada : `${item.id } - ${item.descricao}`
            })
        });;
      this.motivoDesligamentoList = motivoDesligamentoList.map(item =>
        {
          return ({
            id : item.id,
            descricao : item.descricao,
            descricaoConcatenada : `${item.id } - ${item.descricao}`
            })
        });
      this.repercussaoProcessoList = repercussaoProcessoList.map(item =>
        {
          return ({
            id : item.id,
            descricao : item.descricao,
            descricaoConcatenada : `${item.id } - ${item.descricao}`
            })
        });
      this.tipoRegimeTrabalhistaList = tipoRegimeTrabalhistaList.map(item =>
        {
          return ({
            id : item.id,
            descricao : item.descricao,
            descricaoConcatenada : `${item.id } - ${item.descricao}`
            })
        });;

    } catch (error) {
      this.dialog.err('Erro ao Buscar', error.message);
    }
  }

  //#endregion

  //#endregion

  //#endregion

}
