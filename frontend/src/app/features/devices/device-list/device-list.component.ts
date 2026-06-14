import { NgClass, NgFor, NgIf } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DeviceListItemViewModel, DeviceStatus, StatusFilterOption, UiSurfaceState } from '../models/device-pages.models';
import { DeviceTelemetryRealtimeService } from '../services/device-telemetry-realtime.service';
import { DevicePagesFacade } from '../services/device-pages.facade';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [NgIf, NgFor, NgClass, RouterLink],
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.css'
})
export class DeviceListComponent implements OnInit, OnDestroy {
  allDevices: DeviceListItemViewModel[] = [];
  uiState: UiSurfaceState = { status: 'loading', errorMessage: null };
  realtimeWarning: string | null = null;
  selectedStatus: StatusFilterOption = 'all';
  expandedDeviceIds = new Set<string>();

  private readonly facade = new DevicePagesFacade();
  private readonly realtime = new DeviceTelemetryRealtimeService();
  private unsubscribeSensorValueChanged: (() => void) | null = null;
  private unsubscribeConnectionState: (() => void) | null = null;

  async ngOnInit(): Promise<void> {
    const result = await this.facade.getDeviceList();
    this.allDevices = result.items;
    this.uiState = result.state;

    this.unsubscribeSensorValueChanged = this.realtime.onSensorValueChanged((event) => {
      this.allDevices = this.facade.applyRealtimeUpdate(this.allDevices, event);
    });

    this.unsubscribeConnectionState = this.realtime.onConnectionStateChanged((state) => {
      this.realtimeWarning = state === 'connected' ? null : 'Realtime connection is unavailable. Latest values may be stale.';
    });

    try {
      await this.realtime.start();
    } catch {
      this.realtimeWarning = 'Unable to start realtime updates. Latest values may be stale.';
    }
  }

  async ngOnDestroy(): Promise<void> {
    this.unsubscribeSensorValueChanged?.();
    this.unsubscribeConnectionState?.();
    await this.realtime.stop();
  }

  get filteredDevices(): DeviceListItemViewModel[] {
    if (this.selectedStatus === 'all') {
      return this.allDevices;
    }
    return this.allDevices.filter(d => d.status === this.selectedStatus);
  }

  setFilter(status: StatusFilterOption): void {
    this.selectedStatus = status;
  }

  toggleExpanded(deviceId: string): void {
    if (this.expandedDeviceIds.has(deviceId)) {
      this.expandedDeviceIds.delete(deviceId);
      return;
    }

    this.expandedDeviceIds.add(deviceId);
  }

  isExpanded(deviceId: string): boolean {
    return this.expandedDeviceIds.has(deviceId);
  }

  badgeClass(status: DeviceStatus): Record<string, boolean> {
    return {
      'badge': true,
      'bg-success': status === 'online',
      'bg-warning': status === 'warning',
      'text-dark': status === 'warning',
      'bg-danger': status === 'offline',
      'bg-secondary': status === 'unknown'
    };
  }
}
