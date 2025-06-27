import { Injectable } from '@angular/core';
import { ISysInfoRepository } from './isys-info-repository';
import { ApiService } from '../api.service';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import axios from 'axios';
import { parseXML } from '@shared/utils';

@Injectable({
  providedIn: 'root'
})
export class SysInfoRepository implements ISysInfoRepository {

  constructor(private http: ApiService) { }

  /**
   * @description A versão atual do sistema é coletada através de um XML
   * disponível na raíz do projeto
   *
   * @returns XML após o parsing
   */
  async getSysVersion(): Promise<any> {
    const res = await this.readVersionamentoXML();
    const xmlBytes = res.data;
    return parseXML(xmlBytes)['MyVersion']['version'];
  }


  /**
   * @description Lê o arquivo versionamento.xml
   *
   * @returns XML antes do parse.
   */
  private async readVersionamentoXML() {
    return await axios.get(environment.s1_url + '/versionamento.xml',
    {
      method: "GET",
      headers: {
        "accept": "text/xml"
      }
    });
  }


}
