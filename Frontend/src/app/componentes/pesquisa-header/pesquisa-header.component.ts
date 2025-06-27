import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetMenuPermitidoResponse } from './pemissao-menu';
import { PesquisaHeaderService } from './pesquisa-header.service';

import { Component, HostListener, OnInit, ViewEncapsulation } from '@angular/core';
import { environment } from '@environment';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-pesquisa-header',
  templateUrl: './pesquisa-header.component.html',
  styleUrls: ['./pesquisa-header.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PesquisaHeaderComponent implements OnInit {
  [x: string]: any;
  @HostListener('document:click', null) onClick(args?): void {
    this.limpar();
    if (!this.modalService.hasOpenModals())
      (window.document.querySelector('router-outlet').nextElementSibling as HTMLElement).style.display = '';
  }
  private suggestions;
  public display = "none";
  public displayLimpar = "none";
  public opacity = "0.5";
  public classSearchInput = "search-input";
  public height = "0";
  innerHTML: string;
  public text: string = "";
  public cursor: string = "";
  constructor(public modalService: NgbModal) {
    this.suggestions = this.load();
    this.cursor = "pointer";
  }

  ngOnInit() {

  }

  public focus() {
    this.opacity = "1";
  }

  public blur() {
    if ((document.getElementById("input-box") as HTMLInputElement).value == "")
      this.opacity = "0.5";
  }


  public async buscarMenu($event) {
    
    this.display = "block";
    this.opacity = "1";
    let userData = $event.target.value;
    let emptyArray = [];
    if (userData) {

      emptyArray = this.suggestions.filter((data) => {
        return ((data.tituloMenu != '' ? data.tituloMenu.toLocaleLowerCase().includes(userData.toLocaleLowerCase()) : data.tituloMenu.includes(userData.toLocaleLowerCase()))||
        (data.dscCaminhoMenuTela != '' ? data.dscCaminhoMenuTela.toLocaleLowerCase().includes(userData.toLocaleLowerCase()) : data.dscCaminhoMenuTela.includes(userData.toLocaleLowerCase())));

      });
      emptyArray = emptyArray.map((data) => {
        if (this.modalService.hasOpenModals()) {
          this.modalService.dismissAll();
          (window.document.querySelector('router-outlet').nextElementSibling as HTMLElement).style.display = '';
        }

        var pathFinal = "";

        if (data.path.indexOf(".aspx") != -1 || data.path.indexOf(".mvc") != -1) {
          pathFinal = data.path.replace("~/", `${environment.local_url}`);
        } else if (data.path.indexOf("NewSiteRedirect") != -1) {
          pathFinal = data.path.replace('javascript:NewSiteRedirect(', "").replace(/[\\"]/g, "").replace("S1, ", "").replace(')', "");
          pathFinal = `${environment.local_url}/n/${pathFinal}`
        }
        if (pathFinal == '' && data.path.startsWith('javascript:')) {
          return data = '<li style="cursor: pointer" onclick="this.getElementsByTagName(\'a\')[0].click();"><pre style="overflow: hidden;height:38px;margin-bottom: 0rem;"><a onclick="' + data.path + ';limpar();" class="link-description" href="#" target="_self" title="' + data.tooltip + '">' + data.tituloMenu + '<span style="font-weight: 600;margin-top: 13.5px;color:#14273a;">' + data.dscCaminhoMenuTela + '</span></a></pre></li>';
        } else {
          return data = '<li style="cursor: pointer" onclick="this.getElementsByTagName(\'a\')[0].click();"><pre style="overflow: hidden;height:38px;margin-bottom: 0rem;"><a onclick="limpar()" class="link-description" href="' + pathFinal + '" target="_self" title="' + data.tooltip + '">' + data.tituloMenu + '<span style="font-weight: 600;margin-top: 13.5px;color:#14273a;">' + data.dscCaminhoMenuTela + '</span></a></pre></li>';
        }
      });
      this.classSearchInput = "search-input active";
      this.opacity = "1";
      this.height = "280"
      let final = showSuggestions(emptyArray);
      this.innerHTML = final;
      this.displayLimpar = "block"
      this.cursor = "pointer";

    } else {
      this.classSearchInput = "search-input";
      this.opacity = "0.5";
      this.displayLimpar = "none";
      this.height = "0";
      this.cursor = "normal";

    }
    function showSuggestions(list) {
      let listData;
      if (!list.length) {
        listData = `<div style="margin-top:120px;padding-left:5vw;font-size:12px;"><span class="naoEncontrado">Nenhum menu foi encontrado para o filtro digitado!</span></div>`;
      } else {
        listData = list.join('');
      }
      return listData;
    }
  }
  public limpar() {
    this.display = "none"
    this.displayLimpar = "none"
    this.classSearchInput = "search-input";
    this.opacity = "0.5";
    this.height = "0";
    (document.getElementById("input-box") as HTMLInputElement).value = "";
  }
  public load() {
    return JSON.parse(localStorage.getItem('menuPesquisar'))
  }
}
