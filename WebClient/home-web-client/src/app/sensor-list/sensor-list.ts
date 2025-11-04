import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { Sensor } from '../models/sensor';
import { SensorService } from '../services/sensor.service';

@Component({
  selector: 'app-sensor-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sensor-list.html',
  styleUrls: ['./sensor-list.scss'],
})
export class SensorList implements OnInit {
  sensors$!: Observable<Sensor[]>;

  constructor(private sensorService: SensorService) {}

  ngOnInit(): void {
    this.sensors$ = this.sensorService.getSensors();
  }
}
