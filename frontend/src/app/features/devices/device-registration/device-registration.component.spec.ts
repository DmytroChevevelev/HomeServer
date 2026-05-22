import { TestBed } from '@angular/core/testing';
import { DeviceRegistrationComponent } from './device-registration.component';

describe('DeviceRegistrationComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeviceRegistrationComponent]
    }).compileComponents();
  });

  it('creates component', () => {
    const fixture = TestBed.createComponent(DeviceRegistrationComponent);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });
});
