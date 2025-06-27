import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { FiltroFornecedoresService } from '../services/filtro-fornecedores.service';
import { FornecedorFiltroDTO } from '@shared/interfaces/fornecedor-filtro-dto';
import { FnParam } from '@angular/compiler/src/output/output_ast';
import { FormBuilder, FormGroup } from '@angular/forms';
@Component({
  selector: 'filtro-fornecedores',
  templateUrl: './filtro-fornecedores.component.html',
  styleUrls: ['./filtro-fornecedores.component.scss']
})
export class FiltroFornecedoresComponent implements OnInit {

  constructor(private route: ActivatedRoute,
              private filtroFornecedorService: FiltroFornecedoresService,
              private fb: FormBuilder) { }

  subscription: Subscription;
  bancos: any[];
  escritorios: any[];
  profissionais: any[];
  tipoFornecedor: any[];

  fornecedorForm: FormGroup;


  //#region Outputs
  // tslint:disable-next-line: no-output-on-prefix
  @Output() onBuscarFornecedores = new EventEmitter<FornecedorFiltroDTO>();
  //#endregion

  ngOnInit() {
     //#region   Resolver da pagina
    this.subscription = this.route.data.subscribe(info => {
       this.bancos = info.banco;
       this.escritorios = info.escritorio;
       this.profissionais = info.profissionais;
    });
    //#endregion

    this.tipoFornecedor = this.filtroFornecedorService.listaTipoFornecedor;

    this.formulario();
  }

  onClickBuscar() {
    // TODO: alterar esta linha para receber os valores das labels
    let valoresFormulario: FornecedorFiltroDTO;
    valoresFormulario = Object.assign({}, this.fornecedorForm.value);
    this.onBuscarFornecedores.emit(valoresFormulario);
  }


  //#region Criação do Formulário
  formulario() {
    this.fornecedorForm = this.fb.group({
      codigoEscritorio: [null],
      codigoTipoFornecedor: [null],
      codigoProfissional: [null],
      codigoBanco: [null],
      nomeFornecedor: [null],
      codigoFornecedorSAP: [null]
    });
  }
  //#endregion
}
