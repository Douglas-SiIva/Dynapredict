import api from "./api";
import { LoginData, RegisterData, AuthResponse } from "@/types/auth";

export const authService = {
  async login(data: LoginData): Promise<AuthResponse> {
    const response = await api.post("/auth/login", data);
    const authData = response.data;

    localStorage.setItem("authToken", authData.token.accessToken);
    localStorage.setItem("user", JSON.stringify(authData.user));

    return authData;
  },

  async register(data: RegisterData): Promise<AuthResponse> {
    const response = await api.post("/auth/register", data);
    const authData = response.data;

    localStorage.setItem("authToken", authData.token.accessToken);
    localStorage.setItem("user", JSON.stringify(authData.user));

    return authData;
  },

  async logout(): Promise<void> {
    try {
      await api.post("/auth/logout");
    } finally {
      localStorage.removeItem("authToken");
      localStorage.removeItem("user");
    }
  },

  getCurrentUser() {
    const userStr = localStorage.getItem("user");
    return userStr ? JSON.parse(userStr) : null;
  },

  getToken() {
    return localStorage.getItem("authToken");
  },

  isAuthenticated() {
    return !!this.getToken();
  },
};
