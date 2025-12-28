# 🎮 FIAP Cloud Games - Users API

---

## ℹ️ Sobre

Projeto desenvolvido como parte do **Tech Challenge** da pós-graduação em Arquitetura .NET pela FIAP.

`FCG.Users` é o microserviço responsável pela autenticação, autorização e gerenciamento de usuários

---

## 🧭 Objetivo

Refatorar a aplicação monolítica das fase anteriores do projeto em uma arquitetura de microsserviços. O projeto da fase anterior encontra-se no repositorio:

```
https://github.com/louroRafael/fiap-cloud-games

```
---

## 🏗️ Arquitetura do Microsserviço
O projeto está organizado em camadas (DDD) contendo os seguintes projetos:
- **FCG.Users.API** — Expõe endpoints e recebe requisições do cliente.
- **FCG.Users.Service** — Executa regras de negócio e casos de uso.
- **FCG.Users.Domain** — Define o modelo e as regras centrais do negócio.
- **FCG.Users.Infrastructure** — Implementa persistência e integrações externas.

---

## 🛠️ Tecnologias Utilizadas
- **Runtime** — [.NET 8 (C#)](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Segurança** — [JWT Bearer Authentication](https://jwt.io/)
- **Persistência** — [Entity Framework Core](https://learn.microsoft.com/ef/) e [PostgreSQL](https://www.postgresql.org)
- **Validação** — [FluentValidation](https://fluentvalidation.net/)
- **Conteinerização** — [Docker](https://www.docker.com)

---

## 🐳 Execução via Docker (Local)
```bash
#Build da imagem
docker build -t fcg-users-api:latest .

#Executar container
docker run -d --name fcg-users-local -p 8080:8080 \
-e ConnectionStrings__FCG="Sua-String-Conexao" \
-e Jwt__Key="Seu-Segredo-JWT" \
fcg-users-api:latest
```
