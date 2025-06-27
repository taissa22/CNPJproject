import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject, ReplaySubject } from 'rxjs';
import { Router } from '@angular/router';
import { ApiService } from './api.service';
import { JwtService } from './jwt.service';
import { User } from '../models';
import { environment } from '../../../environments/environment';
import { map, distinctUntilChanged, switchMap } from 'rxjs/operators';



@Injectable()
export class UserService {
  private currentUserSubject = new BehaviorSubject<User>({} as User);
  public currentUser = this.currentUserSubject.asObservable().pipe(distinctUntilChanged());

  private isAuthenticatedSubject = new ReplaySubject<boolean>(1);
  public isAuthenticated = this.isAuthenticatedSubject.asObservable();

  constructor(
    private router: Router,
    private apiService: ApiService,
    private http: HttpClient,
    private jwtService: JwtService
  ) { }

  // Verify JWT in localstorage with server & load user's info.
  // This runs once on application startup.
  populate() {

    if (this.jwtService.getToken()) {
      this.apiService.getv2('/Accounts/User')
        .subscribe(
          data => this.setAuth(data),
          err => this.purgeAuth()
        );
    } else {
      // Remove any potential remnants of previous auth states
      this.purgeAuth();
    }
  }

  setAuth(user: User) {
    // Save JWT sent from server in localstorage
    // this.jwtService.saveToken(user.token);
    // Set current user data into observable
    this.currentUserSubject.next(user);
    this.jwtService.saveJwtUsuario(user.username);
    // Set isAuthenticated to true
    this.isAuthenticatedSubject.next(true);
  }

  purgeAuth() {
    // Remove JWT from localstorage
    // this.jwtService.destroyToken();
    this.jwtService.signOut();
    // Set current user to an empty object
    this.currentUserSubject.next({} as User);
    // Set auth status to false
    this.isAuthenticatedSubject.next(false);
    //this.router.navigate(['/home']);
    window.location.href = environment.s1_url + '/logout/logout.aspx';
  }

  acessarOutrosModulos() {
    window.location.href = environment.s1_url + '/2.0/Home.aspx';
  }

  userAuthentication(userName, token) {

    const params: any = {
      ClientId: environment.client_id,
      Granttype: 'password',
      Username: userName,
      Password: token,
      RefreshToken: null
    };
    // Encodes the parameters.
    const body: string = this.encodeParams(params);
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/x-www-urlencoded' });
    return this.http.post(environment.api_url + '/Accounts/login', params, { headers: reqHeader });
  }

  userAuthenticationRefreshToken(userName, refreshToken) {

    const params: any = {
      ClientId: environment.client_id,
      GrantType:'refresh_token',    
      Username: userName,
      Password: null,
      RefreshToken: refreshToken
    };
    // Encodes the parameters.
    const body: string = this.encodeParams(params);
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/x-www-urlencoded' });
    return this.http.post(environment.api_url + '/Accounts/login', params, { headers: reqHeader });
  }

  attemptAuth(userName, token) {
    this.userAuthentication(userName, token).subscribe((data: any) => {
      this.jwtService.saveToken(data.data.access_token);
      this.jwtService.saveRefreshToken(data.data.refresh_token);
      this.populate();

      if (this.router.url === '') {
        this.router.navigate(['/home']);
      }
    },
      (err: HttpErrorResponse) => {
        this.purgeAuth();
      });
  }

  attemptRefreshAuth(userName, refreshToken) {
    this.userAuthenticationRefreshToken(userName, refreshToken).subscribe((data: any) => {
      this.jwtService.saveToken(data.data.access_token);
      this.jwtService.saveRefreshToken(data.data.refresh_token);
      this.populate();
    },
      (err: HttpErrorResponse) => {
        this.purgeAuth();
      });
  }

  getCurrentUser(): User {
    return this.currentUserSubject.value;
  }

  // Update the user on the server (email, pass, etc)
  update(user): Observable<User> {
    return this.apiService
      .put('/user', { user })
      .pipe(map(data => {
        // Update the currentUser observable
        this.currentUserSubject.next(data.user);
        return data.user;
      }));
  }

  /**
   * // Encodes the parameters.
   *
   * @param params The parameters to be encoded
   * @return The encoded parameters
   */
  private encodeParams(params: any): string {

    let body = '';
    // tslint:disable-next-line: forin
    for (const key in params) {
      if (body.length) {
        body += '&';
      }
      body += key + '=';
      body += encodeURIComponent(params[key]);
    }

    return body;
  }


}
