import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { Combobox } from '@shared/interfaces/combobox';
import { GuiasProblema } from '@shared/interfaces/guias-problema';
import { GuiasOK } from '@shared/interfaces/GuiasOK';
import { ResumoImportacaoAquivo } from '@shared/interfaces/resumo-importacao-aquivo';
import { ordenateHeader } from '@shared/utils';
import { BehaviorSubject, Subscription } from 'rxjs';
import { take } from 'rxjs/operators';
import { excluirHeader, ordemGuiasProblema } from '../importacao-arquivo-retorno.constants';
import { ArquivoImportacao } from '../interfaces/arquivo-importacao';
import { ImportarArquivoService } from '../services/importar-arquivo-service.service';

@Component({
  selector: 'app-importar-arquivo',
  templateUrl: './importar-arquivo.component.html',
  styleUrls: ['./importar-arquivo.component.scss']
})
export class ImportarArquivoComponent implements OnInit, OnDestroy {

  constructor(private service: ImportarArquivoService, private messageService: HelperAngular, private router: Router) { }

  //#region informações da tela
  dadosArquivo: ResumoImportacaoAquivo;
  guiasOK: { guias: GuiasOK[], guiasHeaders: string[], exportar: string }
    = { guias: [], guiasHeaders: [], exportar: null }
  guiasComProblema: { guias: GuiasProblema[], guiasHeaders: string[], exportar: string } =
    { guias: [], guiasHeaders: [], exportar: null }
  //#endregion

  //#region Informações complementares para a Combobox
  /**@description index da combo escolhido */
  public indexTab$ = new BehaviorSubject<number>(0);
  /** @description informações do arquivo da combo escolhido */
  private tabData: ArquivoImportacao[] = [];
  /**@description Adicionar o nome dos arquivos para escolha na combobox */
  dadosCombo: Combobox[] = [];
  //#endregion

  private subscription$: Subscription;

  comboArquivos = new FormControl();
  formGroup: FormGroup = new FormGroup({
    decricao : this.comboArquivos}
  );

  ngOnInit(): void {
    this.subscription$ = this.service.arquivosImportacao$.subscribe((arquivos: ArquivoImportacao[]) => {
      this.tabData = arquivos;   
      this.indexTab$.next(0);   
      this.dadosCombo = arquivos.map((e, index) => ({
        id: index,
        descricao: e.nomeArquivo,
      }));
      this.comboArquivos.setValue(0);
    });
    //#region trocar a aba de acordo com o arquivo escolhido
    this.subscription$.add(this.indexTab$.subscribe((index) => {

      let item = this.tabData[index];
      this.dadosArquivo = item['resumo'];
      if (item.guiasOk.length > 0 && item['guiasOk']) {
        //arquivo com as guias ok
        this.guiasOK.guias = item['guiasOk']
        //string com os nomes das colunas filtrando somente os que devem aparecer
        this.guiasOK.guiasHeaders = Object.keys(item['guiasOk'][0]).filter(item => !excluirHeader.includes(item))
        this.guiasOK.exportar = item['arquivoGuiasOk']
      } else this.guiasOK.guias = [];
      if (item.guiasComProblema.length > 0 && item['guiasComProblema']) {
        this.guiasComProblema.guias = item['guiasComProblema']
        this.guiasComProblema.guiasHeaders = Object.keys(item['guiasComProblema'][0])
        this.guiasComProblema.guiasHeaders = ordenateHeader(this.guiasComProblema.guiasHeaders, ordemGuiasProblema)
        this.guiasComProblema.exportar = item['arquivoGuiasNaoOk']
      } else this.guiasComProblema.guias = [];
    }));
    //#endregion
  }

  /** @description Cancelar TODAS as importações */
  cancelar(): void {
    this.messageService.MsgBox2('Essa operação irá cancelar todas as importações. <br>Deseja continuar?', 'Atenção!',
      'warning', 'Sim', 'Não').then(resposta => {
        if (resposta.value) {
          this.subscription$.unsubscribe();
          window.history.back();
        }
      });
  }


  /** @description Verificar o index do arquivo escolhido */
  trocarAba(index): void {
    this.indexTab$.next(index);

  }

  ngOnDestroy(): void {
    this.subscription$.unsubscribe();
  }

  /** @description Exportar arquivos de guias ok ou não ok
   * @param isGuiaOK booleano para diferenciar qual arquivo deve ser exportado
   */
  exportar(isGuiaOK: boolean): void {
    if (isGuiaOK) {
      this.service.exportarGuiasOk(this.guiasOK.exportar)
    } else {
      this.service.exportarGuiasNaoOk(this.guiasComProblema.exportar)
    }
  }

