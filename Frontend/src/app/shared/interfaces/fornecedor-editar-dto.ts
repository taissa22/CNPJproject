export interface FornecedorEditarDto {
    id: number,
    codigoTipoFornecedor: number,
    codigoEscritorio?: number,
    codigoProfissional?: number,
    codigoBanco?: number,
    nomeFornecedor: string,
    codigoFornecedorSAP: string,
    criarCodigoFornecedorSAP: boolean;

}
