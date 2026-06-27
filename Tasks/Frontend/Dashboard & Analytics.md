📊 Jalsa – Phase 10: Dashboard & Analytics Expanded Implementation Handbook · 5 Tasks · 30+ Subtasks
====================================================================================================

📊 Phase 10 – Dashboard & Analytics
-----------------------------------

**Purpose:** Build the therapist dashboard with interactive charts and key performance indicators (KPIs). This phase provides therapists with an overview of patient progress, exercise completion rates, session statistics, and assessment trends using Chart.js.

📋 Tasks: 5 ⏱️ Total Effort: ~16 hours 👤 Owners: M3 (Frontend Lead), M4 (Frontend Developer) 🔗 Dependencies: Phase 7 (Patient Management), Phase 8 (Session Management), Phase 9 (Exercise Management) 🎯 Deliverable: Complete dashboard with stats cards, line charts, bar charts, and trend analysis

## FE-DASH-001Dashboard Module Setup (Routes, Service, State) P1 High 4h ▾

### Task Information

*   **Task ID:** FE-DASH-001
*   **Task Name:** Dashboard Module Setup (Routes, Service, State)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-CORE-001 (HttpClientService), FE-CORE-002 (Models), FE-AUTH-002 (AuthService), FE-LAYOUT-001 (Main layout)
*   **Complexity:** High
*   **Estimated Effort:** 4 hours
*   **Priority:** High

### Objective

Create the dashboard feature with lazy‑loaded routes, a DashboardService for API communication, and a DashboardStateService for managing dashboard data using Signals. This provides the foundation for all dashboard visualisations.

### Business Purpose

The dashboard is the primary landing page for therapists. It provides at‑a‑glance insights into patient activity, engagement, and progress.

### Technical Purpose

Set up lazy‑loaded routes under the main layout, create a service that uses HttpClientService for dashboard endpoints, and create a state service with Signals for reactive data management.

### Prerequisites

*   Understanding of Angular lazy loading with Standalone components.
*   Knowledge of Signals and services.
*   API contract for dashboard endpoints.

### Dependencies

*   **FE-CORE-001:** HttpClientService for API calls.
*   **FE-CORE-002:** Dashboard model interfaces.
*   **FE-AUTH-002:** AuthService for guards.
*   **FE-LAYOUT-001:** Main layout as parent route.

### Inputs

*   API endpoints from `api-endpoints.ts`.

### Outputs

*   `features/dashboard/dashboard.routes.ts` – Lazy‑loaded routes.
*   `core/services/dashboard.service.ts` – Dashboard API service.
*   `core/state/dashboard-state.service.ts` – Dashboard state management with Signals.
*   Placeholder `DashboardComponent`.

### Detailed Workflow

#### Step 1: Create Routes File

```typescript
// features/dashboard/dashboard.routes.ts
import { Routes } from '@angular/router';
import { authGuard, roleGuard } from '../../core/guards';

export const DASHBOARD_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard(['Therapist', 'Admin'])],
        loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent),
    },
];
```
#### Step 2: Register Routes in App Routes

Add lazy loading in `app.routes.ts` under the main layout.

```ts
// app.routes.ts (inside the main layout children)
{
    path: 'dashboard',
    loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.DASHBOARD_ROUTES),
},
```
#### Step 3: Create Dashboard Models

Ensure models exist in `core/models/dashboard.model.ts`.

```ts
// core/models/dashboard.model.ts
export interface DashboardStats {
    totalPatients: number;
    activePatients: number;
    totalSessions: number;
    completedSessions: number;
    pendingExercises: number;
    completedExercises: number;
    pendingReports: number;
    totalAssessments: number;
}

export interface TrendData {
    date: string;
    value: number;
    label?: string;
}

export interface TrendDataSet {
    label: string;
    data: TrendData[];
    color?: string;
}

export interface DashboardData {
    stats: DashboardStats;
    assessmentTrends: TrendDataSet[];
    exerciseCompletion: TrendDataSet[];
    sessionTrends: TrendDataSet[];
}
```
#### Step 4: Create Dashboard Service

