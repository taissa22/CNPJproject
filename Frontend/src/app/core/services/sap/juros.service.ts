import { BaseService } from './../base.service';
import { Injectable } from '@angular/core';
import { JurosDTO } from '../../../shared/interfaces/jurosDTO';

@Injectable({
    providedIn: 'root'
})
export class JurosService extends BaseService<JurosDTO, number> {

    endpoint = 'JuroCorrecaoProcesso';
}