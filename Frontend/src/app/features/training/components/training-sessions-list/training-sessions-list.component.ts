import { Component, computed, effect, inject, signal } from '@angular/core';
import { TrainingSessionResponseDto } from '../../../../shared/models/training-session-response-dto';
import { TrainingService } from '../../services/training.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-training-sessions-list',
  imports: [CommonModule],
  templateUrl: './training-sessions-list.component.html',
  styleUrl: './training-sessions-list.component.css',
})
export class TrainingSessionsListComponent {
  private svc = inject(TrainingService);

  sessions = signal<TrainingSessionResponseDto[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  deletingSessionIds = signal<string[]>([]);

  sortedSessions = computed(() => {
    return [...this.sessions()].sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
  });

  constructor() {
    effect(() => {
      if (this.loading()) {
        this.loadAll()
      }
    });
  }
  private loadAll() {
    this.loading.set(true);
    this.error.set(null);
    this.svc.getAll().subscribe({
      next: data => {
        this.sessions.set(data);
        this.loading.set(false);
      },
      error: error => {
        this.error.set(error.message);
        this.loading.set(false);
      }
    })
  }
  formatDate(iso: string): string {
    const date = new Date(iso);
    return date.toLocaleString(undefined, {
      weekday: 'short',
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
  formatDuration(minutes: number): string {
    if (minutes < 60) return `${minutes} min`;
    const h = Math.floor(minutes / 60);
    const m = minutes % 60;
    return `${h}h ${m}m`;
  }
  levelToPercent(level: number): number {
    return Math.max(1, Math.min(100, Math.round((level / 10) * 100)));
  }

  isDeleting(id: string): boolean {
    return this.deletingSessionIds().includes(id);
  }

  deleteSession(session: TrainingSessionResponseDto): void {
    const id = session.id; 
    const confirmed = window.confirm(`Delete session "${session.trainingType}" on ${this.formatDate(session.date)}?`);

    if (!confirmed) return; 
    const previous = this.sessions();
    this.sessions.set(previous.filter(s => s.id !== id));
    this.deletingSessionIds.set([...this.deletingSessionIds(), id]);
    this.svc.delete(id).subscribe({
      next: () => {
        this.deletingSessionIds.set(this.deletingSessionIds().filter(x => x !== id));
      }
    });

  }
}
