import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FtdM3ClientComponent } from './ftd-m3-client.component';

describe('FtdM3ClientComponent', () => {
  let component: FtdM3ClientComponent;
  let fixture: ComponentFixture<FtdM3ClientComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FtdM3ClientComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FtdM3ClientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
