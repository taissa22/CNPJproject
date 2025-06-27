/** O criador deste código não está mais entre nós
 * Cuide deste código com carinho
 *
 *         _.---,._,'
       /' _.--.<
         /'     `'
       /' _.---._____
       \.'   ___, .-'`
           /'    \\             .
         /'       `-.          -|-
        |                       |
        |                   .-'~~~`-.
        |                 .'         `.
        |                 |  R  I  P  |
  ADS   |                 |           |
        |                 |           |
         \              \\|           |//
   ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

   04/09/2020
 */

import { HelperAngular } from '@shared/helpers/helper-angular';
import { OrdenacaoData } from '@shared/interfaces/ordenacao-data';
import { take } from 'rxjs/operators';
import { IManutencaoCommonService } from '../interfaces/imanutencao-common-service';
import { ordenateHeader } from '@shared/utils';

interface buscarModel {
  codTipoProcesso?: number;
  dataInicio: Date;
  dataFim: Date
}

export class ManutencaoCommonComponent {

  listaHeaders: OrdenacaoData[];
  headersToRemove = [];
  ordemColunas = [];
  data: any[];

  filtroBuscar: buscarModel = {
    codTipoProcesso: null,
    dataInicio: null,
    dataFim: null
  }

  constructor(protected service: IManutencaoCommonService,
    protected messageService = new HelperAngular()) { }

  /**
   * Ao sobrescrever isto, sobrescreva onChangeOrdenacao
   */
  protected setData(data: any[], isRefresh = false) {
    (data.length == 0 || !data) ? this.isNotFound = true : this.isNotFound = false;

    if (Array.isArray(data) && data.length > 0) {
      if (!isRefresh) {
         let t = Object.keys(data[0])
         .filter(e => !this.headersToRemove.includes(e))
         this.ordemColunas.length > 0 ? t = ordenateHeader(t, this.ordemColunas) : t;



        this.listaHeaders = t
          .map(e => ({ campo: e, isActive: false, isAscendente: false }));

      }
      this.data = data;
    } else {
      this.listaHeaders = [];
      this.data = [];

    }
  }

  protected setViewOrdenacao(campo, args?: any) {
    this.listaHeaders && this.listaHeaders.forEach(e => {
      if (e.campo == campo) {
        e.isActive = true;
        e.isAscendente = !e.isAscendente;
      } else {
        e.isAscendente = false;
        e.isActive = false;
      }
    });
  }
  isNotFound = false;
  defineModel(args?: any, nomeOrdenacaoColuna?: string) {
    this.filtroBuscar
    this.service.refreshFiltros(args);
    if (args.hasOwnProperty('ordenacao')) {
      this.setViewOrdenacao(args['ordenacao']);
    }
    this.service.consultarPorFiltros(args)
      .pipe(take(1))
      .subscribe(response =>{
        response && this.setData(response.data);
        (response.data.length == 0 || !response.data) ? this.isNotFound = true : this.isNotFound = false;
      });
  }

  refresh(args?: any, nomeOrdenacaoColuna?: string) {
    this.service.refreshFiltroSemQuantidade(args);
    if (args.hasOwnProperty('ordenacao')) {
      this.setViewOrdenacao(args['ordenacao']);
    }
    this.service.consultarPorFiltros(args)
      .pipe(take(1))
      .subscribe(response =>{
        response && this.setData(response.data);
        (response.data.length == 0 || !response.data) ? this.isNotFound = true : this.isNotFound = false;
      });
  }

  onChangePagina(pagina: number) {
    this.service.trocarPagina(pagina)
      .pipe(take(1))
      .subscribe(response => this.setData(response.data, true));
  }

  onChangeQuantidade(quantidade: number) {
    this.service.trocarQuantidade(quantidade)
      .pipe(take(1))
      .subscribe(response =>{
        this.setData(response.data, true)
      });
  }

  onChangeOrdenacao(data: OrdenacaoData) {
    this.setViewOrdenacao(data.campo);
    this.service.ordenar(data.campo, data.isAscendente)
      .pipe(take(1))
      .subscribe(response => this.setData(response.data, true));
  }

  onClickExportar(nomeArquivo: string) {
    this.service.exportar(nomeArquivo);
  }

  onClickExportarPorFiltro(nomeArquivo: string, json: any) {
    this.service.exportarPorFiltro(nomeArquivo, json);
  }

  onClickExcluir(codTipoProcesso, nomeItem = 'item', artigo = 'o', description: string) {
    this.messageService.MsgBox2(`Deseja excluir ${artigo} ${nomeItem}
    <br> <b>${description}</b>?`,
      `Excluir ${nomeItem}`, 'question', 'Sim', 'Não').then(
        confirm => {
          if (confirm.value) {
            this.service.excluir(codTipoProcesso).pipe(take(1)).subscribe(
              response => response.then(
                responseObject => {
                  if (responseObject.excluir['sucesso'])
                    this.setData(responseObject.consultar.data, true);
                  else if (!responseObject.excluir['sucesso'] && responseObject.excluir['exibeNotificacao']) {
                    this.messageService.MsgBox2(responseObject.excluir['mensagem'], 'Exclusão não permitida',
                      'warning', 'Ok')
                  }
                  else
                    this.messageService.MsgBox(responseObject.excluir['mensagem'], 'Ops!');
                })
            );
          }
        }
      )
  }
}
