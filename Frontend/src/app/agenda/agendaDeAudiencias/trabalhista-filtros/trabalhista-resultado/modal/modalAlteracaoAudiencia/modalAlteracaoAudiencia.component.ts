import { Combobox } from './../../../../../../shared/interfaces/combobox';
import { pluck, filter } from 'rxjs/operators';
import { TrabalhistaResultadoService } from './../../services/trabalhistaResultado.service';
import { TrabalhistaResultadoCrudService } from './../../../services/trabalhistaResultadoCrud.service';
import { FormGroup, FormControl } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap';
import { Component, OnInit, Pipe } from '@angular/core';
import { NgSwitch } from '@angular/common';
import { modalConfigDefaults } from 'ngx-bootstrap/modal/modal-options.class';
import { DialogService } from '@shared/services/dialog.service';

interface alteracaoAudiencia {

  codProcesso: any;
  sequenciaAudiencia: any;
  codigoProfissional?: any;
  codigoAdvogado?: any;
  codigoProfissionalAcompanhante?: any;
  codigoAdvogadoAcompanhante?: any;
  codigoPreposto?: any;
  codigoPrepostoAcompanhante?: any
}
@Component({
  selector: 'app-modalAlteracaoAudiencia',
  templateUrl: './modalAlteracaoAudiencia.component.html',
  styleUrls: ['./modalAlteracaoAudiencia.component.css']
})
export class ModalAlteracaoAudienciaComponent implements OnInit {

  titulo = 'Alteração de Audiência';

  json: alteracaoAudiencia;

  dados: any;
  listaEscritorio: any;
  listaPreposto: any;
  listaAdvogado: any;
  listaAdvogadoAcomp: any;

  //lista selecionados
  escritorioSelect = null;
  escritorioAcompSelect = null;
  prepostoSelect = null;
  prepostoAcompSelect = null;
  advogadoSelect = null;
  advogadoAcompSelect = null

  constructor(public service: TrabalhistaResultadoService, public bsModalRef: BsModalRef, private dialog: DialogService) { }

  ngOnInit() {
    this.carregarComboBox()
    this.dados = this.service.audienciaSelecionada;
  }

  ngOnDestroy(): void {
    this.service.atualizardadosTabela.next(false);
  }


  carregarComboBox() {
    this.service.carregarCombos().pipe(pluck('data')).subscribe(dados => {
      
      this.listaPreposto = dados.listaPrepostos;
      this.listaEscritorio = dados.listaEscritorios;

      if (this.dados.preposto.length > 0) {
       
        this.prepostoSelect = this.listaPreposto[this.listaPreposto.findIndex(e =>
          e.descricao.trim() == this.dados.preposto.trim()
        )].id
      } else {
        this.prepostoSelect = null;
      }

      if (this.dados.escritorioAudiencia.length > 0) {
        this.escritorioSelect = this.listaEscritorio[this.listaEscritorio.findIndex(e => e.descricao.trim() == this.dados.escritorioAudiencia.trim())].id
        this.carregarAdvogado(this.escritorioSelect, false);
      } else {
        this.escritorioSelect = null;
      }

      if (this.dados.prepostoAcompanhante.length > 0) {
        this.prepostoAcompSelect = this.listaPreposto[this.listaPreposto.findIndex(e =>
          e.descricao.trim() == this.dados.prepostoAcompanhante.trim()
        )].id
      } else {
        this.prepostoAcompSelect = null;
      }

      if (this.dados.escritorioAcompanhante.length > 0) {
        this.escritorioAcompSelect = this.listaEscritorio[this.listaEscritorio.findIndex(e => e.descricao.trim() == this.dados.escritorioAcompanhante.trim())].id
        this.carregarAdvogado(this.escritorioAcompSelect, true);
      } else {
        this.escritorioAcompSelect = null;
      }
    })
  }

  carregarAdvogado(escritorio: any, acompanhante: boolean) {

    if (escritorio) {
      this.service.carregarAdvogados(escritorio).pipe(pluck('data')).subscribe(dados => {
        if (!acompanhante) {  
          this.listaAdvogado = dados.listaAdvogados;
          if (this.dados.advogadoAudiencia.length > 0) {
            this.advogadoSelect = this.listaAdvogado[this.listaAdvogado.findIndex(e => e.descricao.trim() == this.dados.advogadoAudiencia.trim())].id
          } else {
            this.advogadoSelect = null
          }
        } else {
          this.listaAdvogadoAcomp = dados.listaAdvogados;
          if (this.dados.advogadoAcompanhante.length > 0) {
            this.advogadoAcompSelect = this.listaAdvogadoAcomp[this.listaAdvogadoAcomp.findIndex(e =>
              e.descricao.trim() == this.dados.advogadoAcompanhante.trim()
            )].id;
          } else {
            this.advogadoAcompSelect = null
          }
        }
      })
    }
  }

  selecionarCombo(valor: any, campo: string, acompanhante: boolean) {

    switch (campo) {
      case 'preposto': {
        !acompanhante ? this.prepostoSelect = valor : this.prepostoAcompSelect = valor
        break;
      }
      case 'escritorio': {
        !acompanhante ?
          (this.escritorioSelect = valor, this.advogadoSelect = null, this.carregarAdvogado(this.escritorioSelect, false))
          :
          (this.escritorioAcompSelect = valor, this.advogadoAcompSelect = null, this.carregarAdvogado(this.escritorioAcompSelect, true));

        break;
      }
      case 'advogado': {
        !acompanhante ? 
        this.advogadoSelect = valor
        : 
        this.advogadoAcompSelect = valor;
        break;
      }
    }
  }


  async alterar() {    
    if (this.escritorioSelect && this.escritorioSelect === this.escritorioAcompSelect) {
      await this.dialog.alert(
        'Escritório acompanhante inválido',
        'O escritório acompanhante tem que ser diferente do escritório da audiência'
      );
      return;
    }

    this.json = {
      codProcesso: this.dados.codProcesso,
      sequenciaAudiencia: this.dados.seqAudiencia,
      codigoProfissional: this.escritorioSelect,
      codigoAdvogado: this.advogadoSelect,
      codigoProfissionalAcompanhante: this.escritorioAcompSelect,
      codigoAdvogadoAcompanhante: this.advogadoAcompSelect,
      codigoPreposto: this.prepostoSelect,
      codigoPrepostoAcompanhante: this.prepostoAcompSelect
    }

    let newObj = {}
    Object.keys(this.json).forEach(key => {
      if (this.json[key])
        newObj[key] = this.json[key]
    });

    this.service.alterar(newObj).subscribe(_ => {
      this.service.atualizardadosTabela.next(true);
      this.bsModalRef.hide();
    });
  }
}


