import { Component, OnInit, OnDestroy } from '@angular/core';
import { User, UserService } from '../../core';
import { SysInfoService } from 'src/app/core/services/system/sys-info.service';
import { environment } from 'src/environments/environment';
import { Subscription } from 'rxjs/internal/Subscription';
import { HeaderService } from '@core/services/header.service';
import { CardMenu } from '@shared/models/card-menu.model';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-layout-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  constructor(
    private userService: UserService,
    private sysInfoService: SysInfoService,
    private headerService: HeaderService,
    public modalService: NgbActiveModal,
    public modal:NgbModal
  ) { }

  currentUser: User;
  versao = "---"
  opcoesLogoOi: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16];
  numeroLogoOi: number;
  subscription: Subscription;

  ngOnInit() {
    var menuPrincipal = localStorage.getItem('menuPrincipal');
    if (menuPrincipal == null || menuPrincipal == "")
    {
      this.headerService.getMenu().subscribe(res => {
        localStorage.setItem('menuPrincipal', JSON.stringify(res));
      });
    }

    var menuPesquisar = localStorage.getItem('menuPesquisar');
    if (menuPesquisar == null || menuPesquisar == ""){
      this.headerService.getMenuPesquisar(environment.s1_url).subscribe(res => {
        localStorage.setItem('menuPesquisar', JSON.stringify(res));
      });
    }

    this.sysInfoService.obterVersaoWeb().then(ver => this.versao = ver);
    this.numeroLogoOi = this.opcoesLogoOi[Math.floor(Math.random() * this.opcoesLogoOi.length)];
    this.subscription = this.userService.currentUser.subscribe(
      (userData) => {
        this.currentUser = userData;
      }
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe()
  }
  controleImplantacaoUrl = environment.s1_url + '/ControleDeImplantacao.Mvc'
  faleConoscoUrl = environment.s1_url + '/faleconosco/faleconosco.aspx'
  usuarioUrl = environment.s1_url + '/2.0/Profile/ShowProfile.aspx'
  versaoUrl = environment.s1_url + '/2.0/versionamento.aspx'

  logout() {
    this.userService.purgeAuth();
  }

  abrirNovaPaginaFaleConosco() {
    window.open(this.faleConoscoUrl,
      'newwindow',
      'width=400,height=400');
  }
  abrirNovaPaginaUsuario() {
    window.open(this.usuarioUrl,
      'newwindow',
      'width=750,height=500');
  }
  public fecharModal() {
    (window.document.querySelector('router-outlet').nextElementSibling as HTMLElement).style.display = '';
    this.modal.dismissAll();
  }


}
