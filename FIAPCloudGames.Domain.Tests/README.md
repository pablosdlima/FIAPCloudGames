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
