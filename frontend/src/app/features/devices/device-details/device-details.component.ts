import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { DeviceDetailsHeaderViewModel, HistoricalTelemetryRowViewModel, UiSurfaceState } from '../models/device-pages.models';
import { DevicePagesFacade } from '../services/device-pages.facade';

@Component({
  selector: 'app-device-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './device-details.component.html',
  styleUrl: './device-details.component.css'
})
export class DeviceDetailsComponent implements OnInit {
  uiState: UiSurfaceState = { status: 'loading', errorMessage: null };
  item: DeviceDetailsHeaderViewModel | null = null;
  telemetryState: UiSurfaceState = { status: 'loading', errorMessage: null };
  telemetryItems: HistoricalTelemetryRowViewModel[] = [];
  unregisterMessage = '';

  private deviceId = '';
  private readonly facade = new DevicePagesFacade();

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  async ngOnInit(): Promise<void> {
    this.deviceId = this.route.snapshot.paramMap.get('deviceId') ?? '';
    await this.loadDetails();
  }

  async unregister(): Promise<void> {
    if (!this.deviceId) {
      this.unregisterMessage = 'Unable to unregister device.';
      return;
    }

    const result = await this.facade.unregisterDevice(this.deviceId);
    if (result.state.status === 'error') {
      this.unregisterMessage = result.state.errorMessage ?? 'Unable to unregister device.';
      return;
    }

    this.unregisterMessage = 'Device unregistered successfully.';
    await this.router.navigate(['/devices']);
  }

  private async loadDetails(): Promise<void> {
    const detailResult = await this.facade.getDeviceDetails(this.deviceId);
    this.uiState = detailResult.state;
    this.item = detailResult.item;

    if (!this.item) {
      this.telemetryState = { status: 'empty', errorMessage: null };
      this.telemetryItems = [];
      return;
    }

    const historyResult = await this.facade.getDeviceHistory(this.deviceId, {
      fromUtc: new Date(Date.now() - 24 * 60 * 60 * 1000).toISOString(),
      toUtc: new Date().toISOString()
    });
    this.telemetryState = historyResult.state;
    this.telemetryItems = historyResult.items;
  }
}
