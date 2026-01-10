# language: pt-BR

Funcionalidade: Gerenciamento de Contatos
    Como um usuário do sistema
    Eu quero gerenciar meus contatos
    Para manter minhas informações de contato atualizadas

Contexto:
    Dado que o serviço de contatos está configurado

# ==================== LISTAR CONTATOS ====================

Cenário: Listar contatos de um usuário existente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o usuário possui 2 contatos cadastrados
    Quando eu listar os contatos do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a listagem deve retornar 2 contatos
    E todos os contatos devem pertencer ao usuário

Cenário: Listar contatos de um usuário sem contatos
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o usuário não possui contatos cadastrados
    Quando eu listar os contatos do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a listagem deve retornar 0 contatos

Cenário: Falha ao listar contatos de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu listar os contatos do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

# ==================== CADASTRAR CONTATO ====================

Cenário: Cadastrar contato com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados do contato:
        | Campo    | Valor                  |
        | Celular  | (11) 98765-4321       |
        | Email    | marcio@email.com      |
    Quando eu cadastrar o contato para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o contato deve ser cadastrado com sucesso
    E o contato retornado deve ter um ID válido
    E o contato deve conter os dados informados

Cenário: Falha ao cadastrar contato para usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados do contato:
        | Campo    | Valor                  |
        | Celular  | (11) 98765-4321       |
        | Email    | marcio@email.com      |
    Quando eu cadastrar o contato para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado para cadastrar o contato"

Cenário: Falha ao cadastrar contato quando serviço retorna null
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o serviço de contato não consegue cadastrar
    E tenho os dados do contato:
        | Campo    | Valor                  |
        | Celular  | (11) 98765-4321       |
        | Email    | marcio@email.com      |
    Quando eu cadastrar o contato para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de domínio
    E a mensagem deve ser "Não foi possível cadastrar o contato. Verifique os dados fornecidos."

# ==================== ATUALIZAR CONTATO ====================

Cenário: Atualizar contato com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um contato com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados do contato:
        | Campo    | Valor                     |
        | Celular  | (11) 91234-5678          |
        | Email    | marcio.novo@email.com    |
    Quando eu atualizar o contato "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o contato deve ser atualizado com sucesso
    E o contato retornado deve ter os novos dados

Cenário: Falha ao atualizar contato de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados do contato:
        | Campo    | Valor                     |
        | Celular  | (11) 91234-5678          |
        | Email    | marcio.novo@email.com    |
    Quando eu atualizar o contato "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado para atualizar o contato"

Cenário: Falha ao atualizar contato inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o contato "2fa85f64-5717-4562-b3fc-2c963f66afa6" não existe
    E tenho os dados atualizados do contato:
        | Campo    | Valor                     |
        | Celular  | (11) 91234-5678          |
        | Email    | marcio.novo@email.com    |
    Quando eu atualizar o contato "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a atualização deve falhar
    E o resultado deve indicar falha

# ==================== DELETAR CONTATO ====================

Cenário: Deletar contato com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um contato com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu deletar o contato "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o contato deve ser deletado com sucesso

Cenário: Falha ao deletar contato de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu deletar o contato "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado para deletar o contato"

Cenário: Falha ao deletar contato inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o contato "2fa85f64-5717-4562-b3fc-2c963f66afa6" não pode ser deletado
    Quando eu deletar o contato "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a exclusão deve falhar
