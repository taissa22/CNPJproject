import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@environment';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from 'src/app/permissoes/permissoes';
import { PermissoesService } from 'src/app/permissoes/permissoes.service';
import { SelectOptionModel } from '../../models/criminal/select-option.model';
import { ResultRequestModel } from '../../models/criminal/result-request.model';
import { ProcessoCriminalModel } from '../../models/criminal/processo-criminal.model';
import { EmpresaProcessoCriminalModel } from '../../models/criminal/empresa-processo-criminal.model';
import { FiltroProcessoCriminalModel } from '../../models/criminal/filtro-processo-criminal.model';
import { ProcessoCriminalService } from '../../services/processo-criminal.service';
import { ListarProcessoCriminalComponent } from '../listar-processo-criminal/listar-processo-criminal.component';

@Component({
  selector: 'app-administrativo',
  templateUrl: './administrativo.component.html',
  styleUrls: ['./administrativo.component.scss']
})
export class AdministrativoComponent implements OnInit {
  constructor(
    private breadcrumbsService: BreadcrumbsService,
    private router: Router,
    private service: ProcessoCriminalService,
    private permissaoService: PermissoesService
  ) {}

  async ngOnInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(
      Permissoes.ACESSAR_CRIMINAL_ADM
    );
    this.podeConsultar = this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_CRIMINAL_ADM);

    if(!this.podeConsultar)
    {
      window.history.go(-1);
      return;
    }

    this.podeIncluir = this.permissaoService.temPermissaoPara(Permissoes.INCLUIR_CRIMINAL_ADM);
    this.podeAlterar = this.permissaoService.temPermissaoPara(Permissoes.ALTERAR_CRIMINAL_ADM);


    await this.service.ObterUltimosProcessosAsync(14).then(x => {
      this.listaUltimosProcessos = x.data;
      this.totalUltimosProcessos = x.total;
    });

    await this.service.ObterEscritoriosAsync(14).then(x => {
      this.listaComboEscritorio = x.result;
    });

    await this.service.ObterOrgaosAsync().then(x => {
      this.listaComboOrgao = x.result;
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

    await this.service.ObterEmpresasDoGrupoAsync(14).then(x => {
      this.listaComboEmpresaDoGrupo = x.result;
    });

    await this.service.ObterTiposProcedimentoAsync().then(x => {
      this.listaComboTipoProcedimento = x.result;
    });

    this.carregaFiltros();
  }
  estadoOnChange(evento) {
    if (this.estadoSelecionado != null) {
      this.service.ObterMunicipiosAsync(this.estadoSelecionado.id).then(x => {
        this.listaComboMunicipio = x.result;
      });
    } else {
      this.listaComboMunicipio = new Array<SelectOptionModel<number>>();
    }
  }

  orgaoOnChange(evento) {
    if (this.orgaoSelecionado != null) {
      this.service.ObterCompetenciasAsync(this.orgaoSelecionado.id).then(x => {
        this.listaComboCompetencia = x.result;
      });
    } else {
      this.listaComboCompetencia = new Array<SelectOptionModel<number>>();
    }
  }

  podeConsultar: boolean = false;
  podeIncluir: boolean = false;
  podeAlterar: boolean = false;

  listaComboEscritorio = new Array<SelectOptionModel<number>>();
  listaComboOrgao = new Array<SelectOptionModel<number>>();
  listaComboCompetencia = new Array<SelectOptionModel<number>>();
  listaComboEstado = new Array<SelectOptionModel<string>>();
  listaComboMunicipio = new Array<SelectOptionModel<number>>();
  listaComboEmpresaDoGrupo = new Array<SelectOptionModel<number>>();
  listaComboAssunto = new Array<SelectOptionModel<number>>();
  listaComboTipoParticipacao = new Array<SelectOptionModel<number>>();
  listaComboTipoProcedimento = new Array<SelectOptionModel<number>>();

  filtros = new FiltroProcessoCriminalModel();

  listaUltimosProcessos = new ProcessoCriminalModel();
  totalUltimosProcessos =0;

  breadcrumb: string;
  maisFiltros = false;
  textFiltro = 'Mais Filtros';
  parametrosConsulta: Array<parametrosConsulta>;
  ambiente = environment.s1_url;

  //#region VALORES SELECIONADOS
  escritorioSelecionado = null;
  procedimentoSelecionado = 0;
  procedimentoNumero = null;
  situacaoSelecionado = 0;
  orgaoSelecionado = null;
  competenciaSelecionado = null;
  estadoSelecionado = null;
  municipioSelecionado = null;
  empresaDoGrupoSelecionado = null;
  CpfCnpjSelecionado = 1;
  CpfCnpjNumero = '';
  tipoDeProcedimentoSelecionado = null;
  nomeParteSelecionado = 1;
  nomeParteNumero = null;
  assuntoSelecionado = null;
  tipoParticipacaoSelecionado = null;
  criticidadeSelecionado = -1;
  procedimentosChecadosSelecionado = 0;
  //#endregion

  //#region LISTAS
  listaProcedimento = [
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
      `${this.ambiente}/2.0/processo/CriminalAdministrativo/impressao/ImprimirProcesso.aspx?codProcesso=${id}`,
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
    this.procedimentoNumero = null;
    this.situacaoSelecionado = 0;
    this.orgaoSelecionado = null;
    this.competenciaSelecionado = null;
    this.estadoSelecionado = null;
    this.municipioSelecionado = null;
    this.empresaDoGrupoSelecionado = null;
    this.CpfCnpjSelecionado = 1;
    this.CpfCnpjNumero = '';
    this.tipoDeProcedimentoSelecionado = null;
    this.nomeParteSelecionado = 1;
    this.nomeParteNumero = '';
    this.assuntoSelecionado = null;
    this.tipoParticipacaoSelecionado = null;
    this.criticidadeSelecionado = -1;
    this.procedimentosChecadosSelecionado = 0;
  }

  pesquisar() {
    this.montaFiltros();
    this.parametrosPesquisa();
    sessionStorage.setItem('criminal_modulo', 'ADMINISTRATIVO');
    sessionStorage.setItem('criminal_parametrosConsulta',JSON.stringify(this.parametrosConsulta));
    sessionStorage.setItem('criminal_filtro_administrativo',JSON.stringify(this.filtros));

    this.router.navigateByUrl('processos/criminal/listar');
  }

  cpfcnpjmask(value) {
    this.CpfCnpjNumero = this.CpfCnpjNumero.replace(/[^0-9]/g, '');
    console.log(value, value.length);
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
  carregaFiltros() {

    var filtro_str = sessionStorage.getItem('criminal_filtro_administrativo');

    if(filtro_str===null)
    return;

    this.filtros = JSON.parse(filtro_str);

    this.escritorioSelecionado = this.filtros.cod_escritorio!=null? this.listaComboEscritorio.find(x=>x.id==this.filtros.cod_escritorio) : null;
    this.situacaoSelecionado = this.filtros.situacao!=null?this.filtros.situacao:this.situacaoSelecionado;
    this.procedimentoNumero = this.filtros.numero_processo!=null?this.filtros.numero_processo:null;
    this.procedimentoSelecionado = this.filtros.tipo_filtro_numero_processo2!=null?this.filtros.tipo_filtro_numero_processo2:this.procedimentoSelecionado;
    
    if(this.filtros.cod_orgao!=null){
      this.orgaoSelecionado = this.listaComboOrgao.find(x=>x.id==this.filtros.cod_orgao)
      this.service.ObterCompetenciasAsync(this.orgaoSelecionado.id).then(x => {
        this.listaComboCompetencia = x.result;
        this.competenciaSelecionado = this.filtros.cod_competencia!=null?this.listaComboCompetencia.find(x=>x.id==this.filtros.cod_competencia):null;
      });
    }
    
    if(this.filtros.cod_estado!=null){
      this.estadoSelecionado = this.listaComboEstado.find(x=>x.id==this.filtros.cod_estado);
      this.service.ObterMunicipiosAsync(this.estadoSelecionado.id).then(x => {
        this.listaComboMunicipio = x.result;
        this.municipioSelecionado = this.filtros.cod_municipio!=null? this.listaComboMunicipio.find(x=>x.id==this.filtros.cod_municipio):null;
      });
    }
    
    this.empresaDoGrupoSelecionado = this.filtros.cod_empresa!=null? this.listaComboEmpresaDoGrupo.find(x=>x.id==this.filtros.cod_empresa):null;
    this.CpfCnpjNumero = this.filtros.documento_parte!=null?this.filtros.documento_parte:null;
    this.CpfCnpjSelecionado = this.filtros.tipo_filtro_documento_parte!=null?this.filtros.tipo_filtro_documento_parte:this.CpfCnpjSelecionado;
    this.nomeParteNumero = this.filtros.nome_parte!=null?this.filtros.nome_parte:null;
    this.nomeParteSelecionado = this.filtros.tipo_filtro_nome_parte!=null?this.filtros.tipo_filtro_nome_parte:this.nomeParteSelecionado;
    this.tipoDeProcedimentoSelecionado = this.filtros.cod_tipo_procedimento!=null? this.listaComboTipoProcedimento.find(x=>x.id==this.filtros.cod_tipo_procedimento):null;
    this.assuntoSelecionado = this.filtros.cod_assunto!=null?this.listaComboAssunto.find(x=>x.id==this.filtros.cod_assunto):null;
    this.tipoParticipacaoSelecionado = this.filtros.cod_tipo_participacao!=null?this.listaComboTipoParticipacao.find(x=>x.id==this.filtros.cod_tipo_participacao):null;
    this.criticidadeSelecionado = this.filtros.cod_criticidade!=null?this.filtros.cod_criticidade:this.criticidadeSelecionado;
    this.procedimentosChecadosSelecionado = this.filtros.checado!=null?this.filtros.checado:this.procedimentosChecadosSelecionado;

    let maisFiltros = ( this.filtros.cod_orgao!=null 
                        || this.filtros.cod_competencia!=null
                        || this.filtros.cod_estado!=null
                        || this.filtros.cod_municipio!=null
                        || this.filtros.cod_empresa!=null
                        || this.filtros.documento_parte!=null
                        || this.filtros.nome_parte!=null
                        || this.filtros.cod_tipo_procedimento!=null
                        || this.filtros.cod_assunto!=null
                        || this.filtros.cod_tipo_participacao!=null
                        || this.filtros.cod_criticidade!=-1
                        || this.filtros.checado!=0
                        );

    if(maisFiltros)
      this.executarFiltros();
  }

  montaFiltros() {
    this.filtros = new FiltroProcessoCriminalModel();
    this.filtros.cod_tipo_processo = 14;
    this.filtros.cod_escritorio =
      this.escritorioSelecionado != null ? this.escritorioSelecionado.id : null;
    this.filtros.situacao = this.situacaoSelecionado;
    this.filtros.numero_processo = this.procedimentoNumero;
    this.filtros.tipo_filtro_numero_processo1 = null;
    this.filtros.tipo_filtro_numero_processo2 = this.procedimentoSelecionado;
    this.filtros.cod_orgao =
      this.orgaoSelecionado != null ? this.orgaoSelecionado.id : null;
    this.filtros.cod_competencia =
      this.competenciaSelecionado != null
        ? this.competenciaSelecionado.id
        : null;
    this.filtros.cod_estado =
      this.estadoSelecionado != null ? this.estadoSelecionado.id : null;
    this.filtros.cod_municipio =
      this.municipioSelecionado != null ? this.municipioSelecionado.id : null;
    this.filtros.cod_empresa =
      this.empresaDoGrupoSelecionado != null
        ? this.empresaDoGrupoSelecionado.id
        : null;
    this.filtros.documento_parte =
      this.CpfCnpjNumero != null && this.CpfCnpjNumero !='' ? this.CpfCnpjNumero : null;
    this.filtros.tipo_filtro_documento_parte =
      this.CpfCnpjSelecionado != null ? this.CpfCnpjSelecionado : null;
    this.filtros.nome_parte =
      this.nomeParteNumero != null &&  this.nomeParteNumero !='' ? this.nomeParteNumero : null;
    this.filtros.tipo_filtro_nome_parte =
      this.nomeParteSelecionado != null ? this.nomeParteSelecionado : null; //se não passar; filtra por igual
    this.filtros.cod_tipo_procedimento =
      this.tipoDeProcedimentoSelecionado != null
        ? this.tipoDeProcedimentoSelecionado.id
        : null;
    this.filtros.cod_assunto =
      this.assuntoSelecionado != null ? this.assuntoSelecionado.id : null;
    this.filtros.cod_tipo_participacao =
      this.tipoParticipacaoSelecionado != null
        ? this.tipoParticipacaoSelecionado.id
        : null;
    this.filtros.cod_criticidade =
      this.criticidadeSelecionado != null ? this.criticidadeSelecionado : null;
    this.filtros.cod_acao = null;
    this.filtros.cpf_testemunha = null;
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

    this.procedimentoSelecionado != null && this.procedimentoNumero != null
      ? parametros.push({
          tipo: `N° Processo(${
            this.listaProcedimento.find(
              x => x.id == this.procedimentoSelecionado
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
      this.orgaoSelecionado != null
        ? parametros.push({
            tipo: 'Orgão',
            descricao: this.orgaoSelecionado.descricao
          })
        : null;

      this.competenciaSelecionado != null
        ? parametros.push({
            tipo: 'Competência',
            descricao: this.competenciaSelecionado.descricao
          })
        : null;

      this.tipoDeProcedimentoSelecionado != null
        ? parametros.push({
            tipo: 'Tipo Procedimento',
            descricao: this.tipoDeProcedimentoSelecionado.descricao
          })
        : null;

      this.estadoSelecionado != null
        ? parametros.push({
            tipo: 'Estado',
            descricao: this.estadoSelecionado.id
          })
        : null;

      this.municipioSelecionado != null
        ? parametros.push({
            tipo: 'Município',
            descricao: this.municipioSelecionado.descricao
          })
        : null;

        this.CpfCnpjNumero !=null && this.CpfCnpjNumero != ''
        ? parametros.push({
            tipo: this.listaCpfCnpj.find(x => x.id == this.CpfCnpjSelecionado)
              .nome + " Parte",
            descricao: this.CpfCnpjNumero
          })
        : null;

      this.nomeParteSelecionado != null && this.nomeParteNumero != null && this.nomeParteNumero !=''
        ? parametros.push({
            tipo: `Parte (${
              this.listaNomeParte.find(x => x.id == this.nomeParteSelecionado)
                .nome
            })`,
            descricao: this.nomeParteNumero
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
            tipo: 'Tipo Participação Empresa',
            descricao: this.tipoParticipacaoSelecionado.descricao
          })
        : null;

      this.assuntoSelecionado != null
        ? parametros.push({
            tipo: 'Assunto',
            descricao: this.assuntoSelecionado.descricao
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
            descricao: this.listaProcedimentosChecados.find(x=>x.id== this.procedimentosChecadosSelecionado).nome
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
