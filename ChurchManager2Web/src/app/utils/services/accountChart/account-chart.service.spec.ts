import { TestBed } from '@angular/core/testing';

import { AccountChartService } from './account-chart.service';

describe('AccountChartService', () => {
  let service: AccountChartService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccountChartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
