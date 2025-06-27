import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CardMenu } from '@shared/models/card-menu.model';
import { environment } from 'src/environments/environment';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-menu-novo',
  templateUrl: './menu-novo.component.html',
  styleUrls: ['./menu-novo.component.scss']
})
export class MenuNovoComponent implements OnInit {

  ngOnInit() {
    this.url = window.location.hash == '#/home' || window.location.hash.indexOf("#/home") != -1 || window.location.hash == '#/' ? 'home' : 'modal';
    this.receberDados();
  }

  constructor(public activeModal: NgbActiveModal, private SpinnerService: NgxSpinnerService) { }

  nome: string;
  caminhoPartesList = [];
  qtdOpcoes: number;
  count: number = 0;
  linkVazio = "";

  qtdColuna1: number;
  qtdColuna2: number;
  qtdColuna3: number;

  cards: CardMenu[];
  url: string;
  caminhoPercorrido = [];
  nivelAtual = [];
  indiceNivelAtual = 0;

  receberDados() {
    this.cards = JSON.parse(localStorage['menuPrincipal'])
  }

  home() {
    document.getElementById('layoutCards').style.display = 'grid';
    document.getElementById('layoutMenu').style.display = 'none';

    this.caminhoPercorrido = [];
    this.caminhoPartesList = [];
    this.nivelAtual = [];
    this.indiceNivelAtual = 0;
    this.nome = "";
  }

  exibirSubMenu(index) {

    if(this.cards[index].url == "")
    {
      document.getElementById('layoutCards').style.display = 'none';
      document.getElementById('layoutMenu').style.display = 'flex';
    }
    this.obterDados(index);
  }

  obterDados(index) {
    if (this.indiceNivelAtual == 0) {
      this.nivelAtual = this.cards[index].subItens;

      if (this.nivelAtual == null && this.cards[index].url != null) {
        var regex = /^~(.*)/is;
        var result = this.cards[index].url.replace(regex, "/juridico$1");
        this.loading();
        window.location.href = result;
        return;
      } else {
        this.nome = this.cards[index].nome;
        this.caminhoPartesList.push(this.nome);
      }
    } else {
      this.caminhoPartesList.push(this.nivelAtual[index].nome);
      this.nivelAtual = this.nivelAtual[index].subItens;
    }
    this.caminhoPercorrido.push(this.nivelAtual);
    this.indiceNivelAtual += 1;

    this.formatarUrl(this.nivelAtual);
    this.organizarColunas();
  }

  formatarUrl(url: any) {
    for (let index = 0; index < url.length; index++) {
      url[index].javascript = "";
      if (url[index].url != null) {
        let urlFormatada = url[index].url;
        if (urlFormatada.indexOf(".aspx") != -1 || urlFormatada.indexOf(".mvc") != -1) {
          url[index].url = urlFormatada.replace("~/", environment.s1_url + '/');
        }
        else if (urlFormatada.indexOf("NewSiteRedirect") != -1) {
          url[index].url = urlFormatada.replace('javascript:NewSiteRedirect(', "").replace(/[\\"]/g, "").replace("S1, ", "").replace(')', "");
        }
        else if (urlFormatada.indexOf("altera_alerta") != -1) {
          url[index].javascript = urlFormatada;
          url[index].url = "#";
        }
      }
    }
  }

  organizarColunas() {
    this.qtdOpcoes = this.nivelAtual.length
    this.qtdColuna1 = this.qtdOpcoes < 8 ? Math.ceil(this.qtdOpcoes / 1.5) : this.qtdOpcoes < 11 ? Math.ceil(this.qtdOpcoes / 2.5) : Math.ceil(this.qtdOpcoes / 3);
    this.qtdColuna2 = this.qtdColuna1 * 2;
    this.qtdColuna3 = this.qtdColuna1 * 3;
  }

  voltar() {

    this.indiceNivelAtual -= 1;
    if (this.indiceNivelAtual == 0) {
      this.home();
    } else {
      this.nivelAtual = this.caminhoPercorrido[this.indiceNivelAtual - 1];
      this.caminhoPercorrido.pop();
      this.caminhoPartesList.pop();
      this.organizarColunas();
    }
  }

  fecharNovoMenu(nivelAtual) {
    
    
    
    if (nivelAtual.javascript != "") {
      eval(nivelAtual.javascript);
    }
    //Só fecha o menu se for navegar no mesmo módulo:
    if(nivelAtual.url!=null && nivelAtual.url.indexOf("#" ) == 0){
      this.activeModal.dismiss('Cross click');
    }
    //Estou indo pra outro módulo
    else{
      this.loading();
    }
  }
  loading(){
    //Coloca um loading na tela quando vai pra um link externo
    this.SpinnerService.show();
      
      // Caso seja necessário colocar um tempo pra fechar sozinho
      // let a = this;
      // window.setTimeout(function(){ 
      //   a.SpinnerService.hide(); 
      //   a.activeModal.dismiss('Cross click');
      // }, 10000);
  }

  voltarPaginaNavegada(indiceRetorno) {
    if (this.indiceNivelAtual == 1 || indiceRetorno == this.indiceNivelAtual - 1) return;
    indiceRetorno += 1;
    this.caminhoPercorrido.length = this.caminhoPartesList.length = indiceRetorno
    this.nivelAtual = this.caminhoPercorrido[this.caminhoPercorrido.length - 1];
    this.indiceNivelAtual = indiceRetorno;
    this.organizarColunas();
  }
}
