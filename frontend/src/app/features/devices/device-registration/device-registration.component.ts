import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DevicesApiService } from '../services/devices-api.service';

@Component({
  selector: 'app-device-registration',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './device-registration.component.html',
  styleUrl: './device-registration.component.css'
})
export class DeviceRegistrationComponent {
  private readonly api = new DevicesApiService();

  externalId = '';
  name = '';
  sensorType = '';
  isEnabled = true;
  submitMessage = '';

  async submit(externalId: string, name: string, sensorType: string, isEnabled: boolean): Promise<Response> {
    return this.api.register({ externalId, name, sensorType, isEnabled });
  }

  async submitForm(): Promise<void> {
    try {
      const response = await this.submit(this.externalId, this.name, this.sensorType, this.isEnabled);
      if (!response.ok) {
        console.warn('[DeviceRegistrationComponent.submitForm] Registration returned non-OK status.', {
          status: response.status,
          statusText: response.statusText
        });
      }
      this.submitMessage = response.ok ? 'Device registered successfully.' : 'Registration failed.';
    } catch (error) {
      console.error('[DeviceRegistrationComponent.submitForm] Registration request failed.', { error });
      this.submitMessage = 'Registration failed. Unable to reach API.';
    }
  }
}
