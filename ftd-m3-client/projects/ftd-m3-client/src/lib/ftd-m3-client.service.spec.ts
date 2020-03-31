import { TestBed } from '@angular/core/testing';

import { FtdM3ClientService } from './ftd-m3-client.service';

describe('FtdM3ClientService', () => {
  let service: FtdM3ClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FtdM3ClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
