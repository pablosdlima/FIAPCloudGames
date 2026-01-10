# language: pt-BR

Funcionalidade: Gerenciamento de Perfil de Usuário
    Como um usuário do sistema
    Eu quero gerenciar meu perfil
    Para manter minhas informações pessoais atualizadas

Contexto:
    Dado que o serviço de perfil está configurado

# ==================== BUSCAR PERFIL ====================

Cenário: Buscar perfil de usuário com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o usuário possui um perfil cadastrado
    Quando eu buscar o perfil do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o perfil deve ser retornado com sucesso
    E o perfil deve conter as informações do usuário

Cenário: Falha ao buscar perfil de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu buscar o perfil do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

Cenário: Falha ao buscar perfil inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o usuário não possui perfil cadastrado
    Quando eu buscar o perfil do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Perfil para o usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

# ==================== CADASTRAR PERFIL ====================

Cenário: Cadastrar perfil com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados do perfil:
        | Campo          | Valor                   |
        | NomeCompleto   | Marcio Silva Santos     |
        | DataNascimento | 1990-05-15              |
        | Pais           | Brasil                  |
        | AvatarUrl      | https://avatar.url.jpg  |
    Quando eu cadastrar o perfil para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o perfil deve ser cadastrado com sucesso
    E o perfil retornado deve ter um ID válido
    E o perfil deve conter os dados informados

Cenário: Falha ao cadastrar perfil para usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados do perfil:
        | Campo          | Valor                   |
        | NomeCompleto   | Marcio Silva Santos     |
        | DataNascimento | 1990-05-15              |
        | Pais           | Brasil                  |
        | AvatarUrl      | https://avatar.url.jpg  |
    Quando eu cadastrar o perfil para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado para cadastrar o perfil"

Cenário: Falha ao cadastrar perfil quando serviço retorna null
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o serviço de perfil não consegue cadastrar
    E tenho os dados do perfil:
        | Campo          | Valor                   |
        | NomeCompleto   | Marcio Silva Santos     |
        | DataNascimento | 1990-05-15              |
        | Pais           | Brasil                  |
        | AvatarUrl      | https://avatar.url.jpg  |
    Quando eu cadastrar o perfil para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de domínio
    E a mensagem deve ser "Não foi possível cadastrar o perfil. Verifique os dados fornecidos ou se o usuário já possui um perfil."

# ==================== ATUALIZAR PERFIL ====================

Cenário: Atualizar perfil com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um perfil com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados do perfil:
        | Campo          | Valor                        |
        | NomeCompleto   | Marcio Silva Santos Junior   |
        | DataNascimento | 1990-05-15                   |
        | Pais           | Portugal                     |
        | AvatarUrl      | https://novo-avatar.url.jpg  |
    Quando eu atualizar o perfil "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o perfil deve ser atualizado com sucesso
    E o perfil retornado deve ter os novos dados

Cenário: Falha ao atualizar perfil de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados do perfil:
        | Campo          | Valor                        |
        | NomeCompleto   | Marcio Silva Santos Junior   |
        | DataNascimento | 1990-05-15                   |
        | Pais           | Portugal                     |
        | AvatarUrl      | https://novo-avatar.url.jpg  |
    Quando eu atualizar o perfil "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado para atualizar o perfil"

Cenário: Falha ao atualizar perfil inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o perfil não existe
    E tenho os dados atualizados do perfil:
        | Campo          | Valor                        |
        | NomeCompleto   | Marcio Silva Santos Junior   |
        | DataNascimento | 1990-05-15                   |
        | Pais           | Portugal                     |
        | AvatarUrl      | https://novo-avatar.url.jpg  |
    Quando eu atualizar o perfil "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a atualização do perfil deve falhar

# ==================== DELETAR PERFIL ====================

Cenário: Deletar perfil com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um perfil com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu deletar o perfil "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o perfil deve ser deletado com sucesso

Cenário: Falha ao deletar perfil de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu deletar o perfil "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado para deletar o perfil"

Cenário: Falha ao deletar perfil inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o perfil não pode ser deletado
    Quando eu deletar o perfil "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a exclusão do perfil deve falhar
