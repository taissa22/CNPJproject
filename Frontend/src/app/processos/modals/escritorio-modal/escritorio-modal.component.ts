import { DialogService } from '@shared/services/dialog.service';
import { stringify } from 'querystring';
import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { DistribuicaoModel } from '../../models/parametrizar-distribuicao-processos/distribuicao.model';
import { EscritorioDistribuicaoModel } from '../../models/parametrizar-distribuicao-processos/escritorio-distribuicao.model';
import { ParametrizarDistribuicaoProcessosService } from '../../services/parametrizar-distribuicao-processos.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { StaticInjector } from '../../static-injector';

@Component({
  selector: 'app-escritorio-modal',
  templateUrl: './escritorio-modal.component.html',
  styleUrls: ['./escritorio-modal.component.scss']
})
export class EscritorioModalComponent implements OnInit {

  constructor(
    private modal: NgbActiveModal,
    private messageService: HelperAngular,
    private dialog: DialogService,
    private service: ParametrizarDistribuicaoProcessosService
  ) { }

  ngOnInit() {
    this.buscarEscritorio()
    this.buscarSolicitantes();
    this.obterEmpresaCentralizadora();
    this.buscarUf()
    this.buscarNatureza()
    this.statusDist(this.distribuicao.status)
  }

  titulo: string;
  editar: boolean;
  //#region ESCRITORIO
  escritorioDistribuicao: EscritorioDistribuicaoModel;
  idEsc: string;
  codProfissional: string;
  codSolicitante: string;
  dataIni: Date;
  dataFim: Date;
  percProcesso: string;
  //#endregion
  escritorioList = [];
  listSolicitante = [];
  novoEscritorio = [{
    id: 0,
    escritorio: -1,
    solicitante: -1,
    percProcesso: '',
    prioridade: 0,
    dataIni: '',
    dataFim: ''
  }];
  empCentList = [];
  comarcaList = [];
  varaList = [];
  ufList = [];
  naturezaList = [];
  totalEscritorio = 0;

  //#region EDITAR DISTRIBUIÇÃO
  editarDistribuicao: boolean;
  distribuicao: DistribuicaoModel;
  naturezaAtual: number = null;
  escritoriosDistribuicao: Array<EscritorioDistribuicaoModel>;
  listaEscritoriosDistribuicaoBack: Array<EscritorioDistribuicaoModel>;
  //#endregion

