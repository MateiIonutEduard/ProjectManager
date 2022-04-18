import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignupWindowComponent } from './signup-window.component';

describe('SignupWindowComponent', () => {
  let component: SignupWindowComponent;
  let fixture: ComponentFixture<SignupWindowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SignupWindowComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SignupWindowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
