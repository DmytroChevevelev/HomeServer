import { environment } from '../../../../environments/environment';

export interface RegisterDeviceRequest {
  externalId: string;
  name: string;
  sensorType: string;
  isEnabled: boolean;
}

type FetchFn = (input: RequestInfo | URL, init?: RequestInit) => Promise<Response>;
const defaultFetch: FetchFn = (input, init) => globalThis.fetch(input, init);

export class DevicesApiService {
  constructor(
    private readonly fetchFn: FetchFn = defaultFetch,
    private readonly apiBaseUrl: string = environment.apiBaseUrl
  ) {}

  async register(payload: RegisterDeviceRequest): Promise<Response> {
    return this.fetchFn(`${this.apiBaseUrl}/devices`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    });
  }

  async list(): Promise<Response> {
    return this.fetchFn(`${this.apiBaseUrl}/devices`, {
      method: 'GET'
    });
  }

  async unregister(deviceId: string): Promise<Response> {
    return this.fetchFn(`${this.apiBaseUrl}/devices/${encodeURIComponent(deviceId)}`, {
      method: 'DELETE'
    });
  }
}
