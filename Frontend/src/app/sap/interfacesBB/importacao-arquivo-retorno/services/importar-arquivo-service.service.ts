import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';
import { DownloadService } from 'src/app/core/services/sap/download.service';
import { concatenarNomeExportacao } from '@shared/utils';
import { BBResumoProcessamentoService } from 'src/app/core/services/sap/bbresumo-processamento.service';
import { ArquivoImportacao } from '../interfaces/arquivo-importacao';

@Injectable({
  providedIn: 'root'
})
export class ImportarArquivoService {

  constructor(private downloadService: DownloadService,
    private bbresumoService: BBResumoProcessamentoService) { }

  public arquivosImportacao$ = new BehaviorSubject<ArquivoImportacao[]>([]);

/**@description exportar guias ok
 * @param data byte das guias ok
 */
  exportarGuiasOk(data) {
    const buffer = this.downloadService.converterBase64ParaBuffer(data);
    return this.downloadService.prepararDownload(buffer, concatenarNomeExportacao('RetornoBB_GuiasOk'));
  }
/** @description exportar guias com problema
 * @param data byte das guias com problema
 */
  exportarGuiasNaoOk(data) {
    const buffer = this.downloadService.converterBase64ParaBuffer(data);
    return this.downloadService.prepararDownload(buffer, concatenarNomeExportacao('RetornoBB_GuiasProblemas'));
  }

  /**@description salvar a importação (endpoint)
   * @param valores arquivos a serem salvos
   */
  salvarImportacao(valores) {


    return this.bbresumoService.salvarImportacao(valores)


  }
}
