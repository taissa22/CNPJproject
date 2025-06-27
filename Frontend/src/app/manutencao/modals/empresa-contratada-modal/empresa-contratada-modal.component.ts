import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { EmpresaContratadaService } from '@manutencao/services/empresa-contratada.service';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-empresa-contratada-modal',
  templateUrl: './empresa-contratada-modal.component.html',
  styleUrls: ['./empresa-contratada-modal.component.scss']
})
export class EmpresaContratadaModalComponent implements OnInit {

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: EmpresaContratadaService,
    private formBuilder: FormBuilder,
  ) { }

  ngOnInit() {
  }
  operacao = '';
  codEmpresaContratada = 0;
  nomeFormControl : FormControl = new FormControl('',[Validators.required]);
  matriculaBuscaFormControl : FormControl = new FormControl('');
  matriculasSelecionadas = [];
  matriculasFiltradas = [];

  formGroup: FormGroup = this.formBuilder.group({
    codEmpresaContratada: this.codEmpresaContratada,
    nomEmpresaContratada: this.nomeFormControl,
    matriculas:null
  });

  public static exibeModalDeIncluir(): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EmpresaContratadaModalComponent, { windowClass: 'empresa-contratada-modal', centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.operacao = "Inclusão";
      return modalRef.result;
  }

  public static exibeModalDeAlterar(codEmpresaContratada: number, nomEmpresaContratada: string): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EmpresaContratadaModalComponent, { windowClass: 'empresa-contratada-modal', centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.operacao = "Alteração";
      modalRef.componentInstance.codEmpresaContratada = codEmpresaContratada;
      modalRef.componentInstance.nomeFormControl.setValue(nomEmpresaContratada);
      modalRef.componentInstance.obterEmpresaContratadaAsync();
    return modalRef.result;
  }

  filtrarMatriculas(){

    if (!this.matriculaBuscaFormControl.value || this.matriculaBuscaFormControl.value.trim() === '') {
      this.matriculasFiltradas = this.matriculasSelecionadas;
      return;
    }
    
    this.matriculasFiltradas = this.matriculasSelecionadas.filter(x => {
      if (typeof x.loginTerceiro === 'string') {
        // return x.loginTerceiro.includes(this.matriculaBuscaFormControl.value.toUpperCase());
        return x.loginTerceiro === this.matriculaBuscaFormControl.value.toUpperCase();
      }
      return false;
    });
  }

  adicionarMatricula() {
    if(!this.matriculasSelecionadas.find(x => x.loginTerceiro == this.matriculaBuscaFormControl.value.toUpperCase()) && this.matriculaBuscaFormControl.value.trim() != ''){
      this.matriculasSelecionadas.push({codTerceiroContratado: 0, loginTerceiro: this.matriculaBuscaFormControl.value.toUpperCase()})
      this.filtrarMatriculas();
    }
  }

  removerMatricula(loginTerceiro: string){
    const index = this.matriculasSelecionadas.findIndex(x => x.loginTerceiro == loginTerceiro);
    if (index !== -1) {
      this.matriculasSelecionadas.splice(index, 1);
      this.filtrarMatriculas();
    }
  }

  onSpaceKeyDown(event: KeyboardEvent) {
    if (event.key === ' ') {
      event.preventDefault(); // Impede a inserção do espaço em branco
    }
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  close(): void {
    this.modal.close(false);
  }

  async save(){
    if(!this.matriculasSelecionadas || this.matriculasSelecionadas.length == 0){
      await this.dialogService.err(
        `${this.operacao} não realizada`,
        `A Empresa Contratada deve conter ao menos 1 matrícula.`
      );
      return;
    }

    this.formGroup.get('codEmpresaContratada').setValue(this.codEmpresaContratada);
    this.formGroup.get('matriculas').setValue(this.matriculasSelecionadas);
    
    if(this.codEmpresaContratada == 0){
      try{
        await this.service.salvarEmpresaContratadaAsync(this.formGroup.value);      
        await this.dialogService.alert('Cadastrar Empresa Contratada.', "Empresa Contratada incluído com sucesso.");
        this.modal.close(true);
      } catch (error) {      
        return this.dialogService.err('Cadastrar Empresa Contratada', error );
      }
    }else{
      try{
        await this.service.editarEmpresaContratadaAsync(this.formGroup.value);      
        await this.dialogService.alert('Editar Empresa Contratada.', "Empresa Contratada alterada com sucesso.");
        this.modal.close(true);
      } catch (error) {      
        return this.dialogService.err('Editar Empresa Contratada', error );
      }
    }

  }

  async obterEmpresaContratadaAsync(){
    try {
      const empresaContratadaRetorno = await this.service.obterEmpresaContratadaAsync(this.codEmpresaContratada, this.nomeFormControl.value);

      this.matriculasSelecionadas = empresaContratadaRetorno.matriculas;
      this.filtrarMatriculas();
    } catch (error) {
      await this.dialogService.err(
        "Erro ao buscar.",
        "Não foi possível realizar a busca da Empresa Contratada."
      );
    }
  }

}
