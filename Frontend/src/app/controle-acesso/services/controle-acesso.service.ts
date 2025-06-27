import { DownloadService } from './../../core/services/sap/download.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormBuilder } from '@angular/forms';

import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

import { LogUsuarioAgendamento } from '../models/log-usuario-agendamento';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class ControleAcessoService {

  backEndURL: any;

  constructor(
    protected http: HttpClient,
    private fb: FormBuilder,
    private exportarService: TransferenciaArquivos
  ) { }

  listarLogsUsuario(usuario, total, dtInicio, dtFim, tipoProcesso): Observable<any> {
    const backEndURL = environment.api_url + '/LogProcesso/LogUsuario';

    let params = new HttpParams().set('usuario', usuario)
                                 .set('total', total)
                                 .set('dataInicio', dtInicio)
                                 .set('dataFim', dtFim)
                                 .set('codigoTipoProcesso', tipoProcesso ? tipoProcesso : 0 );

    return this.http.get<any>(backEndURL, { params: params });
  }

  listarUsuarioLogado(): Observable<any> {
    const backEndURL = environment.api_url + '/LogProcesso/listarUsuarioLogado';
    return this.http.get<any>(backEndURL);
  }

  listarUsuarioPesquisado(codigoUsuario): Observable<any> {
    const backEndURL = environment.api_url + '/LogProcesso/listarUsuarioPesquisado';

    let params = new HttpParams().set('codigoUsuario', codigoUsuario)
    return this.http.get<any>(backEndURL, { params: params });
  }

  listarLogsAgendar(pagina,qtd): Observable<any> {
    const backEndURL = environment.api_url + '/LogUsuarioAgendamento/listar/';

    let params = new HttpParams().set('pagina', pagina)
                                 .set('qtd', qtd)
    return this.http.get<any>(backEndURL, { params: params });
  }

  apagarAgendamento(id): Observable<any> {
    let backEndURL = environment.api_url + '/LogUsuarioAgendamento/delete';
    let params = new HttpParams().set('id', id)
    return this.http.delete(backEndURL, { params: params });
  }

  public agendar(logUsuarioAgendamento : LogUsuarioAgendamento): Observable<any> {
    let backEndURL = environment.api_url + '/LogUsuarioAgendamento/inserir/';
    return this.http.post<any>(backEndURL, logUsuarioAgendamento);
  }


  async baixarAgendamento(agendamentoId: number): Promise<void> {
    const backEndURL = `${environment.api_url}/LogUsuarioAgendamento/download/${agendamentoId}`;

    const response: any = await this.http.get<any>(backEndURL, this.exportarService.downloadOptions).toPromise();
    const file = response.body;
    const fileName = this.exportarService.obterNomeArquivo(response);
    this.exportarService.download(file, fileName);
  }
}
