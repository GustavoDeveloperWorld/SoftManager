# SoftManager
# SoftManager - Gestão de Tarefas

Este é um sistema de **gestão de tarefas** para usuários, permitindo o cadastro de tarefas, agendamentos e acompanhamento do status das atividades. O projeto utiliza o ASP.NET Core MVC, com o Bootstrap para a interface do usuário.

## Tecnologias Utilizadas

- **ASP.NET Core MVC**: Framework para desenvolvimento da aplicação.
- **Bootstrap 4**: Framework CSS para o layout e responsividade.
- **Entity Framework Core**: Para o gerenciamento do banco de dados.
- **SQL Server**: Banco de dados utilizado para armazenar as informações.
- **Identity**: Para o gerenciamento de autenticação e autorização de usuários.
- **Email Services**: Para notificações e lembretes via e-mail.

## Funcionalidades

- Cadastro de tarefas.
- Atribuição de tarefas para usuários específicos.
- Exibição das tarefas com filtros (pendentes, concluídas, etc.).
- Agendamento de tarefas com data e hora.
- Sistema de autenticação e autorização de usuários.
- Envio de notificações por e-mail.

## Instalação

Para rodar este projeto localmente, siga os passos abaixo:

### Pré-requisitos

- **.NET Core 6.0** ou superior instalado: [Baixar .NET Core](https://dotnet.microsoft.com/download).
- **SQL Server** ou **SQL Server Express** para o banco de dados.
- **Visual Studio** ou **Visual Studio Code** para editar o código.

### Passo 1: Clone o Repositório

Clone o repositório do projeto para sua máquina local:

```bash
git clone https://github.com/SEU_USUARIO/SoftManager.git
 - database update
-configure as credenciais de SMTP no arquivo appsettings.json