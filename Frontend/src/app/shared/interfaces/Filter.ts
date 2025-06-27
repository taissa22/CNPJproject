import { FilterOperatorEnum } from '@shared/enums/FilterOperator.enum';


export class Filter {
  public fieldName: string;
  public value: string;
  public value2?: string;
  public filterOperator: FilterOperatorEnum;
}
