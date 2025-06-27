import { Component, ElementRef, OnInit, QueryList, ViewChildren } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { ParametrizarDistribuicaoProcessosService } from '../../services/parametrizar-distribuicao-processos.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { DialogService } from '@shared/services/dialog.service';
import { AnexoModalComponent } from '../anexo-modal/anexo-modal.component';
import { StaticInjector } from '../../static-injector';

@Component({
  selector: 'app-nova-parametrizacao-modal',
  templateUrl: './nova-parametrizacao-modal.component.html',
  styleUrls: ['./nova-parametrizacao-modal.component.scss']
})
export class NovaParametrizacaoModalComponent implements OnInit {
  @ViewChildren('percProcessoNovo') inputsPercProcessoNovo: QueryList<ElementRef>;

  constructor(
    private modal: NgbActiveModal,
    private messageService: HelperAngular,
    private dialog: DialogService,
    private service: ParametrizarDistribuicaoProcessosService
  ) { }

  ngOnInit() {
    this.obterTodos();
    // this.buscarEscritorios();
    this.buscarSolicitantes();
  }

  listaAnexo: number[] = [0];

  //#region MÉTODOS
  static async exibeModal(): Promise<boolean> {
    const modalRef = await StaticInjector.Instance.get(NgbModal)
      .open(NovaParametrizacaoModalComponent, { centered: true, backdrop: true, size: 'xl', backdropClass: 'modal-backdrop-parametrizar' });

    let retorno = modalRef.result.then((res) => {
      return res
    }, () => {
      return false
    });
    return retorno;
  }

  async confirm() {
    let distribuicaoRequest = []
    let escritorioRequest = []
    let isValid = true;

    for (let i = 0; this.novaDistribuicao.length > i; i++) {
      let distribuicao = {
        codEstado: this.novaDistribuicao[i].uf,
        codComarca: this.novaDistribuicao[i].comarca,
        codigos: this.novaDistribuicao[i].vara.codigos,
        // codVara: this.novaDistribuicao[i].vara.codVara,
        // codTipoVara: this.novaDistribuicao[i].vara.codTipoVara,
        codTipoProcesso: this.novaDistribuicao[i].natureza,
        codEmpresaCentralizadora: this.novaDistribuicao[i].empresa,
        codParamDistribuicao: 0,
        indAtivo: true
      }

      this.changeValidOrInvalid(i, true)
      if (await this.service.validarParametrizacao(distribuicao)) {
        this.changeValidOrInvalid(i, false)
        isValid = false;
      }

      distribuicaoRequest.push(distribuicao)
    }

    if (!isValid) return await this.dialog.err('Parâmetro Distribuição já existe!', 'A(s) chave(s) destacada(s) em vermelho já esta(ão) cadastrada(s) no SISJUR');

    this.checkPrioridade();

    for (let i = 0; i < this.novoEscritorio.length; i++) {
      let escritorio = {
        codProfissional: this.novoEscritorio[i].escritorio,
        codSolicitante: this.novoEscritorio[i].solicitante,
        porcentagemProcessos: Number(this.novoEscritorio[i].percProcesso.toString().replace(',', '.')),
        datVigenciaInicial: new Date(this.novoEscritorio[i].dataIni),
        datVigenciaFinal: new Date(this.novoEscritorio[i].dataFim),
        prioridade: this.novoEscritorio[i].prioridade
      }
      escritorioRequest.push(escritorio)
    }

    let request = {
      parametrizacoes: distribuicaoRequest,
      escritorios: escritorioRequest,
      anexos: this.listaAnexo
    };

    try {
      await this.service.SalvarParametrizacaoEmLoteAsync(request)
    } catch (error) {
      return this.messageService.MsgBox2(error.error, 'A inclusão não poderá ser realizada', 'warning', 'Ok');
    }

    this.messageService.MsgBox2('A nova distribuição foi parametrizada com sucesso!', 'Inclusão realizada com sucesso', 'success', 'Ok').then(res => {
      if (res.value) {
        this.modal.close(true);
      }
    });

  }
  //#endregion

  //#region  FUNÇÕES

  async alterarAnexo(): Promise<void> {
    const listaAnexos = await AnexoModalComponent.exibeModalAnexoNovaParametrizacao(this.listaAnexo);
    if (listaAnexos)
      this.listaAnexo = listaAnexos;
  }
  //#endregion

  //#region  FUNÇÕES

