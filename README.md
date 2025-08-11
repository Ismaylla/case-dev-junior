# API de Gerenciamento de Tarefas (To-Do List)

Este projeto consiste em uma solução completa para gerenciamento de uma lista de tarefas (To-Do), desenvolvida como parte de um case técnico para demonstrar habilidades em desenvolvimento full-stack.

A aplicação é composta por uma **API RESTful** robusta, construída com **C# e .NET**, e um **front-end reativo** desenvolvido com **React e Vite**. O back-end segue uma arquitetura em camadas bem definida, inspirada em princípios de design como o **Repository Pattern**, **Dependency Inversion (com uso de Interfaces)** e o uso de **DTOs (Data Transfer Objects)**, garantindo uma clara separação de responsabilidades, alta coesão e baixo acoplamento entre os componentes.

## 🏛️ Arquitetura e Padrões de Projeto

A estrutura do back-end foi desenhada para ser limpa, escalável e de fácil manutenção, separando as responsabilidades em camadas distintas:

* **Controllers (Camada de Apresentação):** Responsável por gerenciar as requisições HTTP, validar os dados de entrada através de DTOs e orquestrar o fluxo de dados. Inclui um `AuthController` dedicado e um `TaskController` para as operações de tarefas.
* **Services (Camada de Serviço/Lógica de Negócio):** Contém a lógica de negócio principal. Ela coordena as operações e atua como uma ponte entre os controllers e os repositórios. O uso de **Interfaces** (`ITaskService`) permite uma arquitetura desacoplada e facilita a implementação de testes.
* **Repositories (Camada de Acesso a Dados):** Implementa o **Repository Pattern**, abstraindo a lógica de acesso aos dados. A camada é exposta através de uma interface (`ITaskRepository`), permitindo que a implementação concreta possa ser facilmente substituída no futuro (ex: por um banco de dados) sem impactar o resto da aplicação.
* **Models (Camada de Domínio):** Contém as classes de domínio (`TaskItem`, `User`) e os DTOs. A organização em subpastas (`Task`, `User`) e o uso de DTOs específicos por ação (`CreateTaskDto`, `RegisterUserDto`, `LoginUserDto`) reforçam a clareza e a separação de responsabilidades.
* **Exceptions:** Pasta dedicada para o tratamento centralizado de exceções e para configurações de serviços, mantendo o código limpo e organizado.


### 🌳 Esqueleto do Sistema

A organização dos projetos e pastas segue a estrutura abaixo, refletindo a separação de responsabilidades e a implementação de autenticação:

```
CASE-DEV-JUNIOR/
│
├── 📁 toDo_CaseDev/                  # Projeto Principal da Solução
│   └── 📁 src/                       # Código-fonte do Back-end (.NET)
│       ├── 📁 Controllers/
│       │   ├── AuthController.cs
│       │   └── TaskController.cs
│       ├── 📁 Exceptions/
│       │   └── ApiErrorResponse.cs
│       ├── 📁 Models/
│       │   ├── 📁 Dtos/
│       │   │   ├── CreateTaskDto.cs
│       │   │   ├── TaskDto.cs
│       │   │   ├── UpdateTaskStatusDto.cs
│       │   │   ├── LoginUserDto.cs
│       │   │   ├── RegisterUserDto.cs
│       │   │   └── UserResponseDto.cs
│       │   ├── 📁 Task/
│       │   │   ├── TaskItem.cs
│       │   │   └── TaskStatus.cs
│       │   └── 📁 User/
│       │       └── User.cs
│       ├── 📁 Repositories/
│       │   ├── ITaskRepository.cs
│       │   └── TaskRepository.cs
│       ├── 📁 Services/
│       │   ├── IAuthService.cs
│       │   ├── ITaskService.cs
│       │   ├── AuthService.cs
│       │   ├── TaskService.cs
│       │   └── UserService.cs
│       └── 📁 Utils/
│
├── 📁 toDo_CaseDevFrontEnd/          # Código-fonte do Front-end (React)
│   └── 📁 src/
│
└── 📁 toDo_CaseDev.UnitTests/        # Projeto para Testes Unitários
```