```ts
// core/services/dashboard.service.ts
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../api/http-client.service';
import { API } from '../api/api-endpoints';
import { DashboardStats, TrendDataSet, DashboardData } from '../models';

@Injectable({
    providedIn: 'root',
})
export class DashboardService {
    private http = inject(HttpClientService);

    getDashboardData(): Observable<DashboardData> {
        return this.http.get<DashboardData>(API.dashboard.stats);
    }

    getStats(): Observable<DashboardStats> {
        return this.http.get<DashboardStats>(API.dashboard.stats);
    }

    getTrends(): Observable<{ assessmentTrends: TrendDataSet[]; exerciseCompletion: TrendDataSet[]; sessionTrends: TrendDataSet[] }> {
        return this.http.get<any>(API.dashboard.trends);
    }
}
```
#### Step 5: Create Dashboard State Service

```ts
// core/state/dashboard-state.service.ts
import { Injectable, signal } from '@angular/core';
import { DashboardStats, TrendDataSet, DashboardData } from '../models';

@Injectable({
    providedIn: 'root',
})
export class DashboardStateService {
    private statsSignal = signal<DashboardStats | null>(null);
    private assessmentTrendsSignal = signal<TrendDataSet[]>([]);
    private exerciseCompletionSignal = signal<TrendDataSet[]>([]);
    private sessionTrendsSignal = signal<TrendDataSet[]>([]);
    private loadingSignal = signal(false);
    private errorSignal = signal<string | null>(null);

    readonly stats = this.statsSignal.asReadonly();
    readonly assessmentTrends = this.assessmentTrendsSignal.asReadonly();
    readonly exerciseCompletion = this.exerciseCompletionSignal.asReadonly();
    readonly sessionTrends = this.sessionTrendsSignal.asReadonly();
    readonly loading = this.loadingSignal.asReadonly();
    readonly error = this.errorSignal.asReadonly();

    setDashboardData(data: DashboardData) {
        this.statsSignal.set(data.stats);
        this.assessmentTrendsSignal.set(data.assessmentTrends || []);
        this.exerciseCompletionSignal.set(data.exerciseCompletion || []);
        this.sessionTrendsSignal.set(data.sessionTrends || []);
        this.errorSignal.set(null);
    }

    setStats(stats: DashboardStats) {
        this.statsSignal.set(stats);
        this.errorSignal.set(null);
    }

    setTrends(assessmentTrends: TrendDataSet[], exerciseCompletion: TrendDataSet[], sessionTrends: TrendDataSet[]) {
        this.assessmentTrendsSignal.set(assessmentTrends);
        this.exerciseCompletionSignal.set(exerciseCompletion);
        this.sessionTrendsSignal.set(sessionTrends);
        this.errorSignal.set(null);
    }

    setLoading(loading: boolean) {
        this.loadingSignal.set(loading);
    }

    setError(error: string | null) {
        this.errorSignal.set(error);
    }

    reset() {
        this.statsSignal.set(null);
        this.assessmentTrendsSignal.set([]);
        this.exerciseCompletionSignal.set([]);
        this.sessionTrendsSignal.set([]);
        this.loadingSignal.set(false);
        this.errorSignal.set(null);
    }
}
```
#### Step 6: Generate Dashboard Component

```bash
ng g c features/dashboard/pages/dashboard --standalone --skip-tests
```
### Files To Create

*   `features/dashboard/dashboard.routes.ts`
*   `core/services/dashboard.service.ts`
*   `core/state/dashboard-state.service.ts`
*   Update `core/models/dashboard.model.ts`
*   `features/dashboard/pages/dashboard/dashboard.component.ts` (placeholder)

### CLI Commands

```bash
ng g s core/services/dashboard --skip-tests
ng g s core/state/dashboard-state --skip-tests
ng g c features/dashboard/pages/dashboard --standalone --skip-tests
```
### Code Flow

```
User navigates to /dashboard → DashboardComponent loads → Calls DashboardService.getDashboardData() → Updates DashboardStateService → Renders stats and charts
```
### Concepts Required

*   Lazy loading with loadChildren
*   Standalone components
*   Angular Signals for state
*   RxJS observables

### Tools/Libraries Required

*   Angular Router

### Testing Steps

1.  Verify routes are lazy‑loaded (network tab).
2.  Test DashboardService methods with mock data.
3.  Test DashboardStateService signals update correctly.

### Expected Deliverables

*   Dashboard routes, service, and state service ready.

### Common Mistakes

**Mistake 1:** Not applying guards to dashboard routes.  
**Fix:** Add `canActivate: [authGuard, roleGuard(['Therapist'])]`.

**Mistake 2:** Not defining all model properties for DashboardData.

### Edge Cases

*   User without Therapist role should not access dashboard.

### Definition of Done

*   Routes, service, and state service are implemented.

### Frontend Architecture Notes

