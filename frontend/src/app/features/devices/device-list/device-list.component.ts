import { NgClass, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DeviceListItemViewModel, DeviceStatus, StatusFilterOption, UiSurfaceState } from '../models/device-pages.models';
import { DevicePagesFacade } from '../services/device-pages.facade';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [NgIf, NgFor, NgClass, RouterLink],
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.css'
})
export class DeviceListComponent implements OnInit {
  allDevices: DeviceListItemViewModel[] = [];
  uiState: UiSurfaceState = { status: 'loading', errorMessage: null };
  selectedStatus: StatusFilterOption = 'all';
  expandedDeviceIds = new Set<string>();

  private readonly facade = new DevicePagesFacade();

  async ngOnInit(): Promise<void> {
    const result = await this.facade.getDeviceList();
    this.allDevices = result.items;
    this.uiState = result.state;
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
