import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ESocialDashboardDTO } from '@esocial/models/esocial-dashboard';
import { DashboardEsocialService } from '@esocial/services/aplicacao/dashboard-esocial.service';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectComponent } from '@ng-select/ng-select';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-dashboard-esocial',
  templateUrl: './dashboard-esocial.component.html',
  styleUrls: ['./dashboard-esocial.component.scss']
})
export class DashboardEsocialComponent implements OnInit {

  constructor(
    private service : DashboardEsocialService,
    private dialog: DialogService
  ) { }

  async ngOnInit() {
    await this.obterUfAsync();
    await this.obterEmpresasAsync();
    await this.aplicar();
  }
  breadcrumb: string = 'Processo > Trabalhista > eSocial > Dashboard eSocial';

  @ViewChild('selectUf', { static: false }) selectUf: NgSelectComponent;

  primeiraAtualizacao = true;

  ehDadoFiltrado = false;

  empresaFormControl = new FormControl();
  ufFormControl = new FormControl();

  ufList = [];
  empresaLista = [];

  dashboard: ESocialDashboardDTO;

  qtdErros: number = 0;

  formFiltro = {
    idsEmpresaAgrupadoras: [],
    uFs: []
  };

  async obterUfAsync(){
    const resposta = await this.service.obterUFAsync()  
    if(resposta){
      this.ufList = resposta.map((x: any) => ({
        id: x.id,
        descricao: x.descricao,
        descricaoConcat: `${x.id} - ${x.descricao}`
      }));
      this.marcarUfs();
    }
  }
  
  async obterEmpresasAsync(){
    this.empresaLista = await this.service.obterEmpresasAsync();
    this.marcarEmpresas();
  }

  async marcarUfs(){
    if(this.ufList.length > 0){
      let opts = [];
      this.ufList.forEach(option => {
        opts.push(option.id);
      });
      this.ufFormControl.setValue(opts)
    }
  }
  
  async marcarEmpresas(){
    if(this.empresaLista.length > 0){
      let opts = [];
      this.empresaLista.forEach(option => {
        if(option.default)
          opts.push(option.id);
      });
      this.empresaFormControl.setValue(opts)
    }
  }

  abrirFiltro(tooltip: NgbTooltip) {
    tooltip.isOpen() ? tooltip.close() : tooltip.open();
  }
  
  async limparFiltros(){
    await this.marcarUfs();
    await this.marcarEmpresas();
  }

  close(tooltip: NgbTooltip){
    tooltip.close()
  }

  aplicar(){
    this.formFiltro.idsEmpresaAgrupadoras = this.empresaFormControl.value;
    this.formFiltro.uFs = this.ufFormControl.value;
    this.service.obterDadosDashboardAsync(this.formFiltro).then(x => {
      if(x){
        this.dashboard = x;
        this.ehDadoFiltrado = true;
      }
      else{
      if(!this.primeiraAtualizacao){
        this.dialog.err(
          'Informações não carregadas',
          'Nenhum dado foi encontrado para o filtro aplicado.'
          );          
        }
        this.primeiraAtualizacao = false;
      }
    });
    // this.qtdErros = 8
    this.service.obterErrosDashboardAsync(this.formFiltro).then(x => {
      this.qtdErros = x
    });
  }

}