*   DashboardStateService is the single source of truth for dashboard data.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use DashboardService for API and DashboardStateService for state.

## FE-DASH-002Stats Cards (KPIs) P1 Medium 3h ▾

### Task Information

*   **Task ID:** FE-DASH-002
*   **Task Name:** Stats Cards (KPIs)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-DASH-001, FE-SHARED-001
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** High

### Objective

Create a reusable stats card component and a container that displays key performance indicators: total patients, active patients, total sessions, completed sessions, pending exercises, completed exercises, and pending reports.

### Business Purpose

Provide therapists with a quick overview of their practice metrics at a glance.

### Technical Purpose

Build a Standalone StatsCardComponent that displays an icon, label, value, and optional trend indicator. The parent Dashboard component will pass data from DashboardStateService.

### Prerequisites

*   Understanding of Angular components and inputs.
*   Bootstrap card utilities.

### Dependencies

*   **FE-DASH-001:** DashboardStateService for stats data.

### Inputs

*   Stats data from DashboardStateService.

### Outputs

*   `shared/components/stats-card/stats-card.component.ts`
*   `stats-card.component.html`
*   `stats-card.component.scss`

### Detailed Workflow

#### Step 1: Generate Stats Card Component

```bash
ng g c shared/components/stats-card --standalone --skip-tests
```
#### Step 2: Implement StatsCardComponent

```ts
// stats-card.component.ts
import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-stats-card',
    standalone: true,
    templateUrl: './stats-card.component.html',
    styleUrls: ['./stats-card.component.scss'],
})
export class StatsCardComponent {
    @Input() label = '';
    @Input() value: number | string = 0;
    @Input() icon = '';
    @Input() color: 'primary' | 'success' | 'danger' | 'warning' | 'info' = 'primary';
    @Input() trend?: number; // percentage change
    @Input() trendLabel = '';
}
```
#### Step 3: Create Template

```html
<!-- stats-card.component.html -->
<div class="stats-card card" [class.border-primary]="color === 'primary'"
      [class.border-success]="color === 'success'"
      [class.border-danger]="color === 'danger'"
      [class.border-warning]="color === 'warning'"
      [class.border-info]="color === 'info'">
    <div class="card-body">
        <div class="d-flex align-items-center">
            <div class="stats-icon" [class.bg-primary]="color === 'primary'"
                 [class.bg-success]="color === 'success'"
                 [class.bg-danger]="color === 'danger'"
                 [class.bg-warning]="color === 'warning'"
                 [class.bg-info]="color === 'info'">
                <i class="bi {{ icon }}"></i>
            </div>
            <div class="stats-content ms-3">
                <h6 class="stats-label text-muted">{{ label }}</h6>
                <h3 class="stats-value">{{ value }}</h3>
                <div *ngIf="trend !== undefined" class="stats-trend">
                    <span class="badge" [class.bg-success]="trend >= 0" [class.bg-danger]="trend < 0">
                        <i class="bi bi-arrow-{{ trend >= 0 ? 'up' : 'down' }}"></i>
                        {{ trend >= 0 ? '+' : '' }}{{ trend }}%
                    </span>
                    <span class="trend-label ms-1 text-muted">{{ trendLabel }}</span>
                </div>
            </div>
        </div>
    </div>
</div>

```
#### Step 4: Add Styles

```css
// stats-card.component.scss
.stats-card {
    border-radius: 8px;
    border-left: 4px solid transparent;
    transition: transform 0.2s, box-shadow 0.2s;
    cursor: default;
}
.stats-card:hover {
    transform: translateY(-2px);
    box-shadow: $shadow-md;
}
.stats-icon {
    width: 48px;
    height: 48px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #fff;
    font-size: 1.25rem;
    flex-shrink: 0;
}
.stats-label {
    font-size: 0.8rem;
    text-transform: uppercase;
    letter-spacing: 0.03em;
    margin-bottom: 2px;
}
.stats-value {
    font-size: 1.75rem;
    font-weight: 700;
    margin-bottom: 2px;
}
.stats-trend {
    display: flex;
    align-items: center;
    gap: 0.25rem;
    font-size: 0.8rem;
}
.trend-label {
    font-size: 0.75rem;
}
```
#### Step 5: Use Stats Cards in Dashboard Component

In the Dashboard component template, use `<app-stats-card>` for each KPI.

