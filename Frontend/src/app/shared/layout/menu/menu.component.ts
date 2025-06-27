import { Component, OnInit, OnDestroy, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { UserService } from '../../../core';
import { convertToItemMenu } from '@shared/utils';
import { environment } from 'src/environments/environment';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { SysInfoService } from 'src/app/core/services/system/sys-info.service';

interface MenuJson {
  /**
  * Submenus
  */
  filhos?: MenuJson[],
  /**
  * Permissões do Menu
  */
  permissoes: string[],
  /**
  * Nome do menu
  */
  titulo: string,
  /**
  * Tooltip do menu
  */
  tooltip: string,
  /**
  * Url do menu
  */
  url: string
}

@Component({
  selector: 'app-layout-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MenuComponent implements OnInit, OnDestroy {
  constructor(
    /**
      * Service do usuário atual
       */
    private userService: UserService,
    /**
      * Service para a troca do changeDetection no componente - Trocado para ChangeDetectionStrategy.OnPush
      * para melhorar a performance na tela. Esse changeDetection troca a detecção de mudanças automaticas
      * somente para Inputs e Outputs, o resto é feito manualmente.
       */
    private changeDetector: ChangeDetectorRef,
    /**
    * Service de rotas
    */
    private router: Router,
    /**
    * Service de informações do sistema
    */
    private systemInfoService: SysInfoService
  ) { }

  /**
  * Variável que recebe o valor do menu convertido para a tela.
  */
  xmlItems: MenuJson[];
  /**
  * Subscription para dar unsubscribed no behavior
  */
  subscription: Subscription;
  /**
  * Permissões do usuário
  */
  permissoes: string[]

  ngOnInit() {
    this.permissoes = this.userService.getCurrentUser().permissoes;
    this.subscription = this.userService.currentUser.subscribe(
      () => {
        this.systemInfoService.getMenuXML().then(resposta => {
          this.xmlItems = convertToItemMenu(resposta)
          // Avisa o menu que deve detectar a mudança nos itens do menu
          this.changeDetector.detectChanges()
        })
      }
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe()
  }

  /**
  * Verifica as rotas que devem ser acessadas ao entrar no menu SAP
  */
  acessarRota(menuPai: string, url: any) {
    let nUrl = '';
    // Verifica se o menu pai que irá para o proximo menu é o SAP também para poder usar o
    //router
    if (url.includes('javascript:NewSiteRedirect')  ) {
      // Faz o replace na string que vem da url do xml para a rota certo no angular
      url = url.toString().replace('javascript:NewSiteRedirect("S1", "#', ""
      ).replace('")', "")
      if(url === this.router.url){
        this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
          this.router.navigate([url]);
      });
      }else{
        this.router.navigate([url])
      }

    } else {
      // Faz o replace na string que vem da url do xml que não é para o SAP e redireciona
      nUrl = environment.s1_url + '/' + url.toString().replace("~/", "")
      window.top.location.href = '';
      window.top.location.href = nUrl;
    }
  }

  /**
  * Verifica no usuário se ele possui a permissão que vem do xml
  */
  hasRole(permissaoMenu) {
    return permissaoMenu.some(r=> this.permissoes.includes(r))
  }

  /**
  * Trackby para melhorar a performance do Ngfor a fim de não precisar mudar todo o menu
  * a cada verificação
  */
  trackByFunction(index, item) {
    if (!item) return null;
    return index;
  }
}


