import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, provideRouter } from '@angular/router';
import { DevicePagesFacade } from '../services/device-pages.facade';
import { DeviceDetailsComponent } from './device-details.component';

describe('DeviceDetailsComponent', () => {
  let facade: {
    getDeviceDetails: jasmine.Spy;
    getDeviceHistory: jasmine.Spy;
    unregisterDevice: jasmine.Spy;
  };

  beforeEach(async () => {
    facade = {
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
        { provide: DevicePagesFacade, useValue: facade as Partial<DevicePagesFacade> }
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

  it('loads telemetry rows and forwards custom limit to facade', async () => {
    facade.getDeviceHistory.and.resolveTo({
      state: { status: 'ready', errorMessage: null },
      items: [
        {
          timestampUtc: '2026-05-23T10:01:00Z',
          metricType: 'temperature',
          metricValue: 22.4,
          metricValueDisplay: '22.4'
        }
      ]
    });

    const fixture = TestBed.createComponent(DeviceDetailsComponent);
    const component = fixture.componentInstance;
    (component as unknown as { facade: Partial<DevicePagesFacade> }).facade = TestBed.inject(
      DevicePagesFacade
    ) as Partial<DevicePagesFacade>;

    await component.ngOnInit();
    await component.applyTelemetryLimit('250');
    fixture.detectChanges();

    expect(component.telemetryItems.length).toBe(1);
    expect(component.telemetryItems[0].metricValueDisplay).toBe('22.4');
    expect(facade.getDeviceHistory).toHaveBeenCalledWith('d-1', jasmine.any(Object), 250);
  });
});
