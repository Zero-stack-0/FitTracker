import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FitnessPlanComponent } from './fitness-plan.component';

describe('FitnessPlanComponent', () => {
  let component: FitnessPlanComponent;
  let fixture: ComponentFixture<FitnessPlanComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FitnessPlanComponent]
    });
    fixture = TestBed.createComponent(FitnessPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
