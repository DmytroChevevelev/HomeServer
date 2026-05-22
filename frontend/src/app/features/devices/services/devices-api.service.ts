import { environment } from '../../../../environments/environment';

export interface RegisterDeviceRequest {
  externalId: string;
  name: string;
  sensorType: string;
}

type FetchFn = (input: RequestInfo | URL, init?: RequestInit) => Promise<Response>;

export class DevicesApiService {
  constructor(
    private readonly fetchFn: FetchFn = fetch,
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
    return this.fetchFn(`${this.apiBaseUrl}/devices`);
  }
}
