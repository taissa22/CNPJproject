import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
import { Router } from '@angular/router';
import { environment } from '@environment';
import { Permissoes } from 'src/app/permissoes/permissoes';
import { PermissoesService } from 'src/app/permissoes/permissoes.service';
import { BsLocaleService, ptBrLocale, defineLocale } from 'ngx-bootstrap';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { FiltroProcessoCriminalModel } from '../../models/criminal/filtro-processo-criminal.model';
import { EmpresaProcessoCriminalModel } from '../../models/criminal/empresa-processo-criminal.model';
import { ProcessoCriminalService } from '../../services/processo-criminal.service';
import { SisjurPaginator } from "@libs/sisjur/sisjur-paginator/sisjur-paginator.component";

@Component({
  selector: 'app-listar-processo-criminal',
  templateUrl: './listar-processo-criminal.component.html',
  styleUrls: ['./listar-processo-criminal.component.scss']
})
export class ListarProcessoCriminalComponent implements OnInit, AfterViewInit {
  constructor(
    private configLocalizacao: BsLocaleService,
    private router: Router,
    private service: ProcessoCriminalService,
    private permissaoService: PermissoesService
  ) {

    this.configLocalizacao.use('pt-br');
    ptBrLocale.invalidDate = 'Data Invalida';
    defineLocale('pt-br', ptBrLocale);
  }

  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  ngAfterViewInit() {
    this.pesquisar();
  }

  ngOnInit() {
    
    this.title = sessionStorage.getItem('criminal_modulo');
    this.parametrosConsulta = JSON.parse(sessionStorage.getItem('criminal_parametrosConsulta'));
    this.filtros = JSON.parse(sessionStorage.getItem('criminal_filtro_'+this.title.toLowerCase()));

    //PERMISSÕES:
    if(this.title=="JUDICIAL")
    {
      this.podeConsultar = this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_CRIMINAL_JUDICIAL);
      
      if(!this.podeConsultar)
      {
        window.history.go(-1);
        return;
      }
        
      this.podeIncluir = this.permissaoService.temPermissaoPara(Permissoes.INCLUIR_CRIMINAL_JUD);
      this.podeAlterar = this.permissaoService.temPermissaoPara(Permissoes.ALTERAR_CRIMINAL_JUD);
    }
    if(this.title=="ADMINISTRATIVO")
    {
      this.podeConsultar = this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_CRIMINAL_ADM);

      if(!this.podeConsultar)
      {
        window.history.go(-1);
        return;
      }

