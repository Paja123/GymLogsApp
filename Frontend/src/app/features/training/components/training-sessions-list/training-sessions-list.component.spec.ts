import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingSessionsListComponent } from './training-sessions-list.component';

describe('TrainingSessionsListComponent', () => {
  let component: TrainingSessionsListComponent;
  let fixture: ComponentFixture<TrainingSessionsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrainingSessionsListComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(TrainingSessionsListComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
