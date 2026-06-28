import {
    ChangeDetectionStrategy,
    Component,
    ContentChildren,
    EventEmitter,
    Input,
    Output,
    QueryList,
    TrackByFunction,
} from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';
import { ColumnCellDirective } from './column-cell.directive';

export interface TableColumn {
    key: string;
    label: string;
    sortable?: boolean;
    width?: string;
    align?: 'left' | 'center' | 'right';
}

export interface SortEvent {
    key: string;
    direction: 'asc' | 'desc';
}

export interface RowActionEvent<T = Record<string, unknown>> {
    action: string;
    row: T;
}

@Component({
    selector: 'app-table',
    imports: [NgTemplateOutlet],
    templateUrl: './table.component.html',
    styleUrl: './table.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TableComponent<T = Record<string, unknown>> {
    @Input() data: T[] = [];
    @Input() columns: TableColumn[] = [];
    @Input() loading = false;
    @Input() emptyMessage = 'لا توجد بيانات';
    @Input() striped = true;
    @Input() bordered = false;
    @Input() hover = true;
    @Input() small = false;

    @Output() sort = new EventEmitter<SortEvent>();
    @Output() rowClick = new EventEmitter<T>();
    @Output() rowAction = new EventEmitter<RowActionEvent<T>>();

    @ContentChildren(ColumnCellDirective) cellTemplates!: QueryList<ColumnCellDirective>;

    private currentSortKey = '';
    private currentSortDirection: 'asc' | 'desc' = 'asc';

    get sortDirection(): 'asc' | 'desc' {
        return this.currentSortDirection;
    }

    get sortKey(): string {
        return this.currentSortKey;
    }

    sortColumn(column: TableColumn): void {
        if (!column.sortable) {
            return;
        }

        if (this.currentSortKey === column.key) {
            this.currentSortDirection = this.currentSortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            this.currentSortKey = column.key;
            this.currentSortDirection = 'asc';
        }

        this.sort.emit({ key: column.key, direction: this.currentSortDirection });
    }

    getSortIcon(column: TableColumn): string {
        if (!column.sortable) {
            return '';
        }

        if (this.currentSortKey !== column.key) {
            return 'bi-arrow-down-up';
        }

        return this.currentSortDirection === 'asc' ? 'bi-arrow-up' : 'bi-arrow-down';
    }

    onRowClick(row: T): void {
        this.rowClick.emit(row);
    }

    onActionClick(action: string, row: T, event: Event): void {
        event.stopPropagation();
        this.rowAction.emit({ action, row });
    }

    getCellValue(row: T, column: TableColumn): unknown {
        const record = row as Record<string, unknown>;
        return record[column.key];
    }

    getCellTemplate(column: ColumnCellDirective): ColumnCellDirective | undefined {
        return this.cellTemplates?.find(tpl => tpl.columnKey === column.columnKey);
    }

    trackByRow: TrackByFunction<T> = (index: number, item: T): unknown => {
        const record = item as Record<string, unknown>;
        return record['id'] ?? index;
    };

    trackByColumn: TrackByFunction<TableColumn> = (index: number, column: TableColumn): string => {
        return column.key;
    };

    trackByCellTemplate: TrackByFunction<ColumnCellDirective> = (
        index: number,
        directive: ColumnCellDirective
    ): string => {
        return directive.columnKey;
    };
}
