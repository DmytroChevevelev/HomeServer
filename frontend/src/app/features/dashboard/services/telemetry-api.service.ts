import { environment } from '../../../../environments/environment';

export class TelemetryApiService {
  async latest(): Promise<Response> {
    return fetch(`${environment.apiBaseUrl}/telemetry/latest`);
  }
}
