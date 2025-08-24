import React, { useEffect, useState } from "react";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { useAuth } from "@/contexts/AuthContext";
import { machineService } from "@/services/machineService";
import { Machine } from "@/types/machine";
import { Settings, Users, Wrench, TrendingUp } from "lucide-react";

export const Dashboard: React.FC = () => {
  const { user, logout } = useAuth();
  const [machines, setMachines] = useState<Machine[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    loadMachines();
  }, []);

  const loadMachines = async () => {
    try {
      const data = await machineService.getAllMachines();
      setMachines(data);
    } catch (error) {
      console.error("Erro ao carregar máquinas:", error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleLogout = async () => {
    try {
      await logout();
    } catch (error) {
      console.error("Erro no logout:", error);
    }
  };

  const activeMachines = machines.filter((m) => m.status === "active").length;
  const maintenanceMachines = machines.filter((m) => m.status === "maintenance").length;
  const errorMachines = machines.filter((m) => m.status === "error").length;

  return (
    <div className="min-h-screen bg-background">
      <header className="border-b bg-card">
        <div className="container mx-auto px-4 py-4 flex justify-between items-center">
          <div>
            <h1 className="text-2xl font-bold text-foreground">Dynapredict</h1>
            <p className="text-muted-foreground">Sistema de Monitoramento Industrial</p>
          </div>
          <div className="flex items-center gap-4">
            <span className="text-sm text-muted-foreground">Olá, {user?.name}</span>
            <Button onClick={handleLogout} variant="outline">
              Sair
            </Button>
          </div>
        </div>
      </header>

      <main className="container mx-auto px-4 py-8">
        <div className="mb-8">
          <h2 className="text-3xl font-bold text-foreground mb-2">Dashboard</h2>
          <p className="text-muted-foreground">Visão geral do sistema de monitoramento</p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Total de Máquinas</CardTitle>
              <Wrench className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{machines.length}</div>
              <p className="text-xs text-muted-foreground">Máquinas cadastradas</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Máquinas Ativas</CardTitle>
              <TrendingUp className="h-4 w-4 text-green-600" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-green-600">{activeMachines}</div>
              <p className="text-xs text-muted-foreground">Em operação normal</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Em Manutenção</CardTitle>
              <Settings className="h-4 w-4 text-yellow-600" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-yellow-600">{maintenanceMachines}</div>
              <p className="text-xs text-muted-foreground">Manutenção programada</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">Com Erro</CardTitle>
              <Users className="h-4 w-4 text-red-600" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-red-600">{errorMachines}</div>
              <p className="text-xs text-muted-foreground">Requerem atenção</p>
            </CardContent>
          </Card>
        </div>

        <Card>
          <CardHeader>
            <CardTitle>Máquinas Recentes</CardTitle>
            <CardDescription>Últimas máquinas adicionadas ao sistema</CardDescription>
          </CardHeader>
          <CardContent>
            {isLoading ? (
              <p className="text-muted-foreground">Carregando máquinas...</p>
            ) : machines.length === 0 ? (
              <p className="text-muted-foreground">Nenhuma máquina cadastrada ainda.</p>
            ) : (
              <div className="space-y-4">
                {machines.slice(0, 5).map((machine) => (
                  <div
                    key={machine.id}
                    className="flex items-center justify-between p-4 border rounded-lg"
                  >
                    <div>
                      <h4 className="font-medium">{machine.name}</h4>
                      <p className="text-sm text-muted-foreground">
                        {machine.type} - {machine.serialNumber}
                      </p>
                    </div>
                    <span
                      className={`px-2 py-1 rounded-full text-xs font-medium ${
                        machine.status === "active"
                          ? "bg-green-100 text-green-800"
                          : machine.status === "maintenance"
                          ? "bg-yellow-100 text-yellow-800"
                          : "bg-red-100 text-red-800"
                      }`}
                    >
                      {machine.status === "active"
                        ? "Ativa"
                        : machine.status === "maintenance"
                        ? "Manutenção"
                        : "Erro"}
                    </span>
                  </div>
                ))}
              </div>
            )}
          </CardContent>
        </Card>
      </main>
    </div>
  );
};
