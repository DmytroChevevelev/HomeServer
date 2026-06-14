import * as signalR from '@microsoft/signalr';
import { environment } from '../../../../environments/environment';
import { SensorValueChangedEventContract } from '../models/device-pages.models';

export type RealtimeConnectionState = 'disconnected' | 'connecting' | 'connected' | 'reconnecting';

const SENSOR_VALUE_CHANGED_EVENT = 'sensorValueChanged';

type SensorValueChangedHandler = (payload: SensorValueChangedEventContract) => void;
type ConnectionStateHandler = (state: RealtimeConnectionState) => void;

export class DeviceTelemetryRealtimeService {
  private connection: signalR.HubConnection | null = null;
  private readonly stateHandlers = new Set<ConnectionStateHandler>();

  constructor(private readonly apiBaseUrl: string = environment.apiBaseUrl) {}

  async start(): Promise<void> {
    const hub = this.ensureConnection();
    if (hub.state !== signalR.HubConnectionState.Disconnected) {
      return;
    }

    this.notifyState('connecting');
    await hub.start();
    this.notifyState('connected');
  }

  async stop(): Promise<void> {
    if (!this.connection) {
      this.notifyState('disconnected');
      return;
    }

    await this.connection.stop();
    this.notifyState('disconnected');
  }

  onSensorValueChanged(handler: SensorValueChangedHandler): () => void {
    const hub = this.ensureConnection();
    hub.on(SENSOR_VALUE_CHANGED_EVENT, handler);
    return () => hub.off(SENSOR_VALUE_CHANGED_EVENT, handler);
  }

  onConnectionStateChanged(handler: ConnectionStateHandler): () => void {
    this.stateHandlers.add(handler);
    handler(this.getConnectionState());
    return () => {
      this.stateHandlers.delete(handler);
    };
  }

  private ensureConnection(): signalR.HubConnection {
    if (this.connection) {
      return this.connection;
    }

    const hubUrl = new URL('/hubs/telemetry', this.apiBaseUrl).toString();

    const connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, { withCredentials: false })
      .withAutomaticReconnect()
      .build();

    connection.onreconnecting(() => {
      this.notifyState('reconnecting');
    });

    connection.onreconnected(() => {
      this.notifyState('connected');
    });

    connection.onclose(() => {
      this.notifyState('disconnected');
    });

    this.connection = connection;
    return connection;
  }

  private getConnectionState(): RealtimeConnectionState {
    if (!this.connection) {
      return 'disconnected';
    }

    switch (this.connection.state) {
      case signalR.HubConnectionState.Connected:
        return 'connected';
      case signalR.HubConnectionState.Connecting:
        return 'connecting';
      case signalR.HubConnectionState.Reconnecting:
        return 'reconnecting';
      default:
        return 'disconnected';
    }
  }

  private notifyState(state: RealtimeConnectionState): void {
    this.stateHandlers.forEach((handler) => handler(state));
  }
}
