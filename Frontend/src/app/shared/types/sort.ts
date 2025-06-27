export class Sort {
  column: string;
  direction: 'asc' | 'desc';
}

export class SortOf<T extends string> extends Sort {
  column: T;
}
