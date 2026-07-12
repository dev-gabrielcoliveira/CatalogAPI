# FCG.CatalogAPI

Microsserviço responsável pelo gerenciamento do catálogo de jogos da plataforma FIAP Cloud Games (FCG).

## Sobre o projeto

O FCG.CatalogAPI faz parte da arquitetura de microsserviços da plataforma FIAP Cloud Games.

Este serviço é responsável pelo gerenciamento dos jogos disponíveis na plataforma, incluindo cadastro, consulta, atualização e exclusão de jogos, além de iniciar o fluxo de compra através de eventos assíncronos.

A aplicação foi desenvolvida utilizando .NET 8, Docker e Kubernetes, seguindo uma arquitetura orientada a eventos.

## Responsabilidades

- Cadastro de jogos
- Consulta de jogos
- Atualização de jogos
- Exclusão de jogos
- Gerenciamento da biblioteca de jogos dos usuários
- Publicação de eventos de compra
- Consumo de eventos de pagamento

## Tecnologias utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- MassTransit
- RabbitMQ
- Docker
- Kubernetes
- Serilog

## Arquitetura

O projeto possui separação de responsabilidades:

- **API**
  - Controllers
  - Endpoints HTTP

- **Application**
  - Casos de uso
  - Serviços da aplicação
  - Consumers de eventos

- **Domain**
  - Entidades
  - Regras de negócio

- **Infrastructure**
  - Persistência
  - Repositórios
  - Configurações externas

## Mensageria

O CatalogAPI participa do fluxo de compra utilizando comunicação assíncrona através do RabbitMQ e MassTransit.

### Fluxo de compra

```text
CatalogAPI
    |
    | OrderPlacedEvent
    ↓
RabbitMQ
    ↓
PaymentsAPI
    |
    | PaymentProcessedEvent
    ↓
CatalogAPI
    +
NotificationsAPI
```

### OrderPlacedEvent

Quando uma compra é iniciada, o CatalogAPI publica o evento:

```
OrderPlacedEvent
```

Contendo informações como:

- UserId
- GameId
- Price

O PaymentsAPI consome esse evento e realiza o processamento do pagamento.

### PaymentProcessedEvent

Após o processamento do pagamento, o PaymentsAPI publica:

```
PaymentProcessedEvent
```

O CatalogAPI consome esse evento.

Quando o pagamento é aprovado:

- O jogo é adicionado à biblioteca do usuário.

Quando o pagamento é rejeitado:

- A compra não é concluída.

## Banco de Dados

O serviço utiliza:

```
SQL Server
```

A persistência é realizada utilizando Entity Framework Core e migrations.

As informações sensíveis, como connection strings e chaves privadas, são armazenadas utilizando Kubernetes Secrets.

## Docker

O projeto possui Dockerfile utilizando multi-stage build.

O processo separa:

1. Compilação da aplicação utilizando o SDK do .NET.
2. Execução utilizando somente o runtime necessário.

Benefícios:

- Imagem final otimizada.
- Menor consumo de recursos.
- Maior segurança no ambiente de produção.

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

Em ambiente Kubernetes os logs podem ser acompanhados utilizando os recursos nativos do cluster.

Exemplo:

```bash
kubectl logs catalog-deployment-xxxxx
```

## Objetivo do serviço

O FCG.CatalogAPI representa o microsserviço responsável pelo catálogo de jogos e pela coordenação inicial do processo de compra, utilizando eventos para comunicação desacoplada com os demais serviços da plataforma FIAP Cloud Games.