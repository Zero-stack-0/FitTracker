import { TestBed } from '@angular/core/testing';

import { FoodMacrosService } from './food-macros.service';

describe('FoodMacrosService', () => {
  let service: FoodMacrosService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FoodMacrosService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
