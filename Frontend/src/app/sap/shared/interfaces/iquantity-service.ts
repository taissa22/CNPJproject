import { BehaviorSubject } from 'rxjs';

export interface IQuantityService {
    quantidadeSubject: BehaviorSubject<number>;
    setQuantidade(quantidade: number);
}
