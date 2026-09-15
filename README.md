# CatalogAPI

> Microsserviço responsável pelo gerenciamento do catálogo de jogos e início do fluxo de compras da plataforma FIAP Cloud Games (FCG).

---

## 💡 Sobre o projeto

O **CatalogAPI** faz parte da arquitetura de microsserviços da plataforma FIAP Cloud Games (FCG). 

Este serviço é responsável pelo gerenciamento dos jogos disponíveis na plataforma (cadastro, consulta, atualização e exclusão), gerenciamento da biblioteca dos usuários e pela orquestração inicial do fluxo de compra por meio de eventos assíncronos.

---

## 🎯 Responsabilidades

- Cadastro de jogos
- Consulta de jogos
- Atualização de jogos
- Exclusão de jogos
- Gerenciamento da biblioteca de jogos dos usuários
- Publicação de eventos de compra
- Consumo de eventos de pagamento

---

## 🛠️ Tecnologias Utilizadas

- .NET 8 (ASP.NET Core Web API)
- Persistencia Poliglota com MongoDB e Redis
- MassTransit & RabbitMQ (Eventos de domínio)
- Azure Storage Queues & Azure Functions (Processamento de notificações)
- Docker & Kubernetes
- Serilog, Prometheus & Grafana (Observabilidade)
  
---

## 🏗️ Arquitetura Interna

O projeto adota os princípios da Clean Architecture com separação clara de responsabilidades:

- **API:** Controllers, endpoints HTTP e middlewares.
- **Application:** Casos de uso, serviços da aplicação e *consumers* de eventos.
- **Domain:** Entidades de domínio e regras de negócio.
- **Infrastructure:** Persistência de dados (EF Core), repositórios e integrações externas.

---

## 🔄 Mensageria e Fluxo de Eventos

O **CatalogAPI** utiliza comunicação assíncrona orientada a eventos via **RabbitMQ** e **MassTransit**.

### Fluxo de Compra (Visão Geral)

```text
  [CatalogAPI]  --(RabbitMQ: OrderPlacedEvent)-->  [PaymentsAPI]
       ^                                                |
       |-------(RabbitMQ: PaymentProcessedEvent)--------|
                                                        |
                                            (Azure Storage Queue)
                                                        ↓
                                           [NotificationsAPI.Serverless]
```

## Kubernetes

Os manifestos Kubernetes estão disponíveis na pasta:

```
/k8s
```

Recursos utilizados:

- Deployment
- Service
- ConfigMap
- Secret

O serviço se comunica dentro do cluster utilizando os Services Kubernetes.

## Execução local

### Docker Compose

```bash
docker compose up
```

### Kubernetes

Aplicar os manifestos:

```bash
kubectl apply -f k8s/
```

Verificar Pods:

```bash
kubectl get pods
```

Visualizar logs:

```bash
kubectl logs <nome-do-pod>
```

## Observabilidade

A aplicação utiliza Serilog para geração de logs estruturados em console.

Em ambiente Kubernetes os logs podem ser acompanhados utilizando os recursos nativos do cluster. E além disso ainda existe um endpoint exposto para acompanhar os dashboards do Grafana através das métricas do Prometheus

## Objetivo do serviço

O CatalogAPI representa o microsserviço responsável pelo catálogo de jogos e pela coordenação inicial do processo de compra, utilizando eventos para comunicação desacoplada com os demais serviços da plataforma FIAP Cloud Games.
