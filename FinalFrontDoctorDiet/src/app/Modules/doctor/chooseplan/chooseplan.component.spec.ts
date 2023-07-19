import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseplanComponent } from './chooseplan.component';

describe('ChooseplanComponent', () => {
  let component: ChooseplanComponent;
  let fixture: ComponentFixture<ChooseplanComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChooseplanComponent]
    });
    fixture = TestBed.createComponent(ChooseplanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
