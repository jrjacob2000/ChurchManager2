import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountChartModalComponent } from './account-chart-modal.component';

describe('AccountChartModalComponent', () => {
  let component: AccountChartModalComponent;
  let fixture: ComponentFixture<AccountChartModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountChartModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountChartModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
