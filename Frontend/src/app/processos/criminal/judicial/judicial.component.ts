import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environment';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from 'src/app/permissoes/permissoes';
import { PermissoesService } from 'src/app/permissoes/permissoes.service';
import { SelectOptionModel } from '../../models/criminal/select-option.model';
import { ProcessoCriminalModel } from '../../models/criminal/processo-criminal.model';
import { EmpresaProcessoCriminalModel } from '../../models/criminal/empresa-processo-criminal.model';
import { FiltroProcessoCriminalModel } from '../../models/criminal/filtro-processo-criminal.model';
import { ProcessoCriminalService } from '../../services/processo-criminal.service';
import { ListarProcessoCriminalComponent } from '../listar-processo-criminal/listar-processo-criminal.component';

@Component({
  selector: 'app-judicial',
  templateUrl: './judicial.component.html',
  styleUrls: ['./judicial.component.scss']
})
export class JudicialComponent implements OnInit {
  constructor(
    private breadcrumbsService: BreadcrumbsService,
    private router: Router,
    private service: ProcessoCriminalService,
    private permissaoService: PermissoesService
  ) {}

  async ngOnInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(
      Permissoes.ACESSAR_CRIMINAL_JUDICIAL
    );
      this.podeConsultar = this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_CRIMINAL_JUDICIAL);
      
    if(!this.podeConsultar)
    {
      window.history.go(-1);
      return;
    }
      
    this.podeIncluir = this.permissaoService.temPermissaoPara(Permissoes.INCLUIR_CRIMINAL_JUD);
    this.podeAlterar = this.permissaoService.temPermissaoPara(Permissoes.ALTERAR_CRIMINAL_JUD);

    await this.service.ObterUltimosProcessosAsync(15).then(x => {
      this.listaUltimosProcessos = x.data;
      this.totalUltimosProcessos = x.total;
    });

    await this.service.ObterEscritoriosAsync(15).then(x => {
      this.listaComboEscritorio = x.result;
    });

    await this.service.ObterEstadosAsync().then(x => {
      this.listaComboEstado = x.result;
    });

    await this.service.ObterAssuntosAsync('S', 'N').then(x => {
      this.listaComboAssunto = x.result;
    });

    await this.service.ObterTiposParticipacaoAsync().then(x => {
      this.listaComboTipoParticipacao = x.result;
    });

    await this.service.ObterEmpresasDoGrupoAsync(15).then(x => {
      this.listaComboEmpresaDoGrupo = x.result;
    });

    await this.service.ObterAcoesAsync().then(x => {
      this.listaComboAcao = x.result;
    });

    await this.service.ObterTiposProcedimentoAsync().then(x => {
      this.listaComboTipoProcedimento = x.result;
    });

    this.carregaFiltros();
  }
  estadoOnChange(evento) {
    if (this.estadoSelecionado != null) {
      this.service.ObterComarcasAsync(this.estadoSelecionado.id).then(x => {
        this.listaComboComarca = x.result;
      });
    } else {
      this.listaComboComarca = new Array<SelectOptionModel<number>>();
    }
  }
  podeConsultar: boolean = false;
  podeIncluir: boolean = false;
  podeAlterar: boolean = false;

  listaComboEscritorio = new Array<SelectOptionModel<number>>();
  listaComboCompetencia = new Array<SelectOptionModel<number>>();
  listaComboEstado = new Array<SelectOptionModel<string>>();
  listaComboComarca = new Array<SelectOptionModel<number>>();
  listaComboEmpresaDoGrupo = new Array<SelectOptionModel<number>>();
  listaComboAssunto = new Array<SelectOptionModel<number>>();
  listaComboTipoParticipacao = new Array<SelectOptionModel<number>>();
  listaComboTipoProcedimento = new Array<SelectOptionModel<number>>();
  listaComboAcao = new Array<SelectOptionModel<number>>();

  listaUltimosProcessos = new ProcessoCriminalModel();
  totalUltimosProcessos = 0;

  filtros = new FiltroProcessoCriminalModel();

  breadcrumb: string;
  maisFiltros = false;
  textFiltro = 'Mais Filtros';
  parametrosConsulta: Array<parametrosConsulta>;
  
  ambiente = environment.s1_url;

  //#region VALORES SELECIONADOS
  escritorioSelecionado = null;
  procedimentoSelecionado = 0;
  procedimentoNumeroSelecionado = 0;
  procedimentoNumero = null;
  situacaoSelecionado = 0;
  estadoSelecionado = null;
  assuntoSelecionado = null;
  comarcaSelecionado = null;
  CpfCnpjSelecionado = 1;
  CpfCnpjNumero = '';
  empresaDoGrupoSelecionado = null;
  nomeParteSelecionado = 0;
  nomeParteNumero = null;
  tipoParticipacaoSelecionado = null;
  cpfTestemunhaSelecionado = null;
  acaoSelecionado = null;
  procedimentosChecadosSelecionado = 0;
  criticidadeSelecionado = -1;
  //#endregion

  //#region LISTAS

  listaProcedimento = [
    { id: 0, nome: 'Atual' },
    { id: 1, nome: 'Antigo' },
    { id: 2, nome: 'Atual ou Antigo' }
  ];
  listaProcedimentoNumero = [
    {
      id: 0,
      nome: 'Igual'
    },
    {
      id: 1,
      nome: 'Começando Por'
    },
    {
      id: 2,
      nome: 'Terminando Por'
    },
    {
      id: 3,
      nome: 'Em Qualquer Parte'
    }
  ];
  listaSituacao = [
    {
      id: 0,
      nome: 'Ambos'
    },
    {
      id: 1,
      nome: 'Inativos'
    },
    {
      id: 2,
      nome: 'Ativos'
    }
  ];
  listaComarca = [];
  listaAcao = [
    {
      id: 0,
      nome: 'AUDIÊNCIA DE CUSTÓDIA ¿ FLAGRA'
    },
    {
      id: 1,
      nome: 'CARTA PRECATÓRIA'
    }
  ];

  listaCpfCnpj = [
    { id: 1, nome: 'CPF' },
    { id: 2, nome: 'CNPJ' }
  ];
  listaNomeParte = [
    {
      id: 0,
      nome: 'Igual'
    },
    {
      id: 1,
      nome: 'Começando Por'
    },
    {
      id: 2,
      nome: 'Terminando Por'
    },
    {
      id: 3,
      nome: 'Em Qualquer Parte'
    }
  ];

  listaCriticidade = [
    { id: -1, nome: 'Indiferente' },
    { id: 0, nome: 'Alta' },
    { id: 1, nome: 'Média' },
    { id: 2, nome: 'Baixa' }
  ];
  listaProcedimentosChecados = [
    { id: 0, nome: 'Indiferente' },
    { id: 1, nome: 'Sim' },
    { id: 2, nome: 'Não' }
  ];
  //#endregion

  imprimir(id: number) {
    window.open(
      `${this.ambiente}/2.0/processo/CriminalJudicial/impressao/ImprimirProcesso.aspx?codProcesso=${id}`,
      '',
      'width=1000,height=600'
    );
  }

  executarFiltros() {
    this.maisFiltros = !this.maisFiltros;
    this.textFiltro = this.maisFiltros ? 'Menos Filtros' : 'Mais Filtros';
  }

  limpar() {
    this.escritorioSelecionado = null;
    this.procedimentoSelecionado = 0;
    this.procedimentoNumeroSelecionado = 0;
    this.procedimentoNumero = null;
    this.situacaoSelecionado = 0;
    this.estadoSelecionado = null;
    this.assuntoSelecionado = null;
    this.comarcaSelecionado = null;
    this.CpfCnpjSelecionado = 1;
    this.CpfCnpjNumero = '';
    this.empresaDoGrupoSelecionado = null;
    this.nomeParteSelecionado = 0;
    this.nomeParteNumero = null;
    this.tipoParticipacaoSelecionado = null;
    this.cpfTestemunhaSelecionado = null;
    this.acaoSelecionado = null;
    this.procedimentosChecadosSelecionado = 0;
    this.criticidadeSelecionado = -1;
  }

  pesquisar() {
    this.montaFiltros();
    this.parametrosPesquisa();
    sessionStorage.setItem('criminal_modulo', 'JUDICIAL');
    sessionStorage.setItem('criminal_parametrosConsulta',JSON.stringify(this.parametrosConsulta));
    sessionStorage.setItem('criminal_filtro_judicial',JSON.stringify(this.filtros));
    this.router.navigateByUrl('processos/criminal/listar');
  }

  cpfcnpjmask(value) {
    this.CpfCnpjNumero = this.CpfCnpjNumero.replace(/[^0-9]/g, '');
    if (this.CpfCnpjSelecionado == 1) {
      //CPF
      if (value.length > 11) {
        this.CpfCnpjNumero = this.CpfCnpjNumero.substring(0, 11);
      }
      this.CpfCnpjNumero = this.CpfCnpjNumero.replace(
        /(\d{3})(\d{3})(\d{3})(\d{2})/g,
        '$1.$2.$3-$4'
      );
      return this.CpfCnpjNumero;
    }
    if (this.CpfCnpjSelecionado == 2) {
      //CNPJ
      if (value.length > 14) {
        this.CpfCnpjNumero = this.CpfCnpjNumero.substring(0, 14);
      }
      this.CpfCnpjNumero = this.CpfCnpjNumero.replace(
        /(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g,
        '$1.$2.$3/$4-$5'
      );
      return this.CpfCnpjNumero;
    }
  }

  cpfmask(value) {
    this.cpfTestemunhaSelecionado = this.cpfTestemunhaSelecionado.replace(
      /[^0-9]/g,
      ''
    );
    if (value.length > 11) {
      this.cpfTestemunhaSelecionado = this.cpfTestemunhaSelecionado.substring(
        0,
        11
      );
    }
    this.cpfTestemunhaSelecionado = this.cpfTestemunhaSelecionado.replace(
      /(\d{3})(\d{3})(\d{3})(\d{2})/g,
      '$1.$2.$3-$4'
    );
    return this.cpfTestemunhaSelecionado;
  }

  carregaFiltros() {

    var filtro_str = sessionStorage.getItem('criminal_filtro_judicial');

    if(filtro_str===null)
    return;

    this.filtros = JSON.parse(filtro_str);

    this.escritorioSelecionado = this.filtros.cod_escritorio!=null? this.listaComboEscritorio.find(x=>x.id==this.filtros.cod_escritorio) : null;
    this.situacaoSelecionado = this.filtros.situacao!=null?this.filtros.situacao:this.situacaoSelecionado;
    this.procedimentoNumero = this.filtros.numero_processo!=null?this.filtros.numero_processo:null;
    this.procedimentoSelecionado = this.filtros.tipo_filtro_numero_processo1!=null?this.filtros.tipo_filtro_numero_processo1:this.procedimentoSelecionado;
    this.procedimentoNumeroSelecionado = this.filtros.tipo_filtro_numero_processo2!=null?this.filtros.tipo_filtro_numero_processo2:this.procedimentoNumeroSelecionado;

    if(this.filtros.cod_estado!=null){
      this.estadoSelecionado = this.listaComboEstado.find(x=>x.id==this.filtros.cod_estado);
      this.service.ObterComarcasAsync(this.estadoSelecionado.id).then(x => {
        this.listaComboComarca = x.result;
        this.comarcaSelecionado=this.filtros.cod_comarca?this.listaComboComarca.find(x=>x.id==this.filtros.cod_comarca):null;
      });
    }
    
    this.empresaDoGrupoSelecionado = this.filtros.cod_empresa!=null? this.listaComboEmpresaDoGrupo.find(x=>x.id==this.filtros.cod_empresa):null;
    this.CpfCnpjNumero = this.filtros.documento_parte!=null?this.filtros.documento_parte:null;
    this.CpfCnpjSelecionado = this.filtros.tipo_filtro_documento_parte!=null?this.filtros.tipo_filtro_documento_parte:this.CpfCnpjSelecionado;
    this.nomeParteNumero = this.filtros.nome_parte!=null?this.filtros.nome_parte:null;
    this.nomeParteSelecionado = this.filtros.tipo_filtro_nome_parte!=null?this.filtros.tipo_filtro_nome_parte:this.nomeParteSelecionado;
    this.assuntoSelecionado = this.filtros.cod_assunto!=null?this.listaComboAssunto.find(x=>x.id==this.filtros.cod_assunto):null;
    this.tipoParticipacaoSelecionado = this.filtros.cod_tipo_participacao!=null?this.listaComboTipoParticipacao.find(x=>x.id==this.filtros.cod_tipo_participacao):null;
    this.criticidadeSelecionado = this.filtros.cod_criticidade!=null?this.filtros.cod_criticidade:this.criticidadeSelecionado;
    this.procedimentosChecadosSelecionado = this.filtros.checado!=null?this.filtros.checado:this.procedimentosChecadosSelecionado;
    this.acaoSelecionado = this.filtros.cod_acao!=null?this.listaComboAcao.find(x=>x.id==this.filtros.cod_acao):null;
    this.cpfTestemunhaSelecionado = this.filtros.cpf_testemunha;
    
    

    let maisFiltros = (    this.filtros.cod_estado!=null
                        || this.filtros.cod_comarca!=null
                        || this.filtros.cod_empresa!=null
                        || this.filtros.documento_parte!=null
                        || this.filtros.nome_parte!=null
                        || this.filtros.cod_assunto!=null
                        || this.filtros.cod_tipo_participacao!=null
                        || this.filtros.cod_criticidade!=-1
                        || this.filtros.checado!=0
                        || this.filtros.cpf_testemunha!=null
                        || this.filtros.cod_acao!=null
                        );

    if(maisFiltros)
      this.executarFiltros();
  }

  montaFiltros() {
    this.filtros = new FiltroProcessoCriminalModel();
    this.filtros.cod_tipo_processo = 15;
    this.filtros.cod_escritorio =
      this.escritorioSelecionado != null ? this.escritorioSelecionado.id : null;
    this.filtros.situacao = this.situacaoSelecionado;
    this.filtros.numero_processo = this.procedimentoNumero;
    this.filtros.tipo_filtro_numero_processo1 = this.procedimentoSelecionado;
    this.filtros.tipo_filtro_numero_processo2 = this.procedimentoNumeroSelecionado;
    this.filtros.cod_estado =
      this.estadoSelecionado != null ? this.estadoSelecionado.id : null;
    this.filtros.cod_comarca =
      this.comarcaSelecionado != null ? this.comarcaSelecionado.id : null;
    this.filtros.cod_empresa =
      this.empresaDoGrupoSelecionado != null
        ? this.empresaDoGrupoSelecionado.id
        : null;
    this.filtros.documento_parte =
      this.CpfCnpjNumero != null && this.CpfCnpjNumero!='' ? this.CpfCnpjNumero : null;
    this.filtros.tipo_filtro_documento_parte =
      this.CpfCnpjSelecionado != null? this.CpfCnpjSelecionado : null;
    this.filtros.nome_parte =
      this.nomeParteNumero != null && this.nomeParteNumero!='' ? this.nomeParteNumero : null;
    this.filtros.tipo_filtro_nome_parte =
      this.nomeParteSelecionado != null ? this.nomeParteSelecionado : null; //se não passar; filtra por igual
    this.filtros.cod_assunto =
      this.assuntoSelecionado != null ? this.assuntoSelecionado.id : null;
    this.filtros.cod_tipo_participacao =
      this.tipoParticipacaoSelecionado != null
        ? this.tipoParticipacaoSelecionado.id
        : null;
    this.filtros.cod_criticidade =
      this.criticidadeSelecionado != null ? this.criticidadeSelecionado : null;
    this.filtros.cod_acao =
      this.acaoSelecionado != null ? this.acaoSelecionado.id : null;
    this.filtros.cpf_testemunha = this.cpfTestemunhaSelecionado!=null? this.cpfTestemunhaSelecionado:null;
    this.filtros.checado= this.procedimentosChecadosSelecionado!=null? this.procedimentosChecadosSelecionado:null;
    this.filtros.page = 1; //se não passar; padrão é 1
    this.filtros.size = 8; //se não passar; padrão é 10
  }

  parametrosPesquisa() {
    let parametros = [];

    
    this.escritorioSelecionado != null
      ? parametros.push({
          tipo: 'Escritorio',
          descricao: this.escritorioSelecionado.descricao
        })
      : null;

    this.procedimentoNumeroSelecionado != null &&
    this.procedimentoNumero != null
      ? parametros.push({
          tipo: `N° Processo ${
            this.listaProcedimento.find(
              x => x.id == this.procedimentoSelecionado
            ).nome
          }(${
            this.listaProcedimentoNumero.find(
              x => x.id == this.procedimentoNumeroSelecionado
            ).nome
          })`,
          descricao: this.procedimentoNumero
        })
      : null;

    this.situacaoSelecionado != null
      ? parametros.push({
          tipo: 'Situação',
          descricao: this.listaSituacao.find(
            x => x.id == this.situacaoSelecionado
          ).nome
        })
      : null;

    if (this.maisFiltros) {
      this.estadoSelecionado != null
        ? parametros.push({
            tipo: 'Estado',
            descricao: this.estadoSelecionado.descricao
          })
        : null;

      this.assuntoSelecionado != null
        ? parametros.push({
            tipo: 'Assunto',
            descricao: this.assuntoSelecionado.descricao
          })
        : null;

      this.comarcaSelecionado != null
        ? parametros.push({
            tipo: 'Comarca',
            descricao: this.comarcaSelecionado.descricao
          })
        : null;

      this.empresaDoGrupoSelecionado != null
        ? parametros.push({
            tipo: 'Empresa Grupo',
            descricao: this.empresaDoGrupoSelecionado.descricao
          })
        : null;

      this.tipoParticipacaoSelecionado != null
        ? parametros.push({
            tipo: 'Tipo Participação',
            descricao: this.tipoParticipacaoSelecionado.descricao
          })
        : null;

      this.nomeParteSelecionado != null && this.nomeParteNumero != null  && this.nomeParteNumero != ''
        ? parametros.push({
            tipo: `Parte (${
              this.listaNomeParte.find(x => x.id == this.nomeParteSelecionado)
                .nome
            })`,
            descricao: this.nomeParteNumero
          })
        : null;

      this.CpfCnpjNumero != null && this.CpfCnpjNumero !=''
        ? parametros.push({
            tipo: this.listaCpfCnpj.find(x => x.id == this.CpfCnpjSelecionado)
              .nome + " Parte",
            descricao: this.CpfCnpjNumero
          })
        : null;

      this.acaoSelecionado != null
        ? parametros.push({
            tipo: 'Ação',
            descricao: this.acaoSelecionado.descricao
          })
        : null;

        this.cpfTestemunhaSelecionado!=null && this.cpfTestemunhaSelecionado!=''? parametros.push({
          tipo: 'CPF Testemunha',
          descricao: this.cpfTestemunhaSelecionado
        })
      : null;

      this.criticidadeSelecionado != -1
        ? parametros.push({
            tipo: 'Criticidade',
            descricao: this.listaCriticidade.find(
              x => x.id == this.criticidadeSelecionado
            ).nome
          })
        : null;

      this.procedimentosChecadosSelecionado != 0
        ? parametros.push({
            tipo: 'Checado',
            descricao: this.listaProcedimentosChecados.find(
              x => x.id == this.procedimentosChecadosSelecionado
            ).nome
          })
        : null;
    }
    this.parametrosConsulta = parametros;
  }
}

interface parametrosConsulta {
  tipo: string;
  descricao: string;
}
