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
  submitMessage = '';

  async submit(externalId: string, name: string, sensorType: string): Promise<Response> {
    return this.api.register({ externalId, name, sensorType });
  }

  async submitForm(): Promise<void> {
    const response = await this.submit(this.externalId, this.name, this.sensorType);
    this.submitMessage = response.ok ? 'Device registered successfully.' : 'Registration failed.';
  }
}
