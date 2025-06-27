import { DialogService } from './../../../shared/services/dialog.service';
import { ListarAnexosResponse } from './../../models/parametrizar-distribuicao-processos/listar-anexos-response';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ParametrizarDistribuicaoProcessosService } from '../../services/parametrizar-distribuicao-processos.service';
import { AnexarNovoDocumentoModalComponent } from '../anexar-novo-documento-modal/anexar-novo-documento-modal.component';
import { StaticInjector } from '../../static-injector';

@Component({
  selector: 'app-anexo-modal',
  templateUrl: './anexo-modal.component.html',
  styleUrls: ['./anexo-modal.component.scss']
})
export class AnexoModalComponent implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    private dialog: DialogService,
    private service: ParametrizarDistribuicaoProcessosService
  ) { }
  ngAfterViewInit(): void {

  }

  //#region VARIAVEIS
  codParamDistribuicao: number = 0;
  listaAnexo: Array<ListarAnexosResponse>;
  listaCodAnexoPesquisa: number[] = [0]
  //#endregion

  static exibeModalAnexo(codParamDistribuicao: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(AnexoModalComponent, { centered: true, backdrop: 'static', size: 'xl', backdropClass: 'modal-backdrop-anexo' });

    modalRef.componentInstance.codParamDistribuicao = codParamDistribuicao;
    modalRef.componentInstance.obterLista(codParamDistribuicao);

    return modalRef.result.then((res) => {
      return res
    }, () => {
      return false
    });
  }

  static async exibeModalAnexoNovaParametrizacao(listaCodAnexo: number[]): Promise<number[]> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(AnexoModalComponent, { centered: true, backdrop: 'static', size: 'xl', backdropClass: 'modal-backdrop-anexo' });

    modalRef.componentInstance.popularCodAnexoLista(listaCodAnexo);
    modalRef.componentInstance.codParamDistribuicao = 0;
    modalRef.componentInstance.obterLista();

    return await modalRef.result;
  }

  //#region METODOS

  async obterLista() {
    try {
      if (this.listaCodAnexoPesquisa == undefined) this.listaCodAnexoPesquisa = [0]
      await this.service.obterListaAnexoAsync(this.codParamDistribuicao, this.listaCodAnexoPesquisa).then((res) => {
        this.listaAnexo = res;
      })
    } catch (error) {
      this.dialog.err("Erro", "Não foi possivel carregar as informações!")
    }
  }

  async excluirAnexo(idAnexoDistEscritorio: number, nome: string) {
    let resposta = await this.dialog.confirm("Excluir anexo", "Deseja excluir o anexo selecionado?")
    if (resposta) {
      try {
        await this.service.excluirAnexoAsync(this.codParamDistribuicao, idAnexoDistEscritorio).then(async () => {
          await this.dialog.alert("Anexo Excluído", "O anexo " + nome + " foi excluido com sucesso!")
          return this.obterLista()
        })
      } catch (error) {
        this.dialog.err("Erro", error.error)
      }
    }
  }

  async downloadAnexo(id: number) {
    await this.service.downloadArquivoZip(id)
  }

  async downloadAnexos() {
    const listaAnexo = this.listaAnexo.map(anexo => anexo.idAnexoDistEscritorio);
    if (listaAnexo.length > 0) {
      return await this.service.downloadArquivosZip(listaAnexo);
    }
  }

  //#endregion

  //#region FUNÇÕES

  async novoDocumento(): Promise<void> {
    const anexo = await AnexarNovoDocumentoModalComponent.exibeModalAnexarNovoDocumento(this.codParamDistribuicao);
    if (anexo) {
      this.listaAnexo.push(anexo);
      this.popularCodAnexoLista([anexo.idAnexoDistEscritorio])
    }
  }

  popularCodAnexoLista(codListaAnexo: number[]) {
    codListaAnexo.forEach(codAnexo => {
      if (!this.listaCodAnexoPesquisa.includes(codAnexo)) {
        this.listaCodAnexoPesquisa.push(codAnexo);
      }
    });
    return this.listaCodAnexoPesquisa;
  }

  closeLista(): void {
    this.modal.close(this.listaCodAnexoPesquisa);
  }

  //#endregion


  close(): void {
    this.modal.close(false);
  }

}