  close(): void {
    this.modal.close(false);
  }

  validarParametrizacao() {
    let campoVazio = false;
    let dataInvalida = false;

    if (this.novaDistribuicao.length == 0 || this.novoEscritorio.length == 0)
      return this.messageService.MsgBox2('Deve existir pelo menos uma chave de parametrização e um escritorio associado', 'A inclusão não poderá ser realizada', 'warning', 'Ok');

    for (let i = 0; i < this.novaDistribuicao.length; i++) {
      let dist = this.novaDistribuicao[i];
      if (dist.uf == '' || dist.comarca == null || dist.vara.codigos == null || dist.natureza == null || dist.empresa == null)
        campoVazio = true
    };
    for (let i = 0; i < this.novoEscritorio.length; i++) {
      let dist = this.novoEscritorio[i];
      if (dist.escritorio == -1 || dist.solicitante == -1 || dist.percProcesso == null || dist.percProcesso == '' || dist.dataIni == null || dist.dataIni == '' || dist.dataFim == null || dist.dataFim == '')
        campoVazio = true
      if (dist.dataIni > dist.dataFim || dist.dataIni > dist.dataFim) {
        dataInvalida = true
      }
    };

    if (campoVazio)
      return this.messageService.MsgBox2('O preenchimento de todos os campos da parametrização e do escritório são obrigatorio', 'A inclusão não poderá ser realizada', 'warning', 'Ok');

    if (dataInvalida)
      return this.messageService.MsgBox2('As datas da vigência informadas estão inválidas', 'A inclusão não poderá ser realizada', 'warning', 'Ok');

    let ehDuplicado = this.campoDuplicado();
    if (ehDuplicado) {

      return this.messageService.MsgBox2('As chaves destacadas em vermelho estão duplicadas. Verifique e tente novamente', 'A inclusão não poderá ser realizada', 'warning', 'Ok');
    }

    this.messageService.MsgBox2('Confirma a inclusão da distribuição parametrizada?', 'Nova Parametrização',
      'question', 'Sim, confirmo a inclusão', 'Não, quero conferir antes').then(res => {
        if (res.value) {
          this.confirm()
        }
      });
  }

  campoDuplicado() {
    let idsParam = [];
    let idsEsc = [];
    const parametrizacaoIguais = this.novaDistribuicao.some((obj, index, self) => {
      const isDuplicate = index !== self.findIndex((o) =>
        o.uf === obj.uf &&
        o.comarca === obj.comarca &&
        o.vara.codigos === obj.vara.codigos &&
        // o.vara.codTipoVara === obj.vara.codTipoVara &&
        // o.vara.codVara === obj.vara.codVara &&
        o.natureza === obj.natureza &&
        o.empresa === obj.empresa
      );
      this.changeValidOrInvalid(index, true);
      if (isDuplicate) {
        idsParam.push(index);
      }
      return isDuplicate;
    });

    if (idsParam.length > 0) {
      idsParam.forEach((index) => {
        this.changeValidOrInvalid(index, false);
      });
    } else {
      this.changeValidOrInvalid(-1, true);
    }

    const escritoriosIguais = this.novoEscritorio.some((obj, index, self) => {
      const isDuplicate = index !== self.findIndex((o) =>
        o.escritorio === obj.escritorio &&
        o.solicitante === obj.solicitante &&
        o.dataIni.toString() === obj.dataIni.toString() &&
        o.dataFim.toString() === obj.dataFim.toString()
      );
      // this.changeValidOrInvalidEscritorio(index, isDuplicate);
      if (isDuplicate) {
        idsEsc.push(index);
      }
      return isDuplicate;
    });


    if (idsEsc.length > 0) {
      idsEsc.forEach((index) => {
        this.changeValidOrInvalidEscritorio(index, false);
      });
    } else {
      for (let index = 0; this.novoEscritorio.length > index; index++) {
        this.changeValidOrInvalidEscritorio(index, true);
      }
    }

    return parametrizacaoIguais || escritoriosIguais
  }

  changeValidOrInvalid(index, isValid) {
    const attributeName = 'ng-reflect-name';
    const select = Array.from(document.querySelectorAll(`[${attributeName}="row_${index}"]`)) as HTMLElement[];
    if (!isValid) {
      for (let i = 0; i < select.length; i++) {
        select[i].classList.add('ng-invalid');
        select[i].classList.add('ng-touched');
      }
      return;
    }
    for (let i = 0; i < select.length; i++) {
      select[i].classList.remove('ng-invalid');
      select[i].classList.remove('ng-touched');
    }
    return;
  }

