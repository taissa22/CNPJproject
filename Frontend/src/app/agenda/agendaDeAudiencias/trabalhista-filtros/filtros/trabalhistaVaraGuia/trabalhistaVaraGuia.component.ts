import { IVara } from './../../interface/iVara';
import { FiltroJson } from './../../interface/FiltroJson';
import { Json } from './../../../../../sap/consultaLote/resultado-sap/modal-alteracao-data-guia/service/modal-alteracao-data-guia.service';
import { Combobox } from './../../../../../shared/interfaces/combobox';
import { pluck } from 'rxjs/operators';



import { Component, OnInit } from '@angular/core';
import { TrabalhistaVaraService } from '../../services/trabalhistaVara.service';
import { TrabalhistaFiltrosService } from '../../services/trabalhista-filtros.service';
import { HelperAngular } from './../../../../../shared/helpers/helper-angular';



@Component({
  selector: 'app-trabalhistaVaraGuia',
  templateUrl: './trabalhistaVaraGuia.component.html',
  styleUrls: ['./trabalhistaVaraGuia.component.scss']
})
export class TrabalhistaVaraGuiaComponent implements OnInit {

  constructor(private service: TrabalhistaVaraService,
    private trabalhistaService: TrabalhistaFiltrosService,
    private messageService: HelperAngular) { }
  public comboEstado: any;
  public comboComarca: any;
  public comboVara: any;
  public comboTipoVara: any;

  public disabledComarca = true;
  public disabledVara = true;
  public disabledTipoVara = true;


  botaoHabilitado = false;




  listaHeaders = [
    'Estado',
    'Comarca',
    'Vara',
    'Tipo de Vara'];

  dados: IVara[] = [];

  varaSelecionada: IVara = {
    estado: '',
    comarca: '',
    vara: '',
    tipoVara: ''
  };

  varaAux: IVara = {
    estado: '',
    comarca: '',
    vara: '',
    tipoVara: ''
  };

  vara: string[] = [];

  ngOnInit() {

    this.vara = this.trabalhistaService.varaIds;
    this.dados = this.trabalhistaService.varaNomes

    this.service.recuperarEstados().pipe(pluck('data')).subscribe(dados => {
      this.comboEstado = dados;
      
    })

  }

  selecionarEstado(event: any) {
    this.varaSelecionada.estado = event
    this.varaSelecionada.comarca = null;
    this.varaSelecionada.vara = null;
    this.varaSelecionada.tipoVara = null;
    this.botaoHabilitado = false;
    this.recuperarDadosComarca(event)
  }

  selecionarComarca(event: any) {
    this.varaSelecionada.comarca = event
    this.varaSelecionada.vara = null;
    this.varaSelecionada.tipoVara = null;
    this.botaoHabilitado = false;
    this.recuperarDadosVara(event)
  }

  selecionarVara(event: any) {
    this.varaSelecionada.vara = event
    this.varaSelecionada.tipoVara = null;
    this.botaoHabilitado = false;
    this.recuperarDadosTipoVara(this.varaSelecionada.comarca, event)
  }

  selecionarTipoVara(event: any) {
    this.varaSelecionada.tipoVara = event
    this.botaoHabilitado = true;

  }

  recuperarDadosComarca(estado: any) {
    this.disabledComarca = false;
    this.disabledVara = true;
    this.disabledTipoVara = true;
    this.service.recuperarComarca(estado).pipe(pluck('data')).subscribe(dados => {
      this.comboComarca = dados;
    })
  }

  recuperarDadosVara(comarca: any) {
    this.disabledVara = false;
    this.disabledTipoVara = true;
    this.service.recuperarVara(comarca).pipe(pluck('data')).subscribe(dados => {

      this.comboVara = dados;
    })
  }

  recuperarDadosTipoVara(comarca: any, vara: any) {
    this.disabledTipoVara = false;
    this.service.recuperarTipoVara(comarca, vara).pipe(pluck('data')).subscribe(dados => {
      this.comboTipoVara = dados;
    })
  }

  excluirVara(index: number) {
    this.vara.splice(index, 1)
    this.dados.splice(index, 1);
    this.atualizarJson();
    this.trabalhistaService.atualizarContagemVara()
  }

  adicionarVara() {

    var t = {
    comarca: this.varaSelecionada.comarca,
    vara: this.varaSelecionada.vara,
    tipoVara: this.varaSelecionada.tipoVara};

    this.varaAux = {
      estado: this.comboEstado[this.comboEstado.findIndex(e => e.id == this.varaSelecionada.estado)].descricao,
      comarca: this.comboComarca[this.comboComarca.findIndex(e => e.id == this.varaSelecionada.comarca)].descricao,
      vara: this.comboVara[this.comboVara.findIndex(e => e.id == this.varaSelecionada.vara)].descricao,
      tipoVara: this.comboTipoVara[this.comboTipoVara.findIndex(e => e.id == this.varaSelecionada.tipoVara)].descricao
    }

    if (this.dados.filter(dado =>
      dado.comarca == this.varaAux.comarca
      && dado.estado == this.varaAux.estado
      && dado.tipoVara == this.varaAux.tipoVara
      && dado.vara == this.varaAux.vara
      ).length == 0) {

        var values = Object.keys(t).map(function(e) {
          return t[e]
        })

      this.vara.push(values.join(','));

      this.dados.push(this.varaAux)
      this.atualizarJson();
    }else{
        this.messageService.MsgBox2(`Não é possível adicionar a vara ,
        pois ela já está adicionada`,
         'Vara já adicionada!', 'info', 'Ok');
    }

  }

  atualizarJson() {

    this.trabalhistaService.json.vara = [...this.vara];


    this.trabalhistaService.varaIds = this.vara;
    this.trabalhistaService.atualizarContagemVara()
    this.trabalhistaService.varaNomes = [...this.dados];
    
  }

}
