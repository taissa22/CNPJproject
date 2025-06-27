export class ResultRequestModel<T> {
  result: T;
  data: T;
  total: number = 0;
  id: number = 1;
  exception: string = null;
  status: number = 5;
  isCanceled: boolean = false;
  isCompleted: boolean = true;
  isCompletedSuccessfully: boolean = true;
  isFaulted: boolean = false;
}
