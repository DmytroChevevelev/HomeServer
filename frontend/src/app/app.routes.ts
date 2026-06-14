import { Routes } from '@angular/router';
import { DEVICES_ROUTES } from './features/devices/devices.routes';

export const routes: Routes = [
	{ path: '', pathMatch: 'full', redirectTo: 'devices' },
	...DEVICES_ROUTES,
	{ path: '**', redirectTo: 'devices' }
];
