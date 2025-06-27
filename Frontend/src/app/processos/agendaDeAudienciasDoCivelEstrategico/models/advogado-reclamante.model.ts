import { Estado } from './estado.model';

export class AdvogadoReclamante {
    id: number;
    nome: string;
    ativo: boolean;
    tipoPessoa: string;
    registroOAB: string;
    estadoOAB: Estado;
    comentario: string;
}
