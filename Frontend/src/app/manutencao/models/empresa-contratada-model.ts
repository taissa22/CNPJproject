export class EmpresaContratadaModel{
    codEmpresaContratada: number;
    nomEmpresaContratada: string;
    matriculas: Array<TerceiroContratado>;
}

class TerceiroContratado{
    codTerceiroContratado: number;
    LoginTerceiro: string;
}