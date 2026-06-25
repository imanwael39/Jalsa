import { Directive, Input, TemplateRef, ViewContainerRef, inject } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';

@Directive({
    selector: '[appRole]',
})
export class RoleDirective {
    private templateRef = inject(TemplateRef<unknown>);
    private viewContainer = inject(ViewContainerRef);
    private authService = inject(AuthService);
    private currentRoles: string[] = [];

    @Input() set appRole(roles: string[]) {
        this.currentRoles = roles;
        this.updateView();
    }

    @Input() appRoleElse?: TemplateRef<unknown>;

    private updateView(): void {
        if (this.authService.hasAnyRole(this.currentRoles)) {
            if (this.viewContainer.length === 0) {
                this.viewContainer.createEmbeddedView(this.templateRef);
            }
        } else {
            this.viewContainer.clear();

            if (this.appRoleElse) {
                this.viewContainer.createEmbeddedView(this.appRoleElse);
            }
        }
    }
}
