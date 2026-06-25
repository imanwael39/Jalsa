import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'dateAgo',
    standalone: true,
})
export class DateAgoPipe implements PipeTransform {
    transform(value: string | Date): string {
        if (!value) return '';
        const date = typeof value === 'string' ? new Date(value) : value;
        const now = new Date();
        const diff = Math.floor((now.getTime() - date.getTime()) / 1000);

        if (diff < 60) return 'just now';
        if (diff < 3600) return Math.floor(diff / 60) + 'm ago';
        if (diff < 86400) return Math.floor(diff / 3600) + 'h ago';
        if (diff < 604800) return Math.floor(diff / 86400) + 'd ago';
        if (diff < 2592000) return Math.floor(diff / 604800) + 'w ago';
        if (diff < 31536000) return Math.floor(diff / 2592000) + 'mo ago';
        return Math.floor(diff / 31536000) + 'y ago';
    }
}
