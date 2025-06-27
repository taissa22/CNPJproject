export class ErrorLib {

  static ConverteMensagemErro(error: any) {
    const excessao = error.error
    let mensagem: string = '<br>';
    if (typeof excessao === 'object'
      && Array.isArray(excessao)) {
      excessao.forEach((erro: string) => {
        mensagem += `<p>- ${erro}</p>`;
      });
    } else if (excessao != null && typeof excessao === 'object') {
      console.error(excessao.erro)
      mensagem = excessao.mensagem;
    } else if (excessao == null){
      mensagem = error.message;
    } else {
      mensagem = excessao;
    }

    return mensagem;
  }

  static UnificaMensagensErroTratadas(erros: any) {
    let mensagem: string = '';
    if (Array.isArray(erros)) {
      erros.forEach((erro: string) => {
        mensagem += `${erro}`;
      });
    } else {
      mensagem = erros;
    }

    return mensagem;
  }

  static ConverteMensagemErroImportacao(error: any) {
    const excessao = error.error
    let mensagem: string;
    if (typeof excessao === 'object'
      && Array.isArray(excessao)) {
      const textoLinhas = excessao.length > 1 ? 'As linhas' : 'A linha';
      const textoPadroes = excessao.length > 1 ? 'estão' : 'está';
      mensagem = `${textoLinhas} ${excessao.join(', ')} não ${textoPadroes} dentro dos padrões de cadastro e por conta disso a carga em bloco não poderá ser realizada. </br> Verifique e tente novamente.`;
    } else {
      mensagem = excessao;
    }
    return mensagem;
  }

}