  changeValidOrInvalidEscritorio(index, isValid) {
    const attributeName = 'ng-reflect-name';
    const select = Array.from(document.querySelectorAll(`[${attributeName}="row_esc_${index}"]`)) as HTMLElement[];
    if (!isValid) {
      for (let i = 0; i < select.length; i++) {
        select[i].classList.add('ng-invalid');
        select[i].classList.add('ng-touched');
      }
      return;
    }
    for (let i = 0; i < select.length; i++) {
      select[i].classList.remove('ng-invalid');
      select[i].classList.remove('ng-touched');
    }
    return;
  }

  async clearListEscritorios(indice: number) {
    let primeiraNatureza = false;
    let resposta: boolean = false;
    if (this.novaDistribuicao[indice].ultimaNatureza == null) {
      primeiraNatureza = true;
    }

    if (!primeiraNatureza) {
      resposta = await this.dialog.confirm('Alteração de Natureza', 'Você está alterando a natureza de uma das chaves de distribuição.<br>Se continuar, será necessário que parametrize os escritórios novamente.')
    }

    if (resposta || primeiraNatureza) {
      this.novaDistribuicao[indice].ultimaNatureza = this.novaDistribuicao[indice].natureza;
      let recarregar = false;

      this.naturezaSelecionadasList = [];
      this.novaDistribuicao.forEach(e => {
        if (!this.naturezaSelecionadasList.find(x => x == e.natureza)) {
          if (e.natureza) {
            this.naturezaSelecionadasList.push(e.natureza)
            recarregar = true;
          }
        }
      })

      if (recarregar) {
        this.novoEscritorio = [{
          id: 0,
          escritorio: -1,
          solicitante: -1,
          percProcesso: '',
          prioridade: null,
          dataIni: '',
          dataFim: ''
        }]
        await this.buscarEscritorios(this.naturezaSelecionadasList);
      }

    } else {
      this.novaDistribuicao[indice].natureza = this.novaDistribuicao[indice].ultimaNatureza;
    }
  }

  //#endregion


  //#region DISTRIBUIÇÃO
  ufList = [];
  empCentList = [];
  comarcaList = [];
  varaList = [];
  naturezaList = [];
  naturezaSelecionadasList = [];

  novaDistribuicao = [{
    id: 0,
    uf: '',
    comarca: null,
    vara: {
      codigos: null,
      nome: ""
    },
    natureza: null,
    ultimaNatureza: null,
    empresa: null
  }];

  optList = [{
    ufList: [],
    empCentList: [],
    comarcaList: [],
    varaList: [],
    naturezaList: []
  }]

  adicionarDistribuicao() {
    let index = this.novaDistribuicao.length
    let escritorio = {
      id: index++,
      uf: '',
      comarca: null,
      vara: {
        codigos: null,
        nome: ""
      },
      natureza: null,
      ultimaNatureza: null,
      empresa: null
    };

    let opts = {
      ufList: [],
      empCentList: [],
      comarcaList: [],
      varaList: [],
      naturezaList: []
    }
    this.optList.push(opts)
    this.obterTodos(index--)

    this.novaDistribuicao.push(escritorio)
  }

  async removerDistribuicao(item) {
    let naturezaPreenchida = false;
    let resposta = false;

    if (item.natureza)
      naturezaPreenchida = true

    if (naturezaPreenchida) {
      resposta = await this.dialog.confirm(
        'Exclusão de Chave de Distribuição',
        'Você está removendo uma das chaves de distribuição.<br> Se continuar, será necessário que parametrize os escritórios novamente.'
      )
    }

    if (resposta || !naturezaPreenchida) {
      let index = this.novaDistribuicao.indexOf(item)
      this.novaDistribuicao.splice(index, 1);
      this.naturezaSelecionadasList = [];
      this.novaDistribuicao.forEach(e => {
        if (!this.naturezaSelecionadasList.find(x => x == e.natureza)) {
          if (e.natureza) {
            this.naturezaSelecionadasList.push(e.natureza)
          }
        }
      })

      if (naturezaPreenchida) {
        this.novoEscritorio = [{
          id: 0,
          escritorio: -1,
          solicitante: -1,
          percProcesso: '',
          prioridade: null,
          dataIni: '',
          dataFim: ''
        }];
        await this.buscarEscritorios(this.naturezaSelecionadasList);
      }
    }
  }

