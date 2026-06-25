import { Directive, ElementRef, Output, EventEmitter, HostListener, inject } from '@angular/core';

@Directive({
    selector: '[appClickOutside]',
})
export class ClickOutsideDirective {
    private elementRef = inject(ElementRef);
    @Output() appClickOutside = new EventEmitter<void>();

    @HostListener('document:click', ['$event'])
    onClick(event: MouseEvent): void {
        const target = event.target as HTMLElement;
        if (!this.elementRef.nativeElement.contains(target)) {
            this.appClickOutside.emit();
        }
    }
}
