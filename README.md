# Teste Técnico Mindtech - Newsletter

**Desenvolvido por:** Rafael Cavalcante de Almeida

---

## Descrição

Projeto de um sistema de inscrição em uma newsletter fictícia.

* **Front-end:** HTML, CSS, JavaScript
* **Back-end:** ASP.NET Core Web API
* **Dados:** Entity Framework Core + SQLite(banco local)

---

## Estrutura do Projeto

```
mindtechNewsletter/
│
├─ mindtechNewsletter.Client/                   # Projeto Front-end
│   ├─ src/                                     # Componentes, serviços, assets
│   ├─ static/                                  # Imagens, fontes
│   ├─ tests/                                   # Testes unitários do script.js
│   ├─ babel.config.js
│   ├─ jest.setup.js
│   ├─ package.json
│   └─ package-lock.json
│
├─ mindtechNewsletter.Server/                  # Projeto ASP.NET Core
│   ├─ Controllers/                            # Endpoints da API
│   ├─ Data/                                   # DbContext e Migrations
│   ├─ DTOs/                                   # Data Transfer Objects
│   ├─ Mapping/                                # Mapeamento entre modelos e DTOs
│   ├─ Migrations/                             # Alterações do banco de dados
│   ├─ Models/                                 # Estrutura de dados
│   ├─ Repositories/                           # Acesso ao banco
│   ├─ Responses/                              # Modelo de resposta da API
│   ├─ Services/                               # Lógica de negócios
│   ├─ Program.cs                              # Inicialização da aplicação
│   ├─ appsettings.json                        # Configurações do projeto
│   ├─ mindtechNewsletter.Server.Tests.csproj
│
mindtechNewsletter.Server.Tests/               # Projeto de testes unitários
│
├─ Services/                       
│   ├─ SubscriberServiceTests.cs               # Testes unitários do SubscriberService.cs
│
├─ mindtechNewsletter.Server.Tests.csproj
│
├─ .gitignore      # Arquivos ignorados pelo Git
└─ README.md       # Documentação
```

---

## Como Rodar

### Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) - Backend
- [Node.js 20 LTS ou superior](https://nodejs.org/en/download/) - Frontend

---

### 1. Rodando o Backend (ASP.NET Core Web API)

Dentro do diretório raíz, abra um terminal e rode: 

```bash
cd mindtechNewsletter.Server
dotnet restore
dotnet run --urls "https://localhost:7035"
```

> A API ficará disponível em: `https://localhost:7035` (por padrão)


> O swagger estará rodando em: `https://localhost:7035/swagger/index.html` - contendo os 2 endpoints da aplicação 
e 1 endpoint para ver as inserções dentro do banco (ou utilize o endpoint direto em `https://localhost:7035/api/Subscriber/all`)

![Endpoints Swagger](https://imgur.com/a/DI8x95R)

---

### 2. Rodando o Frontend

Dentro do diretório raíz, abra outro terminal e rode: 

```bash
cd mindtechNewsletter.Client
npm install -g http-server
http-server .
```

> O frontend ficará disponível em: `http://localhost:8080/index.html` (por padrão)

---

### 3. Testando o sistema

1. **Cadastro de email:** insira um email válido no frontend.
2. **Descadastro:** use a opção de remover email.
3. **Emails duplicados:** o sistema avisará que o email já está cadastrado.
4. Todas as operações refletem imediatamente no arquivo newsletter.db. Visualização dos registros em `https://localhost:7035/api/Subscriber/all` || https://localhost:7035/swagger/index.html

---

### Testes Unitários

O projeto possui cobertura de testes tanto no **backend** quanto no **frontend**, garantindo que a lógica de negócios funcione corretamente.

---

### Backend (ASP.NET Core)

- Testes para os serviços do backend (`SubscriberService`) usando **xUnit**, **Moq** e **Fluent Assertions**.  
- Cobrem cenários como:  
  - Cadastro de emails válidos  
  - Soft delete (descadastro de emails)  
  - Tratamento de emails duplicados  

**Como rodar os testes:**

```bash
cd mindtechNewsletter.Server.Tests
dotnet restore
dotnet test
```

### Frontend

O frontend possui **testes unitários** usando **Jest** para o script.js.

**Como rodar os testes:**

```bash
cd mindtechNewsletter.Client
npm install --save-dev jest @babel/core @babel/preset-env babel-jest @testing-library/dom @testing-library/jest-dom @testing-library/user-event
npm test
```
