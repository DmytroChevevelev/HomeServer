import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { DeviceListItemViewModel, UiSurfaceState } from '../models/device-pages.models';
import { DevicePagesFacade } from '../services/device-pages.facade';
import { DeviceListComponent } from './device-list.component';

function makeDevice(overrides: Partial<DeviceListItemViewModel> = {}): DeviceListItemViewModel {
  return {
    deviceId: 'd-1',
    externalId: 'EXT-1',
    name: 'Test Sensor',
    deviceType: 'temperature',
    status: 'online',
    statusColor: '#16a34a',
    registeredAtUtc: '2026-05-23T10:00:00Z',
    isEnabled: true,
    latestValue: null,
    latestValueDisplay: 'No value',
    latestEventTimeUtc: null,
    ...overrides
  };
}

function makeFacade(state: UiSurfaceState, items: DeviceListItemViewModel[] = []): Partial<DevicePagesFacade> {
  return {
    getDeviceList: jasmine.createSpy('getDeviceList').and.resolveTo({ state, items })
  };
}

describe('DeviceListComponent', () => {
  let fixture: ComponentFixture<DeviceListComponent>;
  let component: DeviceListComponent;

  async function setup(state: UiSurfaceState, items: DeviceListItemViewModel[] = []): Promise<void> {
    const facade = makeFacade(state, items);
    await TestBed.configureTestingModule({
      imports: [DeviceListComponent],
      providers: [
        provideRouter([]),
        { provide: DevicePagesFacade, useValue: facade }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DeviceListComponent);
    component = fixture.componentInstance;
    (component as unknown as { facade: Partial<DevicePagesFacade> })['facade'] = facade;
    await component.ngOnInit();
    fixture.detectChanges();
  }

  it('shows spinner when state is loading', async () => {
    await setup({ status: 'loading', errorMessage: null });
    const spinner = fixture.nativeElement.querySelector('[class*="spinner"]');
    expect(spinner).toBeTruthy();
  });

  it('shows alert-danger when state is error', async () => {
    await setup({ status: 'error', errorMessage: 'Backend unavailable.' });
    const alert = fixture.nativeElement.querySelector('.alert-danger');
    expect(alert).toBeTruthy();
    expect(alert.textContent).toContain('Backend unavailable.');
  });

  it('shows alert-info when state is empty', async () => {
    await setup({ status: 'empty', errorMessage: null });
    const alert = fixture.nativeElement.querySelector('.alert-info');
    expect(alert).toBeTruthy();
  });

  it('renders a table row for each device when state is ready', async () => {
    const items = [makeDevice({ deviceId: 'd-1' }), makeDevice({ deviceId: 'd-2' }), makeDevice({ deviceId: 'd-3' })];
    await setup({ status: 'ready', errorMessage: null }, items);
    const rows = fixture.nativeElement.querySelectorAll('tbody tr');
    expect(rows.length).toBe(3);
  });

  it('filters to only offline rows when selectedStatus is offline', async () => {
    const items = [
      makeDevice({ deviceId: 'd-1', status: 'online' }),
      makeDevice({ deviceId: 'd-2', status: 'offline' }),
      makeDevice({ deviceId: 'd-3', status: 'online' })
    ];
    await setup({ status: 'ready', errorMessage: null }, items);
    component.setFilter('offline');
    fixture.detectChanges();
    const rows = fixture.nativeElement.querySelectorAll('tbody tr');
    expect(rows.length).toBe(1);
  });

  it('shows all rows when selectedStatus is reset to all', async () => {
    const items = [
      makeDevice({ deviceId: 'd-1', status: 'online' }),
      makeDevice({ deviceId: 'd-2', status: 'offline' })
    ];
    await setup({ status: 'ready', errorMessage: null }, items);
    component.setFilter('offline');
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelectorAll('tbody tr').length).toBe(1);

    component.setFilter('all');
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelectorAll('tbody tr').length).toBe(2);
  });

  it('renders latest telemetry value in visible row', async () => {
    await setup(
      { status: 'ready', errorMessage: null },
      [makeDevice({ latestValue: 21.5, latestValueDisplay: '21.5' })]
    );

    const firstRowText = fixture.nativeElement.querySelector('tbody tr')?.textContent ?? '';
    expect(firstRowText).toContain('21.5');
  });

  it('shows metadata row when expanded and hides when toggled again', async () => {
    await setup({ status: 'ready', errorMessage: null }, [makeDevice()]);

    let rows = fixture.nativeElement.querySelectorAll('tbody tr');
    expect(rows.length).toBe(1);

    const button: HTMLButtonElement = fixture.nativeElement.querySelector('tbody tr button');
    button.click();
    fixture.detectChanges();

    rows = fixture.nativeElement.querySelectorAll('tbody tr');
    expect(rows.length).toBe(2);
    expect(fixture.nativeElement.textContent).toContain('Device ID:');

    button.click();
    fixture.detectChanges();

    rows = fixture.nativeElement.querySelectorAll('tbody tr');
    expect(rows.length).toBe(1);
  });

  it('shows realtime warning while disconnected and clears on reconnect', async () => {
    const facade = makeFacade({ status: 'ready', errorMessage: null }, [makeDevice()]);

    const realtimeMock = {
      onSensorValueChanged: jasmine.createSpy('onSensorValueChanged').and.returnValue(() => undefined),
      onConnectionStateChanged: jasmine
        .createSpy('onConnectionStateChanged')
        .and.callFake((handler: (state: 'disconnected' | 'connecting' | 'connected' | 'reconnecting') => void) => {
          handler('disconnected');
          return () => undefined;
        }),
      start: jasmine.createSpy('start').and.resolveTo(undefined),
      stop: jasmine.createSpy('stop').and.resolveTo(undefined)
    };

    await TestBed.configureTestingModule({
      imports: [DeviceListComponent],
      providers: [
        provideRouter([]),
        { provide: DevicePagesFacade, useValue: facade }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DeviceListComponent);
    component = fixture.componentInstance;
    (component as unknown as { facade: Partial<DevicePagesFacade> })['facade'] = facade;
    (component as unknown as { realtime: typeof realtimeMock })['realtime'] = realtimeMock;

    await component.ngOnInit();
    fixture.detectChanges();

    expect(component.realtimeWarning).toContain('Realtime connection is unavailable');

    const reconnectHandler = realtimeMock.onConnectionStateChanged.calls.mostRecent().args[0] as (
      state: 'disconnected' | 'connecting' | 'connected' | 'reconnecting'
    ) => void;
    reconnectHandler('connected');
    fixture.detectChanges();

    expect(component.realtimeWarning).toBeNull();
  });
});