```html
<!-- dashboard.component.html (excerpt) -->
<div class="row g-3 mb-4">
    <div class="col-lg-3 col-md-6">
        <app-stats-card
            label="Total Patients"
            [value]="stats()?.totalPatients || 0"
            icon="bi-people"
            color="primary"
        ></app-stats-card>
    </div>
    <div class="col-lg-3 col-md-6">
        <app-stats-card
            label="Active Patients"
            [value]="stats()?.activePatients || 0"
            icon="bi-person-check"
            color="success"
        ></app-stats-card>
    </div>
    <div class="col-lg-3 col-md-6">
        <app-stats-card
            label="Total Sessions"
            [value]="stats()?.totalSessions || 0"
            icon="bi-calendar"
            color="info"
        ></app-stats-card>
    </div>
    <div class="col-lg-3 col-md-6">
        <app-stats-card
            label="Sessions Completed"
            [value]="stats()?.completedSessions || 0"
            icon="bi-check-circle"
            color="success"
        ></app-stats-card>
    </div>
</div>
<div class="row g-3 mb-4">
    <div class="col-lg-3 col-md-6">
        <app-stats-card
            label="Pending Exercises"
            [value]="stats()?.pendingExercises || 0"
            icon="bi-clipboard"
            color="warning"
        ></app-stats-card>
    </div>
    <div class="col-lg-3 col-md-6">
        <app-stats-card
            label="Completed Exercises"
            [value]="stats()?.completedExercises || 0"
            icon="bi-check2-all"
            color="success"
        ></app-stats-card>
    </div>
    <div class="col-lg-3 col-md-6">
        <app-stats-card
            label="Pending Reports"
            [value]="stats()?.pendingReports || 0"
            icon="bi-file-text"
            color="danger"
        ></app-stats-card>
    </div>
    <div class="col-lg-3 col-md-6">
        <app-stats-card
            label="Total Assessments"
            [value]="stats()?.totalAssessments || 0"
            icon="bi-clipboard-data"
            color="info"
        ></app-stats-card>
    </div>
</div>
```
### Files To Create

*   `shared/components/stats-card/stats-card.component.ts`
*   `stats-card.component.html`
*   `stats-card.component.scss`
*   `shared/components/stats-card/index.ts`

### CLI Commands

```bash
ng g c shared/components/stats-card --standalone --skip-tests
```
### Concepts Required

*   Component inputs
*   Bootstrap grid and cards
*   Bootstrap icons

### Testing Steps

1.  Test card renders with label, value, icon.
2.  Test trend display when provided.
3.  Test different colour variants.

### Expected Deliverables

*   Stats card component ready.

### Common Mistakes

**Mistake 1:** Not using Bootstrap icons correctly (needs import).  
**Fix:** Ensure `bootstrap-icons` is installed and imported in `styles.scss`.

### Edge Cases

*   Value is 0 → show 0, not empty.
*   Trend is undefined → hide trend section.

### Definition of Done

*   Stats card component is complete and integrated.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Stats cards are reusable; add new KPIs as needed.

## FE-DASH-003Chart Components (Line, Bar) P1 High 5h ▾

### Task Information

*   **Task ID:** FE-DASH-003
*   **Task Name:** Chart Components (Line, Bar)
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-DASH-001, ng2-charts, chart.js
*   **Complexity:** High
*   **Estimated Effort:** 5 hours
*   **Priority:** High

### Objective

Create reusable line chart and bar chart components using ng2-charts (Chart.js). These components will display assessment trends, exercise completion, and session statistics.

### Business Purpose

Visualise trends and patterns in patient data to help therapists make data‑driven decisions.

### Technical Purpose

Use Chart.js via ng2-charts to render interactive charts. Create wrapper components that accept data and configuration inputs.

### Prerequisites

*   Install `chart.js` and `ng2-charts`.
*   Understanding of Chart.js configuration.

### Dependencies

*   **FE-DASH-001:** DashboardStateService for trend data.

### Inputs

*   Trend data from DashboardStateService.

### Outputs

*   `shared/components/line-chart/line-chart.component.ts`
*   `shared/components/bar-chart/bar-chart.component.ts`
*   Corresponding HTML and SCSS files.

### Detailed Workflow

#### Step 1: Install Chart.js and ng2-charts

```bash
npm install chart.js ng2-charts
```
#### Step 2: Import Chart.js in styles or angular.json

