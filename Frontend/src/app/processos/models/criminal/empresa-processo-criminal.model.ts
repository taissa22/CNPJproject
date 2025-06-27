import { ProcessoCriminalModel } from './processo-criminal.model';

export class EmpresaProcessoCriminalModel{
    id: number;
    nome: string;
    processos: Array<ProcessoCriminalModel>
}