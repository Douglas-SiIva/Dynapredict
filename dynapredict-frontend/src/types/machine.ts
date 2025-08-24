export interface Machine {
  id: number;
  name: string;
  serialNumber: string;
  description: string;
  type: string;
  status: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateMachineData {
  name: string;
  serialNumber: string;
  description: string;
  type: string;
}

export interface UpdateMachineData {
  name?: string;
  serialNumber?: string;
  description?: string;
  type?: string;
  status?: string;
}
