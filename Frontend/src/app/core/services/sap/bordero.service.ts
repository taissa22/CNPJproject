import { ApiService } from './../api.service';
import { Injectable } from '@angular/core';


import { Observable } from 'rxjs';
import { map, pluck } from 'rxjs/operators';
import { BorderoModel } from '../../models/bordero.model';
import { DownloadService } from './download.service';
import { concatenarNomeExportacao } from '@shared/utils';

@Injectable({
  providedIn: 'root'
})


// colunasGrid: any[];


export class BorderoService {

  public colunasGrid: any[];
  public tituloDetalhamento: string;


  constructor(private apiService: ApiService,
              private downloadService: DownloadService) {
    this.tituloDetalhamento = 'BORDERÔ';
    this.colunasGrid = [
      'Seq',
      'Beneficiário',
      'CPF',
      'CNPJ',
      'Banco',
      'DV',
      'Agência',
      'DV',
      'Nº C/C',
      'DV',
      'Valor',
      'Cidade',
      'Histórico'];




  }


  getBordero(numeroLote: any): Observable<BorderoModel[]> {
    return this.apiService.get('/Bordero/ObterBordero?CodigoLote=' + numeroLote).pipe(pluck('data'));
  }

  baixarExportacaoBordero(codigoLote: number, tipoProcesso: number) {
    return this.apiService.get('/Bordero/ExportarBorderoDoLote?codigoLote=' +
      codigoLote + '&codigoTipoProcesso=' + tipoProcesso)
      .toPromise()
      .then(resultado => {
        const buffer = this.downloadService.converterBase64ParaBuffer(resultado.data);
        this.downloadService.prepararDownload(buffer, concatenarNomeExportacao('Consulta_de_Lotes_Bordero'));
      });
  }

}
