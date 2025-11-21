# 🎮 FIAP Cloud Games (FCG)
O FIAP Cloud Games (FCG) é um projeto acadêmico que reúne conhecimentos adquiridos nas disciplinas onde o desafio envolve o desenvolvimento de uma plataforma que permitirá a venda de jogos digitais e a gestão de servidores para partidas online.
Esta estapa do projeto tem como foco a criação de uma API REST em .NET 8 para gerenciar usuários e suas bibliotecas de jogos adquiridos, garantindo persistência de dados, qualidade do software e boas práticas de desenvolvimento.

## _Microserviço Pagamentos_

Pagamentos.Api é um dos microserviços que compõem a arquitetura do projeto FIAP Cloud Games, responsável por centralizar e gerenciar todas as operações relacionadas aos pagamentos realizados na plataforma.
Este microserviço oferece funcionalidades essenciais para:
- Processamento e registro de transações de pagamento de jogos
- Integração com sistemas externos de pagamento e gateways financeiros
- Consulta e acompanhamento do status de pagamentos efetuados pelos usuários
- Gerenciamento de dados financeiros associados aos pedidos
- Monitoramento e métricas de desempenho via Prometheus
A API foi desenvolvida com foco em segurança, confiabilidade e integração eficiente com outros microserviços da plataforma, utilizando práticas modernas de observabilidade, tratamento centralizado de erros e autenticação. Dessa forma, garante uma experiência segura e transparente para os usuários durante a realização de pagamentos na plataforma.

## 📋 Pré-requisitos

Antes de iniciar o projeto, é necessário atender aos seguintes pré-requisitos para garantir um ambiente de desenvolvimento adequado:

### 🛠 Tecnologias Necessárias
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) – Plataforma de desenvolvimento para criar a API REST
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) – Banco de dados para persistência dos dados
- [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/) ou [VS Code](https://code.visualstudio.com/) – IDE recomendada para desenvolvimento

### 📦 Pacotes e Dependências

O projeto depende dos seguintes pacotes:

#### Projeto Pedidos.Api
- Mensageria e Integração: Azure.Messaging.ServiceBus
- Autenticação via JWT: Microsoft.AspNetCore.Authentication.JwtBearer
- Observabilidade e Telemetria: Microsoft.ApplicationInsights.AspNetCore, Microsoft.Extensions.Logging.ApplicationInsights, prometheus-net.AspNetCore
- ORM e Banco de Dados: Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Design, Microsoft.EntityFrameworkCore.Tools
- Documentação da API: Swashbuckle.AspNetCore, Swashbuckle.AspNetCore.Annotations
- Componentes e utilitários internos: Fcg.Common
- Suporte a containers (Docker): Microsoft.VisualStudio.Azure.Containers.Tools.Targets

```
Install-Package Azure.Messaging.ServiceBus -Version 7.20.1
Install-Package Fcg.Common -Version 1.0.0
Install-Package Microsoft.ApplicationInsights.AspNetCore -Version 2.23.0
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 8.0.15
Install-Package Microsoft.EntityFrameworkCore -Version 8.0.19
Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.19
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.19
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.19
Install-Package Microsoft.Extensions.Logging.ApplicationInsights -Version 2.23.0
Install-Package Microsoft.VisualStudio.Azure.Containers.Tools.Targets -Version 1.22.1
Install-Package prometheus-net.AspNetCore -Version 8.2.1
Install-Package Swashbuckle.AspNetCore -Version 7.3.2
Install-Package Swashbuckle.AspNetCore.Annotations -Version 7.3.2
```

#### Projeto Pagamentos.Api.Tests
- Framework de Testes Unitários: xunit, xunit.runner.visualstudio
- Mock e Simulação de Dependências: Moq
- Infraestrutura de Execução de Testes: Microsoft.NET.Test.Sdk
```
Install-Package xunit -Version 2.9.3
Install-Package xunit.runner.visualstudio -Version 3.1.5
Install-Package Moq -Version 4.20.72
Install-Package Microsoft.NET.Test.Sdk -Version 17.14.1
```

## 🗂️ Estrutura
O projeto Pagamentos.Api está organizado em camadas, seguindo boas práticas de separação de responsabilidades e facilitando a manutenção, testes e evolução do sistema.
```
Pedidos.Api/
│──📂 Applitation/
│   ├──📂 Constants/
│   ├──📂 DTOs/
│   ├──📂 Mappers/
│   ├──📂 Services/
│──📂 Configurations/
│──📂 Controllers/
│──📂 Domain/
│   ├──📂 Entities/
│   ├──📂 Events/
│   ├──📂 Interfaces/
│──📂 Infraestructure/
│   ├──📂 Data/
│   ├──📂 Mappings
│   ├──📂 Search/
Pedidos.Api.Tests/
│──📂 ServicesTests/
```
#### 1. Application
Agrupa a lógica de aplicação, servindo de ponte entre a API e o domínio.
- DTOs: Objetos de transferência de dados, usados para entrada e saída de informações na API.
- Mappers: Classes estáticas ou utilitários para conversão entre entidades do domínio e DTOs.
- Services: Serviços de aplicação que orquestram regras de negócio, validações e interações entre as camadas.

#### 2. Configurations
Contém classes responsáveis pelas configurações globais da aplicação, como injeção de dependências, configuração do Swagger, Application Insights, Prometheus, autenticação, entre outros. Centraliza tudo que é necessário para inicializar e configurar o ambiente da API.

#### 3. Controllers
Reúne os controladores da API, que são responsáveis por expor os endpoints HTTP. Cada controller lida com as requisições, validações iniciais e retorna as respostas apropriadas, delegando a lógica de negócio para os serviços da camada de aplicação.

#### 4. Domain
Representa o núcleo do sistema, onde ficam as regras de negócio e abstrações principais. Suas subpastas normalmente incluem:
- Entities: Entidades de domínio, que representam os objetos principais do negócio (ex: pagamento).
- Interfaces: Contratos para repositórios e serviços, promovendo o desacoplamento entre domínio e infraestrutura.

#### 5. Infraestructure
Responsável pela implementação de detalhes técnicos e integrações externas, como acesso a banco de dados, mecanismos de busca, etc. Suas subpastas podem incluir:
- Data: Implementações de repositórios, contexto do Entity Framework (DbContext) e migrações.

#### 6. ServicesTests
Contém os testes automatizados do sistema, organizados por tipo:
- Unitários: Testam funcionalidades isoladas.
- Integração: Validam a integração entre componentes e camadas.

## 🏛️ Entidades do Domínio
Integrada aos demais serviços do ecossistema FIAP Cloud Games, a Pagamentos.Api atua como o núcleo de dados dos pagamentos, permitindo que suas informações possam ser consumidas de forma segura e eficiente.

## ⚙️ Funcionalidades da Api
A API expõe os seguintes endpoints:

| **Método** | **Endpoint** | **Descrição** |
| ------ | ------ | ------ |
| 🔵 GET | `/Pagamentos/ObterPagamento` | Retorna um pagamento específico com base no ID do pedido informado | 
| 🔵 GET | `/Pagamentos/ObterPagamentos` | Retorna uma lista de todos os pagamentos cadastrados | 
| 🟩 POST | `/Pagamentos/EfetuarPagamento` | Permite que usuários efetuem o pagamento de um pedido | 

## 🚀 Executando os testes

Para garantir a qualidade e a estabilidade do projeto, é essencial executar os testes automatizados. O projeto utiliza xUnit para testes e Moq para simulação de dependências.

### Estrutura dos testes
Os testes estão organizados conforme a estrutura do projeto:

```
Pagamentos.Api.Tests
│── 📂 ServicesTests
│    │── 📄 PagamentoServiceTests

```
Para rodar os testes, siga os passos:

#### ✅ Executar todos os testes
```
dotnet test
```

#### ✅ Executar um teste espesífico

```
dotnet test --filter FullyQualifiedName=Namespace.Classe.Teste
```

Exemplo:
```
dotnet test --filter FullyQualifiedName=FCG.Tests.IntegrationTests.ServicesTests.EfetuarPagamento_ComDadosValidos_DeveSalvarNoBanco
```

#### ✅ Executar apenas testes unitários
```
dotnet test --filter Category=Unit
```

#### ✅ Executar apenas testes de integração
```
dotnet test --filter Category=Integration
```

#### ✅ Executar apenas testes de BDD
```
dotnet test --filter Category=BDD
```

## ⚙️ Arquitetura de Deploy e Execução em AKS

A imagem abaixo representa o fluxo completo de deploy e execução da aplicação containerizada utilizando Azure Kubernetes Service (AKS) como plataforma de orquestração:

[![Fluxo-Kubernetes.png](https://i.postimg.cc/V60vNNNd/Fluxo-Kubernetes.png)](https://postimg.cc/phxRGRzH)

### 🔄 Fluxo de Deploy e Operação
#### 1-Versionamento e Trigger de Pipeline 
O código-fonte é mantido no GitHub, e qualquer alteração aciona o Azure Pipeline, que executa as etapas de CI/CD definidas nos arquivos yml de pipelines.
#### 2-Build e Publicação de Imagem
O pipeline realiza o build da aplicação, gera a imagem Docker e publica no Azure Container Registry (ACR). A imagem é versionada com tags baseadas em versão semântica, sendo que imagem mais recente também possui o sufixo "latest".
#### 3-Deploy no AKS via Manifestos Kubernetes
Após o build, o pipeline aplica os manifestos Kubernetes (Deployment, Service, Secret, etc.) no cluster AKS. O deploy é feito no namespace correspondente ao ambiente produtivo.
#### 4-Execução no Cluster
- O Pod é agendado em um Node do cluster.
- O Container é instanciado a partir da imagem armazenada no ACR.
- Os Secrets são injetados como variáveis de ambiente.
- O Service expõe o Pod externamente.
#### 5-Integração com Serviços Externos
A aplicação se comunica com:
- Base de Dados Microsoft SQL Server para persistência de dados.
- RabbitMQ para troca de mensagens assíncronas entre microsserviços.
- Application Insights para telemetria, rastreamento de requisições e análise de performance.
#### 6-Acesso do Usuário Final
O usuário acessa a aplicação via IP público exposto pelo serviço no AKS. O tráfego é roteado para o Pod ativo, que processa a requisição e interage com os serviços externos conforme necessário.

## 📡 Fluxo de Comunicação Assíncrona com RabbitMQ
A arquitetura utiliza RabbitMQ como broker de mensagens para garantir comunicação assíncrona entre os microsserviços. Esse modelo desacopla produtores e consumidores, permitindo que cada serviço processe eventos no seu próprio ritmo e garantindo escalabilidade horizontal com Kubernetes.

[![Fluxo-Rabbit-MQ.png](https://i.postimg.cc/13MLjhQF/Fluxo-Rabbit-MQ.png)](https://postimg.cc/4Y7LY0n4)

### 🔄 Exemplo Real: Confirmação do pagamentos para baixa do pedido
#### 1-Microserviço Pagamentos
- Após a confirmação do pagamento de um jogo, o serviço publica uma mensagem na fila pagamento-jogo-realizado.
- Essa mensagem contém os dados essenciais da transação.

#### 2-RabbitMQ (Broker)
- Armazena a mensagem na fila até que algum consumidor esteja disponível.
- Garante entrega confiável, podendo aplicar estratégias de retry e dead-letter queue em caso de falhas.

#### 3-Microserviço Pedidos
- Está inscrito como consumidor da fila pagamento-jogo-realizado.
- Ao receber a mensagem, atualiza o status do pedido correspondente para “pago”, garantindo consistência no fluxo de negócio.
- Esse processamento é assíncrono: o usuário não precisa esperar a atualização do pedido para concluir o pagamento.

## ✒️ Autor
*Márcio Henrique Vieira dos Santos - ✉️ marciohenriquev@gmail.com*# FCG
