import { ClassificacaoHierarquica } from './classificacao-hierarquica';

export abstract class ClassificacoesHierarquicas {
  static readonly UNICO = new ClassificacaoHierarquica('U','ÚNICO');
  static readonly PRIMARIO = new ClassificacaoHierarquica('P','PRIMÁRIO');
  static readonly SECUNDARIO = new ClassificacaoHierarquica('S','SECUNDÁRIO');
  

  static obterClassicacoes() {
    return [
      this.UNICO,
      this.PRIMARIO,
      this.SECUNDARIO
      
    ];
  }
}
