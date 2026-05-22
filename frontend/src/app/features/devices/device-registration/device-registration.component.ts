import { DevicesApiService } from '../services/devices-api.service';

export class DeviceRegistrationComponent {
  private readonly api = new DevicesApiService();

  async submit(externalId: string, name: string, sensorType: string): Promise<Response> {
    return this.api.register({ externalId, name, sensorType });
  }
}
