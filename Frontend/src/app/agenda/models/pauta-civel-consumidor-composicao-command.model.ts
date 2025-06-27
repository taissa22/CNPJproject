export class PautaCivelConsumidorComposicaoCommandModel {
    public codParteEmpresa: string;
    public codComarca: number;
    public codTipoVara: number;
    public codVara: number;
    public dataAudiencia: string;
    public codPreposto: number[] = [];
    public codPrepostoPrincipal: number;
}
