import { Injector } from '@angular/core';

export class StaticInjector {
    private static instance: Injector;

    static get Instance(): Injector {
        return this.instance;
    }

    static setInjectorInstance(instance: Injector): void {
        this.instance = instance;
    }
}
