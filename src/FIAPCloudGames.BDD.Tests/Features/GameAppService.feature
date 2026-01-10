# language: pt-BR

Funcionalidade: Gerenciamento de Games
    Como um administrador do sistema
    Eu quero gerenciar os games cadastrados
    Para manter o catálogo atualizado

Contexto:
    Dado que o serviço de games está configurado

# ==================== CADASTRAR GAME ====================

Cenário: Cadastrar game com sucesso
    Dado que tenho os dados do game:
        | Campo          | Valor                              |
        | Nome           | The Last of Us Part II             |
        | Descricao      | Jogo de ação e aventura            |
        | Genero         | Ação/Aventura                      |
        | Desenvolvedor  | Naughty Dog                        |
        | Preco          | 299.90                             |
        | DataRelease    | 2020-06-19                         |
    Quando eu cadastrar o game
    Então o game deve ser cadastrado com sucesso
    E o game retornado deve ter um ID válido
    E o game deve conter os dados informados

# ==================== BUSCAR GAME POR ID ====================

Cenário: Buscar game por ID com sucesso
    Dado que existe um game com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu buscar o game por ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o game deve ser retornado com sucesso
    E o game deve ter o ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"

Cenário: Falha ao buscar game inexistente
    Dado que não existe um game com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu buscar o game por ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve ser "Game não encontrado."

# ==================== LISTAR GAMES PAGINADO ====================

Cenário: Listar games paginado com sucesso
    Dado que existem 15 games cadastrados
    Quando eu listar os games com página 1 e tamanho 10
    Então a listagem deve retornar 10 games
    E deve indicar que existe próxima página
    E deve indicar que não existe página anterior
    E o total de páginas deve ser 2

Cenário: Listar games da segunda página
    Dado que existem 15 games cadastrados
    Quando eu listar os games com página 2 e tamanho 10
    Então a listagem deve retornar 5 games
    E deve indicar que não existe próxima página
    E deve indicar que existe página anterior

Cenário: Listar games sem resultados
    Dado que não existem games cadastrados
    Quando eu listar os games com página 1 e tamanho 10
    Então a listagem deve retornar 0 games
    E o total de páginas deve ser 0

Cenário: Listar games com filtro por nome
    Dado que existem games cadastrados com diferentes nomes
    Quando eu listar os games com filtro "Last of Us" na página 1 e tamanho 10
    Então a listagem deve retornar apenas games filtrados

Cenário: Listar games com filtro por gênero
    Dado que existem games cadastrados com diferentes gêneros
    Quando eu listar os games com gênero "Ação" na página 1 e tamanho 10
    Então a listagem deve retornar apenas games do gênero "Ação"

# ==================== ATUALIZAR GAME ====================

Cenário: Atualizar game com sucesso
    Dado que existe um game com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados do game:
        | Campo          | Valor                              |
        | Nome           | The Last of Us Part II - Remastered|
        | Descricao      | Versão remasterizada               |
        | Genero         | Ação/Aventura                      |
        | Desenvolvedor  | Naughty Dog                        |
        | Preco          | 349.90                             |
        | DataRelease    | 2023-01-15                         |
    Quando eu atualizar o game "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o game deve ser atualizado com sucesso
    E o game retornado deve ter os novos dados

Cenário: Falha ao atualizar game inexistente
    Dado que não existe um game com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados do game:
        | Campo          | Valor                              |
        | Nome           | Game Inexistente                   |
        | Descricao      | Descrição                          |
        | Genero         | RPG                                |
        | Desenvolvedor  | Developer                          |
        | Preco          | 199.90                             |
        | DataRelease    | 2024-01-01                         |
    Quando eu atualizar o game "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a atualização do game deve falhar
    E o resultado do game deve indicar falha
