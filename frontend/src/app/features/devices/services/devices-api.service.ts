import { environment } from '../../../environments/environment';

export interface RegisterDeviceRequest {
  externalId: string;
  name: string;
  sensorType: string;
}

export class DevicesApiService {
  async register(payload: RegisterDeviceRequest): Promise<Response> {
    return fetch(`${environment.apiBaseUrl}/devices`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    });
  }

  async list(): Promise<Response> {
    return fetch(`${environment.apiBaseUrl}/devices`);
  }
}
