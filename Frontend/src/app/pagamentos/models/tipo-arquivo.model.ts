export abstract class TipoArquivo {
  static readonly ARQUIVO_CARREGADO: string = 'ArquivosCarregados';
  static readonly ARQUIVO_PADRAO: string = 'ArquivosPadrao';
  static readonly RESULTADO_CARGA: string = 'ResultadoCarga';
  static readonly NAO_INFORMADO: string = 'NaoInformado';

  static obterTodos(): Array<TipoArquivo> {
    return [
      this.ARQUIVO_CARREGADO,
      this.ARQUIVO_PADRAO,
      this.RESULTADO_CARGA,
      this.NAO_INFORMADO
    ];
  }
}
