export interface Advisor {
  id: number;
  firstName: string;
  lastName: string;
  sin: string;
  address: string;
  phone: string;
  healthStatus: string;
}

export type AdvisorCreationDto = Omit<Omit<Advisor, "id">, "healthStatus">;
export type AdvisorUpdateDto = Omit<AdvisorCreationDto, "sin">;
