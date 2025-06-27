export class Command {
  constructor(prop: any = {}) {
    Object.assign(this, prop);
  }

  [nome: string]: any;
}
