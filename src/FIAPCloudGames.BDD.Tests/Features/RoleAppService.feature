# language: pt-BR

Funcionalidade: Gerenciamento de Roles
    Como um administrador do sistema
    Eu quero gerenciar as roles do sistema
    Para controlar as permissões dos usuários

Contexto:
    Dado que o serviço de roles está configurado

# ==================== CADASTRAR ROLE ====================

Cenário: Cadastrar role com sucesso
    Dado que tenho os dados da role:
        | Campo       | Valor                                      |
        | Id          | 1                                          |
        | RoleName    | Administrador                              |
        | Description | Role com acesso total ao sistema          |
    Quando eu cadastrar a role
    Então a role deve ser cadastrada com sucesso
    E a role retornada deve ter um ID válido
    E a role deve conter os dados informados

Cenário: Falha ao cadastrar role quando serviço retorna null
    Dado que o serviço de role não consegue cadastrar
    E tenho os dados da role:
        | Campo       | Valor                                      |
        | Id          | 1                                          |
        | RoleName    | Gerente                                    |
        | Description | Role de gerência                           |
    Quando eu cadastrar a role
    Então deve lançar uma exceção de domínio
    E a mensagem deve ser "Não foi possível cadastrar a role. Verifique os dados fornecidos."

# ==================== LISTAR ROLES ====================

Cenário: Listar todas as roles
    Dado que existem 3 roles cadastradas
    Quando eu listar as roles
    Então a listagem deve retornar 3 roles

Cenário: Listar roles quando não há roles cadastradas
    Dado que não existem roles cadastradas
    Quando eu listar as roles
    Então a listagem deve retornar 0 roles

Cenário: Listar roles e verificar dados
    Dado que existem roles cadastradas com diferentes nomes
    Quando eu listar as roles
    Então a listagem deve conter roles com nomes distintos

# ==================== ATUALIZAR ROLE ====================

Cenário: Atualizar role com sucesso
    Dado que existe uma role com ID 1
    E tenho os dados atualizados da role:
        | Campo       | Valor                                      |
        | RoleName    | Super Administrador                        |
        | Description | Role com acesso completo ao sistema       |
    Quando eu atualizar a role
    Então a role deve ser atualizada com sucesso
    E a role retornada deve ter os novos dados

Cenário: Falha ao atualizar role inexistente
    Dado que não existe uma role com ID 999
    E tenho os dados atualizados da role:
        | Campo       | Valor                                      |
        | RoleName    | Role Inexistente                           |
        | Description | Esta role não existe                       |
    Quando eu atualizar a role
    Então a atualização da role deve falhar
    E o resultado da role deve indicar falha
