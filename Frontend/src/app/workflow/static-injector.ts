import { Injector } from '@angular/core';

export class StaticInjector {
  private static instance: Injector;

  static setInjectorInstance(instance: Injector): void {
    this.instance = instance;
  }

  static get Instance(): Injector {
    if (!this.instance) {
      throw new Error('Injector instance not set!');
    }
    return this.instance;
  }
}
