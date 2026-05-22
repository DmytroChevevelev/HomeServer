import { DEVICES_ROUTES } from './features/devices/devices.routes';

export const APP_ROUTES = [
  ...DEVICES_ROUTES,
  { path: 'dashboard', component: 'TelemetryDashboardComponent' }
];
