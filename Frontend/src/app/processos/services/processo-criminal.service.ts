import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SelectOptionModel } from '../models/criminal/select-option.model';
import { ResultRequestModel } from '../models/criminal/result-request.model';
import { EmpresaProcessoCriminalModel } from '../models/criminal/empresa-processo-criminal.model';
import { ProcessoCriminalModel } from '../models/criminal/processo-criminal.model';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';


@Injectable({
  providedIn: 'root'
})
export class ProcessoCriminalService {

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

   url: string = environment.api_v2_url

   async ObterAcoesAsync(): Promise<ResultRequestModel<Array<SelectOptionModel<number>>>> {
    let newUrl = `${this.url}/Criminal/Acoes`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<number>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

   async ObterOrgaosAsync(): Promise<ResultRequestModel<Array<SelectOptionModel<number>>>> {
    let newUrl = `${this.url}/Criminal/Orgaos`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<number>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }
  async ObterCompetenciasAsync(cod_parte): Promise<ResultRequestModel<Array<SelectOptionModel<number>>>> {
    let newUrl = `${this.url}/Criminal/Competencias?cod_parte=${cod_parte}`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<number>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }
  async ObterAssuntosAsync(ind_criminal_adm,ind_criminal_judicial): Promise<ResultRequestModel<Array<SelectOptionModel<number>>>> {
    let newUrl = `${this.url}/Criminal/Assuntos?ind_criminal_adm=${ind_criminal_adm}&ind_criminal_judicial=${ind_criminal_judicial}`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<number>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }
  async ObterTiposParticipacaoAsync(): Promise<ResultRequestModel<Array<SelectOptionModel<number>>>> {
    let newUrl = `${this.url}/Criminal/TiposParticipacao`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<number>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }
  async ObterTiposProcedimentoAsync(): Promise<ResultRequestModel<Array<SelectOptionModel<number>>>> {
    let newUrl = `${this.url}/Criminal/TiposProcedimento`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<number>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  async ObterEscritoriosAsync(cod_tipo_processo): Promise<ResultRequestModel<Array<SelectOptionModel<number>>>> {
    let newUrl = `${this.url}/Criminal/Escritorios?cod_tipo_processo=${cod_tipo_processo}`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<number>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }
  async ObterEmpresasDoGrupoAsync(cod_tipo_processo): Promise<ResultRequestModel<Array<SelectOptionModel<number>>>> {
    let newUrl = `${this.url}/Criminal/EmpresasDoGrupo?cod_tipo_processo=${cod_tipo_processo}`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<number>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  async ObterEstadosAsync(): Promise<ResultRequestModel<Array<SelectOptionModel<string>>>> {
    let newUrl = `${this.url}/Criminal/Estados`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<string>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  async ObterMunicipiosAsync(cod_estado): Promise<ResultRequestModel<Array<SelectOptionModel<number>>>> {
    let newUrl = `${this.url}/Criminal/Municipios/?cod_estado=${cod_estado}`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<number>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  async ObterComarcasAsync(cod_estado): Promise<ResultRequestModel<Array<SelectOptionModel<number>>>> {
    let newUrl = `${this.url}/Criminal/Comarcas/?cod_estado=${cod_estado}`
    return await this.http.get<ResultRequestModel<Array<SelectOptionModel<number>>>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  async ObterProcessosAsync(filtros): Promise<ResultRequestModel<Array<EmpresaProcessoCriminalModel>>> {
    let newUrl = `${this.url}/Criminal/Processos`
    return await this.http.post<ResultRequestModel<Array<EmpresaProcessoCriminalModel>>>(newUrl,filtros).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  async ObterProcessosDownloadAsync(filtros): Promise<void> {

    try {
      let link = `${this.url}/Criminal/download`;
      const response: any = await this.http.post(link,filtros, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      return Promise.reject(error);
    }
  }

  
  async ObterUltimosProcessosAsync(cod_tipo_processo): Promise<ResultRequestModel<ProcessoCriminalModel>> {
    let newUrl = `${this.url}/Criminal/ListarUltimosProcessos?cod_tipo_processo=${cod_tipo_processo}`
    return await this.http.get<ResultRequestModel<ProcessoCriminalModel>>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

}

