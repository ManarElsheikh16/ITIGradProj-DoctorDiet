import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanDashComponent } from './plan-dash.component';

describe('PlanDashComponent', () => {
  let component: PlanDashComponent;
  let fixture: ComponentFixture<PlanDashComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlanDashComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlanDashComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