  //#region salvar arquivos
  /**@description salvar todas as importações
   * @validations somente arquivos que não possuem guias com problemas serão salvos e aqueles
   * que não forem importados será informado o motivo do problema.
   */
  salvarTodos(): void {
    let valores = this.tabData.map(e => ({
      guiasOk: e.guiasOk,
      guiasComProblema: e.guiasComProblema,
      resumo: e.resumo,
      nomeArquivo: e.nomeArquivo
    }));



    const podeImportar = valores.filter(e => e.guiasComProblema.length == 0);
    // Verificar se só existem arquivos com problemas
    if (!(podeImportar.length > 0)) {
      this.messageService.MsgBox2(`Não é permitido salvar a importação quando
                          apenas houverem guias com problemas.`, 'Operação não permitida', 'warning', 'Ok');
      return
    }
    const naoPodeImportar = valores.filter(e => e.guiasComProblema.length > 0);
    const nomesArquivosSemImportar = naoPodeImportar.map(e => e.nomeArquivo);

    //Verificar qual arquivo possui problema e informar ao usuário
    if (naoPodeImportar.length > 0 && (podeImportar.length > 0)) {
      this.messageService.MsgBox2(this.montarMensagem(nomesArquivosSemImportar) + `:<br>${nomesArquivosSemImportar.join('<br/>')}<br><br>Deseja continuar?`,
        'Atenção!', 'warning', 'Sim', 'Não').then(
          e => e.value && this.salvarImportacaoNaAPI(valores)
        );
    }
    else {
      // se so tiver arquivos com guias ok
      this.salvarImportacaoNaAPI(valores, false)
    }
  }

  /**@description Salvar somente um arquivo
   * @validation Só será salvo se o arquivo não possuir guia com problema
   */
  salvar(): void {
    if (this.guiasComProblema.guias.length <= 0 || !this.guiasComProblema.guias) {
      let valores = [{
        guiasOk: this.guiasOK.guias,
        resumo: this.dadosArquivo,
        guiasComProblema: this.guiasComProblema.guias,

      }];
      this.salvarImportacaoNaAPI(valores, true);
    } else {
      this.messageService.MsgBox2(`Não é permitido salvar a importação quando
    houverem guias com problemas.`, 'Operação não permitida', 'warning', 'Ok');
    }
  }

  montarMensagem(nomeArquivo: Array<string>) {
    if (nomeArquivo.length > 1)
      return '<br> Os arquivos abaixo não podem ser importados, pois possuem guias com problemas';

    return '<br> O arquivo abaixo não pode ser importado, pois possui guias com problemas'

  }

  monstarMensagemOk(nomeArquivo: Array<string>) {
    if (nomeArquivo.length > 1)
      return 'Arquivos importados com sucesso.';
    return 'Arquivo importado com sucesso.';
  }

  /** @description método para salvar no banco os valores após passar pelas validações
   * @param dados arquivos a serem salvos
   * @param salvarUmSo verifica se é para salvar um só arquivo
   */
  private salvarImportacaoNaAPI(dados, salvarUmSo?): void {
    const dadosAPI = [...dados].filter(e => e.guiasComProblema.length == 0);

    this.service.salvarImportacao(dadosAPI).pipe(take(1)).subscribe(
      response => {
        const nomeArquivosOk = dados.filter(val => val.guiasOk.length > 0)
          .map(e => e.nomeArquivo);
        const nomeArquivos = dados.filter(val => val.guiasComProblema.length > 0)
          .map(e => e.nomeArquivo);

        if (dados.some(e => e.guiasComProblema.length > 0) ) {
          this.messageService.MsgBox2(nomeArquivos.join('<br/>'),
            this.monstarMensagemOk(nomeArquivosOk) +
            this.montarMensagem(nomeArquivos), 'warning', 'Ok').then(
              e => e.value && this.router.navigate(['sap/interfaceBB/importacao/consulta/criteriosGeraisGuia'])
            );
        }
        else if (response['sucesso'] && salvarUmSo) {

          const nomeArquivosOk = dados.filter(val => val.guiasOk.length > 0)
            .map(e => e.nomeArquivo);
          const nomeArquivos = dados.filter(val => val.guiasComProblema.length > 0)
            .map(e => e.nomeArquivo);
          let indexTab = 0;
          while (this.tabData.some(e => e.guiasComProblema.length == 0)) {
            if (this.tabData[indexTab].guiasComProblema.length == 0) {
              this.dadosCombo.splice(indexTab, 1);
              this.tabData.splice(indexTab, 1);
              indexTab = 0;
              continue;
            }
            indexTab++;
          }

          this.messageService.MsgBox2(this.monstarMensagemOk(nomeArquivosOk),
            "Sucesso!"
            , 'success', 'Ok');

          this.dadosCombo = this.dadosCombo.map((e, index) => {
            e.id = index;
            return e;
          });

          if (this.dadosCombo.length > 0)
            this.trocarAba(0);
            else {
              this.messageService.MsgBox2('Todas as importações foram salvas com sucesso!',
                'Sucesso!', 'success', 'Ok').then(
                    e => e.value &&  this.router.navigate(['sap/interfaceBB/importacao/consulta/criteriosGeraisGuia'])
                );


            }
        }
        else if (response['sucesso'] && !dados.some(e => e.guiasComProblema.length > 0) && !salvarUmSo) {
          this.messageService.MsgBox2('Todas as importações foram salvas com sucesso!',
          'Sucesso!', 'success', 'Ok').then(
              e => e.value &&  this.router.navigate(['sap/interfaceBB/importacao/consulta/criteriosGeraisGuia']));
        }
        else this.messageService.MsgBox2(response['mensagem'], 'Ocorreu um erro durante a importação', 'warning', 'Ok');
      }
    );
  }
  //#endregion
}



