import { DevicesApiService } from '../services/devices-api.service';

export class DeviceListComponent {
  private readonly api = new DevicesApiService();

  async load(): Promise<Response> {
    return this.api.list();
  }
}
