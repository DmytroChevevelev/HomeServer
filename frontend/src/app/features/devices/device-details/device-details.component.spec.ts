import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, provideRouter } from '@angular/router';
import { DevicePagesFacade } from '../services/device-pages.facade';
import { DeviceDetailsComponent } from './device-details.component';

describe('DeviceDetailsComponent', () => {
  beforeEach(async () => {
    const facade: Partial<DevicePagesFacade> = {
      getDeviceDetails: jasmine.createSpy('getDeviceDetails').and.resolveTo({
        state: { status: 'ready', errorMessage: null },
        item: {
          deviceId: 'd-1',
          externalId: 'EXT-1',
          name: 'Living Room Sensor',
          deviceType: 'temperature',
          status: 'online',
          statusColor: '#16a34a',
          lastUpdatedUtc: null
        }
      }),
      getDeviceHistory: jasmine.createSpy('getDeviceHistory').and.resolveTo({
        state: { status: 'ready', errorMessage: null },
        items: []
      }),
      unregisterDevice: jasmine.createSpy('unregisterDevice').and.resolveTo({
        state: { status: 'ready', errorMessage: null }
      })
    };

    await TestBed.configureTestingModule({
      imports: [DeviceDetailsComponent],
      providers: [
        provideRouter([]),
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              paramMap: {
                get: (key: string) => (key === 'deviceId' ? 'd-1' : null)
              }
            }
          }
        },
        { provide: DevicePagesFacade, useValue: facade }
      ]
    }).compileComponents();
  });

  it('creates and loads device details', async () => {
    const fixture = TestBed.createComponent(DeviceDetailsComponent);
    const component = fixture.componentInstance;
    (component as unknown as { facade: Partial<DevicePagesFacade> }).facade = TestBed.inject(
      DevicePagesFacade
    ) as Partial<DevicePagesFacade>;

    await component.ngOnInit();
    fixture.detectChanges();

    expect(component.item?.deviceId).toBe('d-1');
    expect(fixture.nativeElement.textContent).toContain('Living Room Sensor');
  });
});
