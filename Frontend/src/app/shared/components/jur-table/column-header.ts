/**
 * @deprecated Use '@sisjur/sisjur-table' instead.
 */
export declare class ColumnHeader {
  /** The name for display. */
  columnName: string;
  /** The column name to emit on sort event. */
  sortColumn?: string;
  /** The size of column, defined in '%', 'px' or 'auto'. */
  flex: string | number | 'auto';
}
