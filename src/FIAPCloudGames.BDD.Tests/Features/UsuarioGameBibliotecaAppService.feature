# language: pt-BR

Funcionalidade: Gerenciamento de Biblioteca de Games
    Como um usuário do sistema
    Eu quero gerenciar minha biblioteca de games
    Para controlar os jogos que possuo

Contexto:
    Dado que o serviço de biblioteca está configurado

# ==================== LISTAR BIBLIOTECA ====================

Cenário: Listar biblioteca de um usuário com jogos
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o usuário possui 2 jogos na biblioteca
    Quando eu listar a biblioteca do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a listagem deve retornar 2 jogos
    E todos os jogos devem pertencer ao usuário

Cenário: Listar biblioteca de um usuário sem jogos
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o usuário não possui jogos na biblioteca
    Quando eu listar a biblioteca do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a listagem deve retornar 0 jogos

Cenário: Falha ao listar biblioteca de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu listar a biblioteca do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

# ==================== COMPRAR GAME ====================

Cenário: Comprar game com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um game com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados da compra:
        | Campo          | Valor      |
        | TipoAquisicao  | Compra     |
        | PrecoAquisicao | 299.90     |
        | DataAquisicao  | 2024-01-10 |
    Quando eu comprar o game "2fa85f64-5717-4562-b3fc-2c963f66afa6" para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a compra deve ser realizada com sucesso
    E o jogo deve estar na biblioteca do usuário

Cenário: Falha ao comprar game para usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um game com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados da compra:
        | Campo          | Valor      |
        | TipoAquisicao  | Compra     |
        | PrecoAquisicao | 299.90     |
        | DataAquisicao  | 2024-01-10 |
    Quando eu comprar o game "2fa85f64-5717-4562-b3fc-2c963f66afa6" para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

Cenário: Falha ao comprar game inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E não existe um game com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados da compra:
        | Campo          | Valor      |
        | TipoAquisicao  | Compra     |
        | PrecoAquisicao | 299.90     |
        | DataAquisicao  | 2024-01-10 |
    Quando eu comprar o game "2fa85f64-5717-4562-b3fc-2c963f66afa6" para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Jogo com ID 2fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

Cenário: Falha ao comprar game quando serviço retorna erro
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um game com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o serviço de biblioteca não consegue comprar o game
    E tenho os dados da compra:
        | Campo          | Valor      |
        | TipoAquisicao  | Compra     |
        | PrecoAquisicao | 299.90     |
        | DataAquisicao  | 2024-01-10 |
    Quando eu comprar o game "2fa85f64-5717-4562-b3fc-2c963f66afa6" para o usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a compra deve falhar

# ==================== ATUALIZAR BIBLIOTECA ====================

Cenário: Atualizar item da biblioteca com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um game com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um item na biblioteca com ID "1fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados da biblioteca:
        | Campo          | Valor      |
        | TipoAquisicao  | Presente   |
        | PrecoAquisicao | 0.00       |
        | DataAquisicao  | 2024-01-15 |
    Quando eu atualizar o item "1fa85f64-5717-4562-b3fc-2c963f66afa6" da biblioteca
    Então a atualização deve ser realizada com sucesso
    E o item deve ter os dados atualizados

Cenário: Falha ao atualizar item de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um game com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados da biblioteca:
        | Campo          | Valor      |
        | TipoAquisicao  | Presente   |
        | PrecoAquisicao | 0.00       |
        | DataAquisicao  | 2024-01-15 |
    Quando eu atualizar o item "1fa85f64-5717-4562-b3fc-2c963f66afa6" da biblioteca
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

Cenário: Falha ao atualizar item com game inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E não existe um game com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E tenho os dados atualizados da biblioteca:
        | Campo          | Valor      |
        | TipoAquisicao  | Presente   |
        | PrecoAquisicao | 0.00       |
        | DataAquisicao  | 2024-01-15 |
    Quando eu atualizar o item "1fa85f64-5717-4562-b3fc-2c963f66afa6" da biblioteca
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Jogo com ID 2fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

Cenário: Falha ao atualizar item inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um game com ID "2fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o item da biblioteca não existe
    E tenho os dados atualizados da biblioteca:
        | Campo          | Valor      |
        | TipoAquisicao  | Presente   |
        | PrecoAquisicao | 0.00       |
        | DataAquisicao  | 2024-01-15 |
    Quando eu atualizar o item "1fa85f64-5717-4562-b3fc-2c963f66afa6" da biblioteca
    Então a atualização da biblioteca deve falhar

# ==================== DELETAR DA BIBLIOTECA ====================

Cenário: Deletar item da biblioteca com sucesso
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E existe um item na biblioteca com ID "1fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu deletar o item "1fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então o item deve ser deletado com sucesso

Cenário: Falha ao deletar item de usuário inexistente
    Dado que não existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Quando eu deletar o item "1fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então deve lançar uma exceção de não encontrado
    E a mensagem deve conter "Usuário com ID 3fa85f64-5717-4562-b3fc-2c963f66afa6 não encontrado"

Cenário: Falha ao deletar item inexistente
    Dado que existe um usuário com ID "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    E o item da biblioteca não pode ser deletado
    Quando eu deletar o item "1fa85f64-5717-4562-b3fc-2c963f66afa6" do usuário "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    Então a exclusão da biblioteca deve falhar
