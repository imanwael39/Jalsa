import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component } from '@angular/core';
import { Router, provideRouter, Routes } from '@angular/router';
import { BreadcrumbComponent } from './breadcrumb.component';

@Component({ selector: 'app-stub', template: '', standalone: true })
class StubComponent {}

const testRoutes: Routes = [
    {
        path: 'patients',
        data: { breadcrumb: 'المرضى' },
        children: [
            { path: '', component: StubComponent },
            { path: 'new', data: { breadcrumb: 'مريض جديد' }, component: StubComponent },
            { path: ':id/edit', data: { breadcrumb: 'تعديل المريض' }, component: StubComponent },
        ],
    },
    {
        path: 'dashboard',
        data: { breadcrumb: 'لوحة التحكم' },
        component: StubComponent,
    },
];

describe('BreadcrumbComponent', () => {
    let component: BreadcrumbComponent;
    let fixture: ComponentFixture<BreadcrumbComponent>;
    let router: Router;

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [provideRouter(testRoutes)],
        });

        fixture = TestBed.createComponent(BreadcrumbComponent);
        component = fixture.componentInstance;
        router = TestBed.inject(Router);
    });

    it('should create', () => {
        fixture.detectChanges();
        expect(component).toBeTruthy();
    });

    it('should hide the breadcrumb trail on a single-level route', async () => {
        await router.navigateByUrl('/dashboard');
        fixture.detectChanges();
        expect(component.breadcrumbs().length).toBe(1);
    });

    it('should build a two-level trail for a nested route', async () => {
        await router.navigateByUrl('/patients/new');
        fixture.detectChanges();
        expect(component.breadcrumbs()).toEqual([
            { label: 'المرضى', url: '/patients' },
            { label: 'مريض جديد', url: '/patients/new' },
        ]);
    });

    it('should build the correct trail for a route with a dynamic segment', async () => {
        await router.navigateByUrl('/patients/123/edit');
        fixture.detectChanges();
        expect(component.breadcrumbs()).toEqual([
            { label: 'المرضى', url: '/patients' },
            { label: 'تعديل المريض', url: '/patients/123/edit' },
        ]);
    });

    it('should update the trail when navigation changes', async () => {
        await router.navigateByUrl('/patients/new');
        fixture.detectChanges();
        expect(component.breadcrumbs().length).toBe(2);

        await router.navigateByUrl('/dashboard');
        fixture.detectChanges();
        expect(component.breadcrumbs()).toEqual([{ label: 'لوحة التحكم', url: '/dashboard' }]);
    });
});
