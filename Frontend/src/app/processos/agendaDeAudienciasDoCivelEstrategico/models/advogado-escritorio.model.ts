export class AdvogadoEscritorio {
  constructor(id: number, nome: string, profissionalId?: number) {
    this.id = id;
    this.nome = nome;
    this.profissionalId = profissionalId;
  }

  id: number;
  nome: string;
  profissionalId?: number;
}
