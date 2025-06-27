import { ApiService } from '../api.service';
import { Injectable } from '@angular/core';
import { formatDate } from '@angular/common';
import { concatenarNomeExportacao } from '@shared/utils';
import { HelperAngular } from '@shared/helpers/helper-angular';

@Injectable({
  providedIn: 'root'
})
export class DownloadService {
  constructor(private apiService: ApiService, private messageService: HelperAngular) { }

  baixarExportacaoLotes(json, idTipoProcesso) {
    return this.apiService.post('/Lotes/ExportarLote?codigoTipoProcesso=' + idTipoProcesso, json)
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, concatenarNomeExportacao('Consulta_de_Lotes'));
      });
  }

  baixarExportacaoGuiasOk(numeroLoteBB) {
    return this.apiService.get('/BBResumoProcessamento/ExportarGuias?numeroLoteBB=' + numeroLoteBB)
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, concatenarNomeExportacao('RetornoBB_GuiasOk'));
      });
  }

  baixarExportacaoFormaPagamento(json) {
    return this.apiService.post('/FormaPagamento/ExportarFormaPagamento', json).toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, 'Formas de Pagamento.csv');
      });
  }
  baixarExportacaoArquivoRetorno(json) {
    return this.apiService.post('/BBResumoProcessamento/ExportarArquivoRetorno', json).toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, concatenarNomeExportacao('Importacao_Arquivo_RetornoBB'));
      });
  }

  converterBase64ParaBuffer(base64) {
    const binaryString = window.atob(base64);
    const bytes = new Uint8Array(binaryString.length);
    for (let indice = 0; indice < bytes.length; indice++) {
      bytes[indice] = binaryString.charCodeAt(indice);
    }
    return bytes;
  }

  prepararDownload(buffer, nomeDoArquivo) {
    const blob = new Blob([buffer]);
    if (navigator.msSaveBlob) {
      navigator.msSaveBlob(blob, nomeDoArquivo);
      return;
    }
    const link = document.createElement('a');
    if (link.download !== undefined) {
      link.setAttribute('href', URL.createObjectURL(blob));
      link.setAttribute('download', nomeDoArquivo);
      link.style.visibility = 'hidden';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  }


  baixarExportacaoLancamentos(codigoLote: number, tipoProcesso: number) {
    return this.apiService.get('/LancamentoProcesso/ExportarLancamentoDoLote?codigoLote=' +
      codigoLote + '&codigoTipoProcesso=' + tipoProcesso)
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, concatenarNomeExportacao('Consulta_de_Lotes_Lançamentos'));
      });
  }


  baixarExportacaoFornecedores(json) {
    return this.apiService.post('/Fornecedor/ExportarFornecedores', json)
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, concatenarNomeExportacao('Fornecedores'));
      });
  }

  baixarExportacaoEmpresaSap() {
    return this.apiService.get('/EmpresasSap/ExportarEmpresas')
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, concatenarNomeExportacao('Empresa_do_Sap'));
      });
  }

  concatenarNomeExtportacao(origem: string) {
    return origem + '_' + formatDate(new Date(), 'yyyyMMdd_HHmmss', 'pt_BR') + '.csv';
  }

  concatenarNomeExtportacaoPdf(origem: string) {
    return origem + '_' + formatDate(new Date(), 'yyyyMMdd_HHmmss', 'pt_BR') + '.pdf';
  }

  baixarExportacaoCentroCustoSap() {
    return this.apiService.get('/CentroCusto/ExportarCentroCusto')
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, this.concatenarNomeExtportacao('Centros_de_Custo'));
      });
  }

  baixarExportacaoComarcaBB() {
    return this.apiService.get('/BBComarca/ExportarBBComarca')
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, this.concatenarNomeExtportacao('Comarca_BB'));
      });
  }

  baixarExportacaoCategoriaPagamento(json, nome) {
    return this.apiService.post('/CategoriaPagamento/ExportarCategoriaPagamento', json)
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, this.concatenarNomeExtportacao(nome));
      });
  }

  baixarExportacaoAgenciaTrabalhista(json, nome) {
    return this.apiService.post('/AudienciaProcesso/exportar', json)
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, this.concatenarNomeExtportacao(nome));
      });
  }

  baixarExportacaoCompletaAudienciaTrabalhista(json, nome) {
    return this.apiService.post('/AudienciaProcesso/exportarCompleto', json)
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, this.concatenarNomeExtportacao(nome));
      });
  }  
  

  baixarImpressaoAgenciaTrabalhista(json, nome) {
    return this.apiService.post('/AudienciaProcesso/download', json)
      .toPromise()
      .then(resultado => {
        const buffer = this.converterBase64ParaBuffer(resultado.data);
        this.prepararDownload(buffer, this.concatenarNomeExtportacaoPdf(nome));
      });
  }
}


