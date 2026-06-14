import { DevicePagesFacade } from './device-pages.facade';
import { DevicesApiService } from './devices-api.service';

describe('DevicePagesFacade', () => {
  it('maps backend latestMetricValue to latestValueDisplay', async () => {
    const fetchMock = jasmine.createSpy('fetchMock').and.resolveTo(
      new Response(
        JSON.stringify([
          {
            deviceId: 'd-1',
            externalId: 'EXT-1',
            name: 'Living Room Sensor',
            sensorType: 'temperature',
            status: 'online',
            latestMetricValue: 42.5
          }
        ]),
        { status: 200 }
      )
    );

    const api = new DevicesApiService(fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');
    const facade = new DevicePagesFacade(api, fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');

    const result = await facade.getDeviceList();

    expect(result.state.status).toBe('ready');
    expect(result.items.length).toBe(1);
    expect(result.items[0].latestValue).toBe(42.5);
    expect(result.items[0].latestValueDisplay).toBe('42.5');
  });

  it('maps device list rows and placeholder values', async () => {
    const fetchMock = jasmine.createSpy('fetchMock').and.resolveTo(
      new Response(
        JSON.stringify([
          {
            id: 'd-1',
            externalId: 'EXT-1',
            name: 'Living Room Sensor',
            sensorType: 'temperature',
            status: 'online',
            latestValue: null
          }
        ]),
        { status: 200 }
      )
    );

    const api = new DevicesApiService(fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');
    const facade = new DevicePagesFacade(api, fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');

    const result = await facade.getDeviceList();

    expect(result.state.status).toBe('ready');
    expect(result.items.length).toBe(1);
    expect(result.items[0].latestValueDisplay).toBe('No value');
  });

  it('returns validation error state for invalid date-time range', async () => {
    const fetchMock = jasmine.createSpy('fetchMock').and.resolveTo(new Response(JSON.stringify([]), { status: 200 }));

    const api = new DevicesApiService(fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');
    const facade = new DevicePagesFacade(api, fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');

    const result = await facade.getDeviceHistory('d-1', {
      fromUtc: '2026-05-03T00:00:00.000Z',
      toUtc: '2026-05-01T00:00:00.000Z'
    });

    expect(result.state.status).toBe('error');
    expect(result.state.errorMessage).toContain('earlier than or equal');
    expect(fetchMock).not.toHaveBeenCalled();
  });

  it('returns detail not-found state when selected device does not exist', async () => {
    const fetchMock = jasmine
      .createSpy('fetchMock')
      .and.resolveTo(new Response(JSON.stringify([]), { status: 200 }));

    const api = new DevicesApiService(fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');
    const facade = new DevicePagesFacade(api, fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');

    const result = await facade.getDeviceDetails('missing-device');

    expect(result.item).toBeNull();
    expect(result.state.status).toBe('error');
  });

  it('maps API status "active" to DeviceStatus "online"', async () => {
    const fetchMock = jasmine.createSpy('fetchMock').and.resolveTo(
      new Response(
        JSON.stringify([{ id: 'd-1', externalId: 'EXT-1', name: 'Sensor', sensorType: 'temperature', status: 'active' }]),
        { status: 200 }
      )
    );

    const api = new DevicesApiService(fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');
    const facade = new DevicePagesFacade(api, fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');

    const result = await facade.getDeviceList();

    expect(result.items[0].status).toBe('online');
  });

  it('maps API status "stale" to DeviceStatus "offline"', async () => {
    const fetchMock = jasmine.createSpy('fetchMock').and.resolveTo(
      new Response(
        JSON.stringify([{ id: 'd-1', externalId: 'EXT-1', name: 'Sensor', sensorType: 'temperature', status: 'stale' }]),
        { status: 200 }
      )
    );

    const api = new DevicesApiService(fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');
    const facade = new DevicePagesFacade(api, fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');

    const result = await facade.getDeviceList();

    expect(result.items[0].status).toBe('offline');
  });

  it('loads selected-device telemetry from endpoint with limit and maps rows', async () => {
    const fetchMock = jasmine.createSpy('fetchMock').and.resolveTo(
      new Response(
        JSON.stringify([
          {
            deviceId: 'd-1',
            metricType: 'temperature',
            metricValue: 30.25,
            eventTimeUtc: '2026-05-23T11:30:00Z'
          }
        ]),
        { status: 200 }
      )
    );

    const api = new DevicesApiService(fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');
    const facade = new DevicePagesFacade(api, fetchMock as unknown as typeof fetch, 'http://localhost:5151/api');

    const result = await facade.getDeviceHistory(
      'd-1',
      {
        fromUtc: '2026-05-23T10:00:00.000Z',
        toUtc: '2026-05-23T12:00:00.000Z'
      },
      25
    );

    expect(fetchMock).toHaveBeenCalledWith('http://localhost:5151/api/devices/d-1/telemetry?limit=25');
    expect(result.state.status).toBe('ready');
    expect(result.items.length).toBe(1);
    expect(result.items[0].metricType).toBe('temperature');
    expect(result.items[0].metricValueDisplay).toBe('30.25');
    expect(result.items[0].timestampUtc).toBe('2026-05-23T11:30:00Z');
  });
});
