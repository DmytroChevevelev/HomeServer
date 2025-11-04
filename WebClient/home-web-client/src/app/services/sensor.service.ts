import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Sensor } from '../models/sensor';
import { API_BASE } from '../app.settings';

@Injectable({ providedIn: 'root' })
export class SensorService {
  // Use API_BASE so we can easily point to a separate backend during dev
  private readonly base = `${API_BASE}/api/sensor`;

  constructor(private http: HttpClient) {}

  /**
   * Returns an observable of sensors fetched from WebApi (/api/sensor)
   */
  getSensors(): Observable<Sensor[]> {
    return this.http.get<Sensor[]>(this.base);
  }
}
