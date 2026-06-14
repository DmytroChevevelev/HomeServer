import { Routes } from '@angular/router';

export const DEVICES_ROUTES: Routes = [
  {
    path: 'devices',
    loadComponent: () => import('./device-list/device-list.component').then((m) => m.DeviceListComponent)
  },
  {
    path: 'devices/register',
    loadComponent: () =>
      import('./device-registration/device-registration.component').then((m) => m.DeviceRegistrationComponent)
  },
  {
    path: 'devices/:deviceId',
    loadComponent: () => import('./device-details/device-details.component').then((m) => m.DeviceDetailsComponent)
  }
];
