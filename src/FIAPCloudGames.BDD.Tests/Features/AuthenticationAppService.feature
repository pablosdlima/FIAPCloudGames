# language: pt-BR

Funcionalidade: Autenticação de Usuários
    Como um usuário do sistema
    Eu quero realizar login com minhas credenciais
    Para obter acesso autenticado à aplicação

Contexto:
    Dado que o serviço de autenticação está configurado

Cenário: Login bem-sucedido com credenciais válidas
    Dado que existe um usuário com login "marcio.silva" e senha "Senha@123"
    Quando eu realizar o login com usuário "marcio.silva" e senha "Senha@123"
    Então o login deve ser realizado com sucesso
    E um token JWT válido deve ser retornado

Cenário: Login falha com usuário inexistente
    Dado que não existe um usuário com login "usuario.invalido"
    Quando eu realizar o login com usuário "usuario.invalido" e senha "SenhaQualquer@123"
    Então o login deve falhar com uma exceção de autenticação
    E a mensagem de erro deve ser "Usuário ou senha inválidos."

Cenário: Login falha com senha incorreta
    Dado que existe um usuário com login "marcio.silva" e senha "Senha@123"
    Quando eu realizar o login com usuário "marcio.silva" e senha "SenhaErrada@123"
    Então o login deve falhar com uma exceção de autenticação
    E a mensagem de erro deve ser "Usuário ou senha inválidos."

Cenário: Login falha com credenciais vazias
    Quando eu realizar o login com usuário "" e senha ""
    Então o login deve falhar com uma exceção de autenticação
    E a mensagem de erro deve ser "Usuário ou senha inválidos."

Cenário: Token gerado contém informações do usuário
    Dado que existe um usuário com login "marcio.silva" e senha "Senha@123"
    Quando eu realizar o login com usuário "marcio.silva" e senha "Senha@123"
    Então o login deve ser realizado com sucesso
    E o token JWT deve conter as informações do usuário autenticado
