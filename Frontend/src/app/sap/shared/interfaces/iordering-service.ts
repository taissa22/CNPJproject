import { BehaviorSubject } from 'rxjs';

export interface IOrderingService {
    campoSubject: BehaviorSubject<string>;
    ascendenteSubject: BehaviorSubject<boolean>;
    setAscencao(isAscendente: boolean);
    setCampo(campo: string);
}
