import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, TrackByFunction } from '@angular/core';

@Component({
    selector: 'app-pagination',
    templateUrl: './pagination.component.html',
    styleUrl: './pagination.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PaginationComponent {
    @Input() totalItems = 0;
    @Input() pageSize = 10;
    @Input() currentPage = 1;
    @Input() maxVisible = 5;

    @Output() pageChange = new EventEmitter<number>();

    get totalPages(): number {
        if (this.totalItems <= 0 || this.pageSize <= 0) {
            return 0;
        }
        return Math.ceil(this.totalItems / this.pageSize);
    }

    get pages(): number[] {
        const total = this.totalPages;
        if (total === 0) {
            return [];
        }

        const current = this.currentPage;
        const max = this.maxVisible;
        let start = Math.max(1, current - Math.floor(max / 2));
        const end = Math.min(total, start + max - 1);

        if (end - start + 1 < max) {
            start = Math.max(1, end - max + 1);
        }

        return Array.from({ length: end - start + 1 }, (_, i) => start + i);
    }

    get isFirstPage(): boolean {
        return this.currentPage <= 1;
    }

    get isLastPage(): boolean {
        return this.currentPage >= this.totalPages;
    }

    get hasPages(): boolean {
        return this.totalPages > 1;
    }

    get startIndex(): number {
        if (this.totalItems === 0) {
            return 0;
        }
        return (this.currentPage - 1) * this.pageSize + 1;
    }

    get endIndex(): number {
        return Math.min(this.currentPage * this.pageSize, this.totalItems);
    }

    goToPage(page: number): void {
        if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
            this.pageChange.emit(page);
        }
    }

    goToFirstPage(): void {
        this.goToPage(1);
    }

    goToLastPage(): void {
        this.goToPage(this.totalPages);
    }

    goToPreviousPage(): void {
        this.goToPage(this.currentPage - 1);
    }

    goToNextPage(): void {
        this.goToPage(this.currentPage + 1);
    }

    trackByPage: TrackByFunction<number> = (index: number, page: number): number => {
        return page;
    };
}
