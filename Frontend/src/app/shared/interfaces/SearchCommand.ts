import { Filter } from './Filter';
import { SortOrder } from './SortOrder';

export interface SearchCommand {
  filters: Filter[];
  sortOrders: SortOrder[];
  pageNumber: number;
  pageSize: number;
  isExportMethod: boolean;
}
