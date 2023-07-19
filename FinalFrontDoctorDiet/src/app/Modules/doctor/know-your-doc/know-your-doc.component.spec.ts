import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KnowYourDocComponent } from './know-your-doc.component';

describe('KnowYourDocComponent', () => {
  let component: KnowYourDocComponent;
  let fixture: ComponentFixture<KnowYourDocComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ KnowYourDocComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(KnowYourDocComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
