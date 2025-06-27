import { Component, OnInit } from '@angular/core';
import { HelperAngular } from '@shared/helpers/helper-angular';

import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { UM_MEGABYTE } from 'src/app/sap/sap.constants';

import { AlteracaoProcessoBlocoService } from 'src/app/core/services/alteracao-processo-bloco/alteracao-processo-bloco.service';
import { PermissoesAlteracaoProcessoBlocoWebService } from 'src/app/core/services/alteracao-processo-bloco/permissoes-alteracao-processo-bloco.service';
import { TipoProcessoAlteracaoProcessoBloco } from '../models/tipo-processo-alteracao-processo-bloco';
import { CorStatusAgendamento } from '../models/cor-status-agendamento';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  selector: 'app-alteracao-processo-bloco-web',
  templateUrl: './alteracao-processo-bloco-web.component.html',
  styleUrls: ['./alteracao-processo-bloco-web.component.scss']
})

export class AlteracaoProcessoBlocoWebComponent implements OnInit {
  public tituloPagina = 'Alteração de Processos em Bloco Web';
  public caminhoPagina : string

  public todosTiposProcesso = TipoProcessoAlteracaoProcessoBloco;
  public tipoSelecionado = new BehaviorSubject<number>(null);
  public tiposProcessosComPermissao = [];

  public uploadHabilitado = false;
  public agendarAlteracao = false;
  public colunasAlteradasJec = false;
  public colunasAlteradasCE = false;
  public colunasAlteradasPex = false;
  public finalizacao = false;
  public colunasAlteradasCC = false;
  public colunasAlteradas = false;
  public colunasAlteradasmediacao = false;
  public msg = true;
  public vermais = false;
  public clickAqui = false;
  public visualizarAgendamentos = false;

  public nomeArquivo = '';
  public arquivo: any;
  public extensao: string;
  public listaAgendamentos = [];
  public codigoTipoProcesso: number;
  public quantidadeMB: number = 10;

  constructor(
    private service: AlteracaoProcessoBlocoService,
    private messageService: HelperAngular,
    private permissoes: PermissoesAlteracaoProcessoBlocoWebService,
    private breadcrumbsService: BreadcrumbsService
  ) {
  }

  ngOnInit() {
    this.atualizarLista();
    this.permissaoTipoProcesso();
  }

