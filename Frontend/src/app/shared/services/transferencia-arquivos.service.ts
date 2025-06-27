import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TransferenciaArquivos {

  downloadOptions = { observe: 'response' as 'body', responseType: 'blob' as 'json' };

  constructor(private http: HttpClient) { }

  obterNomeArquivo(respostaRequisicao: { headers: any }): string {
    return respostaRequisicao.headers.get('Content-Disposition').split(';')[1].split('filename')[1].split('=')[1].trim();
  }

  downloadFile(blob: Blob, filename: string) {
    const url = window.URL.createObjectURL(blob);

    const a = document.createElement('a');
    a.setAttribute('style', 'display:none;');
    document.body.appendChild(a);

    a.href = url;
    a.download = filename;
    a.click();

    document.body.removeChild(a);
  }

  /**
  * @deprecated Este método não deve ser utilizado, utilizar o método baixarArquivo(url: string)
  */
  download(file: Blob, filename: string) {
    if (navigator.appVersion.toString().indexOf('.NET') > 0) {
      window.navigator.msSaveBlob(file, filename);
    } else {
      this.downloadFile(file, filename);
    }
  }

  async baixarArquivo(url: string) {
    try {
      const response: any = await this.http
        .get(url, this.downloadOptions)
        .toPromise();
      const file = response.body;
      let filename = this.obterNomeArquivo(response);
      filename = filename.replace(/"/g, '')
      this.downloadFile(file, filename);
    } catch (error) {
      throw error;
    }
  }

  async baixarArquivoPost(url: string, obj:object) {
    try {
      const response: any = await this.http
        .post(url,obj, this.downloadOptions)
        .toPromise();
      const file = response.body;
      let filename = this.obterNomeArquivo(response);
      filename = filename.replace(/"/g, '')
      this.downloadFile(file, filename);
    } catch (error) {
      throw error;
    }
  }

}
