import { ArquivoImportacaodto } from '@shared/interfaces/arquivo-Importacao-dto';
import { DownloadService } from 'src/app/core/services/sap/download.service';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BBResumoProcessamentoService } from 'src/app/core/services/sap/bbresumo-processamento.service';
import { ConsultaArquivoRetornoFiltroDTO } from '@shared/interfaces/consulta-arquivo-retorno-filtro-dto';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ResultadoArquivoRetornoService {

  resultadoSubject = new BehaviorSubject<any>([]);
  filtrosUtilizadosSubject$ = new BehaviorSubject<ConsultaArquivoRetornoFiltroDTO>({
    pagina: 1,
    quantidade: 8,
    total: 0
  });

  importacaoSelecionadoSubject = new BehaviorSubject<ArquivoImportacaodto>(null)

  constructor(private service: BBResumoProcessamentoService,
  private downlaodService: DownloadService) { }

  limparResultados() {
    this.resultadoSubject.next([]);
  }

  verMaisArquivoRetorno() : void {
    let currentFiltros = { ...this.filtrosUtilizadosSubject$.value };
    currentFiltros.pagina++;
    this.filtrosUtilizadosSubject$.next(currentFiltros);
    this.service.consultarArquivoRetorno(currentFiltros).pipe(take(1))
                .subscribe(response => {
                  this.filtrosUtilizadosSubject$.next({
                    ...this.filtrosUtilizadosSubject$.value,
                    total: response['total']
                  });
                  this.appendToResultado(response['data']);
                });
  }

  appendToResultado(data: any[]): void {
    this.resultadoSubject.next([
      ...this.resultadoSubject.value,
      ...data
    ])
  }

  exportarArquivoRetorno() {
    let currentFiltros = { ...this.filtrosUtilizadosSubject$.value };
    this.downlaodService.baixarExportacaoArquivoRetorno(currentFiltros);
  }



}
