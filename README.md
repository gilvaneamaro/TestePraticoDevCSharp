# TestePraticoDevCSharp
Teste Técnico — Desenvolvedor C# Pleno (Windows Forms + PostgreSQL)


Este projeto foi desenvolvido como parte de um **teste técnico para Desenvolvedor C# Pleno**, com o objetivo de demonstrar domínio de **C#**, **Windows Forms**, **PostgreSQL**, **arquitetura em camadas**, **DDD**, **injeção de dependências** e **boas práticas de desenvolvimento**.

## Tecnologias Utilizadas
- **C# 7.3**
- **.NET Framework**
- **Windows Forms**
- **PostgreSQL 15**
- **Npgsql**
- **Docker / Docker Compose**
- **ADO.NET**
- **Git**

## Arquitetura Adotada

O projeto foi estruturado seguindo princípios de **DDD (Domain-Driven Design)** e **separação de responsabilidades**, organizado nas seguintes camadas:
- UI (Windows Forms)
- Application / Services
- Domain (Entidades e Value Objects)
- Infrastructure (Acesso a dados, Repositórios, UnitOfWork)

## Principais decisões arquiteturais:

-Entidades de domínio com regras de negócio encapsuladas
- Uso de Value Objects (ex.: E-mail)
- Uso de interfaces para repositórios e serviços
- Injeção de dependências nativa do .NET
- Acesso a dados via ADO.NET + Npgsql
- Unit of Work para controle de transações

## Banco de Dados
- SGBD: PostgreSQL 15
- Executado via Docker
- O script SQL completo para criação do banco de dados está disponível em: TestePraticoDevCSharp/docker/postgres


## Subindo o Banco com Docker
Pré-requisitos: 
- Docker
- Docker Compose
Abra o terminal, acesse o diretório do projeto e execute o comando docker-compose para subir o container com a imagem do banco de dados:
```
cd TestePraticoDevCSharp/
docker-compose up -d
```
O banco será inicializado automaticamente com o script SQL.

## Utilizando Banco local
- Script 01-create-database.sql para criação das tabelas disponível em:
```
TestePraticoDevCSharp\docker\postgres
```
- O banco deve estar configurado de acordo com a string connection:

```
<connectionStrings>
	<add name="PostgresConnection" connectionString="Host=localhost;Port=5432;Database=vendasdb;Username=user;Password=pass;" providerName="Npgsql" />
</connectionStrings>
```


```
Host=localhost;
Port=5432;
Database=vendasdb;
Username=user;
Password=pass;
```

## Funcionalidades Implementadas
### Cadastro de Clientes

- Nome, E-mail, Telefone
- Validação de e-mail duplicado
- Cadastro, edição e remoção
### Cadastro de Produtos
- Nome, Descrição, Preço, Estoque
- Validação de preço > 0
- Não permite estoque negativo
- Cadastro, edição e remoção

### Registro de Venda
- Seleção de cliente
- Seleção de produtos e quantidades
- Cálculo automático do valor total
- Validação de estoque
- Registro de data/hora da venda
- Transação (Unit of Work)

## Relatório de Vendas (Não implementado)


## Executando o Projeto
- Subir o banco via Docker
- Abrir a solução no Visual Studio
- Restaurar pacotes NuGet
- Compilar o projeto
- Executar a aplicação