Add Chart.js CSS if needed (Chart.js v4 doesn't require CSS).

#### Step 3: Generate Line Chart Component

```bash
ng g c shared/components/line-chart --standalone --skip-tests
```
#### Step 4: Implement LineChartComponent

```ts
// line-chart.component.ts
import { Component, Input, OnChanges, ViewChild, ElementRef } from '@angular/core';
import { Chart, ChartConfiguration, ChartData, ChartDataset, ChartOptions, registerables } from 'chart.js';

@Component({
    selector: 'app-line-chart',
    standalone: true,
    templateUrl: './line-chart.component.html',
    styleUrls: ['./line-chart.component.scss'],
})
export class LineChartComponent implements OnChanges {
    @Input() title = '';
    @Input() labels: string[] = [];
    @Input() datasets: ChartDataset[] = [];
    @Input() loading = false;
    @Input() height = '250px';

    @ViewChild('chartCanvas') chartCanvas!: ElementRef<HTMLCanvasElement>;
    private chart: Chart | null = null;

    ngOnChanges(): void {
        if (this.datasets.length > 0 && this.labels.length > 0) {
            this.renderChart();
        }
    }

    private renderChart() {
        if (this.chart) {
            this.chart.destroy();
        }

        Chart.register(...registerables);

        const config: ChartConfiguration = {
            type: 'line',
            data: {
                labels: this.labels,
                datasets: this.datasets.map(ds => ({
                    ...ds,
                    tension: 0.3,
                    pointRadius: 4,
                    pointHoverRadius: 6,
                })),
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: this.datasets.length > 1,
                        position: 'top',
                    },
                    tooltip: {
                        mode: 'index',
                        intersect: false,
                    },
                },
                scales: {
                    y: {
                        beginAtZero: true,
                    },
                },
            },
        };

        this.chart = new Chart(this.chartCanvas.nativeElement, config);
    }
}
```
#### Step 5: Create Line Chart Template

```html
<!-- line-chart.component.html -->
<div class="line-chart">
    <div class="chart-header" *ngIf="title">
        <h6 class="chart-title">{{ title }}</h6>
    </div>
    <div class="chart-container" [style.height]="height">
        <div *ngIf="loading" class="chart-loading">
            <app-spinner size="md"></app-spinner>
        </div>
        <canvas #chartCanvas *ngIf="!loading && labels.length > 0"></canvas>
        <div *ngIf="!loading && labels.length === 0" class="chart-empty">
            No data available
        </div>
    </div>
</div>
```
#### Step 6: Add Styles

```css
// line-chart.component.scss
.line-chart {
    background: #fff;
    border-radius: 8px;
    padding: 1rem;
    border: 1px solid $border-color;
}
.chart-title {
    font-size: 0.9rem;
    font-weight: 600;
    margin-bottom: 0.75rem;
    color: $text-secondary;
}
.chart-container {
    position: relative;
    width: 100%;
}
.chart-loading {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 100%;
    min-height: 150px;
}
.chart-empty {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 100%;
    min-height: 150px;
    color: $text-muted;
}
```
#### Step 7: Generate Bar Chart Component

Repeat the same process for bar chart, changing `type: 'bar'` and configuring accordingly.

```bash
ng g c shared/components/bar-chart --standalone --skip-tests
```
#### Step 8: Implement Bar Chart with Similar Pattern

```ts
// bar-chart.component.ts (excerpt – similar to line-chart)
const config: ChartConfiguration = {
    type: 'bar',
    data: {
        labels: this.labels,
        datasets: this.datasets.map(ds => ({
            ...ds,
            borderRadius: 4,
        })),
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                display: this.datasets.length > 1,
                position: 'top',
            },
        },
        scales: {
            y: {
                beginAtZero: true,
            },
        },
    },
};
```
#### Step 9: Export Components in Shared Barrel

Add to `shared/components/index.ts`.

```ts
export * from './stats-card';
export * from './line-chart';
export * from './bar-chart';
```
### Files To Create

*   `shared/components/line-chart/line-chart.component.ts`
*   `line-chart.component.html`
*   `line-chart.component.scss`
*   `shared/components/bar-chart/bar-chart.component.ts`
*   `bar-chart.component.html`
*   `bar-chart.component.scss`
*   Index exports.

### CLI Commands

```bash
npm install chart.js ng2-charts
ng g c shared/components/line-chart --standalone --skip-tests
ng g c shared/components/bar-chart --standalone --skip-tests
```
### Concepts Required

*   Chart.js configuration
*   ng2-charts integration
*   Canvas API
*   Change detection with OnChanges

### Tools/Libraries Required

*   chart.js
*   ng2-charts

### Testing Steps

1.  Test line chart with sample data.
2.  Test bar chart with sample data.
3.  Test loading state.
4.  Test empty state.

### Expected Deliverables

*   Line and bar chart components ready.

### Common Mistakes

**Mistake 1:** Not destroying chart on component destroy.  
**Fix:** Implement `ngOnDestroy` and call `this.chart.destroy()`.

**Mistake 2:** Registering Chart.js components multiple times.  
**Fix:** Register once in a central location or use `Chart.register(...registerables)` in each component.

### Edge Cases

*   Empty data → show empty state.
*   Single data point → chart should still render.
*   Large datasets → performance should be acceptable.

### Definition of Done

*   Chart components work and are reusable.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Use `<app-line-chart>` and `<app-bar-chart>` for all dashboard charts.

## FE-DASH-004Dashboard Main Component P1 High 4h ▾

### Task Information

*   **Task ID:** FE-DASH-004
*   **Task Name:** Dashboard Main Component
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** FE-DASH-001, FE-DASH-002, FE-DASH-003
*   **Complexity:** High
*   **Estimated Effort:** 4 hours
*   **Priority:** High

### Objective

Build the main dashboard page that combines stats cards, line charts for assessment trends and session trends, and a bar chart for exercise completion. This is the landing page for therapists.

### Business Purpose

Provide a comprehensive overview of all key metrics in one place.

### Technical Purpose

Use DashboardService to fetch data, DashboardStateService for state, and integrate the StatsCard, LineChart, and BarChart components.

### Prerequisites

*   All dashboard components ready.

### Dependencies

*   **FE-DASH-001:** Service and state.
*   **FE-DASH-002:** StatsCardComponent.
*   **FE-DASH-003:** LineChartComponent, BarChartComponent.

### Inputs

*   None.

### Outputs

*   `features/dashboard/pages/dashboard/dashboard.component.ts`
*   `dashboard.component.html`
*   `dashboard.component.scss`

### Detailed Workflow

#### Step 1: Implement Dashboard Component Logic

```ts
// dashboard.component.ts
import { Component, inject, OnInit, effect } from '@angular/core';
import { DashboardService } from '../../../../core/services/dashboard.service';
import { DashboardStateService } from '../../../../core/state/dashboard-state.service';
import { StatsCardComponent } from '../../../../shared/components/stats-card/stats-card.component';
import { LineChartComponent } from '../../../../shared/components/line-chart/line-chart.component';
import { BarChartComponent } from '../../../../shared/components/bar-chart/bar-chart.component';
import { SpinnerComponent } from '../../../../shared/components/spinner/spinner.component';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [StatsCardComponent, LineChartComponent, BarChartComponent, SpinnerComponent],
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
    private dashboardService = inject(DashboardService);
    private state = inject(DashboardStateService);

    stats = this.state.stats;
    assessmentTrends = this.state.assessmentTrends;
    exerciseCompletion = this.state.exerciseCompletion;
    sessionTrends = this.state.sessionTrends;
    loading = this.state.loading;
    error = this.state.error;

    ngOnInit(): void {
        this.loadDashboardData();
    }

    loadDashboardData() {
        this.state.setLoading(true);
        this.dashboardService.getDashboardData().subscribe({
            next: (data) => {
                this.state.setDashboardData(data);
                this.state.setLoading(false);
            },
            error: (err) => {
                this.state.setError(err.message || 'Failed to load dashboard data');
                this.state.setLoading(false);
            },
        });
    }

    // Helper to convert trend data to Chart.js format
    getChartData(trends: any[], label: string, color: string) {
        if (!trends || trends.length === 0) return { labels: [], datasets: [] };
        return {
            labels: trends.map(t => t.date),
            datasets: [{
                label: label,
                data: trends.map(t => t.value),
                borderColor: color,
                backgroundColor: color + '33',
                fill: true,
            }],
        };
    }
}
```
#### Step 2: Create Template

```html
<!-- dashboard.component.html -->
<div class="dashboard">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2>Dashboard</h2>
        <button class="btn btn-outline-secondary btn-sm" (click)="loadDashboardData()">
            <i class="bi bi-arrow-clockwise"></i> Refresh
        </button>
    </div>

    <div *ngIf="loading()" class="text-center py-5">
        <app-spinner size="lg"></app-spinner>
    </div>

    <div *ngIf="error()" class="alert alert-danger">{{ error() }}</div>

    <div *ngIf="!loading() && !error()">
        <!-- Stats Cards Row 1 -->
        <div class="row g-3 mb-4">
            <div class="col-lg-3 col-md-6">
                <app-stats-card
                    label="Total Patients"
                    [value]="stats()?.totalPatients || 0"
                    icon="bi-people"
                    color="primary"
                ></app-stats-card>
            </div>
            <div class="col-lg-3 col-md-6">
                <app-stats-card
                    label="Active Patients"
                    [value]="stats()?.activePatients || 0"
                    icon="bi-person-check"
                    color="success"
                ></app-stats-card>
            </div>
            <div class="col-lg-3 col-md-6">
                <app-stats-card
                    label="Total Sessions"
                    [value]="stats()?.totalSessions || 0"
                    icon="bi-calendar"
                    color="info"
                ></app-stats-card>
            </div>
            <div class="col-lg-3 col-md-6">
                <app-stats-card
                    label="Sessions Completed"
                    [value]="stats()?.completedSessions || 0"
                    icon="bi-check-circle"
                    color="success"
                ></app-stats-card>
            </div>
        </div>

        <!-- Stats Cards Row 2 -->
        <div class="row g-3 mb-4">
            <div class="col-lg-3 col-md-6">
                <app-stats-card
                    label="Pending Exercises"
                    [value]="stats()?.pendingExercises || 0"
                    icon="bi-clipboard"
                    color="warning"
                ></app-stats-card>
            </div>
            <div class="col-lg-3 col-md-6">
                <app-stats-card
                    label="Completed Exercises"
                    [value]="stats()?.completedExercises || 0"
                    icon="bi-check2-all"
                    color="success"
                ></app-stats-card>
            </div>
            <div class="col-lg-3 col-md-6">
                <app-stats-card
                    label="Pending Reports"
                    [value]="stats()?.pendingReports || 0"
                    icon="bi-file-text"
                    color="danger"
                ></app-stats-card>
            </div>
            <div class="col-lg-3 col-md-6">
                <app-stats-card
                    label="Total Assessments"
                    [value]="stats()?.totalAssessments || 0"
                    icon="bi-clipboard-data"
                    color="info"
                ></app-stats-card>
            </div>
        </div>

        <!-- Charts -->
        <div class="row g-3">
            <div class="col-lg-6">
                <app-line-chart
                    title="Assessment Trends"
                    [labels]="assessmentTrends().length > 0 ? assessmentTrends()[0]?.data?.map(t => t.date) || [] : []"
                    [datasets]="assessmentTrends().map(t => ({
                        label: t.label,
                        data: t.data.map(d => d.value),
                        borderColor: t.color || '#3b82f6',
                        backgroundColor: (t.color || '#3b82f6') + '33',
                        fill: true,
                    }))"
                    [loading]="loading()"
                ></app-line-chart>
            </div>
            <div class="col-lg-6">
                <app-line-chart
                    title="Session Trends"
                    [labels]="sessionTrends().length > 0 ? sessionTrends()[0]?.data?.map(t => t.date) || [] : []"
                    [datasets]="sessionTrends().map(t => ({
                        label: t.label,
                        data: t.data.map(d => d.value),
                        borderColor: t.color || '#22c55e',
                        backgroundColor: (t.color || '#22c55e') + '33',
                        fill: true,
                    }))"
                    [loading]="loading()"
                ></app-line-chart>
            </div>
            <div class="col-12">
                <app-bar-chart
                    title="Exercise Completion"
                    [labels]="exerciseCompletion().length > 0 ? exerciseCompletion()[0]?.data?.map(t => t.label || t.date) || [] : []"
                    [datasets]="exerciseCompletion().map(t => ({
                        label: t.label,
                        data: t.data.map(d => d.value),
                        backgroundColor: t.color || '#f59e0b',
                    }))"
                    [loading]="loading()"
                ></app-bar-chart>
            </div>
        </div>
    </div>
</div>
```
#### Step 3: Add Styles

```css
// dashboard.component.scss
.dashboard {
    padding: 0.5rem 0;
}
.btn-refresh {
    font-size: 0.85rem;
}
```
### Files To Create

*   `dashboard.component.ts`
*   `dashboard.component.html`
*   `dashboard.component.scss`

### CLI Commands

```bash
\# Already generated; now modify
```
### Concepts Required

*   Component composition
*   Signal reactivity
*   Data transformation for charts

### Testing Steps

1.  Verify dashboard loads data.
2.  Verify stats cards display correct values.
3.  Verify charts render with data.
4.  Test refresh button.

### Expected Deliverables

*   Dashboard component complete.

### Common Mistakes

**Mistake 1:** Not handling empty data for charts.  
**Fix:** Provide empty arrays and show empty state.

### Edge Cases

*   API returns empty trends → charts show empty state.
*   Stats are all 0 → cards show 0.

### Definition of Done

*   Dashboard displays all stats and charts.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Dashboard is the landing page after login.

## FE-DASH-005State Management and Integration P1 Medium 3h ▾

### Task Information

*   **Task ID:** FE-DASH-005
*   **Task Name:** State Management and Integration
*   **Owner:** M3 (Frontend Lead)
*   **Dependencies:** All previous dashboard tasks
*   **Complexity:** Medium
*   **Estimated Effort:** 3 hours
*   **Priority:** High

### Objective

Ensure the dashboard components use the DashboardStateService consistently, add caching and error handling, and integrate with the navigation system (redirect to dashboard after login).

### Business Purpose

Unified state management reduces bugs and ensures data consistency across the dashboard.

### Technical Purpose

Refine the dashboard implementation, add auto‑refresh capabilities, and ensure proper cleanup.

### Prerequisites

*   All dashboard components implemented.

### Dependencies

*   All FE-DASH tasks.

### Inputs

*   Existing components.

### Outputs

*   Updated components using state service consistently.
*   Dashboard integrated with auth redirect.
*   Auto‑refresh or periodic refresh (optional).

### Detailed Workflow

#### Step 1: Ensure State Service Consistency

Verify that DashboardComponent uses `DashboardStateService` for all data and loading states.

#### Step 2: Add Auto‑Refresh Capability (Optional)

Implement a periodic refresh (e.g., every 5 minutes) to keep data up‑to‑date.

```ts
// In dashboard.component.ts
private refreshInterval: any;

ngOnInit(): void {
    this.loadDashboardData();
    this.refreshInterval = setInterval(() => {
        this.loadDashboardData();
    }, 300000); // 5 minutes
}

ngOnDestroy(): void {
    if (this.refreshInterval) {
        clearInterval(this.refreshInterval);
    }
}
```
#### Step 3: Handle Auth Redirect

Ensure that after login, the user is redirected to `/dashboard`.

```ts
// In login.component.ts
this.router.navigate(['/dashboard']);
```
#### Step 4: Add Error Handling for Empty State

If API returns empty data, show appropriate messages.

#### Step 5: Add Unit Tests for State Service

Test that state updates correctly.

### Files To Modify

*   `dashboard.component.ts` (add interval and ngOnDestroy).
*   `login.component.ts` (redirect to dashboard).

### CLI Commands

No new commands.

### Concepts Required

*   State management best practices
*   Lifecycle hooks (ngOnDestroy)
*   Auto‑refresh patterns

### Testing Steps

1.  Verify dashboard loads on login.
2.  Verify state persists across navigation.
3.  Test auto‑refresh (if implemented).

### Expected Deliverables

*   Dashboard fully integrated.

### Common Mistakes

**Mistake 1:** Not clearing interval on destroy, causing memory leaks.

### Edge Cases

*   User navigates away and back → state should still be available (or reload).

### Definition of Done

*   All components use state service and dashboard is fully integrated.

### Team Handoff Notes

*   **To:** All developers.
*   **Key Takeaways:** Dashboard is the default landing page.

* * *

✅ Phase Completion Criteria
---------------------------

*   All 5 dashboard tasks are complete (FE-DASH-001 to FE-DASH-005).
*   Dashboard routes are lazy‑loaded and guarded.
*   DashboardService and DashboardStateService are implemented.
*   Stats cards display all key metrics.
*   Line chart shows assessment trends.
*   Line chart shows session trends.
*   Bar chart shows exercise completion.
*   Dashboard loads data on init.
*   Refresh button reloads data.
*   All components use the state service.
*   All tests pass.
*   All changes are committed.

📋 Code Review Checklist
------------------------

*   DashboardService methods are typed and handle errors.
*   State service uses Signals correctly.
*   Stats cards display correct values.
*   Chart components render with data.
*   Auto‑refresh (if implemented) has cleanup.
*   Redirect after login works.

🚀 Pull Request Checklist
-------------------------

*   Branch up‑to‑date with main.
*   All tests pass.
*   Code review completed.
*   Documentation updated.

🧪 Deployment Readiness Checklist
---------------------------------

*   Dashboard loads after login.
*   Stats and charts display real data.
*   Refresh updates data.
*   Dashboard is responsive.

* * *

Jalsa – Phase 10: Dashboard & Analytics – Expanded Implementation Handbook • v1.0 • For development team

