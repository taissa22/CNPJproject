import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Permissao } from '@manutencao/models/permissao.model';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PermissaoService } from '@manutencao/services/permissao.service';
import { DialogService } from '@shared/services/dialog.service';
import { HttpErrorResult } from '@core/http';

@Component({
  selector: 'app-permissao-modal',
  templateUrl: './permissao-modal.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PermissaoModalComponent implements OnInit {

  constructor(
    private modal: NgbActiveModal,
    private service: PermissaoService,
    private dialogService: DialogService,
    private formBuilder: FormBuilder,
    private cdRef: ChangeDetectorRef
  ) {
  }

  ngOnInit() {
    this.permissaoForm.patchValue(this.permissao)
  }

  permissaoForm: FormGroup;

  permissao: Permissao;

  modulos = [];

  public static exibeModalDeAlterar(item: any, listaModulos: any, modulos: any) {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(PermissaoModalComponent, { centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.permissao = item;
    modalRef.componentInstance.modulos = modulos;
    modalRef.componentInstance.criarForm(listaModulos);

    return modalRef.result;
  }

  criarForm(valor: any) {
    this.permissaoForm = this.formBuilder.group({
      permissaoId: ['', Validators.required],
      descricao: ['', Validators.required],
      caminho: [''],
      listaModulos: this.marcarModulos(valor)
    });
  }

  marcarModulos(valor: any) {
    let moduloMarcado = [];
    const listaModulosString = valor.replaceAll('; ',';');
    const listaModulos = listaModulosString.split(';');

    this.modulos.forEach(function (modulo) {
      listaModulos.find(item => item == modulo.descricao) ? moduloMarcado.push(new FormControl(true)) : moduloMarcado.push(new FormControl(false))
    });

    return this.formBuilder.array(moduloMarcado)
  }

  async save(): Promise<void> {
    let valueSubmit = Object.assign({}, this.permissaoForm.value);
    valueSubmit = Object.assign(valueSubmit, {
      listaModulos: valueSubmit.listaModulos
        .map((value, index) => value ? this.modulos[index].id : null)
        .filter(value => value !== null)
    })
    valueSubmit.caminho = valueSubmit.caminho == null ? '' : valueSubmit.caminho;

    console.log(valueSubmit);

    try {
      await this.service.salvar(valueSubmit);
      await this.dialogService.alert(`Alteração realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`Alteração não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  close(): void {
    this.modal.close(false);
  }
}