      this.podeIncluir = this.permissaoService.temPermissaoPara(Permissoes.INCLUIR_CRIMINAL_ADM);
      this.podeAlterar = this.permissaoService.temPermissaoPara(Permissoes.ALTERAR_CRIMINAL_ADM);
    }

    this.iniciarValores(this.title);
    this.reload(this.title);
  }

  podeConsultar: boolean = false;
  podeIncluir: boolean = false;
  podeAlterar: boolean = false;

  title: string;
  filtros: FiltroProcessoCriminalModel;

  modal = false;

  primeiraColuna: string;
  segundaColuna: string;
  quintaColuna: string;

  qtdRegistros:number;
  ambiente = environment.s1_url;
  rotaImprimir: string;
  rotaDownload: string;
  rotaNovo: string;

  empresas = []; //remover
  resultadoBusca = new Array<EmpresaProcessoCriminalModel>();

  parametrosConsulta: Array<parametrosConsulta>;
  objectKeys = Object.keys;


  dadosColuna1(proc) {
    if (this.title == 'ADMINISTRATIVO') {
      return (
        proc.procedimento +
        '<br />' +
        proc.nro_processo +
        '<br />' +
        proc.status +
        '/ Criticidade ' +
        proc.criticidade
      );
    }

    return (
      proc.acao +
      '<br/>' +
      proc.nro_processo +
      '<br />' +
      proc.status +
      '/ Criticidade ' +
      proc.criticidade +
      '<br />' +
      'Número Antigo: ' +
      (proc.nro_processo_antigo != null
        ? proc.nro_processo_antigo
        : '(Não informado)')
    );
  }
  dadosColuna2(proc) {
    if (this.title == 'ADMINISTRATIVO') {
      return (
        proc.orgao +
        '<br/>' +
        proc.competencia +
        '<br/>' +
        proc.estado +
        ' - ' +
        proc.municipio
      );
    }
    return proc.estado + ' - ' + proc.comarca + '<br/>' + proc.vara;
  }
  dadosColuna3(proc)
  {
      return proc.assunto.join("<br/>");
  }

  iniciarValores(item) {
    if (item == 'ADMINISTRATIVO') {
      this.primeiraColuna = 'TIPO PROCEDIMENTO / Nº PROCEDIMENTO';
      this.segundaColuna = 'ORGÃO / COMPETÊNCIA / ESTADO / MUNICÍPIO';
      this.quintaColuna = 'INSTAURAÇÃO';
      this.rotaImprimir = `${this.ambiente}/2.0/processo/CriminalAdministrativo/impressao/ImprimirProcesso.aspx?`;
      this.rotaDownload =
        '/2.0/processo/CriminalAdministrativo/Pesquisa/ListaDownload.aspx';
      this.rotaNovo =
        '/2.0/processo/criminaladministrativo/Novo/NovoProcesso.aspx?vl=1';

    }
    if (item == 'JUDICIAL') {
      this.primeiraColuna = 'AÇÃO / PROCESSO';
      this.segundaColuna = 'ESTADO / COMARCA / VARA';
      this.quintaColuna = 'DISTRIBUIÇÃO';
      this.rotaImprimir = `${this.ambiente}/2.0/processo/criminaljudicial/impressao/ImprimirProcesso.aspx?`;
      this.rotaDownload =
        '/2.0/processo/CriminalJudicial/Pesquisa/ListaDownload.aspx';
      this.rotaNovo =
        '/2.0/processo/CriminalJudicial/Novo/NovoProcesso.aspx?vl=0';

    }
  }

  imprimir(id: number) {
    id == null
      ? (this.rotaImprimir = `${this.ambiente}/2.0/processo/CriminalJudicial/Impressao/ImprimirProcesso.aspx?p=1`)
      : (this.rotaImprimir = `${this.ambiente}/2.0/processo/criminaljudicial/impressao/ImprimirProcesso.aspx?codProcesso=${id}`);
    console.log(this.rotaImprimir);
    window.open(this.rotaImprimir, '', 'width=1000,height=600');
  }

  download(){

    this.filtros.size = this.paginator === undefined ? 8 : this.paginator.pageSize;
    this.filtros.page = this.paginator === undefined? 1 : this.paginator.pageIndex+1;

    this.service.ObterProcessosDownloadAsync(this.filtros);
  }

  modalParametro(){
    this.modal = !this.modal;
  }

  reload(item) {
    if (item == undefined) {
      let modulo = sessionStorage.getItem('criminal_modulo');
      this.router.navigateByUrl(`processos/criminal/${modulo.toLowerCase()}`);
      sessionStorage.removeItem('criminal_modulo');
    }
  }



  async pesquisar() {

    this.filtros.size = this.paginator === undefined ? 8 : this.paginator.pageSize;
    this.filtros.page = this.paginator === undefined? 1 : this.paginator.pageIndex+1;

    this.service.ObterProcessosAsync(this.filtros).then(x => {
      this.resultadoBusca = x.data;
      this.qtdRegistros =x.total;
    });
  }

  voltar() {
    sessionStorage.removeItem('criminal_modulo');
    this.router.navigateByUrl(`processos/criminal/${this.title.toLowerCase()}`);
  }

  titleCase(str) {
    let upper = true;
    let newStr = '';
    for (let i = 0, l = str.length; i < l; i++) {
      if (str[i] == ' ') {
        upper = true;
        newStr += ' ';
        continue;
      }
      newStr += upper ? str[i].toUpperCase() : str[i].toLowerCase();
      upper = false;
    }
    return newStr;
  }
}

interface parametrosConsulta {
  tipo: string;
  descricao: string;
}
