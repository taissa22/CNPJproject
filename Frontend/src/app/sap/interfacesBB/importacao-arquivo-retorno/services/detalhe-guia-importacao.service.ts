import { Injectable } from '@angular/core';
import { ArquivoImportacaodto } from '@shared/interfaces/arquivo-Importacao-dto';
import { BBResumoProcessamentoService } from 'src/app/core/services/sap/bbresumo-processamento.service';
import { DownloadService } from 'src/app/core/services/sap/download.service';
import { ResultadoArquivoRetornoService } from './resultado-arquivo-retorno.service';
import { concatenarNomeExportacao } from '@shared/utils';

@Injectable({
  providedIn: 'root'
})
export class DetalheGuiaImportacaoService {

  constructor(private service: BBResumoProcessamentoService,
    private resultadoArquivoService: ResultadoArquivoRetornoService,
    ) { }



  guiasOk() {
    return this.service.buscarGuiasOk
      (this.resultadoArquivoService.importacaoSelecionadoSubject.value['numeroLoteBB']);

  }

  get dadosArquivo(): ArquivoImportacaodto {
    return this.resultadoArquivoService.importacaoSelecionadoSubject.value
  }

  exportar(numeroLoteBB) {
    this.service.exportar(numeroLoteBB);
  }

  

}
