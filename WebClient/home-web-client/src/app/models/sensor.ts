export interface Sensor {
  id: number;
  name: string;
  description?: string | null;
  type: string;
  location: string;
  isActive: boolean;
  lastUpdated: string; // ISO datetime string from server
}
