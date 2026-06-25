import { Directive, Input, TemplateRef } from '@angular/core';

@Directive({
    selector: '[appColumnCell]',
})
export class ColumnCellDirective {
    @Input('appColumnCell') columnKey = '';

    constructor(public templateRef: TemplateRef<unknown>) {}
}
