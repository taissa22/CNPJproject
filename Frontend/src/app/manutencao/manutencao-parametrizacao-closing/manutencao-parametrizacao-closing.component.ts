import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { ParametizacaoClosing } from '@manutencao/models/parametrizacao.closing.model';
import { DialogService } from '@shared/services/dialog.service';
import { ManutencaoParametrizacaoClosingService } from './services/manutencao-parametrizacao-closing.service';
import { Router } from '@angular/router';
import { PermissoesService } from 'src/app/permissoes/permissoes.service';
import { Permissoes } from 'src/app/permissoes/permissoes';
import { CivelConsumidorComponent } from './civel-consumidor/civel-consumidor.component';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { CivelEstrategicoComponent } from './civel-estrategico/civel-estrategico.component';
import { JuizadoEspecialComponent } from './juizado-especial/juizado-especial.component';
import { TrabalhistaAdministrativoComponent } from './trabalhista-administrativo/trabalhista-administrativo.component';
import { TrabalhistaJudicialComponent } from './trabalhista-judicial/trabalhista-judicial.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
@Component({
    selector: 'app-manutencao-parametrizacao-closing',
    templateUrl: './manutencao-parametrizacao-closing.component.html',
    styleUrls: ['./manutencao-parametrizacao-closing.component.scss']
})

export class ManutencaoParametrizacaoClosingComponent implements OnInit {
    lista: ParametizacaoClosing[];
    exibir: boolean = false;
    ativo1: boolean = false;
    ativo2: boolean = false;
    ativo3: boolean = false;
    ativo4: boolean = false;
    ativo5: boolean = false;
    desabilitado: boolean = false;
    temPermissao: boolean;
    permissoes: boolean[] = [
        this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_TAB_PARAMETRIZACAO_CLOSING_TRAB_JUD),
        this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_TAB_PARAMETRIZACAO_CLOSING_CC),
        this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_TAB_PARAMETRIZACAO_CLOSING_TRAB_ADM),
        this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_TAB_PARAMETRIZACAO_CLOSING_CE),
        this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_TAB_PARAMETRIZACAO_CLOSING_JEC)
    ];

    @ViewChild(CivelConsumidorComponent, null) civelConsumidorComponent: CivelConsumidorComponent;
    @ViewChild(CivelEstrategicoComponent, null) civelEstrategicoComponent: CivelEstrategicoComponent; 
    @ViewChild(JuizadoEspecialComponent, null) juizadoEspecialComponent: JuizadoEspecialComponent; 
    @ViewChild(TrabalhistaAdministrativoComponent, null) trabalhistaAdministrativoComponent: TrabalhistaAdministrativoComponent; 
    @ViewChild(TrabalhistaJudicialComponent, null) trabalhistaJudicialComponent: TrabalhistaJudicialComponent; 
    @ViewChild('staticTabs', { static: false }) staticTabs?: TabsetComponent;
    breadcrumb: any;

    constructor(private crudService: ManutencaoParametrizacaoClosingService,
        private dialogService: DialogService,
        private router: Router,
        private permissaoService: PermissoesService,
        private breadcrumbsService: BreadcrumbsService
    ) { }

    ngOnInit() {
        this.buscar();
        this.temPermissao = false;
        this.naoTemPermissao();
    }

    async ngAfterViewInit(): Promise<void> {
        this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_PARAMETRO_CLOSING);
    }

    buscar() {
        this.crudService.obter()
            .subscribe(
                resultado => {
                    this.lista = resultado;
                    this.exibir = true;
                },
                error => console.log(error)
            );
    }

    naoTemPermissao() {
        this.permissoes.find(res => {
            if (res == true) {
                return this.temPermissao = true;
            }
        })
    }

    async bloquearAbas(tab)
    {
        this.staticTabs.tabs[0].disabled = true;
        this.staticTabs.tabs[1].disabled = true;
        this.staticTabs.tabs[2].disabled = true;
        this.staticTabs.tabs[3].disabled = true;
        this.staticTabs.tabs[4].disabled = true;

        this.staticTabs.tabs[tab].disabled = false;
        this.desabilitado = true;
    }

    async desbloquearAbas()
    {
        this.staticTabs.tabs[0].disabled = false;
        this.staticTabs.tabs[1].disabled = false;
        this.staticTabs.tabs[2].disabled = false;
        this.staticTabs.tabs[3].disabled = false;
        this.staticTabs.tabs[4].disabled = false;
        this.desabilitado = false;
    }

    async monitoraAlteracao(e) {
        if(this.civelConsumidorComponent.existeAlteracao())
            this.bloquearAbas(0);
        else if(this.civelEstrategicoComponent.existeAlteracao())
            this.bloquearAbas(1);
        else if(this.trabalhistaAdministrativoComponent.existeAlteracao())
            this.bloquearAbas(2);
        else if(this.trabalhistaJudicialComponent.existeAlteracao())
            this.bloquearAbas(3);
        else if(this.juizadoEspecialComponent.existeAlteracao())
            this.bloquearAbas(4);
        else
            this.desbloquearAbas();
    }

    exportar() {
        this.crudService.exportar();
    }
}
