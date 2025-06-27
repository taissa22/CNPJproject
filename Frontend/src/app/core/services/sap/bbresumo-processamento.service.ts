import { DownloadService } from 'src/app/core/services/sap/download.service';
import { Injectable } from '@angular/core';
import { ApiService } from '..';
import { ConsultaArquivoRetornoFiltroDTO } from '@shared/interfaces/consulta-arquivo-retorno-filtro-dto';
import { of, Observable } from 'rxjs';
import { BBResumoProcessamentoGuiaExibidaDTO } from '@shared/interfaces/BB-Resumo-Processamento-Guia-Exibida-DTO';

@Injectable({
  providedIn: 'root'
})
export class BBResumoProcessamentoService {

  endpoint = '/BBResumoProcessamento'

  constructor(private http: ApiService,
  private downloadService: DownloadService) { }

  consultarArquivoRetorno(filtros: ConsultaArquivoRetornoFiltroDTO) {
    return this.http.post(`${this.endpoint}/ConsultarArquivoRetorno`, filtros);
  }

  buscarGuiasOk(numeroLotebb) {
    return this.http.get(this.endpoint + '/BuscarGuias?numeroLoteBB=' + numeroLotebb);
  }

  exportar(numeroLoteBB) {
  return this.downloadService.baixarExportacaoGuiasOk(numeroLoteBB);
  }

  buscarImagemGuia(codigoProcesso, codigoLancamento) : Observable<BBResumoProcessamentoGuiaExibidaDTO> {
    return this.http.get(`/BBResumoProcessamento/BuscarGuiaExibicao?codigoProcesso=` + codigoProcesso+ '&codigoLancamento=' + codigoLancamento)
  }

  salvarImportacao(valores){
    return this.http.post('/BBResumoProcessamento/SalvarImportacao', valores);
  }

  /** Pegar parametro juridico de maximo de arquivos de uploud e a quantidade de MB limite*/
  getparametroJuridico(){
    return this.http.get(this.endpoint + '/ConsultarParametrosUpload');
  }
}
