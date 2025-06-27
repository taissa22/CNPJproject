import { HttpErrorResponse } from '@angular/common/http';

export class HttpErrorResult {
    constructor(messages: string[]) {
        this.messages = messages;
    }
    readonly messages: string[];
    public static fromError(error: HttpErrorResponse): HttpErrorResult {
        const httpError: HttpErrorResponse = error;   
        if (Array.isArray(httpError.error) && !httpError.error.join('\n').includes('exception')) {            
            return new HttpErrorResult(httpError.error);
        } else if (httpError instanceof Error) {
          return new HttpErrorResult(httpError.error);
        }

        if (httpError.error && (httpError.error.value != ''))
            return new HttpErrorResult([httpError.error.value]);
        
        return new HttpErrorResult(['Erro desconhecido']);
    }
}
