export interface Advisor {
  id?: number;
  firstName: string;
  lastName: string;
  email: string;
  sin: string;
  address: string;
  phone: string;
  healthStatus: HealthStatus;
}

export enum HealthStatus {
  Green = 'Green',
  Yellow = 'Yellow',
  Red = 'Red',
}