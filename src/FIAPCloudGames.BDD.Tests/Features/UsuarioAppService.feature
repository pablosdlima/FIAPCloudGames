# language: pt-BR

Funcionalidade: Gerenciamento de Usuários
    Como um administrador do sistema
    Eu quero gerenciar os usuários
    Para controlar acessos e informações

Contexto:
    Dado que o serviço de usuários está configurado

# ==================== CADASTRAR USUÁRIO ====================

Cenário: Cadastrar usuário com sucesso
    Dado que tenho os dados do usuário:
        | Campo          | Valor                   |
        | Nome           | marcio.silva            |
        | Senha          | Senha@123               |
        | Celular        | (11) 98765-4321         |
        | Email          | marcio@email.com        |
        | NomeCompleto   | Marcio Silva            |
        | DataNascimento | 1990-05-15              |
        | Pais           | Brasil                  |
        | AvatarUrl      | https://avatar.url      |
    Quando eu cadastrar o usuário
    Então o usuário deve ser cadastrado com sucesso
    E o usuário retornado deve ter um ID válido

Cenário: Falha ao cadastrar usuário quando serviço retorna null
    Dado que o serviço de usuário não consegue cadastrar
    E tenho os dados do usuário:
        | Campo          | Valor                   |
        | Nome           | usuario.invalido        |
        | Senha          | Senha@123               |
        | Celular        | (11) 98765-4321         |
        | Email          | invalido@email.com      |
        | NomeCompleto   | Usuario Invalido        |
        | DataNascimento | 1990-05-15              |
        | Pais           | Brasil                  |
        | AvatarUrl      | https://avatar.url      |
    Quando eu cadastrar o usuário
    Então deve lançar uma exceção de domínio
    E a mensagem deve ser "Não foi possível cadastrar o usuário. Verifique os dados fornecidos."

# ==================== BUSCAR USUÁRIO POR ID ====================

Cenário: Buscar usuário por ID com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu buscar o usuário por ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o usuário deve ser retornado com sucesso
    E o usuário deve ter o ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"

Cenário: Buscar usuário com perfil e roles
    Dado que existe um usuário completo com perfil e roles com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu buscar o usuário por ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o usuário deve ter perfil preenchido
    E o usuário deve ter roles associadas

Cenário: Falha ao buscar usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu buscar o usuário por ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve ser "Usuário não encontrado."

# ==================== ALTERAR SENHA ====================

Cenário: Alterar senha com sucesso
    Dado que existe um usuário para alterar senha com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu alterar a senha do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6" para "NovaSenha@456"
    Então a alteração de senha deve ser bem-sucedida

Cenário: Falha ao alterar senha
    Dado que a alteração de senha vai falhar
    Quando eu alterar a senha do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6" para "SenhaInvalida"
    Então a alteração de senha deve falhar

# ==================== ALTERAR STATUS ====================

Cenário: Alterar status do usuário para inativo
    Dado que existe um usuário ativo para alteração de status com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu alterar o status do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o status deve ser alterado com sucesso
    E o status retornado deve ser "Inativo"

Cenário: Alterar status do usuário para ativo
    Dado que existe um usuário inativo para alteração de status com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu alterar o status do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o status deve ser alterado com sucesso
    E o status retornado deve ser "Ativo"