## 🚀 Começando

Essas instruções permitirão que você obtenha uma cópia do projeto em operação na sua máquina local para fins de desenvolvimento e teste.

### 📋 Pré-requisitos

Para executar este projeto, você precisará ter os seguintes softwares instalados em sua máquina:

```
- .NET 8 SDK ou superior
- Node.js v20.x ou superior (inclui npm)
- Git para versionamento de código
```

### 🔧 Instalação

Siga o passo-a-passo abaixo para configurar o ambiente de desenvolvimento.

**1. Clone o repositório**

Primeiro, clone o repositório do GitHub para a sua máquina local.

```bash
git clone https://github.com/Ismaylla/case-dev-junior.git
cd case-dev-junior
```

**2. Configure e execute o Back-end (API em C#)**

Em um terminal, navegue até a pasta do projeto back-end e execute os seguintes comandos:

```bash
# Navegue até a pasta do código-fonte da API
cd toDo_CaseDev/src

# Restaure as dependências do .NET
dotnet restore

# Execute a API
dotnet run
```
A API estará em execução. O terminal indicará a porta, geralmente algo como `http://localhost:5161`. Você pode acessar `http://localhost:5161/swagger` para interagir com a API.

**3. Configure e execute o Front-end (React + Vite)**

1. Abra um **novo terminal**, mantendo o terminal do back-end em execução.
2. Acessar a pasta do projeto frontend:
```bash
cd toDo_CaseDev.FrontEnd
```

3. Instalar as dependências do projeto frontend:

```bash
npm install
```
4. Iniciar o servidor de desenvolvimento:

```bash
npm run dev
```
5. Acessar a aplicação

- O terminal exibirá uma mensagem como:

```bash
Local: http://localhost:5173/
```
A aplicação React estará disponível em `http://localhost:5173`. Ao abrir esta URL no seu navegador, você verá a interface consumindo os dados da sua API.

## ⚙️ Executando os testes

O projeto inclui uma suíte de testes unitários para garantir a qualidade e a corretude da lógica do back-end. Os testes foram criados para validar o comportamento de cada camada da aplicação (Repositories, Services e Controllers) de forma isolada.

### 🔧 Execução

Para executar todos os testes automatizados, siga os passos:

1.  Abra um terminal na raiz do projeto.
2.  Execute o comando para buildar o projeto:
    ```bash
    dotnet build
    ```
3.  Execute o comando para rodar os testes:
    ```bash
    dotnet test
    ```

O terminal exibirá o resultado de todos os testes, indicando se passaram ou falharam.

## 🛠️ Construído com

* **Back-end:**
    * [.NET 9](https://dotnet.microsoft.com/pt-br/download/dotnet/9.0) - Framework para o desenvolvimento da aplicação.
    * [ASP.NET Core](https://dotnet.microsoft.com/pt-br/apps/aspnet) - Para a construção da API RESTful.
* **Front-end:**
    * [React](https://react.dev/) - Biblioteca para a construção da interface de usuário.
    * [Vite](https://vitejs.dev/) - Ferramenta de build e servidor de desenvolvimento de alta performance.
* **Linguagens:**
    * [C#](https://learn.microsoft.com/pt-br/dotnet/csharp/)
    * [JavaScript](https://developer.mozilla.org/pt-BR/docs/Web/JavaScript) e [JSX](https://react.dev/learn/writing-markup-with-jsx)

## ✒️ Autores

* **Erick Gabriel** - [ErickGabriel7](https://github.com/ErickGabriel7)
* **Ismaylla Batista** - [Ismaylla](https://github.com/Ismaylla)
* **Lucas Henrique** - [Wolf-gangSE](https://github.com/Wolf-gangSE)
* **Ramon Silva** - [ramonsilva186](https://github.com/ramonsilva186)
* **Yann Leão** - [YannLeao](https://github.com/YannLeao)

## 📄 Referências

1. [Tutorial: Create a controller-based web API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&utm_source=chatgpt.com&tabs=visual-studio-code)
2. [Tailwind CSS Maps](https://preline.co/docs/maps.html)
