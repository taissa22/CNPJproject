import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SysInfoRepository } from './sys-info.repository';
import { environment } from 'src/environments/environment';
import { parseXML } from '@shared/utils';
import axios from 'axios'


@Injectable({
  providedIn: 'root'
})
export class SysInfoService {

  /**
   * @description Indica a versão atual do sistema
   */
  private versaoSource = new BehaviorSubject<string>('---');
  versao = this.versaoSource.asObservable();

  constructor(private repository: SysInfoRepository) { }

  /**
  * Pega o XML do menu do enpoint. Utilização do axios para o IE conseguir usar o GET
  */
 getMenuXML(): Promise<any> {
  return axios.get(environment.s1_url + '/2.0/Layout/menuitems.xml',
    {
      method: "GET",
      headers: {
        "accept": "text/xml"
      }
    }).then(res => res.data)
    .then(str => {
      // Fazendo o parse do valor do xml
      var parser = new DOMParser();
      // Trocando o tipo do xml para 'text/xml'
      var xmlDoc = parser.parseFromString(str, 'text/xml')
      // Chama o parseXml e pega os valores do MenuItem que está dentro do MenuItems
      return parseXML(str).MenuItems.MenuItem;
    })
}

/**
  * Pega a versão do xml do Sisjur para colocar no projeto
  */
  async obterVersaoWeb() {
    const ver = await this.repository.getSysVersion();
    this.versaoSource.next(ver);
    return ver;
  }


}
