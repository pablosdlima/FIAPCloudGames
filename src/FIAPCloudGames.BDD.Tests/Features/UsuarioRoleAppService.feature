# language: pt-BR

Funcionalidade: Gerenciamento de Roles de Usuário
    Como um administrador do sistema
    Eu quero gerenciar as roles dos usuários
    Para controlar as permissões e acessos de cada usuário

Contexto:
    Dado que o serviço de usuario role está configurado

# ==================== ALTERAR ROLE DO USUÁRIO ====================

Cenário: Alterar role do usuário com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe uma role com ID 2
    E existe uma associação usuario-role com ID "1fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados para alterar a role:
        | Campo       | Valor         |
        | TipoUsuario | Administrador |
    Quando eu alterar a role da associação "1fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a alteração da role deve ser bem-sucedida


Cenário: Falha ao alterar role de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados para alterar a role:
        | Campo        | Valor                                      |
        | TipoUsuario  | Administrador                              |
    Quando eu alterar a role da associação "1fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

Cenário: Falha ao alterar para role inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E não existe uma role com ID 2
    E existe uma associação usuario-role com ID "1fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados para alterar a role:
        | Campo        | Valor                                      |
        | TipoUsuario  | Administrador                              |
    Quando eu alterar a role da associação "1fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Role com ID 2 não encontrada"

Cenário: Falha ao alterar associação usuario-role inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe uma role com ID 1
    E não existe uma associação usuario-role com ID "1fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados para alterar a role:
        | Campo        | Valor                                      |
        | TipoUsuario  | Usuario                                    |
    Quando eu alterar a role da associação "1fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Associação Usuário-Role com ID 1fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrada"

Cenário: Falha ao alterar role quando serviço retorna erro
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe uma role com ID 2
    E existe uma associação usuario-role com ID "1fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o serviço de usuario role não consegue atualizar
    E tenho os dados para alterar a role:
        | Campo       | Valor         |
        | TipoUsuario | Administrador |
    Quando eu alterar a role da associação "1fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de domínio
    E a mensagem deve ser "Não foi possível alterar a role do usuário. Verifique os dados fornecidos."


# ==================== LISTAR ROLES DO USUÁRIO ====================

Cenário: Listar roles de um usuário com roles associadas
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o usuário possui 2 roles associadas
    Quando eu listar as roles do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a listagem de roles do usuário deve retornar 2 roles
    E todas as roles devem pertencer ao usuário

Cenário: Listar roles de um usuário sem roles associadas
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o usuário não possui roles associadas
    Quando eu listar as roles do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a listagem de roles do usuário deve retornar 0 roles

Cenário: Falha ao listar roles de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu listar as roles do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"
