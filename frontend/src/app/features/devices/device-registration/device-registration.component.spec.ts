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

  it('handles rejected submit promise and sets error message', async () => {
    const fixture = TestBed.createComponent(DeviceRegistrationComponent);
    const component = fixture.componentInstance;

    spyOn(component, 'submit').and.rejectWith(new Error('network down'));

    await component.submitForm();

    expect(component.submitMessage).toBe('Registration failed. Unable to reach API.');
  });

  it('submits complete payload including enabled flag', async () => {
    const fixture = TestBed.createComponent(DeviceRegistrationComponent);
    const component = fixture.componentInstance;

    component.externalId = 'EXT-200';
    component.name = 'Boiler Sensor';
    component.sensorType = 'temperature';
    component.isEnabled = false;

    const submitSpy = spyOn(component, 'submit').and.resolveTo(new Response(null, { status: 201 }));

    await component.submitForm();

    expect(submitSpy).toHaveBeenCalledWith('EXT-200', 'Boiler Sensor', 'temperature', false);
    expect(component.submitMessage).toBe('Device registered successfully.');
  });
});
