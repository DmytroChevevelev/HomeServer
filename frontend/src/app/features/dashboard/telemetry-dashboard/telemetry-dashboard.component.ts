import { TelemetryApiService } from '../services/telemetry-api.service';

export class TelemetryDashboardComponent {
  private readonly api = new TelemetryApiService();
  private timer?: ReturnType<typeof setInterval>;

  async loadLatest(): Promise<Response> {
    return this.api.latest();
  }

  startPolling(intervalMs = 5000): void {
    this.stopPolling();
    this.timer = setInterval(() => {
      void this.loadLatest();
    }, intervalMs);
  }

  stopPolling(): void {
    if (this.timer) {
      clearInterval(this.timer);
      this.timer = undefined;
    }
  }
}
