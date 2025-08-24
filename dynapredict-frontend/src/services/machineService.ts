import api from "./api";
import { Machine, CreateMachineData, UpdateMachineData } from "@/types/machine";

export const machineService = {
  async getAllMachines(): Promise<Machine[]> {
    const response = await api.get("/machines");
    return response.data;
  },

  async getMachineById(id: number): Promise<Machine> {
    const response = await api.get(`/machines/${id}`);
    return response.data;
  },

  async createMachine(data: CreateMachineData): Promise<Machine> {
    const response = await api.post("/machines", data);
    return response.data;
  },

  async updateMachine(id: number, data: UpdateMachineData): Promise<void> {
    await api.put(`/machines/${id}`, data);
  },

  async deleteMachine(id: number): Promise<void> {
    await api.delete(`/machines/${id}`);
  },

  async updateMachineStatus(id: number, status: string): Promise<void> {
    await api.patch(`/machines/${id}/status`, JSON.stringify(status), {
      headers: { "Content-Type": "application/json" },
    });
  },
};
