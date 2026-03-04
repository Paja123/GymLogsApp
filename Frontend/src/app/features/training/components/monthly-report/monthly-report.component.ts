import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, effect, inject, signal, computed } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TrainingService } from '../../services/training.service';
import { WeeklyReportDto } from '../../../../shared/models/weekly-report.model';

@Component({
  selector: 'app-monthly-report',
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './monthly-report.component.html',
  styleUrl: './monthly-report.component.css',
})
export class MonthlyReportComponent {
  private svc = inject(TrainingService);

  readonly months = [ 
    { value: 1, label: 'January' },
    { value: 2, label: 'February'}, 
    { value: 3, label: 'March'},
    { value: 4, label: 'April'}, 
    { value: 5, label: 'May'},
    { value: 6, label: 'June'}, 
    { value: 7, label: 'July'},
    { value: 8, label: 'August'},
    { value: 9, label: 'September'},
    { value: 10, label: 'October'},
    { value: 11, label: 'November'},
    { value: 12, label: 'December'}];

  public now = new Date();
  readonly currentyear = this.now.getFullYear();
  readonly years = Array.from({ length: 6 }, (_, i) => this.currentyear - i).reverse();

  selectedMonth = signal<number>(this.now.getMonth() + 1);
  selectedYear = signal<number>(this.currentyear);
  loading = signal(false);
  error = signal<string | null>(null);
  report = signal<WeeklyReportDto[]>([]);

  onMonthChange(value: number){
    this.error.set(null);
    this.selectedMonth.set(Number(value));
  }
  
  onYearChange(value: number){
    this.error.set(null);
    this.selectedYear.set(Number(value));
  }



  isFuture = computed(() => {
    const sel = new Date(this.selectedYear(), this.selectedMonth() - 1, 1);
    return sel.getFullYear() > this.now.getFullYear() ||
      (sel.getFullYear() === this.now.getFullYear() && sel.getMonth() > this.now.getMonth());
  });

  totals = computed(() => {
    const data = this.report();
    const totalDuration = data.reduce((sum, week) => sum + (week.totalDuration ?? 0), 0);
    const totalTrainingSessionsCount = data.reduce((sum, week) => sum + (week.trainingSessionsCount ?? 0), 0);
    
    const avgIntesity = totalTrainingSessionsCount
    ? +(data.reduce((sum, week) => sum + (week.averageIntensity ?? 0) * (week.trainingSessionsCount ?? 0), 0) / totalTrainingSessionsCount).toFixed(2): 0;

  const avgTiredness = totalTrainingSessionsCount
    ? +(data.reduce((sum, week) => sum + (week.averageTiredness ?? 0) * (week.trainingSessionsCount ?? 0), 0) / totalTrainingSessionsCount).toFixed(2): 0;

    return { totalDuration, totalTrainingSessionsCount, avgIntesity, avgTiredness};
  });

  constructor() {
    effect(() =>{
      this.fetchReport();
    });
  }

  fetchReport(): void{
    if (this.isFuture()) {
      return;
  }

  this.loading.set(true);
  this.error.set(null);
  this.report.set([]);

  const month = this.selectedMonth();
  const year = this.selectedYear();

  this.svc.getMonthlyReport(year, month).subscribe({
    next: (data) => {
      //Now we make sure it displays all the 5 weeks, even if some are empty
      const weeksMap = new Map<number, WeeklyReportDto>();
      data.forEach(w => weeksMap.set(w.weekNumber, w));
      const normalized : WeeklyReportDto[] = [];
      for(let i = 1; i<=5; i++){
        const w = weeksMap.get(i);
        normalized.push(w ?? {weekNumber: i, totalDuration: 0, trainingSessionsCount: 0, averageIntensity: 0, averageTiredness: 0});
      }
      this.report.set(normalized);
      this.loading.set(false);
    },
    error: (err) => {
      this.error.set('Failed to fetch report: ' + (err?.error?.message || err.message || 'Unknown error'));
      this.loading.set(false);
    }
  });
  }

  formatDuration(minutes: number): string {
    if (!minutes) return '0 min'; 
    if (minutes < 60) return `${minutes} min`; 
    const h = Math.floor(minutes / 60); 
    const m = minutes % 60;
    return `${h}h ${m}m`;
  }
  percentFromAvg(value: number) : number{
    return Math.max(1, Math.min(100, Math.round((value / 10) * 100)));
  }

  
}
