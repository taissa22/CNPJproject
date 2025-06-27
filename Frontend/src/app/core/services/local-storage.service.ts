import { PLATFORM_ID, Injectable, Inject } from '@angular/core';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';

@Injectable({
  providedIn: 'root',
 })
export class LocalStorageService {
    constructor() {
    }

    private getLocalStorage() {
      return (typeof window !== 'undefined') ? window.localStorage : null;
    }

    setItem(key, value) {
      const localStorage = this.getLocalStorage();
      if (!localStorage) return;

      localStorage.setItem(key, value);
    }

    setItemJson(key, value) {
      const localStorage = this.getLocalStorage();
      if (!localStorage) return;

      localStorage.setItem(key, JSON.stringify(value));
    }

    getItem(key) {
      const localStorage = this.getLocalStorage();
      if (!localStorage) return;

      return localStorage.getItem(key);
    }

    getItemJson(key) {
      const localStorage = this.getLocalStorage();
      if (!localStorage) return;

      const json = localStorage.getItem(key);
      return JSON.parse(json);
    }

    removeItem(key) {
      const localStorage = this.getLocalStorage();
      if (!localStorage) return;

      return localStorage.removeItem(key);
    }

    hasItem(key) {
      if (!localStorage[key]) return false;
      if (typeof localStorage[key] === 'undefined') return false;
      return true;
    }
}
