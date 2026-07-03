import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { TableComponent, TableColumn } from './table.component';
import { ColumnCellDirective } from './column-cell.directive';

interface TestRow {
    id: string;
    name: string;
    status: string;
}

@Component({
    standalone: true,
    imports: [TableComponent, ColumnCellDirective],
    template: `
        <app-table [data]="data" [columns]="columns">
            <ng-template appColumnCell="status" let-row>
                <span class="status-cell">{{ row.status }}-{{ row.name }}</span>
            </ng-template>
        </app-table>
    `,
})
class HostComponent {
    data: TestRow[] = [{ id: '1', name: 'Test Patient', status: 'Active' }];
    columns: TableColumn[] = [
        { key: 'name', label: 'Name' },
        { key: 'status', label: 'Status' },
    ];
}

describe('TableComponent', () => {
    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [HostComponent],
        }).compileComponents();
    });

    it('should create', () => {
        const fixture = TestBed.createComponent(TableComponent);
        expect(fixture.componentInstance).toBeTruthy();
    });

    it('should render plain column values without a custom template', () => {
        const fixture = TestBed.createComponent(HostComponent);
        fixture.detectChanges();
        const cells = fixture.nativeElement.querySelectorAll('td');
        expect(cells[0].textContent).toContain('Test Patient');
    });

    // Regression test: a custom appColumnCell template's `let-row` variable (bound to the
    // ngTemplateOutlet context's $implicit) must receive the full row object, not the single
    // cell's raw value — otherwise every template referencing `row.<field>` throws and the
    // whole cell silently fails to render (see table.component.html / getCellTemplate).
    it('should pass the full row object to a custom column-cell template via let-row', () => {
        const fixture = TestBed.createComponent(HostComponent);
        fixture.detectChanges();
        const statusCell = fixture.nativeElement.querySelector('.status-cell');
        expect(statusCell).toBeTruthy();
        expect(statusCell.textContent.trim()).toBe('Active-Test Patient');
    });

    it('should show the empty message when there is no data', () => {
        const fixture = TestBed.createComponent(HostComponent);
        fixture.componentInstance.data = [];
        fixture.detectChanges();
        expect(fixture.nativeElement.textContent).toContain('لا توجد بيانات');
    });

    it('should show the loading row when loading is true', () => {
        const fixture = TestBed.createComponent(TableComponent);
        fixture.componentInstance.loading = true;
        fixture.detectChanges();
        expect(fixture.nativeElement.textContent).toContain('جاري التحميل...');
    });
});
