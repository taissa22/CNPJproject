import { Component, OnInit, OnDestroy } from '@angular/core';
import { ConsultaArquivoRetornoService } from '../services/consulta-arquivo-retorno.service';
import { Subscription } from 'rxjs';
import { FiltroModel } from 'src/app/core/models/filtro.model';
import { ActivatedRoute, Router } from '@angular/router';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { toUpperFirstLetter, fileListToArray } from '@shared/utils';
import { role, UM_MEGABYTE } from 'src/app/sap/sap.constants'
import { ImportarArquivoService } from '../services/importar-arquivo-service.service';
import { take } from 'rxjs/operators';
import { CriteriosImportacaoArquivoRetornoService } from 'src/app/sap/filtros/criterios-importacao-arquivo-retorno/services/criterios-importacao-arquivo-retorno.service'
import { APIResponse } from 'src/app/sap/shared/interfaces/apiresponse';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  selector: 'app-consulta-arquivo-retorno',
  templateUrl: './consulta-arquivo-retorno.component.html',
  styleUrls: ['./consulta-arquivo-retorno.component.scss']
})
export class ConsultaArquivoRetornoComponent implements OnInit, OnDestroy {
  breadcrumb: string;

  constructor(private service: ConsultaArquivoRetornoService,
    private criteriosGuiaService: CriteriosImportacaoArquivoRetornoService,
    private route: ActivatedRoute,
    private messageService: HelperAngular,
    private router: Router,
    private importarArquivo: ImportarArquivoService,
    private breadcrumbsService: BreadcrumbsService) { }

  /**@description Lista de valores das guias a esquerda do filtro. */
  listaFiltro: Array<FiltroModel> = [];
  subscription: Subscription;
  /**
   * @description Verifica se o critérios gerais está valido para liberar o botão de busca.
   */
  isValid = true;

  /**
   * @description Parametro Juridico (SAP_QTD_MAX_ARQUIV_UPLOAD) para o limite de arquivos para uploud da tela.
   * @default - 5 arquivos
   */
  parametroJuridicoMaxUpload: number = 5;
    /**
   * @description Parametro Juridico (SAP_QTD_MAX_SIZE_UPLOAD) para o limite de MB dos arquivos da tela.
   * @default - 4 MB
   */
  parametroJuridicoMaxMB: number = 4;
  ngOnInit() {

    this.service.iniciarSelecionado();
    //#region   Resolver do parametro juridico
    this.subscription = this.route.data.subscribe(info => {
      this.parametroJuridicoMaxUpload = info.parametroJuridico.data.quantidadeMaxArquivosUpload;
      this.parametroJuridicoMaxMB= info.parametroJuridico.data.tamanhoMaxArquivosUpload;

    });
    //#endregion

    //#region subscriptions

    this.subscription.add(this.service.contagemCriteriosMudou$.subscribe(item => {
      this.service.atualizarContagem(item, 1);
    }))
    this.subscription.add(this.service.contagemGuiaMudou$.subscribe(item => {
      this.service.atualizarContagem(item, 2);
    }))
    this.subscription.add(this.service.contagemContaJudicialMudou$.subscribe(item => {
      this.service.atualizarContagem(item, 3);
    }))
    this.subscription.add(this.service.contagemProcessosJEC$.subscribe(
      item => this.service.atualizarContagem(item, 4)
    ))
    
    this.subscription.add(this.service.contagemProcessosCCMudou$.subscribe(
      item => this.service.atualizarContagem(item, 5)
    ))
    this.subscription.add(this.service.contagemProcessosPexMudou$.subscribe(
      item => this.service.atualizarContagem(item, 6)
    ))

    this.subscription.add(this.criteriosGuiaService.isValidSubject.subscribe(
      e => this.isValid = e));

    //#endregion

    this.goToInicio();
    this.listaFiltro = this.service.listaCriterioSelecao;
    this.criteriosGuiaService.limparFormulario();
  }
  
  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuInterfaceImportacaoConsultaArquivoRetorno);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  /** @description verifica se a quantidade de arquivos importadas ultrapassa a quantidade
   * estipulada pelo parametro juridico
   * @param fileList lista dos arquivos importados
   */
  private isValidMaxFilesUpload(fileList: File[]): boolean {
    if (fileList.length > this.parametroJuridicoMaxUpload) {
      return false;
    }
    return true;
  }

  onClickbtn() {
    this.service.consultarArquivoRetorno();
  }

  /** @description Volta para a guia de critérios gerais
  */
  goToInicio(): void {
    this.router.navigate(['criteriosGeraisGuia'], { relativeTo: this.route });
  }

  /** @description Importação dos arquivos
   * @param fileEvent arquivos do form data
   * @validations  A extensão do arquivo não pode ser diferente de .txt,
   * o tamanho não pode ultrapassar ao parametro juridico de tamanho,
   * a quantidade de arquivos não pode ultrapassar o parametro juridico de quantidade.
   * só serão importados arquivos que não ocorram erro,
   * a quantidade de arquivos que poderão ser importadas vem do parametro juridico.
   *
  */
  onFileChange(fileEvent: Event): void {
    /**@param message Apresenta os erros de validação de arquivo  */
    const showErr = (message: string): Promise<any> => this.messageService.MsgBox2(message, 'Desculpe, a importação não poderá ser realizada', 'warning', 'Ok');
    const fileList = fileListToArray(fileEvent.target['files']);
    const errors: string[] = [];
    const quantidadeMB = this.parametroJuridicoMaxMB;

    // Validações
    if (!this.isValidMaxFilesUpload(fileList))
      errors.push('Não podem ser importados mais de ' + this.parametroJuridicoMaxUpload +
        ' arquivos');
    if (fileList.some(() => !fileEvent.srcElement['value'].endsWith('.txt')))
      errors.push('A extensão dos arquivos devem ser do tipo ".txt"');
    if (fileList.some(file => file.size >= (UM_MEGABYTE * quantidadeMB)))
      errors.push(`O tamanho dos arquivos importados ultrapassa o limite permitido de ${quantidadeMB}mb.`);

    // Se tiver com erro, não deixa carregar e mostra mensagem
    if (errors.length > 0) {
      fileEvent.preventDefault();
      fileEvent.srcElement['value'] = null;
      errors[0] = toUpperFirstLetter(errors[0]);
      const errorString = errors.join(';<br><br>');
      showErr(errorString);
      return
    }
    this.service.upload(fileList).subscribe((response: APIResponse) => {
      if (response.sucesso) {
        this.importarArquivo.arquivosImportacao$.next(response.data);
        // Redireciona p/
        // src/app/sap/interfacesBB/importacao-arquivo-retorno/importar-arquivo/importar-arquivo.component.ts
        return this.router.navigate(['sap/interfaceBB/importacao/resultado/importar']);
      }
      fileEvent.preventDefault();
      fileEvent.srcElement['value'] = null;
      showErr(response.mensagem);
    });
  }
}
