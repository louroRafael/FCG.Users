# 🎮 FIAP Cloud Games – Users API

API responsável pelo **gerenciamento de usuários e autenticação** no ecossistema **FIAP Cloud Games**, centralizando cadastro, login, emissão de tokens JWT e controle de acesso aos demais microsserviços de forma segura e padronizada.

---

## 🚀 Tech Challenge – FIAP (Fase 3)

Este projeto faz parte do **Tech Challenge** do curso de pós-graduação em **Arquitetura de Sistemas .NET**, aplicando conceitos de **microsserviços**, **segurança**, **DDD** e **autenticação distribuída**.

---

## 🧩 Visão Geral da Solução

A **Users API** é um microsserviço independente, responsável exclusivamente pelo domínio de usuários e identidade.

Ela atua como:
- Provedora de autenticação (JWT)
- Central de cadastro e gerenciamento de usuários
- Base para autorização e controle de acesso entre APIs

---

## 🏗️ Arquitetura do Microsserviço

O projeto está organizado em camadas (DDD), contendo os seguintes projetos:

- **FCG.Users.API** — Expõe endpoints de autenticação e gerenciamento de usuários.
- **FCG.Users.Service** — Implementa regras de negócio, validações e casos de uso.
- **FCG.Users.Domain** — Define entidades, enums e regras centrais do domínio de usuários.
- **FCG.Users.Infrastructure** — Implementa persistência, hashing de senha e integrações externas.

---

## 🔄 Fluxo Principal

### 📝 Cadastro de Usuário

1 → O cliente envia os dados de cadastro  
2 → A Users API valida as informações  
3 → A senha é criptografada (hash)  
4 → O usuário é persistido no banco de dados  
5 → O usuário fica apto a autenticar no sistema  

---

### 🔐 Autenticação (Login)

1 → O cliente envia credenciais (email e senha)  
2 → A Users API valida as credenciais  
3 → Um **JWT** é gerado contendo as claims do usuário  
4 → O token é retornado ao cliente  
5 → O token é utilizado para acessar as demais APIs via APIM  

---

## 📌 Responsabilidades da Users API

- 👤 Cadastro e gerenciamento de usuários
- 🔐 Autenticação e geração de JWT
- 🧾 Validação de credenciais
- 🛡️ Emissão de tokens com roles e permissões
- 🔑 Base para autorização dos demais microsserviços

---

## 🔐 Segurança

- Autenticação baseada em **JWT**
- Tokens assinados com chave segura
- Controle de expiração e audiência
- Integração com **Azure API Management (APIM)**

---

## 🛠️ Tecnologias Utilizadas

- ⚙️ **Runtime** — [.NET 8 (C#)](https://dotnet.microsoft.com/download/dotnet/8.0)
- 🔐 **Segurança** — [JWT Bearer Authentication](https://jwt.io/)
- 🐘 **Persistência** — [Entity Framework Core](https://learn.microsoft.com/ef/) e [PostgreSQL](https://www.postgresql.org)
- 🧱 **Validação** — [FluentValidation](https://fluentvalidation.net/)
- 🐳 **Conteinerização** — [Docker](https://www.docker.com)

---

## 🐳 Execução via Docker (Local)

```bash
# Build da imagem
docker build -t fcg-users-api:latest .

# Executar container
docker run -d --name fcg-users-local -p 8080:8080 \
-e ConnectionStrings__FCG="Sua-String-Conexao" \
-e Jwt__Key="Seu-Segredo-JWT" \
-e Jwt__Issuer="http://localhost" \
-e Jwt__Audience="fcg-clients" \
fcg-users-api:latest

