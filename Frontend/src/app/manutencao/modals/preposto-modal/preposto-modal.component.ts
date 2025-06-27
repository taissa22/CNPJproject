import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { Preposto } from '@manutencao/models/preposto.model';
import { PrepostoService } from '@manutencao/services/preposto.service';
import { HttpErrorResult } from '@core/http';
import { Usuario } from '@manutencao/models/usuario';
//import { EstadoEnum } from '@manutencao/models';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { AlocacaoModalComponent } from './alocacao-preposto/alocacao-preposto.component';
import { TipoProcesso } from '@core/models/tipo-processo';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { List } from 'linqts';
import { NullVisitor } from '@angular/compiler/src/render3/r3_ast';

@Component({
  selector: 'app-preposto-modal',
  templateUrl: './preposto-modal.component.html',
  styleUrls: ['./preposto-modal.component.scss']
})
export class PrepostoModalComponent implements OnInit {
  preposto: Preposto;
  usuarioLista: Array<Usuario>;
  // estadoLista : Array<EstadoEnum>;
  alocado: boolean;
  usuarioAtualId: string;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private servicePreposto: PrepostoService,
  ) { }

  nomeFormControl: FormControl = new FormControl('', [Validators.required, Validators.maxLength(50)]);
  //estadoIdFormControl: FormControl = new FormControl(null,[Validators.required]);
  ativoFormControl: FormControl = new FormControl(true);
  ehCivelEstrategicoFormControl: FormControl = new FormControl(false);
  ehCivelFormControl: FormControl = new FormControl(false);
  ehTrabalhistaFormControl: FormControl = new FormControl(false);
  ehJuizadoFormControl: FormControl = new FormControl(false);
  usuarioIdFormControl: FormControl = new FormControl();
  ehProconFormControl: FormControl = new FormControl(false);
  ehPexFormControl: FormControl = new FormControl(false);
  ehEscritorioFormControl: FormControl = new FormControl(false);
  matriculaFormControl: FormControl = new FormControl('', [Validators.maxLength(10)]);

  formGroup: FormGroup = new FormGroup({
    nome: this.nomeFormControl,
    //  estadoId : this.estadoIdFormControl,
    ativo: this.ativoFormControl,
    ehCivelEstrategico: this.ehCivelEstrategicoFormControl,
    ehCivel: this.ehCivelFormControl,
    ehTrabalhista: this.ehTrabalhistaFormControl,
    ehJuizado: this.ehJuizadoFormControl,
    usuarioId: this.usuarioIdFormControl,
    ehProcon: this.ehProconFormControl,
    ehPex: this.ehPexFormControl,
    ehEscritorio: this.ehEscritorioFormControl,
    matricula: this.matriculaFormControl,
  });

  ngOnInit(): void {
    //this.estadoLista = EstadoEnum.Todos;
    this.InicilizaForm();
  }

  InicilizaForm() {
    this.alocado = false;

    if (this.preposto) {
      this.nomeFormControl.setValue(this.removerCaracter(this.preposto.nome));
      this.ativoFormControl.setValue(this.preposto.ativo);
      this.ehCivelEstrategicoFormControl.setValue(this.preposto.ehCivelEstrategico);
      this.ehCivelFormControl.setValue(this.preposto.ehCivel);
      this.ehTrabalhistaFormControl.setValue(this.preposto.ehTrabalhista);
      this.ehJuizadoFormControl.setValue(this.preposto.ehJuizado);
      this.usuarioIdFormControl.setValue(this.preposto.usuarioId);
      this.ehProconFormControl.setValue(this.preposto.ehProcon);
      this.ehPexFormControl.setValue(this.preposto.ehPex);
      this.ehEscritorioFormControl.setValue(this.preposto.ehEscritorio);
      //this.estadoIdFormControl.setValue(this.preposto.estadoId);
      this.matriculaFormControl.setValue(this.removerCaracter(this.preposto.matricula).split(' ').join(''));
      this.usuarioAtualId = this.usuarioIdFormControl.value;

      if (this.preposto.usuarioId && !this.usuarioLista.some(usuario => usuario.id == this.preposto.usuarioId)) {
        this.usuarioLista.push(Usuario.fromObj({ id: this.preposto.usuarioId, nome: this.preposto.nomeUsuario, ativo: this.preposto.usuarioAtivo, nomeCompleto: '' }))
        this.usuarioLista.sort((a, b) => a.nome.localeCompare(b.nome));
      }
    }

  }

  close(): void {
    this.modal.close(false);
  }

  async carregarTodosPrepostos(): Promise<void> {
    this.usuarioLista = await this.servicePreposto.obterTodosPrepostos();
  }

  processoAlterado(): Array<number> {
    let tiposProcessoDesmarcados: Array<number> = [];

    if (this.preposto.ehCivelEstrategico && (this.ehCivelEstrategicoFormControl.value != this.preposto.ehCivelEstrategico)) {
      tiposProcessoDesmarcados.push(TiposProcesso.CIVEL_ESTRATEGICO.id);
    }

    if (this.preposto.ehCivel && (this.preposto.ehCivel != this.ehCivelFormControl.value)) {
      tiposProcessoDesmarcados.push(TiposProcesso.CIVEL_CONSUMIDOR.id)
    }

    if (this.preposto.ehJuizado && (this.preposto.ehJuizado != this.ehJuizadoFormControl.value)) {
      tiposProcessoDesmarcados.push(TiposProcesso.JEC.id);
    }

    if (this.preposto.ehTrabalhista && (this.preposto.ehTrabalhista != this.ehTrabalhistaFormControl.value)) {
      tiposProcessoDesmarcados.push(TiposProcesso.TRABALHISTA.id);
    }

    if (this.preposto.ehProcon && (this.preposto.ehProcon != this.ehProconFormControl.value)) {
      tiposProcessoDesmarcados.push(TiposProcesso.PROCON.id);
    }

    if (this.preposto.ehPex && (this.preposto.ehPex != this.ehPexFormControl.value)) {
      tiposProcessoDesmarcados.push(TiposProcesso.PEX.id);
    }

    return tiposProcessoDesmarcados;
  }

  async verificarAlocacao(): Promise<void> {
    try {
      this.alocado = false;
      const processosAlterados: Array<number> = this.processoAlterado();
      if (processosAlterados.length > 0) {
        const result = await this.servicePreposto.estaAlocado(processosAlterados, this.preposto.id).toPromise();
        this.alocado = result;
      }

    }
    catch (error) {
      await this.dialogService.err(`Informações não carregadas`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  verificaCheckbox() {
    this.matriculaFormControl = new FormControl('', [Validators.maxLength(10)]);

    if ((this.ehCivelFormControl.value || this.ehJuizadoFormControl.value || this.ehProconFormControl.value) && !this.ehEscritorioFormControl.value) {

      if (this.preposto != undefined && this.preposto != null && this.preposto.matricula != "") {
        this.matriculaFormControl = new FormControl('', Validators.maxLength(10));
        this.matriculaFormControl.setValue(this.preposto.matricula);
      }
      else {
        this.matriculaFormControl = new FormControl('', Validators.maxLength(10));
      }

    }

    this.formGroup = new FormGroup({
      nome: this.nomeFormControl,
      // estadoId : this.estadoIdFormControl,
      ativo: this.ativoFormControl,
      ehCivelEstrategico: this.ehCivelEstrategicoFormControl,
      ehCivel: this.ehCivelFormControl,
      ehTrabalhista: this.ehTrabalhistaFormControl,
      ehJuizado: this.ehJuizadoFormControl,
      usuarioId: this.usuarioIdFormControl,
      ehProcon: this.ehProconFormControl,
      ehPex: this.ehPexFormControl,
      ehEscritorio: this.ehEscritorioFormControl,
      matricula: this.matriculaFormControl
    });
  }

  async save(): Promise<void> {

    const operacao = this.preposto ? 'Alteração' : 'Inclusão';

    /* var caracteresNaoPermitidos = /^[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]*$/;

    if (this.nomeFormControl.value.match(caracteresNaoPermitidos)) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `O campo nome não pode conter caracteres especiais.`
      );
      return;
    } */

    if (!this.nomeFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `O campo nome não pode conter apenas espaços.`
      );
      return;
    }

    if ((this.ehCivelFormControl.value || this.ehJuizadoFormControl.value || this.ehProconFormControl.value) && !this.ehEscritorioFormControl.value) {
      if (this.matriculaFormControl.value == null || !this.matriculaFormControl.value.replace(/\s/g, '').length) {
        await this.dialogService.err(
          `${operacao} não realizada`,
          `O campo matrícula não pode conter apenas espaços.`
        );
        return;
      }
    }

    const idPrepostoDuplicidade = operacao == 'Alteração' ? this.preposto.id : 0;
    const prepostoComNomeDuplicado = await this.servicePreposto.ValidarDuplicidadeDeNomePreposto(this.nomeFormControl.value, idPrepostoDuplicidade).toPromise();

    if (prepostoComNomeDuplicado != null) {
      let resposta: Boolean;
      let matriculaFormatada = prepostoComNomeDuplicado.matricula == null || prepostoComNomeDuplicado.matricula == undefined ? '(Não informada)' : prepostoComNomeDuplicado.matricula
      resposta = await this.dialogService.confirm(
        `Existe um outro preposto ativo com o mesmo nome cadastrado com a matrícula ${matriculaFormatada}.`,
        `Deseja prosseguir?`
      );
      if (!resposta) {
        return;
      } else {
        await this.IncluirOuAlterar(operacao);
      }
    } else {
      await this.IncluirOuAlterar(operacao);
    }

  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(usuarios: Array<Usuario>): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(PrepostoModalComponent, { windowClass: 'modal-preposto', centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.usuarioLista = usuarios;
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(preposto: Preposto, usuarios: Array<Usuario>): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(PrepostoModalComponent, { windowClass: 'modal-preposto', centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.preposto = preposto;
    modalRef.componentInstance.usuarioLista = usuarios;
    return modalRef.result;
  }

  async validaUsuarioSelecionado(usuario: Usuario) {
    if (!usuario.ativo) {
      await this.dialogService.err('Usuário Inativo', 'Não é possível selecionar um usuário inativo.')
      this.usuarioIdFormControl.setValue(this.usuarioAtualId);
    }
  }

  private async IncluirOuAlterar(operacao: string) {
    try {

      if(this.usuarioIdFormControl.value==null)
      this.matriculaFormControl.setValue(null);

      if (this.preposto) {
        await this.verificarAlocacao();
        if (this.alocado) {
          await AlocacaoModalComponent.exibeModal(this.preposto, this.processoAlterado());
          this.modal.close(false);
        }

        else
          await this.servicePreposto.alterar({ ...this.formGroup.value, id: this.preposto.id });

      } else {
        await this.servicePreposto.incluir(this.formGroup.value);
      }

      if (!this.alocado) {
        await this.dialogService.alert(`${operacao} realizada com sucesso`);
      }
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`Desculpe, não foi possivel a ${operacao}`, (error as HttpErrorResult).messages.join('\n'));
    };
  }

  validarCaracterEspecial() {
    this.nomeFormControl.setValue(this.removerCaracter(this.nomeFormControl.value));
    this.matriculaFormControl.setValue(this.removerCaracter(this.matriculaFormControl.value).split(' ').join(''));
  }

  removerCaracter(texto: string): string {
    if (texto == null ) return texto;

    const a = 'àáäâãèéëêìíïîõòóöôùúüûñçßÿœæŕśńṕẃǵǹḿǘẍźḧ'
    const b = 'aaaaaeeeeiiiiooooouuuuncsyoarsnpwgnmuxzh'
    const p = new RegExp(a.split('').join('|'), 'g')

    texto = texto.split(' / ').join('');
    texto = texto.split(' /').join('');
    texto = texto.split('/').join('Þ');
    texto = texto.split(' . ').join('');
    texto = texto.split(' .').join('');
    texto = texto.split(' ').join('<>');
    texto = texto.split('><').join('');
    texto = texto .split('<>').join(' ');

    texto = texto.toString().toLowerCase().trim()
      .replace(p, c => b.charAt(a.indexOf(c))) // Replace special chars
      .replace(' . ', ' ')
      .replace(' - ', ' ');

    texto = texto.replace(/[^a-z0-9\-s \- \Þ  \.]/gi, "");
    texto = texto.split('þ').join('/');
    texto = texto.trim();

    return texto;


    //   texto = texto.split('!').join(' ');
    //   texto = texto.split('#').join(' ');
    //   texto = texto.split('%').join(' ');
    //   texto = texto.split('&').join(' ');
    //   texto = texto.split('(').join(' ');
    //   texto = texto.split('_').join(' ');
    //   texto = texto.split('}').join(' ');
    //   texto = texto.split(']').join(' ');
    //   texto = texto.split('º').join(' ');
    //   texto = texto.split(';').join(' ');
    //   texto = texto.split('|').join(' ');
    //   texto = texto.split('/').join(' ');
    //   texto = texto.split('<').join(' ');
    //   texto = texto.split('@').join(' ');
    //   texto = texto.split('$').join(' ');
    //   texto = texto.split('¨').join(' ');
    //   texto = texto.split('*').join(' ');
    //   texto = texto.split(')').join(' ');
    //   texto = texto.split('{').join(' ');
    //   texto = texto.split('[').join(' ');
    //   texto = texto.split('ª').join(' ');
    //   texto = texto.split('?').join(' ');
    //   texto = texto.split(':').join(' ');
    //   texto = texto.split(',').join(' ');
    //   texto = texto.split('+').join(' ');
    //   texto = texto.split('>').join(' ');
    //   texto = texto.split('²').join(' ');
    //   texto = texto.split('³').join(' ');
    //   texto = texto.split('?').join(' ');
    //   texto = texto.split('"').join(' ');
    //   texto = texto.split('¹').join(' ');
    //   texto = texto.split("\\").join(' ');
    //   texto = texto.split("'").join(' ');
    //   texto = texto.split("^").join(' ');
    //   texto = texto.split("~").join(' ');
    //   texto = texto.split("´").join(' ');
    //   texto = texto.split("`").join(' ');

    //   texto = texto.split(' ').join('<>');
    //   texto = texto.split('><').join('');
    //   texto = texto .split('<>').join(' ');
    // return texto;

  }

}

