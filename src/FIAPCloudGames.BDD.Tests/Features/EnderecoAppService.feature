# language: pt-BR

Funcionalidade: Gerenciamento de Endereços
    Como um usuário do sistema
    Eu quero gerenciar meus endereços
    Para manter minhas informações de endereço atualizadas

Contexto:
    Dado que o serviço de endereços está configurado

# ==================== LISTAR ENDEREÇOS ====================

Cenário: Listar endereços de um usuário existente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o usuário possui 2 endereços cadastrados
    Quando eu listar os endereços do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a listagem de endereços deve retornar 2 endereços
    E todos os endereços devem pertencer ao usuário

Cenário: Listar endereços de um usuário sem endereços
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o usuário não possui endereços cadastrados
    Quando eu listar os endereços do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a listagem de endereços deve retornar 0 endereços

Cenário: Falha ao listar endereços de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu listar os endereços do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

# ==================== CADASTRAR ENDEREÇO ====================

Cenário: Cadastrar endereço com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados do endereço:
        | Campo       | Valor                    |
        | Rua         | Rua das Flores           |
        | Numero      | 123                      |
        | Complemento | Apto 45                  |
        | Bairro      | Centro                   |
        | Cidade      | São Paulo                |
        | Estado      | SP                       |
        | Cep         | 01310-100                |
    Quando eu cadastrar o endereço para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o endereço deve ser cadastrado com sucesso
    E o endereço retornado deve ter um ID válido
    E o endereço deve conter os dados informados

Cenário: Cadastrar endereço sem complemento
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados do endereço sem complemento:
        | Campo  | Valor              |
        | Rua    | Avenida Paulista   |
        | Numero | 1000               |
        | Bairro | Bela Vista         |
        | Cidade | São Paulo          |
        | Estado | SP                 |
        | Cep    | 01310-100          |
    Quando eu cadastrar o endereço para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o endereço deve ser cadastrado com sucesso
    E o complemento deve ser nulo ou vazio

Cenário: Falha ao cadastrar endereço para usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados do endereço:
        | Campo       | Valor                    |
        | Rua         | Rua das Flores           |
        | Numero      | 123                      |
        | Complemento | Apto 45                  |
        | Bairro      | Centro                   |
        | Cidade      | São Paulo                |
        | Estado      | SP                       |
        | Cep         | 01310-100                |
    Quando eu cadastrar o endereço para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado para cadastrar o endereço"

Cenário: Falha ao cadastrar endereço quando serviço retorna null
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o serviço de endereço não consegue cadastrar
    E tenho os dados do endereço:
        | Campo       | Valor                    |
        | Rua         | Rua das Flores           |
        | Numero      | 123                      |
        | Complemento | Apto 45                  |
        | Bairro      | Centro                   |
        | Cidade      | São Paulo                |
        | Estado      | SP                       |
        | Cep         | 01310-100                |
    Quando eu cadastrar o endereço para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de domínio
    E a mensagem deve ser "Não foi possível cadastrar o endereço. Verifique os dados fornecidos."

# ==================== ATUALIZAR ENDEREÇO ====================

Cenário: Atualizar endereço com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um endereço com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados do endereço:
        | Campo       | Valor                    |
        | Rua         | Rua Nova                 |
        | Numero      | 456                      |
        | Complemento | Casa                     |
        | Bairro      | Jardim Paulista          |
        | Cidade      | São Paulo                |
        | Estado      | SP                       |
        | Cep         | 01405-000                |
    Quando eu atualizar o endereço "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o endereço deve ser atualizado com sucesso
    E o endereço retornado deve ter os novos dados

Cenário: Falha ao atualizar endereço de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados do endereço:
        | Campo       | Valor                    |
        | Rua         | Rua Nova                 |
        | Numero      | 456                      |
        | Complemento | Casa                     |
        | Bairro      | Jardim Paulista          |
        | Cidade      | São Paulo                |
        | Estado      | SP                       |
        | Cep         | 01405-000                |
    Quando eu atualizar o endereço "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado para atualizar o endereço"

Cenário: Falha ao atualizar endereço inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o endereço "2fa85f64-5717-4562-b3fc-2c963f66afa6" não existe
    E tenho os dados atualizados do endereço:
        | Campo       | Valor                    |
        | Rua         | Rua Nova                 |
        | Numero      | 456                      |
        | Complemento | Casa                     |
        | Bairro      | Jardim Paulista          |
        | Cidade      | São Paulo                |
        | Estado      | SP                       |
        | Cep         | 01405-000                |
    Quando eu atualizar o endereço "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a atualização do endereço deve falhar
    E o resultado do endereço deve indicar falha

# ==================== DELETAR ENDEREÇO ====================

Cenário: Deletar endereço com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um endereço com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu deletar o endereço "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o endereço deve ser deletado com sucesso

Cenário: Falha ao deletar endereço de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu deletar o endereço "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado para deletar o endereço"

Cenário: Falha ao deletar endereço inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o endereço "2fa85f64-5717-4562-b3fc-2c963f66afa6" não pode ser deletado
    Quando eu deletar o endereço "2fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a exclusão do endereço deve falhar
