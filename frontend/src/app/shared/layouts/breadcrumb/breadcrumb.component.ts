import { Component, ChangeDetectionStrategy, inject, signal, OnInit, DestroyRef } from '@angular/core';
import { ActivatedRouteSnapshot, NavigationEnd, Router, RouterLink } from '@angular/router';
import { filter } from 'rxjs/operators';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

export interface Breadcrumb {
    label: string;
    url: string;
}

@Component({
    selector: 'app-breadcrumb',
    standalone: true,
    imports: [RouterLink],
    templateUrl: './breadcrumb.component.html',
    styleUrl: './breadcrumb.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BreadcrumbComponent implements OnInit {
    private router = inject(Router);
    private destroyRef = inject(DestroyRef);

    breadcrumbs = signal<Breadcrumb[]>([]);

    ngOnInit(): void {
        this.updateBreadcrumbs();
        this.router.events
            .pipe(
                filter((event): event is NavigationEnd => event instanceof NavigationEnd),
                takeUntilDestroyed(this.destroyRef)
            )
            .subscribe(() => this.updateBreadcrumbs());
    }

    private updateBreadcrumbs(): void {
        this.breadcrumbs.set(this.buildBreadcrumbs(this.router.routerState.snapshot.root));
    }

    private buildBreadcrumbs(snapshot: ActivatedRouteSnapshot, parentUrl = '', trail: Breadcrumb[] = []): Breadcrumb[] {
        const segment = snapshot.url.map(s => s.path).join('/');
        const url = segment ? `${parentUrl}/${segment}` : parentUrl;

        const label = snapshot.data['breadcrumb'] as string | undefined;
        const nextTrail = label ? [...trail, { label, url }] : trail;

        if (snapshot.firstChild) {
            return this.buildBreadcrumbs(snapshot.firstChild, url, nextTrail);
        }
        return nextTrail;
    }
}
