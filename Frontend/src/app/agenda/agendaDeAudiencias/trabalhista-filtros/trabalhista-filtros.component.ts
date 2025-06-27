import { Combobox } from '@shared/interfaces/combobox';


import { FiltrosAgendaAudienciaResolver } from './interface/Filtros';
import { Component, OnInit, Output } from '@angular/core';
import { TrabalhistaFiltrosService } from './services/trabalhista-filtros.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AdvogadoService } from './services/Advogado.service';
import { ComarcaFiltroService } from './services/ComarcaFiltro.service';
import { EmpresaGrupoFiltroService } from './services/EmpresaGrupoFiltro.service';
import { EscritorioFiltroService } from './services/EscritorioFiltro.service';
import { EstadoService } from './services/Estado.service';
import { PrepostoService } from './services/Preposto.service';
import { CriteriosGeraisFiltroService } from './services/CriteriosGeraisFiltro.service'

import { FiltroModel } from 'src/app/core/models/filtro.model';
import { FormGroup } from '@angular/forms';
import { Subscription, BehaviorSubject } from 'rxjs';
import { ListaFiltroAgendaAudienciaTrabalhistaEnum } from './interface/ListaFiltroAgendaAudienciaTrabalhista.enum';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';



interface ResolverFiltro {
  filtros: { data: FiltrosAgendaAudienciaResolver }
}

@Component({
  selector: 'trabalhista-filtros',
  templateUrl: './trabalhista-filtros.component.html',
  styleUrls: ['./trabalhista-filtros.component.scss']
})
export class TrabalhistaFiltrosComponent implements OnInit {
  constructor(public service: TrabalhistaFiltrosService, public comarcaservice: ComarcaFiltroService,
    private route: ActivatedRoute,
    private advogadoService: AdvogadoService,
    private comarcaService: ComarcaFiltroService,
    private empresaService: EmpresaGrupoFiltroService,
    private escritorioSerivce: EscritorioFiltroService,
    private estadoService: EstadoService,
    private prepostoService: PrepostoService,
    private criterio: CriteriosGeraisFiltroService,
    private router: Router,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  form: FormGroup;
  listaFiltro: Array<FiltroModel> = [];
  tituloAgendaAudiencia = 'Agenda de Audiências Trabalhista';
  caminhoPaginaAgendaAudiencia : string
  filtroAgendaAudiencia = 'Selecione um período e clique em buscar para listar as audiências. Você também pode utilizar os outros filtros para refinar ainda mais a busca.';
  subscription: Subscription;




  ngOnInit() {
        
    this.limparFiltro(null);
    this.service.limparFiltro();
    this.listaFiltro = this.service.ListarFiltros();
    this.listaFiltro.map(
      itens => {
        {
          itens.id == ListaFiltroAgendaAudienciaTrabalhistaEnum.criteriosGerais
            ? itens.ativo = true : itens.ativo = false
        }
      });

    this.service.atualizarContagemVara();

    this.service.contagemProcessos$.subscribe(c =>
      this.service.atualizarContagem(c, ListaFiltroAgendaAudienciaTrabalhistaEnum.processos)
    );

    //#region Resolver dos filtros

    this.subscription =
      this.route.data.subscribe((info: ResolverFiltro) => {
        if (info.filtros != null) {
          this.advogadoService.listaAdvogados
            = this.service.transformToDualList(info.filtros.data.listaAdvogado);

          this.advogadoService.listaAdvogadoAcompanahnte
            = this.service.transformToDualList(info.filtros.data.listaAdvogado);

          this.comarcaService.listaComarcas
            = this.service.transformToDualList(info.filtros.data.listaComarca);

          this.empresaService.listaEmpresas
            = this.service.transformToDualList(info.filtros.data.listaEmpresa.map((item:any) => ({id: item.id, descricao: item.nome})));

          this.escritorioSerivce.listaEscritorio
            = this.service.transformToDualList(info.filtros.data.listaEscritorio);

          this.escritorioSerivce.listaEscritorioAcompanahnte
            = this.service.transformToDualList(info.filtros.data.listaEscritorio);

          this.estadoService.listaEstados
            = this.service.transformToDualList(info.filtros.data.listaEstado);

          this.prepostoService.listaPreposto
            = this.service.transformToDualList(info.filtros.data.listaPreposto);

          this.prepostoService.listaPrepostoAcompanhante
            = this.service.transformToDualList(info.filtros.data.listaPreposto);
        }

      });
    this.service.setListaEscritorios(this.escritorioSerivce.listaEscritorio);
    this.service.setListaAdvogados(this.advogadoService.listaAdvogadoAcompanahnte);
    this.service.setListaComarca(this.comarcaService.listaComarcas)
    this.service.setListaEmpresaGrupo(this.empresaService.listaEmpresas)
    this.service.setListaestado(this.estadoService.listaEstados)
    this.service.setListaPreposto(this.prepostoService.listaPreposto)



  }
  //#endregion

  async ngAfterViewInit(): Promise<void> {
    this.caminhoPaginaAgendaAudiencia = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_AGENDA_AUDIENCIA_TRABALHISTA);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();

  }

  goToInicio() {
    this.router.navigate(['criteriosGuia'], { relativeTo: this.route });
    this.listaFiltro.map(item => {
      item.ativo = false;
      if (item.id === 1) {
        item.ativo = true;
      }
    });
  }

  onClickbtn(e) {
    this.service.buscar();
  }

  limparFiltro(event: any) {
    if (event) {
      // usado para limpar as listas dos filtros advogados até preposto acompanhante
      this.reiniciarListas();
      // mando para o primeiro filtro criterio gerais
      this.zerarContadoresFiltros();
      // Função para limpar filtros
      this.service.btnLimparFiltro()
      // jogo a pagina para o inicio da pagina
    }
    this.goToInicio();
    // Função para limpar criteurios
    this.criterio.limparCriterios.next(true)
    this.criterio.isRangeValido = true
  }


  //função para zerar contadores do filtro
  zerarContadoresFiltros() {
    let count = 0;
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.advogado)
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.advogadoAcompanhante)
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.comarca)
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.empresaGrupo)
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.escritorio)
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.escritorioAcompanhante)
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.estado)
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.preposto)
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.prepostoAcompanhante)
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.processos)
    this.service.atualizarContagem(count, ListaFiltroAgendaAudienciaTrabalhistaEnum.vara)
    this.service.atualizarContagem(1, ListaFiltroAgendaAudienciaTrabalhistaEnum.criteriosGerais)

  }

  // função para reiniciar todas as listas
  reiniciarListas() {
    this.advogadoService.listaAdvogados.map(obj => obj.selecionado = false);
    this.advogadoService.listaAdvogadoAcompanahnte.map(obj => obj.selecionado = false);
    this.comarcaService.listaComarcas.map(obj => obj.selecionado = false);
    this.empresaService.listaEmpresas.map(obj => obj.selecionado = false);
    this.escritorioSerivce.listaEscritorio.map(obj => obj.selecionado = false);
    this.escritorioSerivce.listaEscritorioAcompanahnte.map(obj => obj.selecionado = false);
    this.estadoService.listaEstados.map(obj => obj.selecionado = false);
    this.prepostoService.listaPreposto.map(obj => obj.selecionado = false);
    this.prepostoService.listaPrepostoAcompanhante.map(obj => obj.selecionado = false);
  }



  IdToCodigoInterno(lista: any): Array<Combobox> {

    return lista.map(item => {
      return {
        id: item.codigoInterno,
        descricao: item.descricao.toUpperCase()
      };
    });
  }
}
