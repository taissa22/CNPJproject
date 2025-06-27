import { Component, OnInit } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { UserService, JwtService } from './core';
import { DomSanitizer, SafeResourceUrl, Title } from '@angular/platform-browser';
import { HeaderService } from '@core/services/header.service';
import { environment } from '@environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  private token = '';
  private login = '';
  isAuthenticated: boolean;
  // prettier-ignore
  opcoesLogoOi: number[] = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16];
  numeroLogoOi: number;
  get displayHeader(): boolean {
    return this.header.headerVisible;
  }

  urlSafe: SafeResourceUrl = null ;
  url: string = `${environment.s1_url}/KeepSessionAlive.aspx`;

  constructor(
    jwtService: JwtService,
    private userService: UserService,
    private header: HeaderService,
    private titleService: Title,
    public sanitizer: DomSanitizer
  ) {
    if (!jwtService.saveTokenByCrossSite()) {
      // let params = (new URL(location.href)).searchParams;
      this.login = this.getParamValueQueryString('u');
      this.token = this.getParamValueQueryString('tk');
    }
  }

  getParamValueQueryString(paramName) {
    const url = window.location.href;
    let paramValue;
    if (url.includes('?')) {
      const httpParams = new HttpParams({ fromString: url.split('?')[1] });
      paramValue = httpParams.get(paramName);
    }
    return paramValue;
  }
  ngOnInit() {
    this.urlSafe = this.sanitizer.bypassSecurityTrustResourceUrl(this.url);

    this.userService.currentUser.subscribe(user => {
      user.nome && this.setTitle(`Oi - Jurídico - Usuário: ${user.nome}`);
      //console.log(user)
    });

    this.numeroLogoOi = this.opcoesLogoOi[
      Math.floor(Math.random() * this.opcoesLogoOi.length)
    ];
    this.userService.isAuthenticated.subscribe(authenticated => {
      this.isAuthenticated = authenticated;
    });
    if (this.token && this.login) {
      this.userService.attemptAuth(this.login, this.token);
    } else {
      this.userService.populate();
      // this.router.navigate(['/cargapagamentos']);
    }
    // this.isAuthenticated = true;
  }

  public setTitle(newTitle: string) {
    this.titleService.setTitle(newTitle);
  }
}