  async obterTodos(indice?: number) {
    let index = indice ? indice - 1 : 0
    this.buscarUf(index)
    this.obterEmpresaCentralizadora(index);
    this.obterComarca(null, index);
    this.buscarNatureza(index);
    this.obterVara(null, index);
  }

  async buscarUf(indice?: number) {
    await this.service.obterUf().then(x => {
      this.optList[indice].ufList = x;
    });
  }

  async buscarNatureza(indice?: number) {
    await this.service.obterNatureza().then(x => {
      this.optList[indice].naturezaList = x;
    });
  }

  async obterEmpresaCentralizadora(indice?: number) {
    await this.service.obterEmpresasCentralizadora().then(x => {
      this.optList[indice].empCentList = x;
    });
  }

  async obterComarca(uf: string, indice: number) {
    await this.service.obterComarca(uf).then(x => {
      this.optList[indice].comarcaList = x;
    });
  }

  async obterVara(codComarca?: number, indice?: number, natureza?: number) {
    await this.service.obterVara(codComarca, natureza).then(x => {
      this.optList[indice].varaList = x;
      this.novaDistribuicao[indice].vara = {
        codigos: null,
        nome: ""
      }
    });
  }

  async fazerBuscaComarca(uf, indice) {
    await this.obterComarca(uf, indice)
    await this.obterVara(null, indice)
    this.novaDistribuicao[indice].comarca = null
    this.novaDistribuicao[indice].vara = {
      codigos: null,
      nome: ""
    }
  }
  //#endregion

  //#region Escritorio
  escritorioList = [];
  solicitanteList = [];

  novoEscritorio = [{
    id: 0,
    escritorio: -1,
    solicitante: -1,
    percProcesso: '',
    prioridade: null,
    dataIni: '',
    dataFim: ''
  }];

  async buscarEscritorios(naturezaList: number[]) {
    await this.service.obterEscritorios(naturezaList).then(x => {
      this.escritorioList = x;
    });
  }

  async buscarSolicitantes() {
    await this.service.obterSolicitantes().then(x => {
      this.solicitanteList = x;
    });
  }

  adicionarEscritorio() {
    let index = this.novoEscritorio.length
    let escritorio = {
      id: index++,
      escritorio: -1,
      solicitante: -1,
      percProcesso: '',
      prioridade: null,
      dataIni: '',
      dataFim: ''
    };
    this.novoEscritorio.push(escritorio)
  }

  removerEscritorio(item) {
    let index = this.novoEscritorio.indexOf(item)
    this.novoEscritorio.splice(index, 1);
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.novoEscritorio, event.previousIndex, event.currentIndex);
    this.checkPrioridade()
  }

  checkPrioridade() {
    for (let i = 0; i < this.novoEscritorio.length; i++) {
      this.novoEscritorio[i].prioridade = i + 1;
    }
  }

  distribuirPercentual() {
    let percentualRestante = 100;
    let percentualTotalAtual = 0;
    const percentuaisPreenchidos = this.novoEscritorio.filter(x => x.percProcesso != '')
    const inputsVazios = this.novoEscritorio.filter(x => x.percProcesso == '')
    percentuaisPreenchidos.forEach(x => {
      percentualTotalAtual += Number(x.percProcesso.replace(',', '.'))
    })
    percentualRestante -= percentualTotalAtual;
    const percentualADistribuir = Number((percentualRestante / inputsVazios.length).toFixed(2));

    if (percentualRestante > 0 && inputsVazios.length > 0) {
      let somaValoresAtribuidos = 0;

      inputsVazios.forEach((campoPerc, index) => {
        const valorAtribuir = (index === inputsVazios.length - 1)
          ? (percentualRestante - somaValoresAtribuidos).toFixed(2)
          : percentualADistribuir;

        let valorFinal = valorAtribuir.toString().includes('.00') ? valorAtribuir.toString().replace('.00', '') : valorAtribuir.toString().replace('.', ',');
        campoPerc.percProcesso = valorFinal;
        somaValoresAtribuidos += Number(valorAtribuir.toString().replace(',', '.'));
      });
    } else {
      inputsVazios.forEach(campoPerc => {
        campoPerc.percProcesso = '0';
      });
    }
  }


  //#endregion  
}
