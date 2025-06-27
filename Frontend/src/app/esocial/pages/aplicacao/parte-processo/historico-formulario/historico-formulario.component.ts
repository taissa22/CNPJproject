import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Formulario2500 } from '@esocial/models/formulario-2500';
import { Formulario2501 } from '@esocial/models/formulario-2501';
import { RetornoLista } from '@esocial/models/retorno-lista';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-historico-formulario',
  templateUrl: './historico-formulario.component.html',
  styleUrls: ['./historico-formulario.component.scss']
})
export class HistoricoFormularioComponent implements OnInit {
  @Input() formulario2500: Array<Formulario2500>;
  @Input() formulario2501: Array<Formulario2501>;
  @Output() aoConsultarFormulario2500 = new EventEmitter<Formulario2500>();
  @Output() aoConsultarFormulario2501 = new EventEmitter<Formulario2501>();

  statusFormularioList: Array<RetornoLista> = null;

  constructor(private formularioService: ESocialListaFormularioService, private dialogService: DialogService) { }

  async ngOnInit(): Promise<void> {
    await this.setStatusEsocial();
  }

  //#region FUNÇÕES

  async setStatusEsocial() {
    try {
      const resposta = await this.formularioService.obterStatusFormularioAsync();
      if (resposta) {
        this.statusFormularioList = resposta.map<RetornoLista>((retorno: RetornoLista) : RetornoLista => {return RetornoLista.fromObj(retorno)});
      }
      else {
        await this.dialogService.err(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
        );
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialogService.err('Informações não carregadas', mensagem);
    }
  }

  convertEnumFormulario(f2500: Formulario2500): string {

    let {statusFormulario : status, dataRetornoOK: dataRetorno, dataRetornoExclusao,indRetificado, idF2500} = f2500;
    let descricao = '';

    const dataRetonroArray = dataRetorno.toString().split('T');
    const dataRetornoOk = `${dataRetonroArray[0].split("-").reverse().join("/")} ${dataRetonroArray[1].split(".")[0]}`;

    const dataRetonroExclusaoArray = dataRetornoExclusao.toString().split('T');
    const dataRetornoExclusaoOK = `${dataRetonroExclusaoArray[0].split("-").reverse().join("/")} ${dataRetonroExclusaoArray[1].split(".")[0]}`;

    switch (status) {
      case 4:
        descricao = indRetificado ? `Retificação (ID ${idF2500}). Retornado OK em ${dataRetornoOk}` : `Formulário Original (ID ${idF2500}). Retornado Ok em ${dataRetornoOk}`;
        break;
      case 12:
        descricao = indRetificado ? `Retificação (ID ${idF2500}). Retornado OK em ${dataRetornoOk}. Excluído em ${dataRetornoExclusaoOK}` : `Formulário Original (ID ${idF2500}). Retornado Ok em ${dataRetornoOk}. Excluído em ${dataRetornoExclusaoOK}`;
        break;
      default:
        descricao = '';
        break;
    }

    return descricao;
  }

  convertEnumFormulario2501(f2501: Formulario2501): string {
    let descricao = '';

    const {statusFormulario : status, idF2501, indRetificado = false} = f2501;

    switch (status) {

      case 4:
        descricao = indRetificado ? `Retificação (ID ${idF2501}). Retornado OK` : `Formulário Original (ID ${idF2501}). Retornado Ok`;
        break;
      case 12:
        descricao = indRetificado ? `Retificação (ID ${idF2501}). Retornado OK. Excluído` : `Formulário Original (ID ${idF2501}). Retornado Ok. Excluído`;
        break;
      default:
        descricao = '';
        break;
    }

    return descricao;
  }


  consultarFormulario2500(f2500: Formulario2500) {
    this.aoConsultarFormulario2500.emit(f2500);
  }

  consultarFormulario2501(f2501: Formulario2501) {
    this.aoConsultarFormulario2501.emit(f2501);
  }
  //#endregion

}
