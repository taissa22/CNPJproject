import { map } from 'rxjs/operators';
import { TipoProcesso } from './../../../../core/models/tipo-processo';
//import { Fornecedor } from '@shared/interfaces/fornecedor';
import { FornecedorEditarDto } from './../../../../shared/interfaces/fornecedor-editar-dto';

import { Fornecedor } from './../../../../shared/interfaces/fornecedor';

import { ModalFornecedoresService } from './../services/modal-fornecedores.service';
import { FornecedorService } from 'src/app/core/services/sap/fornecedor.service';

import { CadastrarFornecedorDTO } from './../../../../shared/interfaces/cadastrar-fornecedor-dto';
import { ManutencaoFornecedoresService } from './../services/manutencao-fornecedores.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FiltroFornecedoresService } from '../services/filtro-fornecedores.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'modal-adicionar-fornecedores',
  templateUrl: './modal-adicionar-fornecedores.component.html',
  styleUrls: ['./modal-adicionar-fornecedores.component.scss']
})
export class ModalAdicionarFornecedoresComponent implements OnInit, OnDestroy {
  public titulo = "Incluir Fornecedor"
  public tipoFornecedor: any[] = [];
  public campoSelecionado: number = null;
  bancoSelecionado;

  public cadFornecedor: CadastrarFornecedorDTO;
  public editFornecedorDto: FornecedorEditarDto;
  registerForm: FormGroup;

  subscription: Subscription;
  bancos: any[];
  escritorios: any[];
  profissionais: any[];

  private modoSalvar = 'Cadastrar';
  subsFecharModal: Subscription;
  editarFornec: Subscription;

  //editar fornecedor
  editCodFornecedor = null;
  editBanco = null;
  editEscritorio = null;
  editProfissional = null;
  idFornecedor: number = null
  campoNomeFornecedor = '';
  campoFornecedorSap = '';

  fornecedorParaEditar: Fornecedor;


  constructor(private service: ModalFornecedoresService,
    private manutencaoService: ManutencaoFornecedoresService,
    private filtroFornecedorService: FiltroFornecedoresService,
    public bsModalRef: BsModalRef,
    private fb: FormBuilder) { }

  ngOnInit() {


    this.bancos = this.manutencaoService.banco


    this.escritorios = this.manutencaoService.escritorios;
    this.profissionais = this.manutencaoService.profissional;
    this.tipoFornecedor = this.service.tirarNumerosListaFornecedores();

    this.subsFecharModal = this.manutencaoService.fecharModal.subscribe(bool => this.fecharModal(bool));
    this.validation();
    this.manutencaoService.selectedFornecedoresSubject.subscribe(forn => this.fornecedorParaEditar = forn[0])
    this.editarFornec = this.service.editarFornecedor.subscribe(bool => {
      if (bool) {
        this.pegarValoresEdicao();
        this.modoSalvar = 'editar';
        this.titulo = "Alteração de Fornecedores"
      }
    })


  }


  pegarValoresFornecedor() {

  }


  ngOnDestroy() {
    this.subsFecharModal.unsubscribe();
    this.editarFornec.unsubscribe();
  }



  pegarTipoProcesso(evento: any) {
    this.registerForm.get('tipoFornecedorBox').setValue(evento);
    this.campoSelecionado = evento

    this.alterarTipoProcesso(this.campoSelecionado);

  }


  validation() {
    this.registerForm = this.fb.group({
      bancoBox: [null],
      escritorioBox: [null],
      profissionalBox: [null],
      tipoFornecedorBox: ['', [Validators.required]],
      nomeFornecedor: ['', [Validators.required, Validators.maxLength(100)]],
      codigoSap: ['', [Validators.required, Validators.maxLength(10)]],

    });
  }



  alterarTipoProcesso(processo: any) {
    const bancoControl = this.registerForm.get('bancoBox');
    const escritorioControl = this.registerForm.get('escritorioBox');
    const profissionalControl = this.registerForm.get('profissionalBox');

    //this.registerForm.valueChanges.subscribe(() => {

    if (processo == 1) {
      bancoControl.setValidators([Validators.required]);
      escritorioControl.setValidators(null);
      escritorioControl.setValue(null);
      profissionalControl.setValidators(null);
      profissionalControl.setValue(null);

    } else if (processo == 2) {
      bancoControl.setValidators(null);
      bancoControl.setValue(null);
      escritorioControl.setValidators(null);
      escritorioControl.setValue(null);
      profissionalControl.setValidators([Validators.required]);

    } else if (processo == 3) {
      bancoControl.setValidators(null);
      bancoControl.setValue(null);
      escritorioControl.setValidators([Validators.required]);
      profissionalControl.setValidators(null);
      profissionalControl.setValue(null)

    }
    bancoControl.updateValueAndValidity();
    escritorioControl.updateValueAndValidity();
    profissionalControl.updateValueAndValidity();
    //})
  }



