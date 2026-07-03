import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
    selector: 'app-ai-disclaimer',
    standalone: true,
    templateUrl: './ai-disclaimer.component.html',
    styleUrl: './ai-disclaimer.component.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AiDisclaimerComponent {
    blocked = input(false);
    message = input('');
}
