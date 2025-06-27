import { BehaviorSubject } from 'rxjs';

export interface IPaginatorService {
    pageSubject: BehaviorSubject<number>;
    pageMaxSubject: BehaviorSubject<number>;
    setPage(page: number);
    setMaxPage(page: number);
    setTotal(total: number, quantidade: number);
}
