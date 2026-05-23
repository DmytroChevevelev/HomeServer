/// <reference types="jasmine" />

import { DeviceTelemetryRealtimeService } from './device-telemetry-realtime.service';

describe('DeviceTelemetryRealtimeService', () => {
  it('creates with disconnected initial state', () => {
    const service = new DeviceTelemetryRealtimeService('http://localhost:5151/api');
    let initialState: unknown = null;

    const unsubscribe = service.onConnectionStateChanged((state) => {
      initialState = state;
    });

    expect(service).toBeTruthy();
    expect(initialState).toBe('disconnected');

    unsubscribe();
  });
});
