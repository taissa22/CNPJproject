import { ComarcaBBDTO } from './../../../shared/interfaces/comarca-BB-DTO';
import { BaseService } from './../base.service';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class InterfaceBBComarcaBBendpointService extends BaseService<ComarcaBBDTO, number>  {

    
    endpoint = 'BBComarca';

}
