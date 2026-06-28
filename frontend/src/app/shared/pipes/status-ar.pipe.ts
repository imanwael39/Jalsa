import { Pipe, PipeTransform } from '@angular/core';

const STATUS_MAP: Record<string, string> = {
    Active: 'نشط',
    Archived: 'مؤرشف',
    Draft: 'مسودة',
    Completed: 'مكتمل',
    Approved: 'معتمد',
    Submitted: 'مُقدَّم',
    NotStarted: 'لم يبدأ',
    InProgress: 'قيد التنفيذ',
    Overdue: 'متأخر',
    PartiallyCompleted: 'مكتمل جزئياً',
    Skipped: 'متخطى',
    Male: 'ذكر',
    Female: 'أنثى',
};

@Pipe({ name: 'statusAr', standalone: true })
export class StatusArPipe implements PipeTransform {
    transform(value: string | null | undefined): string {
        if (!value) return '';
        return STATUS_MAP[value] ?? value;
    }
}
