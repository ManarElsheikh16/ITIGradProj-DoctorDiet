import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscrebtionComponent } from './subscrebtion.component';

describe('SubscrebtionComponent', () => {
  let component: SubscrebtionComponent;
  let fixture: ComponentFixture<SubscrebtionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubscrebtionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubscrebtionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
