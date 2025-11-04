import { Routes } from '@angular/router';

export const routes: Routes = [
	{
		path: 'sensors',
		loadComponent: () => import('./sensor-list/sensor-list').then((m) => m.SensorList),
	},
	{
		path: '',
		pathMatch: 'full',
		redirectTo: '',
	},
];