  async ngAfterViewInit(): Promise<void> {
    this.caminhoPagina = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_ALTERACAO_PROC_BLOCO_WEB);
  }

  permissaoTipoProcesso() {
    this.todosTiposProcesso.map(element => {

      switch (element.permissao) {
        case 'f_AlteraProcBlocoWebAdm':
          this.permissoes.f_AlteraProcBlocoWebAdm ? this.tiposProcessosComPermissao.push(element) : null;
          break;
        case 'f_AlteraProcBlocoWebCC':
          this.permissoes.f_AlteraProcBlocoWebCC ? this.tiposProcessosComPermissao.push(element) : null;
          break;
        case 'f_AlteraProcBlocoWebCE':
          this.permissoes.f_AlteraProcBlocoWebCE ? this.tiposProcessosComPermissao.push(element) : null;
          break;
        case 'f_AlteraProcBlocoWebJEC':
          this.permissoes.f_AlteraProcBlocoWebJEC ? this.tiposProcessosComPermissao.push(element) : null;
          break;
        case 'f_AlteraProcBlocoWebPEX':
          this.permissoes.f_AlteraProcBlocoWebPEX ? this.tiposProcessosComPermissao.push(element) : null;
          break;
        case 'f_AlteraProcBlocoWebTrab':
          this.permissoes.f_AlteraProcBlocoWebTrab ? this.tiposProcessosComPermissao.push(element) : null;
          break;
        case 'f_AlteraProcBlocoWebTribJud':
          this.permissoes.f_AlteraProcBlocoWebTribJud ? this.tiposProcessosComPermissao.push(element) : null;
          break;
        case 'f_AlteraProcBlocoWebProcon':
          this.permissoes.f_AlteraProcBlocoWebProcon ? this.tiposProcessosComPermissao.push(element) : null;
          break;
      }
    });

    if (this.tiposProcessosComPermissao.length === 1) {
      this.tiposProcessosComPermissao.map(e => { console.log(e);
        this.tipoSelecionado.next(e.id);
        this.camposDisponiveisAlteracao(e.id);
      });
    }
  }

  camposDisponiveisAlteracao(tipoProcessoId) {
    this.codigoTipoProcesso = tipoProcessoId;
    this.uploadHabilitado = true;
    this.msg = false;
    
    this.colunasAlteradasPex = false;
    this.colunasAlteradasCC = false;
    this.colunasAlteradasCE = false;
    this.colunasAlteradasJec = false;
    this.colunasAlteradasmediacao = false;
    this.finalizacao = false;

    if (tipoProcessoId === 7)
    {
      this.colunasAlteradasCC = true;
    }
    else if (tipoProcessoId === 18)
    {
      this.colunasAlteradasPex = true;
      this.colunasAlteradasmediacao = true;
    }
    else
    {
      this.colunasAlteradas = true;
    }
    
    if(tipoProcessoId == 9){
      this.colunasAlteradasCE = true;
    }

    if(tipoProcessoId == 1){
      this.colunasAlteradasCC = true;
    }

    if(tipoProcessoId == 18){
      this.finalizacao = true;
    }
  }

  abrirInstrucoes() {
    this.clickAqui = true;
  }

  fecharInstrucoes() {
    this.clickAqui = false;
  }

  pegarArquivo(arquivo) {
    this.agendarAlteracao = true;
    this.arquivo = arquivo[0];
    this.nomeArquivo = arquivo[0].name;
    this.extensao = this.nomeArquivo.split('.')[1];
  }

  public fazerUploadArquivo() {
    let msgErro = 'Desculpe, o upload do arquivo não poderá ser realizado';

      this.service.upload(this.arquivo, this.codigoTipoProcesso).subscribe(response => {
        if (response['sucesso']) {
          this.messageService.MsgBox2('', 'Agendamento realizado com sucesso', 'success', 'Ok')
          this.atualizarLista();
        } else {
          this.messageService.MsgBox2(response['mensagem'], msgErro, 'warning', 'Ok');
        }
      });

    this.nomeArquivo = '';
    (document.getElementById('file') as HTMLInputElement).value = "";
    this.agendarAlteracao = false;
  }

  public deletarPlanilhaAgendada(id) {
    this.messageService.MsgBox2('Deseja excluir o agendamento?', 'Excluir Agendamento',
      'question', 'Sim', 'Não').then(resposta => {
        if (resposta.value) {
          this.service.removerPlhanilhaAgendada(id).subscribe(() => {
            this.atualizarLista();
          });
        }
      });
  }

  verMais() {
    this.service.consultarMais().subscribe(
      agendamentos => {
        this.listaAgendamentos.push.apply(this.listaAgendamentos, agendamentos)
        if (this.service.paginacaoVerMais.value['total'] == this.listaAgendamentos.length)
          this.vermais = false;
        else this.vermais = true;
      }
    );
  }

  atualizarLista() {
    this.service.limpar();
    this.service.consultar()
      .pipe(take(1))
      .subscribe(
        agendamentos => {
          this.listaAgendamentos = agendamentos
          if (this.service.paginacaoVerMais.value['total'] == this.listaAgendamentos.length)
            this.vermais = false;
          else this.vermais = true;
        }
      );
  }

  verificarCorStatusAgendamento(status) {
    if (status)
      return CorStatusAgendamento[status]
  }

  downloadPlanilhaCarregada(id) {
    this.service.downloadPlanilhaCarregada(id).subscribe(res => {
      if (!res.sucesso) {
        this.messageService.MsgBox2(res.mensagem, 'Erro', 'warning', 'Ok');
      }

      var buffer = this.converterBase64ParaBuffer(res.data.arquivo);
      this.prepararDownload(buffer, res.data.nomeDoArquivo);
    });
  }

  downloadPlanilhaResultado(id) {
    this.service.downloadPlanilhaResultado(id).subscribe(res => {
      var buffer = this.converterBase64ParaBuffer(res.data.arquivo);
      this.prepararDownload(buffer, res.data.nomeDoArquivo);
    });
  }

  prepararDownload(buffer, nomeDoArquivo) {
    const blob = new Blob([buffer]);
    if (navigator.msSaveBlob) {
      navigator.msSaveBlob(blob, nomeDoArquivo);
      return;
    }

    const link = document.createElement('a');

    if (link.download != undefined) {
      link.setAttribute('href', URL.createObjectURL(blob));
      link.setAttribute('download', nomeDoArquivo);
      link.style.visibility = 'hidden';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  }

  converterBase64ParaBuffer(base64) {
    const binaryString = window.atob(base64);
    const bytes = new Uint8Array(binaryString.length);

    for (var indice = 0; indice < bytes.length; indice++) {
      bytes[indice] = binaryString.charCodeAt(indice);
    }
    return bytes;
  }
}
