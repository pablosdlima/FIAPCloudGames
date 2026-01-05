# 🎮 Projeto **CloudGames** – Grupo 24

Bem-vindo ao repositório do **Grupo 24** para o projeto **CloudGames**, desenvolvido como parte da pós-graduação FIAP.

## 🧑‍💻 Integrantes do Grupo

* **Marcio Lima Torquato** – `marcio.torquato7001`
* **Maria Eduarda Benício** – `maduu__15`
* **Mateus Vieira Cardoso** – `dragonladino`
* **Pablo Victor Simões de Lima** – `pablosdlima`

## 📄Documentação 

 - [Event Storming](https://miro.com/welcomeonboard/MVRMajhDQndrOEpNalA2bzBxMG5ndllqempCYWEzNnp2WnYvMVdlWTNRTlV2Q1A2a3NEQTF1V2pCQysyS2tXN1Y4eWhCOEw0V2EyaGtGUW5oQjRaMEFDeklLcXo1UFAxZzRmQTFHV3BnNll4ZmQvODNuV3YydDVKa3BhYjBnZGxBd044SHFHaVlWYWk0d3NxeHNmeG9BPT0hdjE=?share_link_id=166651382196)



## 🧪 Testes Automatizados

Foram implementados **testes unitários** com o objetivo de validar as **regras de negócio** e as **validações de entrada** do sistema, contemplando os testes abaixo:

- Email válido e inválido no cadastro de usuário
- Regras de senha forte (tamanho mínimo e complexidade)
- Campos obrigatórios no cadastro de usuário
- Login com usuário inexistente
- Login com senha incorreta
- Login com usuário inativo

Esses testes **não dependem de banco de dados**. Os testes utilizam as seguintes ferramentas:

- **xUnit** – framework de testes
- **FluentAssertions** – asserções mais legíveis
- **Moq** – criação de mocks para dependências


**Como Executar os Testes**

- *Opção 1:Executar via CLI*

Na raiz da solução, execute o comando:

```bash
dotnet test
```
- *Opção 2: Executar pelo Visual Studio*

1. Abra a solução no Visual Studio
2. Acesse Test > Test Explorer
3. Execute todos os testes ou selecione testes específicos

---

🔒 *Este repositório faz parte de um projeto acadêmico da Pós-graduação FIAP e destina-se exclusivamente a fins educacionais.*