  //#region FUNÇÕES ESCRITORIOS
  static exibeModalDeAlterar(distribuicao: DistribuicaoModel, escritorio: any): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EscritorioModalComponent, { centered: true, backdrop: true, size: 'xl', backdropClass: 'modal-backdrop-parametrizar' });

    modalRef.componentInstance.titulo = 'Editar Escritório';
    modalRef.componentInstance.editarDistribuicao = false;
    modalRef.componentInstance.editar = true;
    modalRef.componentInstance.obterComarca(distribuicao.codEstado);
    modalRef.componentInstance.obterVara(distribuicao.codComarca);
    modalRef.componentInstance.assingDistribuicao(distribuicao)

    modalRef.componentInstance.escritorioDistribuicao = escritorio
    modalRef.componentInstance.escritorioDistribuicao.datVigenciaInicial = new Date(escritorio.datVigenciaInicial);
    modalRef.componentInstance.escritorioDistribuicao.datVigenciaFinal = new Date(escritorio.datVigenciaFinal);

    let retorno = modalRef.result.then((res) => {
      return res
    }, () => {
      return false
    });
    return retorno;
  }

  static exibeModalDeIncluir(distribuicao: DistribuicaoModel): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EscritorioModalComponent, { centered: true, backdrop: true, size: 'xl', backdropClass: 'modal-backdrop-parametrizar' });

    modalRef.componentInstance.titulo = 'Incluir Escritório';
    modalRef.componentInstance.editarDistribuicao = false;
    modalRef.componentInstance.editar = false;
    modalRef.componentInstance.obterComarca(distribuicao.codEstado);
    modalRef.componentInstance.obterVara(distribuicao.codComarca);
    modalRef.componentInstance.assingDistribuicao(distribuicao)

    modalRef.componentInstance.obterEscritorios(distribuicao.codigo)
    let retorno = modalRef.result.then((res) => {
      return res
    }, () => {
      return false
    });
    return retorno;
  }

  async buscarEscritorio() {
    await this.service.obterEscritorios([this.distribuicao.codTipoProcesso]).then(x => {
      this.escritorioList = x;
    });
  }

  async buscarSolicitantes() {
    await this.service.obterSolicitantes().then(x => {
      this.listSolicitante = x;
    });
  }

  async obterEscritorios(cod) {
    this.service.obterEscritorioDistribuicao(cod, '', 'Prioridade', true).then(x => {
      this.listaEscritoriosDistribuicaoBack = x.lista;
      this.escritoriosDistribuicao = JSON.parse(JSON.stringify(x.lista));
      this.totalEscritorio = x.total;
      this.convertStringDate();
    })
  }

  convertStringDate() {
    for (let i = 0; i < this.escritoriosDistribuicao.length; i++) {
      this.escritoriosDistribuicao[i].datVigenciaInicial = new Date(this.escritoriosDistribuicao[i].datVigenciaInicial)
      this.escritoriosDistribuicao[i].datVigenciaFinal = new Date(this.escritoriosDistribuicao[i].datVigenciaFinal)
    }
  }

  adicionarEscritorio() {
    if (!this.editarDistribuicao) {
      let index = this.novoEscritorio.length
      let escritorio = {
        id: index++,
        escritorio: -1,
        solicitante: -1,
        percProcesso: '',
        prioridade: 0,
        dataIni: '',
        dataFim: ''
      };
      this.novoEscritorio.push(escritorio)
    }
    else {
      let index = this.escritoriosDistribuicao == undefined ? -1 : this.escritoriosDistribuicao.length
      let escritorio = {
        codParamDistribEscrit: 0,
        codParamDistribuicao: this.distribuicao.codigo,
        codProfissional: null,
        codSolicitante: null,
        porcentagemProcessos: '',
        datVigenciaInicial: null,
        datVigenciaFinal: null,
        nomProfissional: null,
        nomSolicitante: null,
        prioridade: null,
      };
      if (this.escritoriosDistribuicao == undefined) {
        this.escritoriosDistribuicao = [escritorio]
      }
      else {
        this.escritoriosDistribuicao.push(escritorio)
      }
    }

  }

  removerEscritorio(item) {
    if (!this.editarDistribuicao) {
      let index = this.novoEscritorio.indexOf(item)
      if (index > -1) {
        this.novoEscritorio.splice(index, 1);
      }
    }
    else {
      let existe = false;
      this.escritoriosDistribuicao.forEach(async (escritorio: EscritorioDistribuicaoModel) => {
        existe = this.listaEscritoriosDistribuicaoBack.some((esc: EscritorioDistribuicaoModel) => {
          let newEsc = JSON.parse(JSON.stringify(esc));
          newEsc.datVigenciaFinal = new Date(newEsc.datVigenciaFinal);
          newEsc.datVigenciaInicial = new Date(newEsc.datVigenciaInicial);
          return (JSON.stringify(newEsc) === JSON.stringify(escritorio) ? true : false);
        });

        if (existe) {
          const index = this.escritoriosDistribuicao.indexOf(item)
          if (index > -1) {
            this.listaEscritoriosDistribuicaoBack.splice(index, 1);
            this.escritoriosDistribuicao.splice(index, 1);
            return;
          }
        }
        else {
          const index = this.escritoriosDistribuicao.indexOf(item)
          if (index > -1) {
            return this.escritoriosDistribuicao.splice(index, 1);
          }
        }
      });
    }
  }

  //#endregion

  //#region FUNÇÕES DISTRIBUIÇÃO
  static exibeModalDeAlterarDistribuicao(distribuicao: DistribuicaoModel): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EscritorioModalComponent, { centered: true, backdrop: true, size: 'xl', backdropClass: 'modal-backdrop-parametrizar' });
    modalRef.componentInstance.titulo = 'Editar Distribuição de Processos aos Escritórios';
    modalRef.componentInstance.editarDistribuicao = true;
    modalRef.componentInstance.editar = false;
    modalRef.componentInstance.naturezaAtual = distribuicao.codTipoProcesso;
    modalRef.componentInstance.obterComarca(distribuicao.codEstado);
    modalRef.componentInstance.obterVara(distribuicao.codComarca, distribuicao.codTipoProcesso);
    modalRef.componentInstance.assingDistribuicao(distribuicao)
    setTimeout(() => {
      modalRef.componentInstance.statusDist(distribuicao.status);
    }, 700)
    modalRef.componentInstance.obterEscritorios(distribuicao.codigo)

    let retorno = modalRef.result.then((res) => {
      return res
    }, () => {
      return false
    });
    return retorno;
  }

  fazerBuscaComarca(uf: string) {
    this.obterComarca(uf)
    this.distribuicao.codComarca = -1
    this.distribuicao.codigos = "-1|-1"
    this.obterVara(-1)
  }

  async obterEmpresaCentralizadora() {
    await this.service.obterEmpresasCentralizadora().then(x => {
      this.empCentList = x;
    });
  }

  async obterComarca(uf) {
    await this.service.obterComarca(uf).then(x => {
      this.comarcaList = x;
    });
  }

  async obterVara(codComarca?, natureza?) {
    await this.service.obterVara(codComarca, natureza).then(x => {
      this.varaList = x;
    });
  }

  async buscarUf() {
    await this.service.obterUf().then(x => {
      this.ufList = x;
    });
  }

  async buscarNatureza() {
    await this.service.obterNatureza().then(x => {
      this.naturezaList = x;
    });
  }

  statusDist(item) {
    let check = (<HTMLInputElement>document.getElementById('customSwitch1'))
    if (item == "Inativo" || !item) {
      check.checked = false
      return this.distribuicao.status = 'Inativo'
    }
    check.checked = true
    return this.distribuicao.status = 'Ativo'
  }

  assingDistribuicao(distrib: DistribuicaoModel) {
    const distribuicao = Object.assign({}, distrib)
    this.distribuicao = distribuicao;
    return;
  }
  //#endregion

  //#region VALIDAÇÃO
  validarParametrizacao() {
    let campoVazio = false;
    let dataInvalida = false;

    if (this.distribuicao.codEstado == null || this.distribuicao.codComarca == null || this.distribuicao.codigos == null || this.distribuicao.codigos == '' || this.distribuicao.codTipoProcesso == null || this.distribuicao.codEmpresaCentralizadora == null)
      campoVazio = true

    if (this.editarDistribuicao) {
      this.checkPrioridade()
      for (let i = 0; i < this.escritoriosDistribuicao.length; i++) {
        let esc = this.escritoriosDistribuicao[i];
        if (esc.codProfissional == null || esc.codProfissional == -1 || esc.codSolicitante == null || esc.codSolicitante == -1 || esc.datVigenciaInicial == null || esc.datVigenciaFinal == null || esc.porcentagemProcessos == null || esc.prioridade == null) {
          campoVazio = true
        }
        if (esc.datVigenciaInicial > esc.datVigenciaFinal || esc.datVigenciaInicial > esc.datVigenciaFinal) {
          dataInvalida = true
        }
      };
    }
    else if (this.editar) {
      let teste = [];
      teste.push(this.escritorioDistribuicao)
      if (teste[0].codProfissional == null || teste[0].codSolicitante == null || teste[0].codSolicitante == '' || teste[0].porcentagemProcessos == null || teste[0].porcentagemProcessos == '' || !teste[0].porcentagemProcessos || teste[0].datVigenciaInicial == null || teste[0].datVigenciaInicial == '' || teste[0].datVigenciaFinal == null || teste[0].datVigenciaFinal == '')
        campoVazio = true
      if (teste[0].datVigenciaInicial > teste[0].datVigenciaFinal || teste[0].datVigenciaInicial > teste[0].datVigenciaFinal) {
        dataInvalida = true
      }
    }
    else {
      for (let i = 0; i < this.novoEscritorio.length; i++) {
        let esc = this.novoEscritorio[i];
        if (esc.escritorio == -1 || esc.solicitante == -1 || esc.percProcesso == null || esc.percProcesso == '' || esc.dataIni == null || esc.dataIni == '' || esc.dataFim == null || esc.dataFim == '')
          campoVazio = true
        if (esc.dataIni > esc.dataFim || esc.dataIni > esc.dataFim) {
          dataInvalida = true
        }
      };
    }

    if (campoVazio)
      return this.messageService.MsgBox2('O preenchimento de todos os campos é obrigatorio', 'A inclusão não poderá ser realizada', 'warning', 'Ok');

    if (dataInvalida)
      return this.messageService.MsgBox2('As datas da vigência informadas estão inválidas', 'A inclusão não poderá ser realizada', 'warning', 'Ok');

    let ehDuplicado = this.campoDuplicado();
    if (ehDuplicado)
      return this.messageService.MsgBox2('Existem registros duplicados. Verifique e tente novamente', 'A inclusão não poderá ser realizada', 'warning', 'Ok');

    let msg = !this.editar && !this.editarDistribuicao ? 'Confirma a inclusão do escritório?' :
      this.editar && !this.editarDistribuicao ? 'Confirma a alteração do escritorio?' :
        'Confirma a alteração da distribuição parametrizada?';

    let title = !this.editar && !this.editarDistribuicao ? 'Novo Escritório' :
      this.editar && !this.editarDistribuicao ? 'Alterar Escritório' :
        'Alterar Parametrização?';

    let btn = !this.editar && !this.editarDistribuicao ? 'inclusão' : 'alteração'

    this.messageService.MsgBox2(msg, title,
      'question', 'Sim, confirmo a ' + btn, 'Não, quero conferir antes').then(res => {
        if (res.value) {
          this.confirm()
        }
      });
  }

  campoDuplicado() {
    let esc = []
    let duplicado = false;

    if (this.editarDistribuicao) {
      for (let i = 0; i < this.escritoriosDistribuicao.length; i++) {
        esc.push(JSON.stringify(this.escritoriosDistribuicao[i]))
      }
    }
    else {
      for (let i = 0; i < this.novoEscritorio.length; i++) {
        delete this.novoEscritorio[i].id
        esc.push(JSON.stringify(this.novoEscritorio[i]))
      }
    }

    for (let i = 0; i < esc.length; i++) {
      let primeira = esc.indexOf(esc[i])
      let segunda = esc.lastIndexOf(esc[i])
      if (primeira != segunda) {
        duplicado = true
      }
    }

    return duplicado
  }
  //#endregion

  async confirm() {
    if (this.editarDistribuicao) {

      let distribuicao = {
        codEstado: this.distribuicao.codEstado,
        codComarca: this.distribuicao.codComarca,
        codigos: this.distribuicao.codigos,
        codTipoProcesso: this.distribuicao.codTipoProcesso,
        codEmpresaCentralizadora: this.distribuicao.codEmpresaCentralizadora,
        codParamDistribuicao: this.distribuicao.codigo,
        indAtivo: this.distribuicao.status == 'Ativo'
      }

      let escritorioSend = this.escritoriosDistribuicao.map((e) => {
        return {
          codParamDistribEscrit: e.codParamDistribEscrit,
          codParamDistribuicao: e.codParamDistribuicao,
          codProfissional: e.codProfissional,
          codSolicitante: e.codSolicitante,
          porcentagemProcessos: Number(e.porcentagemProcessos.toString().replace(',', '.')),
          datVigenciaInicial: new Date(e.datVigenciaInicial),
          datVigenciaFinal: new Date(e.datVigenciaFinal),
          prioridade: e.prioridade,
        };
      })

      let request = {
        parametrizacoes: [distribuicao],
        escritorios: escritorioSend,
        anexos: []
      }

      try {
        await this.service.SalvarParametrizacaoEmLoteAsync(request);
      } catch (error) {
        return this.messageService.MsgBox2(error.error, 'A alteração não poderá ser realizada', 'warning', 'Ok');
      }
    }

    let msg = !this.editar && !this.editarDistribuicao ? 'A inclusão do escritório foi realizada com sucesso!' :
      this.editar && !this.editarDistribuicao ? 'A alteração do escritório foi realizada com sucesso!' :
        'A alteração da distribuição foi parametrizada com sucesso!';

    let title = !this.editar && !this.editarDistribuicao ? 'Inclusão realizada com sucesso' :
      this.editar && !this.editarDistribuicao ? 'Alteração realizada com sucesso' :
        'Alteração realizada com sucesso';

    await this.dialog.alert(title, msg);
    this.modal.close(true);
  }

  close(): void {
    this.modal.close(false);
  }

  drop(event: CdkDragDrop<string[]>) {
    if (!this.editar && !this.editarDistribuicao) {
      moveItemInArray(this.novoEscritorio, event.previousIndex, event.currentIndex);
    }
    else {
      moveItemInArray(this.escritoriosDistribuicao, event.previousIndex, event.currentIndex);
    }
    this.checkPrioridade()
  }

  checkPrioridade() {
    if (!this.editar && !this.editarDistribuicao) {
      for (let i = 0; i < this.novoEscritorio.length; i++) {
        this.novoEscritorio[i].prioridade = i + 1 + this.totalEscritorio;
      }
    } else {
      for (let i = 0; i < this.escritoriosDistribuicao.length; i++) {
        this.escritoriosDistribuicao[i].prioridade = i + 1;
      }
      for (let i = 0; i < this.listaEscritoriosDistribuicaoBack.length; i++) {
        this.listaEscritoriosDistribuicaoBack[i].prioridade = i + 1;
      }
    }
  }

  distribuirPercentual() {
    let percentualRestante = 100;
    let percentualTotalAtual = 0;
    const percentuaisPreenchidos = this.escritoriosDistribuicao.filter(x => x.porcentagemProcessos == null || x.porcentagemProcessos.toString() != '')
    const inputsVazios = this.escritoriosDistribuicao.filter(x => x.porcentagemProcessos.toString() == '')
    percentuaisPreenchidos.forEach(x => {
      percentualTotalAtual += Number(x.porcentagemProcessos.toString().replace(',', '.'))
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
        campoPerc.porcentagemProcessos = valorFinal;
        somaValoresAtribuidos += Number(valorAtribuir.toString().replace(',', '.'));
      });
    } else {
      inputsVazios.forEach(campoPerc => {
        campoPerc.porcentagemProcessos = '0';
      });
    }
  }

  async clearEcritorioList() {
    const resposta = await this.dialog.confirm('Alteração de Natureza', 'Você está alterando a natureza de uma das chaves de distribuição.<br>Se continuar, será necessário que parametrize os escritórios novamente.')
    if (resposta) {
      this.naturezaAtual = this.distribuicao.codTipoProcesso;
      this.escritoriosDistribuicao = [
        {
          codParamDistribEscrit: 0,
          codParamDistribuicao: this.distribuicao.codigo,
          codProfissional: null,
          codSolicitante: null,
          porcentagemProcessos: '',
          datVigenciaInicial: null,
          datVigenciaFinal: null,
          nomProfissional: null,
          nomSolicitante: null,
          prioridade: null,
        }
      ];

      this.buscarEscritorio();
    } else {
      this.distribuicao.codTipoProcesso = this.naturezaAtual;
    }
  }

}
