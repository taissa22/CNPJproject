import { Observable } from 'rxjs';

export interface ISysInfoRepository {
    getSysVersion(): Promise<any>;
}
