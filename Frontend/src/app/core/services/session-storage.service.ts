import { PLATFORM_ID, Injectable, Inject } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class SessionStorageService {

  constructor() { }

  private getSessionStorage() {
    return (typeof window !== 'undefined') ? window.sessionStorage : null;
  }

  setItem(key, value) {
    // const sessionStorage = this.getSessionStorage();
    const sessionStorage = this.getSessionStorage();
    if (!sessionStorage) return;

    sessionStorage.setItem(key, value);
  }

  setItemJson(key, value) {
    const sessionStorage = this.getSessionStorage();
    if (!sessionStorage) return;

    sessionStorage.setItem(key, JSON.stringify(value));
  }

  getItem(key) {
    const sessionStorage = this.getSessionStorage();
    if (!sessionStorage) return;

    return sessionStorage.getItem(key);
  }

  getItemJson(key) {
    const sessionStorage = this.getSessionStorage();
    if (!sessionStorage) return;

    const json = sessionStorage.getItem(key);
    return JSON.parse(json);
  }

  removeItem(key) {
    const sessionStorage = this.getSessionStorage();
    if (!sessionStorage) return;

    return sessionStorage.removeItem(key);
  }

  hasItem(key) {
    if (!sessionStorage[key]) return false;
    if (typeof sessionStorage[key] === 'undefined') return false;
    return true;
  }
}