  validacaoTextos(nomeControl: string, nomeCampo: string) {
    if (
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('required')
      ||
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('whitespace')
    ) {
      return `${nomeCampo} é obrigatório!`;
    }
    if (
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('minlength')
    ) {
      return `${nomeCampo} deve possuir no mínimo
                  ${
        this.registerForm.get(nomeControl).errors.minlength
          .requiredLength
        } caracteres!`;
    }
    if (
      this.registerForm.invalid &&
      this.registerForm.get(nomeControl).touched &&
      this.registerForm.get(nomeControl).hasError('maxlength')
    ) {
      return `${nomeCampo} deve possuir no máximo
                  ${
        this.registerForm.get(nomeControl).errors.maxlength
          .requiredLength
        } caracteres!`;
    }

  }

  fecharModal(bool: boolean) {
    if (bool) {
      this.bsModalRef.hide();
      this.manutencaoService.fecharModal.next(false);
    }
  }


  salvarAlteracao() {
    if (this.registerForm.valid) {

      if (this.modoSalvar == 'Cadastrar') {
        this.cadFornecedor = {
          'codigoTipoFornecedor': parseFloat(this.registerForm.value.tipoFornecedorBox),
          'codigoBanco': parseFloat(this.registerForm.value.bancoBox),
          'codigoEscritorio': parseFloat(this.registerForm.value.escritorioBox),
          'codigoProfissional': parseFloat(this.registerForm.value.profissionalBox),
          'codigoFornecedorSAP': this.registerForm.value.codigoSap,
          'nomeFornecedor': this.registerForm.value.nomeFornecedor,
          'criarCodigoFornecedorSAP': false
        }

        //this.manutencaoService.cadastrarFornecedores(this.cadFornecedor);
        this.service.cadastrarFornecedor(this.cadFornecedor);
        //this.bsModalRef.hide();
      } else {

        this.editFornecedorDto = {
          'id': this.idFornecedor,
          'codigoTipoFornecedor': parseFloat(this.registerForm.value.tipoFornecedorBox),
          'codigoBanco': parseFloat(this.registerForm.value.bancoBox),
          'codigoEscritorio': parseFloat(this.registerForm.value.escritorioBox),
          'codigoProfissional': parseFloat(this.registerForm.value.profissionalBox),
          'codigoFornecedorSAP': this.registerForm.value.codigoSap,
          'nomeFornecedor': this.registerForm.value.nomeFornecedor,
          'criarCodigoFornecedorSAP': false
        }
        //this.manutencaoService.editarFornecedor(this.editFornecedorDto);
        this.manutencaoService.editarFornecedor(this.editFornecedorDto);
        //this.bsModalRef.hide();

      }
    }
  }

  pegarValoresEdicao() {


    if (this.fornecedorParaEditar) {

      this.editCodFornecedor = this.fornecedorParaEditar.codigoTipoFornecedor;
      this.campoSelecionado = this.editCodFornecedor;

      this.editBanco = this.fornecedorParaEditar.codigoBanco;
      this.editEscritorio = this.fornecedorParaEditar.codigoEscritorio;
      this.editProfissional = this.fornecedorParaEditar.codigoProfissional;
      this.campoNomeFornecedor = this.fornecedorParaEditar.nomeFornecedor;
      this.campoFornecedorSap = this.fornecedorParaEditar.codigoFornecedorSap;

      this.registerForm.get('tipoFornecedorBox').setValue(this.editCodFornecedor);
      this.registerForm.get('bancoBox').setValue(this.editBanco);
      this.registerForm.get('escritorioBox').setValue(this.editEscritorio);
      this.registerForm.get('profissionalBox').setValue(this.editProfissional);
      this.registerForm.get('nomeFornecedor').setValue(this.campoNomeFornecedor);
      this.registerForm.get('codigoSap').setValue(this.campoFornecedorSap);

      this.idFornecedor = this.fornecedorParaEditar.id;



    }

  }

  converterCodigoEmNomeProcesso(tipoProcesso: number, id: number) {

    let nome
    if (tipoProcesso == 1) {
      nome = this.bancos.find(obj => obj.id == id);
      this.registerForm.get('bancoBox').setValue(id)

    } else if (tipoProcesso == 3) {
      nome = this.escritorios.find(obj => obj.id == id);
      this.registerForm.get('escritorioBox').setValue(id)
    } else {
      nome = this.profissionais.find(obj => obj.id == id);
      this.registerForm.get('profissionalBox').setValue(id)
    }


    //this.editEscritorio = nome.descricao;
    this.registerForm.get('nomeFornecedor').setValue(nome.descricao)
    //return nome.descricao;

  }


  setValores(){



  }



}



